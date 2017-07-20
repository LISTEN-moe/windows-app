using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	class User
	{
		static bool loggedIn = false;
		static HashSet<Action> loginCallbacks = new HashSet<Action>();

		public static bool LoggedIn
		{
			get { return loggedIn; }
		}

		public static void AddLoginCallback(Action callback)
		{
			loginCallbacks.Add(callback);
		}

		/// <summary>
		/// Attempts to login with the specified credentials. Returns an AuthenticateResponse, containing
		/// whether or not the request was successful, along with the token and failure message if unsuccessful.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static async Task<AuthenticateResponse> Login(string username, string password)
		{
			var postData = new Dictionary<string, string>();
			postData.Add("username", username);
			postData.Add("password", password);

			string resp = await WebHelper.Post("https://listen.moe/api/authenticate", postData);
			var response = Json.Parse<AuthenticateResponse>(resp);
			Settings.SetStringSetting("Username", response.success ? username : "");
			loggedIn = response.success;
			if (loggedIn)
			{
				foreach (var callback in loginCallbacks)
					callback();
				//Save successful credentials
				Settings.SetStringSetting("Username", username);
				Settings.SetStringSetting("Token", response.token);
			}

			return response;
		}

		/// <summary>
		/// Attempts to login with the specified auth token. Returns whether or not the login was successful.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static async Task<bool> Login(string token)
		{
			string response = await WebHelper.Get("https://listen.moe/api/user", token);
			var result = Json.Parse<ListenMoeResponse>(response);
			loggedIn = result.success;
			if (loggedIn)
				foreach (var callback in loginCallbacks)
					callback();
			//Clear saved credentials on failure
			if (!result.success)
			{
				Settings.SetStringSetting("Username", "");
				Settings.SetStringSetting("Token", "");
			}
			return result.success;
		}
	}
}
