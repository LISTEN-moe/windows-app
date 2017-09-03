using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	static class Program
	{
		static Mutex mutex = new Mutex(true, "{6431a734-2693-40d4-8dff-ea662d8777d7}");

		public const int HWND_BROADCAST = 0xffff;
		public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
		[DllImport("user32")]
		public static extern int RegisterWindowMessage(string message);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				Environment.SetEnvironmentVariable("LANG", "ja_JP.utf-8");
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			else
			{
				PostMessage((IntPtr)HWND_BROADCAST, WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
			}
		}
	}
}
