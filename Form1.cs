using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

using Timer = System.Timers.Timer;

using NAudio;
using NAudio.Wave;
using System.Threading;

namespace CrappyListenMoe
{
	public partial class Form1 : Form
	{
		//Font loading stuff
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
		private PrivateFontCollection fonts = new PrivateFontCollection();

		WebStreamPlayer player;

		Font titleFont;
		Font artistFont;
        Font volumeFont;

		Timer getStatsTimer;

		//Drag form to move
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
            else if (e.Button == MouseButtons.Right)
            {
                contextMenu1.Show(this, e.Location);
            }
		}

		//Screen edge snapping
		private const int SnapDist = 10;
		private bool DoSnap(int pos, int edge)
		{
			int delta = Math.Abs(pos - edge);
			return delta <= SnapDist;
		}
		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			Screen scn = Screen.FromPoint(this.Location);
			if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
			if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
			if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
			if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;

            Settings.SetIntSetting("LocationX", this.Location.X);
            Settings.SetIntSetting("LocationY", this.Location.Y);
            Settings.WriteSettings();
		}

		public Form1()
		{
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
            Settings.LoadSettings();
            this.Location = new Point(Settings.GetIntSetting("LocationX"), Settings.GetIntSetting("LocationY"));

            this.MouseWheel += Form1_MouseWheel;

			new Thread(() => GetStats()).Start();
			getStatsTimer = new Timer(5000);
			getStatsTimer.Elapsed += GetStatsTimer_Elapsed;
			getStatsTimer.Start();

			this.Icon = Properties.Resources.icon;

			LoadOpenSans();

			lblTitle.Font = titleFont;
			lblArtist.Font = artistFont;
            lblVol.Font = volumeFont;

            player = new WebStreamPlayer("http://listen.moe:9999/stream");
			player.Open();
			player.Play();

            float vol = Settings.GetFloatSetting("Volume");
            SetVolumeLabel(vol);

            bool topmost = Settings.GetBoolSetting("TopMost");
            this.TopMost = topmost;
            menuItemTopmost.Checked = topmost;
		}

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                float volumeChange = (e.Delta / (float)SystemInformation.MouseWheelScrollDelta) * 0.05f;
                float newVol = player.AddVolume(volumeChange);
                Settings.SetFloatSetting("Volume", newVol);
                Settings.WriteSettings();
                SetVolumeLabel(newVol);
            }
        }

        private void SetVolumeLabel(float vol)
        {
            float newVol = vol * 100;
            if (Math.Abs(newVol - 100) < 0.001)
                newVol = 100;
            lblVol.Text = ((int)newVol).ToString() + "%";
        }

        private void LoadOpenSans()
		{
			byte[] fontData = Properties.Resources.OpenSans_Regular;
			IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
			Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
			uint dummy = 0;
			fonts.AddMemoryFont(fontPtr, Properties.Resources.OpenSans_Regular.Length);
			AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.OpenSans_Regular.Length, IntPtr.Zero, ref dummy);
			Marshal.FreeCoTaskMem(fontPtr);

			titleFont = new Font(fonts.Families[0], 11.0f);
			artistFont = new Font(fonts.Families[0], 8.0f);
            volumeFont = new Font(fonts.Families[0], 8.0f);
		}

		private void picPlayPause_Click(object sender, EventArgs e)
		{
			if (player.IsPlaying())
			{
				player.Stop();
				picPlayPause.Image = Properties.Resources.play;
			}
			else
			{
				player.Open();
				player.Play();
				picPlayPause.Image = Properties.Resources.pause;
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
            this.Close();
		}

		private void GetStatsTimer_Elapsed(object sender, EventArgs e)
		{
			GetStats();
		}

		void GetStats()
		{
			Stats stats = DownloadStats();
			lblTitle.Text = stats.song_name;
			string middle = string.IsNullOrWhiteSpace(stats.artist_name) ? "Requested by " : "; Requested by ";
			middle = string.IsNullOrEmpty(stats.requested_by) ? "" : middle;
			lblArtist.Text = stats.artist_name.Trim() + middle + stats.requested_by;
		}

		private Stats DownloadStats()
		{
			var url = "https://listen.moe/stats.json";
			var data = new WebClient().DownloadString(url);

			DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(Stats));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				return (Stats)s.ReadObject(stream);
			}
		}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            player.Stop();
            player.Dispose();
        }

        private void menuItemTopmost_Click(object sender, EventArgs e)
        {
            menuItemTopmost.Checked = !menuItemTopmost.Checked;
            this.TopMost = menuItemTopmost.Checked;
            Settings.SetBoolSetting("TopMost", menuItemTopmost.Checked);
            Settings.WriteSettings();
        }
    }
}
