using Newtonsoft.Json;
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

			(bool success, string resp) = await WebHelper.Post("https://dev.listen.moe/api/login", postData, true);
			var response = JsonConvert.DeserializeObject<AuthenticateResponse>(resp);
			if (success)
			{
				loggedIn = true;
				//Save successful credentials
				Settings.Set(Setting.Username, username);
				Settings.Set(Setting.Token, response.token);
				Settings.WriteSettings();

				OnLoginComplete();
			}
			else
			{
				//Login failure; clear old saved credentials
				Settings.Set(Setting.Username, "");
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
			(bool success, string resp) = await WebHelper.Get("https://dev.listen.moe/api/users/@me", "Bearer " + token, true);
			var response = JsonConvert.DeserializeObject<ListenMoeResponse>(resp);
			if (success)
			{
				loggedIn = true;
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
