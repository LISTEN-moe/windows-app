using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListenMoeClient
{
	class User
	{
		private static bool loggedIn = false;
		public static event Action OnLoginComplete;
		public static event Action OnLogout;

		public static bool LoggedIn => loggedIn;

		/// <summary>
		/// Attempts to login with the specified credentials. Returns an AuthenticateResponse, containing
		/// whether or not the request was successful, along with the token and failure message if unsuccessful.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static async Task<AuthenticateResponse> Login(string username, string password)
		{
			Dictionary<string, string> postData = new Dictionary<string, string>
			{
				{ "username", username },
				{ "password", password }
			};

			(bool success, string resp) = await WebHelper.Post("https://listen.moe/api/login", postData, true);
			AuthenticateResponse response = JsonConvert.DeserializeObject<AuthenticateResponse>(resp);
			if (success)
			{
				loggedIn = true;
				//Save successful credentials
				Settings.Set(Setting.Username, username);
				Settings.Set(Setting.Token, response.token);
				Settings.WriteSettings();

				if (!response.mfa)
				{
					OnLoginComplete();
				}
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
			(bool success, string resp) = await WebHelper.Get("https://listen.moe/api/users/@me", token, true);
			ListenMoeResponse response = JsonConvert.DeserializeObject<ListenMoeResponse>(resp);
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

		public static async Task<bool> LoginMfa(string mfaCode)
		{
			Dictionary<string, string> postData = new Dictionary<string, string>
			{
				{ "token", mfaCode }
			};

			string token = Settings.Get<string>(Setting.Token);
			(bool success, string resp) = await WebHelper.Post("https://listen.moe/api/login/mfa", token, postData, true);
			AuthenticateResponse response = JsonConvert.DeserializeObject<AuthenticateResponse>(resp);
			if (success)
			{
				loggedIn = true;
				Settings.Set(Setting.Token, response.token);
				Settings.WriteSettings();

				OnLoginComplete();
			}
			return success;
		}

		public static async Task<bool> FavoriteSong(string id, bool favorite)
		{
			string token = Settings.Get<string>(Setting.Token);
			bool success = false;
			string result = null;

			if (favorite)
				(success, result) = await WebHelper.Post("https://listen.moe/api/favorites/" + id, token, null, true);
			else
				(success, result) = await WebHelper.Delete("https://listen.moe/api/favorites/" + id, token, true);

			return success;
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
