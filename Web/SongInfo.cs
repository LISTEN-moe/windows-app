using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using Newtonsoft.Json;

namespace ListenMoeClient
{

	public class WelcomeResponse
	{
		public int op { get; set; }
		public WelcomeResponseData d { get; set; }
	}

	public class WelcomeResponseData
	{
		public string message { get; set; }
		public WelcomeResponseUserData user { get; set; }
		public int heartbeat { get; set; }
	}

	public class WelcomeResponseUserData
	{
		public string uuid { get; set; }
		public string email { get; set; }
		public string username { get; set; }
		public string displayName { get; set; }
		public int uploads { get; set; }
	}

	public class SongInfoResponse
	{
		public int op { get; set; }
		public SongInfoResponseData d { get; set; }
		public string t { get; set; }
	}

	public class SongInfoResponseData
	{
		public Song song { get; set; }
		public Requester requester { get; set; }
		[JsonProperty("event")]
		public Event _event { get; set; }
		public DateTime startTime { get; set; }
		public Song[] lastPlayed { get; set; }
		public int listeners { get; set; }
	}

	public class Event
	{
		public int id { get; set; }
		public string name { get; set; }
		public string slug { get; set; }
		public string presence { get; set; }
		public string image { get; set; }
	}

	public class Requester
	{
		public string uuid { get; set; }
		public string username { get; set; }
		public string displayName { get; set; }
	}

	public class Song
	{
		public int id { get; set; }
		public string title { get; set; }
		public Source[] sources { get; set; }
		public Artist[] artists { get; set; }
		public Album[] albums { get; set; }
		public int duration { get; set; }
		public bool favorite { get; set; }
	}

	public class Source
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameRomaji { get; set; }
		public object image { get; set; }
	}

	public class Artist
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameRomaji { get; set; }
		public object image { get; set; }
	}

	public class Album
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameRomaji { get; set; }
		public string image { get; set; }
	}

	public class SongInfoStream
	{
		private WebSocket socket;
		private TaskFactory factory;
		public delegate void StatsReceived(SongInfoResponseData info);
		public event StatsReceived OnSongInfoReceived = (info) => { };
		public SongInfoResponseData currentInfo;

		private const string JPOP_SOCKET_ADDR = "wss://listen.moe/gateway";
		private const string KPOP_SOCKET_ADDR = "wss://listen.moe/kpop/gateway";

		private Thread heartbeatThread;
		private CancellationTokenSource cts;

		public SongInfoStream(TaskFactory factory)
		{
			this.factory = factory;
			Reconnect();
		}

		public void Reconnect()
		{
			socket?.Close();
			string address = Settings.Get<StreamType>(Setting.StreamType) == StreamType.Jpop ? JPOP_SOCKET_ADDR : KPOP_SOCKET_ADDR;
			socket = new WebSocket(address);

			socket.OnMessage += (sender, e) => ParseSongInfo(e.Data);
			socket.OnError += (sender, e) => throw e.Exception;
			socket.OnClose += (sender, e) => Connect();

			socket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
			Connect();
		}

		private void Connect()
		{
			try
			{
				socket.Connect();
				Authenticate();
			}
			catch (Exception) { }
		}

		public void Authenticate()
		{
			try
			{
				string token;
				if (User.LoggedIn)
					token = "Bearer " + Settings.Get<string>(Setting.Token);
				else
					token = "";

				socket.Send("{ \"op\": 0, \"d\": { \"auth\": \"" + token + "\" } }");
			}
			catch (Exception) { }
		}

		private void SendHeartbeat()
		{
			try
			{
				socket.Send("{ \"op\": 9 }");
			} catch (Exception) { }
		}

		private void ProcessWelcomeResponse(WelcomeResponse resp)
		{
			int heartbeatInterval = resp.d.heartbeat;

			if (heartbeatThread != null)
			{
				cts.Cancel();

				//Running on the websocket thread, so no need to async/await
				heartbeatThread.Join();
			}

			cts = new CancellationTokenSource();
			heartbeatThread = new Thread(() =>
			{
				Stopwatch watch = new Stopwatch();
				watch.Start();
				long lastMillis = watch.ElapsedMilliseconds;
				while (!cts.IsCancellationRequested)
				{
					long currentMillis = watch.ElapsedMilliseconds;
					if (currentMillis - lastMillis > heartbeatInterval) {
						SendHeartbeat();
						lastMillis = currentMillis;
					}
					Thread.Sleep(1000);
				}
			});
			heartbeatThread.Start();
		}

		private string Clean(string input) => input != null ? input.Trim().Replace('\n', ' ') : "";

		private void ParseSongInfo(string data)
		{
			string noWhitespaceData = new string(data.Where(c => !char.IsWhiteSpace(c)).ToArray());
			if (noWhitespaceData.Contains("\"op\":0"))
			{
				//Identify/Welcome response
				WelcomeResponse resp = JsonConvert.DeserializeObject<WelcomeResponse>(data);
				ProcessWelcomeResponse(resp);
			}
			else if (noWhitespaceData.Contains("\"op\":1"))
			{
				//Song info
				SongInfoResponse resp = JsonConvert.DeserializeObject<SongInfoResponse>(data);
				if (resp.t != "TRACK_UPDATE")
					return;

				currentInfo = resp.d;
				currentInfo.song.sources = currentInfo.song.sources ?? new Source[0];
				foreach (Source source in currentInfo.song.sources)
				{
					source.name = Clean(source.name);
				}

				foreach (Artist artist in currentInfo.song.artists)
				{
					artist.name = Clean(artist.name);
				}
				currentInfo.song.title = Clean(currentInfo.song.title);

				if (currentInfo.requester != null)
				{
					currentInfo.requester.displayName = Clean(currentInfo.requester.displayName);
					currentInfo.requester.username = Clean(currentInfo.requester.username);
				}

				if (currentInfo._event != null)
					currentInfo._event.name = Clean(currentInfo._event.name);

				factory.StartNew(() =>
				{
					OnSongInfoReceived(currentInfo);
				});
			}
		}
	}
}
