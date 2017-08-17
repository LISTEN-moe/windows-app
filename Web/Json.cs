using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ListenMoeClient
{
	class Json
	{
		public static T Parse<T>(string input)
		{
			DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
			{
				return (T)s.ReadObject(stream);
			}
		}
	}
}
