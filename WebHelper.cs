using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	public class ListenMoeResponse
	{
		public bool success { get; set; }
		public string message { get; set; }
	}

	public class FavouritesResponse : ListenMoeResponse
	{
		public bool favorite { get; set; }
	}

	public class AuthenticateResponse : ListenMoeResponse
	{
		public string token { get; set; }
	}

	class WebHelper
	{
		private static byte[] createPostData(Dictionary<string, string> postData)
		{
			StringBuilder result = new StringBuilder("{");
			foreach (var keyValuePair in postData)
			{
				result.Append("\"" + keyValuePair.Key + "\":");
				result.Append("\"" + keyValuePair.Value + "\",");
			}
			result[result.Length - 1] = '}';

			return Encoding.ASCII.GetBytes(result.ToString());
		}

		public static async Task<string> Post(string endpoint, string token, Dictionary<string, string> postData)
		{
			return await Post(endpoint, token, postData, "application/json");
		}

		public static async Task<string> Post(string endpoint, Dictionary<string, string> postData)
		{
			return await Post(endpoint, "", postData, "application/json");
		}

		public static async Task<string> Post(string endpoint, string token, Dictionary<string, string> postData, string contentType)
		{
			HttpWebRequest hwr = WebRequest.CreateHttp("https://listen.moe" + endpoint);
			hwr.ContentType = contentType;
			hwr.Method = "POST";
			hwr.Timeout = 2000;
			if (token.Trim() != "")
				hwr.Headers["authorization"] = token;

			byte[] postDataBytes = createPostData(postData);
			hwr.ContentLength = postDataBytes.Length;

			Stream reqStream = await hwr.GetRequestStreamAsync();
			reqStream.Write(postDataBytes, 0, postDataBytes.Length);
			reqStream.Flush();
			reqStream.Close();

			Stream respStream;
			try
			{
				respStream = (await hwr.GetResponseAsync()).GetResponseStream();
			}
			catch (WebException e)
			{
				respStream = e.Response.GetResponseStream();
			}

			string result = await new StreamReader(respStream).ReadToEndAsync();
			return result;
		}

		public static async Task<string> Get(string endpoint)
		{
			return await Get(endpoint, "");
		}
		
		public static async Task<string> Get(string endpoint, string token)
		{
			HttpWebRequest hwr = WebRequest.CreateHttp("https://listen.moe" + endpoint);
			hwr.Method = "GET";
			hwr.Timeout = 2000;
			if (token.Trim() != "")
				hwr.Headers["authorization"] = token;

			Stream respStream;
			try
			{
				respStream = (await hwr.GetResponseAsync()).GetResponseStream();
			}
			catch (WebException e)
			{
				respStream = e.Response.GetResponseStream();
			}

			string result = await new StreamReader(respStream).ReadToEndAsync();
			return result;
		}
	}
}
