using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
	class Updater
	{
		//Mono has issues with Application.ProductVersion in the ILMerge'd binary, so we store our version here.
		//Application.ProductVersion will still stay up to date 
		const string CURRENT_VERS = "1.2.6";

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

		public static async Task<bool> CheckGithubVersion()
		{
			string rawResponse = await WebHelper.Get("https://api.github.com/repos/anonymousthing/ListenMoeClient/releases/latest");
			LatestReleaseResponse response = Json.Parse<LatestReleaseResponse>(rawResponse);

			var version = response.tag_name;

			if (version.StartsWith("v"))
				version = version.Substring(1);

			Console.WriteLine(CURRENT_VERS);
			Console.WriteLine(version);

			//Same version
			if (version.Trim() == CURRENT_VERS)
				return false;

			var latestParts = version.Trim().Split(new char[] { '.' });
			var ourParts = CURRENT_VERS.Split(new char[] { '.' });

			//Must be really out of date if we've changed versioning schemes...
			if (latestParts.Length != ourParts.Length)
				return true;

			//Compare sub version numbers
			for (int i = 0; i < latestParts.Length; i++)
			{
				int latestVers;
				int ourVers;
				if (!int.TryParse(latestParts[i], out latestVers))
					return true;
				if (!int.TryParse(ourParts[i], out ourVers))
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
			string rawResponse = await WebHelper.Get("https://api.github.com/repos/anonymousthing/ListenMoeClient/releases/latest");
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
			catch (Exception e)
			{
				MessageBox.Show("Unable to replace with updated executable. Check whether the executable is marked as read-only, or whether it is in a protected folder that requires administrative rights.");
			}
		}
	}
}
