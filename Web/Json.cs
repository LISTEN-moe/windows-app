using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

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
