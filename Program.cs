using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
	static class Program
	{
		[DllImport("kernel32", SetLastError = true)]
		static extern IntPtr LoadLibrary(string lpFileName);

		static bool CheckLibrary(string fileName)
		{
			return LoadLibrary(fileName) != IntPtr.Zero;
		}

		static void DeployOpenAL()
		{
			//Extract installer
			byte[] installer = Properties.Resources.oalinst;
			File.WriteAllBytes("oalinst.exe", installer);

			//Run
			var process = Process.Start("oalinst.exe");
			process.WaitForExit();

			//Delete installer
			File.Delete("oalinst.exe");
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			PlatformID p = Environment.OSVersion.Platform;
			if (p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows || p == PlatformID.WinCE)
			{
				if (!CheckLibrary("OpenAL32.dll") || !CheckLibrary("wrap_oal.dll"))
				{
					MessageBox.Show("The Listen.moe client uses the OpenAL framework for audio playback, and we have detected that you do not have OpenAL installed; the installer will now run.");
					DeployOpenAL();
				}
			}

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			Environment.SetEnvironmentVariable("LANG", "ja_JP.utf-8");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
