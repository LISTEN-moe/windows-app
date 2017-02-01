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
		public string requested_by { get; set; }
		public string listeners { get; set; }
		public string song_name { get; set; }
		public string artist_name { get; set; }
		public string anime_name { get; set; }
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
			socket.Connect();
		}

		public void ReconnectIfDead()
		{
			if (!socket.IsAlive)
				socket.Connect();
		}

		private void ParseSongInfo(string data)
		{
			if (data.Trim() == "")
				return;
			DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(SongInfo));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				currentInfo = (SongInfo)s.ReadObject(stream);
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
}
