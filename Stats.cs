using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	public class Stats
	{
		public string requested_by { get; set; }
		public string listeners { get; set; }
		public string song_name { get; set; }
		public string artist_name { get; set; }

        public static Stats DownloadStats()
        {
            var url = "https://listen.moe/api/info";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var data = new WebClient().DownloadString(url);

            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(Stats));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                return (Stats)s.ReadObject(stream);
            }
        }
    }
}
