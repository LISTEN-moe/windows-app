using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
    public partial class Form1 : Form
    {
        #region Magical form stuff
        //Form dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
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
		private bool CloseToEdge(int pos, int edge)
		{
			return Math.Abs(pos - edge) <= SnapDist;
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			Screen s = Screen.FromPoint(this.Location);
			if (CloseToEdge(this.Left, s.WorkingArea.Left)) this.Left = s.WorkingArea.Left;
			if (CloseToEdge(this.Top, s.WorkingArea.Top)) this.Top = s.WorkingArea.Top;
			if (CloseToEdge(s.WorkingArea.Right, this.Right)) this.Left = s.WorkingArea.Right - this.Width;
			if (CloseToEdge(s.WorkingArea.Bottom, this.Bottom)) this.Top = s.WorkingArea.Bottom - this.Height;

            Settings.SetIntSetting("LocationX", this.Location.X);
            Settings.SetIntSetting("LocationY", this.Location.Y);
            Settings.WriteSettings();
        }
        #endregion

        WebStreamPlayer player;
		StatsStream statsStream;

		Font titleFont;
        Font artistFont;
        Font volumeFont;

		float updatePercent = 0;
		int updateState = 0; //0 = not updating, 1 = in progress, 2 = complete

        public Form1()
		{
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
            Settings.LoadSettings();
            ApplyLoadedSettings();

			if (!Settings.GetBoolSetting("IgnoreUpdates"))
			{
				CheckForUpdates();
			}

            this.MouseWheel += Form1_MouseWheel;
			this.Icon = Properties.Resources.icon;

			LoadWebSocket();
			LoadOpenSans();

			lblTitle.Font = titleFont;
			lblArtist.Font = artistFont;
            lblVol.Font = volumeFont;

            player = new WebStreamPlayer("https://listen.moe/stream");
			StartPlayback();
        }

		//Assumes that the player is in the stopped state
		private async void StartPlayback()
		{
			player.Open();
			await Task.Run(() => player.Play());
		}

		private async void LoadWebSocket()
		{
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			await Task.Run(() =>
			{
				statsStream = new StatsStream(scheduler);
				statsStream.OnStatsReceived += GetStats;
			});
		}

		private async void CheckForUpdates()
		{
			if (await CheckGithubVersion())
			{
				System.Media.SystemSounds.Beep.Play(); //DING
				if (MessageBox.Show(this, "An update is available for the Listen.moe player. Do you want to update and restart the application now?",
						"Listen.moe client - Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					await UpdateToNewVersion();
				}
			}
		}

		public async Task<bool> CheckGithubVersion()
		{
			string html = await new WebClient().DownloadStringTaskAsync("https://github.com/anonymousthing/ListenMoeClient/releases");
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
		
		public async Task UpdateToNewVersion()
		{
			updateState = 1;
			string html = await new WebClient().DownloadStringTaskAsync("https://github.com/anonymousthing/ListenMoeClient/releases");
			html = html.Substring(html.IndexOf("release label-latest"));
			html = html.Substring(html.IndexOf("release-body"));
			html = html.Substring(html.IndexOf("release-downloads"));

			//First download link is fine for now... probably
			html = html.Substring(html.IndexOf("<a href=") + 9);
			var link = "https://github.com" + html.Substring(0, html.IndexOf('"'));

			var downloadPath = Path.GetTempFileName();
			WebClient wc = new WebClient();
			wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
			wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
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

		private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			updateState = 2;
			this.Invalidate();
		}

		private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			updatePercent = e.BytesReceived / (float)e.TotalBytesToReceive;
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (updateState == 0)
				return;

			Brush brush = new SolidBrush(updateState == 1 ? Color.Yellow : Color.LimeGreen);
			e.Graphics.FillRectangle(brush, 48, this.Height - 3, (this.Width - 48) * updatePercent, 3);
		}

		private void ApplyLoadedSettings()
        {
            this.Location = new Point(Settings.GetIntSetting("LocationX"), Settings.GetIntSetting("LocationY"));

            float vol = Settings.GetFloatSetting("Volume");
            SetVolumeLabel(vol);

            bool topmost = Settings.GetBoolSetting("TopMost");
            this.TopMost = topmost;
            menuItemTopmost.Checked = topmost;
        }

        private void LoadOpenSans()
        {
            titleFont = OpenSans.CreateFont(11.0f);
            artistFont = OpenSans.CreateFont(8.0f);
            volumeFont = OpenSans.CreateFont(8.0f);
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                float volumeChange = (e.Delta / (float)SystemInformation.MouseWheelScrollDelta) * 0.05f;
                float newVol = player.AddVolume(volumeChange);
				if (newVol > 0)
				{
					Settings.SetFloatSetting("Volume", newVol);
					Settings.WriteSettings();
					SetVolumeLabel(newVol);
				}
            }
        }

        private void SetVolumeLabel(float vol)
        {
            int newVol = (int)Math.Round(vol * 100);
            lblVol.Text = newVol.ToString() + "%";
        }

		private void playPause_Click(object sender, EventArgs e)
		{
			if (player.IsPlaying())
			{
				player.Stop();
				picPlayPause.Image = Properties.Resources.play;
			}
			else
			{
				picPlayPause.Image = Properties.Resources.pause;
				statsStream.ReconnectIfDead();
				StartPlayback();
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
            this.Close();
		}

		void GetStats(Stats stats)
		{
			lblTitle.Text = stats.song_name;
			string artistAnimeName = stats.artist_name;
			if (!string.IsNullOrWhiteSpace(stats.anime_name))
			{
				if (!string.IsNullOrWhiteSpace(stats.artist_name))
					artistAnimeName += " (" + stats.anime_name + ")";
				else
					artistAnimeName = stats.anime_name;
			}
			string middle = string.IsNullOrWhiteSpace(artistAnimeName) ? "Requested by " : "; Requested by ";
			middle = string.IsNullOrEmpty(stats.requested_by) ? "" : middle;
			lblArtist.Text = artistAnimeName.Trim() + middle + stats.requested_by;
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

		private void panel1_MouseEnter(object sender, EventArgs e)
		{
			picPlayPause.Size = new Size(18, 18);
			picPlayPause.Location = new Point(15, 15);
		}

		private void panel1_MouseLeave(object sender, EventArgs e)
		{
			if (panel1.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
				return;
			picPlayPause.Size = new Size(16, 16);
			picPlayPause.Location = new Point(16, 16);
		}
	}
}
