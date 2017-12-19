using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	class Updater
	{
		[DataContract]
		class LatestReleaseResponse
		{
			[DataMember]
			public string tag_name { get; set; }
			[DataMember]
			public LatestReleaseAsset[] assets { get; set; }
		}

		[DataContract]
		class LatestReleaseAsset
		{
			[DataMember]
			public string browser_download_url { get; set; }
			[DataMember]
			public string name { get; set; }
		}

		private const string UPDATE_ENDPOINT = "https://api.github.com/repos/LISTEN-moe/windows-app/releases/latest";

		public static async Task<bool> CheckGithubVersion()
		{
			string rawResponse = await WebHelper.Get(UPDATE_ENDPOINT);
			LatestReleaseResponse response = Json.Parse<LatestReleaseResponse>(rawResponse);

			var version = response.tag_name;
			if (version == null)
				return false;

			if (version.StartsWith("v"))
				version = version.Substring(1);

			Console.WriteLine(Globals.VERSION);
			Console.WriteLine(version);

			//Same version
			if (version.Trim() == Globals.VERSION)
				return false;

			var latestParts = version.Trim().Split(new char[] { '.' });
			var ourParts = Globals.VERSION.Split(new char[] { '.' });

			//Must be really out of date if we've changed versioning schemes...
			if (latestParts.Length != ourParts.Length)
				return true;

			//Compare sub version numbers
			for (int i = 0; i < latestParts.Length; i++)
			{
				if (!int.TryParse(latestParts[i], out int latestVers))
					return true;
				if (!int.TryParse(ourParts[i], out int ourVers))
					return true;

				if (latestVers == ourVers)
					continue;
				else
					return latestVers > ourVers;
			}

			return false;
		}

		public static async Task UpdateToNewVersion(DownloadProgressChangedEventHandler dpceh, System.ComponentModel.AsyncCompletedEventHandler aceh)
		{
			string rawResponse = await WebHelper.Get(UPDATE_ENDPOINT);
			LatestReleaseResponse response = Json.Parse<LatestReleaseResponse>(rawResponse);

			if (response.assets.Length == 0)
			{
				MessageBox.Show("Unable to download new executable. Please update manually from the Github releases page.");
				return;
			}

			//First download link is fine for now... probably
			var link = response.assets[0].browser_download_url;

			var downloadPath = Path.GetTempFileName();
			WebClient wc = new WebClient();
			wc.DownloadProgressChanged += dpceh;
			wc.DownloadFileCompleted += aceh;
			await wc.DownloadFileTaskAsync(link, downloadPath);

			//Rename current executable as backup
			try
			{
				//Wait for a second before restarting so we get to see our nice green finished bar
				await Task.Delay(1000);

				string exeName = Process.GetCurrentProcess().ProcessName;
				File.Delete(exeName + ".bak");
				File.Move(exeName + ".exe", exeName + ".bak");
				File.Move(downloadPath, exeName + ".exe");

				Process.Start(exeName + ".exe");
				Environment.Exit(0);
			}
			catch (Exception)
			{
				MessageBox.Show("Unable to replace with updated executable. Check whether the executable is marked as read-only, or whether it is in a protected folder that requires administrative rights.");
			}
		}
	}
}
