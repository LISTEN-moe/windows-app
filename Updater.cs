using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
	//The worst updater you've ever laid eyes upon
	class Updater
	{
		public static bool CheckForUpdates()
		{
			string html = new WebClient().DownloadString("https://github.com/anonymousthing/ListenMoeClient/releases");
			html = html.Substring(html.IndexOf("release label-latest"));
			html = html.Substring(html.IndexOf("release-meta"));
			html = html.Substring(html.IndexOf("tag-references"));
			html = html.Substring(html.IndexOf("octicon-tag")); //Under the assumption that the tag icon comes before the span
			html = html.Substring(html.IndexOf("<span"));
			html = html.Substring(html.IndexOf('>') + 1);

			string version = html.Substring(0, html.IndexOf('<'));
			if (version.StartsWith("v"))
				version = version.Substring(1);

			string ourVersion = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.')); //Strip build number

			//Same version
			if (version.Trim() == ourVersion)
				return false;

			var latestParts = version.Trim().Split(new char[] { '.' });
			var ourParts = ourVersion.Split(new char[] { '.' });

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

		public static void Update()
		{
			Task.Run(() =>
			{
				string html = new WebClient().DownloadString("https://github.com/anonymousthing/ListenMoeClient/releases");
				html = html.Substring(html.IndexOf("release label-latest"));
				html = html.Substring(html.IndexOf("release-body"));
				html = html.Substring(html.IndexOf("release-downloads"));

				//First download link is fine for now... probably
				html = html.Substring(html.IndexOf("<a href=") + 9);
				var link = "https://github.com" + html.Substring(0, html.IndexOf('"'));

				var downloadPath = Path.GetTempFileName();
				new WebClient().DownloadFile(link, downloadPath);

				//Rename current executable as backup
				try
				{
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
			});

			new UpdateDialog().ShowDialog();
		}
	}
}
