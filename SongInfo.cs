using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace CrappyListenMoe
{
	public class SongInfo
	{
		public int song_id { get; set; }
		public string requested_by { get; set; }
		public string listeners { get; set; }
		public string song_name { get; set; }
		public string artist_name { get; set; }
		public string anime_name { get; set; }

		public PreviousSongInfo last { get; set; }
		public PreviousSongInfo second_last { get; set; }

		public ExtendedSongInfo extended { get; set; }
    }

	public class PreviousSongInfo
	{
		public string song_name { get; set; }
		public string artist_name { get; set; }
	}

	public class ExtendedSongInfo
	{
		public bool favorite { get; set; }
	}

	public class SongQueue
	{
		public int songsInQueue { get; set; }
		public bool hasSongInQueue { get; set; }
		public int inQueueBeforeUserSong { get; set; }
		public int userSongsInQueue { get; set; }
	}

	public class SongInfoStream
	{
		private WebSocket socket;
		private TaskFactory factory;
		public delegate void StatsReceived(SongInfo info);
		public event StatsReceived OnSongInfoReceived = (info) => { };
		public SongInfo currentInfo;

		public SongInfoStream(TaskScheduler scheduler)
		{
			factory = new TaskFactory(scheduler);
			socket = new WebSocket("wss://listen.moe/api/v2/socket");

			socket.OnMessage += (sender, e) => ParseSongInfo(e.Data);
			socket.OnError += (sender, e) => { throw e.Exception; };
			socket.OnClose += (sender, e) =>
			{
				socket.Connect();
			};

			socket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
			Connect();
		}

		private void Connect()
		{
			socket.Connect();

			string token = Settings.GetStringSetting("Token");
			if (token != "")
				Authenticate(token);
		}

		public void Update()
		{
			socket.Send("update");
		}

		public void Authenticate(string token)
		{
			socket.Send("{ \"token\": \"" + token + "\" }");
		}

		public void ReconnectIfDead()
		{
			if (!socket.IsAlive)
				Connect();
		}

		private void ParseSongInfo(string data)
		{
			if (data.Trim() == "")
				return;
			currentInfo = Json.Parse<SongInfo>(data);
			currentInfo.anime_name = currentInfo.anime_name.Trim().Replace('\n', ' ');
			currentInfo.artist_name = currentInfo.artist_name.Trim().Replace('\n', ' ');
			currentInfo.song_name = currentInfo.song_name.Trim().Replace('\n', ' ');

			factory.StartNew(() =>
			{
				OnSongInfoReceived(currentInfo);
			});
		}
	}
}
