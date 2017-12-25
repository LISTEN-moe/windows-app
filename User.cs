using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListenMoeClient
{
	class User
	{
		static bool loggedIn = false;
		public static event Action OnLoginComplete;
		public static event Action OnLogout;

		public static bool LoggedIn
		{
			get { return loggedIn; }
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
			var postData = new Dictionary<string, string>
			{
				{ "username", username },
				{ "password", password }
			};

			string resp = await WebHelper.Post("https://listen.moe/api/authenticate", postData);
			var response = Json.Parse<AuthenticateResponse>(resp);
			Settings.Set(Setting.Username, response.success ? username : "");
			loggedIn = response.success;
			if (loggedIn)
			{
				//Save successful credentials
				Settings.Set(Setting.Username, username);
				Settings.Set(Setting.Token, response.token);
				Settings.WriteSettings();

				OnLoginComplete();
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
			{
				OnLoginComplete();
			}
			else
			{
				//Clear saved credentials on failure
				Settings.Set(Setting.Username, "");
				Settings.Set(Setting.Token, "");
				Settings.WriteSettings();
			}
			return loggedIn;
		}

		public static void Logout()
		{
			loggedIn = false;
			Settings.Set(Setting.Username, "");
			Settings.Set(Setting.Token, "");
			Settings.WriteSettings();
			OnLogout();
		}
	}
}
