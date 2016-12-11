using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Timer = System.Timers.Timer;

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

        Font titleFont;
        Font artistFont;
        Font volumeFont;

        public Form1()
		{
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
            Settings.LoadSettings();
            ApplyLoadedSettings();

            this.MouseWheel += Form1_MouseWheel;

			StatsStream statsStream = new StatsStream();
			statsStream.OnStatsReceived += GetStats;

			this.Icon = Properties.Resources.icon;

			LoadOpenSans();

			lblTitle.Font = titleFont;
			lblArtist.Font = artistFont;
            lblVol.Font = volumeFont;

            player = new WebStreamPlayer("https://listen.moe/stream");
			player.Open();
			player.Play();
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
                Settings.SetFloatSetting("Volume", newVol);
                Settings.WriteSettings();
                SetVolumeLabel(newVol);
            }
        }

        private void SetVolumeLabel(float vol)
        {
            int newVol = (int)Math.Round(vol * 100);
            lblVol.Text = newVol.ToString() + "%";
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
    }
}
