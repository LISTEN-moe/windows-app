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
	public class Stats
	{
		public string requested_by { get; set; }
		public string listeners { get; set; }
		public string song_name { get; set; }
		public string artist_name { get; set; }
		public string anime_name { get; set; }
    }

	public class StatsStream
	{
		private WebSocket socket;
		public delegate void StatsReceived(Stats stats);
		public event StatsReceived OnStatsReceived = (stats) => { };

		public StatsStream()
		{
			socket = new WebSocket("wss://listen.moe/api/v2/socket");

			socket.OnMessage += (sender, e) => ParseStats(e.Data);
			socket.OnError += (sender, e) => { throw e.Exception; };
			socket.OnClose += (sender, e) =>
			{
				socket.Connect();
			};

			socket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
			socket.Connect();
		}

		private void ParseStats(string data)
		{
			if (data.Trim() == "")
				return;
			DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(Stats));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				Stats stats = (Stats)s.ReadObject(stream);
				OnStatsReceived(stats);
			}
		}
	}
}
