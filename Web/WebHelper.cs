using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ListenMoeClient
{
	public class ListenMoeResponse
	{
		public string message { get; set; }
	}

	public class FavouritesResponse : ListenMoeResponse
	{
		public bool favorite { get; set; }
	}

	public class AuthenticateResponse : ListenMoeResponse
	{
		public string token { get; set; }
		public bool mfa { get; set; }
	}

	class WebHelper
	{
		private static byte[] createPostData(Dictionary<string, string> postData)
		{
			StringBuilder result = new StringBuilder("{");
			foreach (var keyValuePair in postData)
			{
				result.Append("\"" + HttpUtility.JavaScriptStringEncode(keyValuePair.Key) + "\":");
				result.Append("\"" + HttpUtility.JavaScriptStringEncode(keyValuePair.Value) + "\",");
			}
			result[result.Length - 1] = '}';

			return Encoding.UTF8.GetBytes(result.ToString());
		}

		public static async Task<(bool, string)> Post(string url, string token, Dictionary<string, string> postData, bool isListenMoe)
		{
			return await Post(url, token, postData, "application/json", isListenMoe);
		}

		public static async Task<(bool, string)> Post(string url, Dictionary<string, string> postData, bool isListenMoe)
		{
			return await Post(url, "", postData, "application/json", isListenMoe);
		}

		public static async Task<(bool, string)> Post(string url, string token, Dictionary<string, string> postData, string contentType, bool isListenMoe)
		{
			HttpWebRequest hwr = WebRequest.CreateHttp(url);
			hwr.ContentType = contentType;
			hwr.Method = "POST";
			hwr.Timeout = 2000;
			if (token.Trim() != "")
				hwr.Headers["authorization"] = token;

			if (isListenMoe)
				hwr.Accept = "application/vnd.listen.v4+json";

			byte[] postDataBytes = createPostData(postData);
			hwr.ContentLength = postDataBytes.Length;

			Stream reqStream = await hwr.GetRequestStreamAsync();
			reqStream.Write(postDataBytes, 0, postDataBytes.Length);
			reqStream.Flush();
			reqStream.Close();

			Stream respStream;
			bool success = true;
			try
			{
				respStream = (await hwr.GetResponseAsync()).GetResponseStream();
			}
			catch (WebException e)
			{
				success = false;
				respStream = e.Response.GetResponseStream();
			}

			string result = await new StreamReader(respStream).ReadToEndAsync();
			return (success, result);
		}

		public static async Task<(bool, string)> Get(string endpoint, bool isListenMoe)
		{
			return await Get(endpoint, "", isListenMoe);
		}

		public static async Task<(bool, string)> Get(string url, string token, bool isListenMoe)
		{
			HttpWebRequest hwr = WebRequest.CreateHttp(url);
			hwr.Method = "GET";
			hwr.Timeout = 2000;
			hwr.UserAgent = Globals.USER_AGENT;
			if (token.Trim() != "")
				hwr.Headers["authorization"] = token;

			if (isListenMoe)
				hwr.Accept = "application/vnd.listen.v4+json";

			Stream respStream;
			bool success = true;
			try
			{
				respStream = (await hwr.GetResponseAsync()).GetResponseStream();
			}
			catch (WebException e)
			{
				success = false;
				respStream = e.Response.GetResponseStream();
			}

			string result = await new StreamReader(respStream).ReadToEndAsync();
			return (success, result);
		}
	}
}
