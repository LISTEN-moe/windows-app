using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
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
		SongInfoStream songInfoStream;

		Font titleFont;
        Font artistFont;
        Font volumeFont;

		float updatePercent = 0;
		int updateState = 0; //0 = not updating, 1 = in progress, 2 = complete
		FormLogin loginForm;

		Sprite favSprite;
		Sprite fadedFavSprite;

		bool shiftDown = false;

        public Form1()
		{
			InitializeComponent();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
			
			favSprite = SpriteLoader.LoadFavSprite();
			fadedFavSprite = SpriteLoader.LoadFadedFavSprite();
			picFavourite.Image = favSprite.Frames[0];
			
			player = new WebStreamPlayer("https://listen.moe/stream");
			StartPlayback();

			TestToken();
		}

		private async void TestToken()
		{
			//lol
			string token = Settings.GetStringSetting("Token");
			string response = await WebHelper.Get("/api/user/favorites", token);
			var result = Json.Parse<ListenMoeResponse>(response);
			if (result.success)
				picFavourite.Visible = true;
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
				songInfoStream = new SongInfoStream(scheduler);
				songInfoStream.OnSongInfoReceived += ProcessSongInfo;
			});
		}

		private async void CheckForUpdates()
		{
			if (await Updater.CheckGithubVersion())
			{
				System.Media.SystemSounds.Beep.Play(); //DING
				if (MessageBox.Show(this, "An update is available for the Listen.moe player. Do you want to update and restart the application now?",
						"Listen.moe client - Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					updateState = 1;
					await Updater.UpdateToNewVersion(Wc_DownloadProgressChanged, Wc_DownloadFileCompleted);
				}
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
			//48px for pause/play button, 75 for the RHS area
			e.Graphics.FillRectangle(brush, 48, this.Height - 3, (this.Width - 48 - 75) * updatePercent, 3);
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
            titleFont = OpenSans.GetFont(11.0f);
            artistFont = OpenSans.GetFont(8.0f);
            volumeFont = OpenSans.GetFont(8.0f);
        }



        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
				float delta = 0.05f;
				if (shiftDown)
					delta = 0.01f;
                float volumeChange = (e.Delta / (float)SystemInformation.MouseWheelScrollDelta) * delta;
                float newVol = player.AddVolume(volumeChange);
				if (newVol >= 0)
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
				songInfoStream.ReconnectIfDead();
				StartPlayback();
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
            this.Close();
		}

		void ProcessSongInfo(SongInfo songInfo)
		{
			lblTitle.Text = songInfo.song_name;
			string artistAnimeName = songInfo.artist_name;
			if (!string.IsNullOrWhiteSpace(songInfo.anime_name))
			{
				if (!string.IsNullOrWhiteSpace(songInfo.artist_name))
					artistAnimeName += " (" + songInfo.anime_name + ")";
				else
					artistAnimeName = songInfo.anime_name;
			}
			string middle = string.IsNullOrWhiteSpace(artistAnimeName) ? "Requested by " : "; Requested by ";
			middle = string.IsNullOrEmpty(songInfo.requested_by) ? "" : middle;
			lblArtist.Text = artistAnimeName.Trim() + middle + songInfo.requested_by;

			if (songInfo.extended != null)
				SetFavouriteSprite(songInfo.extended.favorite);
			else
				picFavourite.Visible = false;
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
			if (loginForm != null)
				loginForm.TopMost = this.TopMost;
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

		private void menuItemCopySongInfo_Click(object sender, EventArgs e)
		{
			SongInfo info = songInfoStream.currentInfo;
			Clipboard.SetText(info.song_name + " \n" + info.artist_name + " \n" + info.anime_name);
		}

		public void SaveToken(bool success, string token, string username, string message)
		{
			picLogin.Image = Properties.Resources.up;
			loginForm.Dispose();
			loginForm = null;
			if (success)
			{
				Settings.SetStringSetting("Token", token);
				Settings.SetStringSetting("Username", username);
				Settings.WriteSettings();
				songInfoStream.Authenticate(token);
				picFavourite.Visible = true;
			}
		}

		private void picLogin_Click(object sender, EventArgs e)
		{
			if (loginForm == null)
			{
				picLogin.Image = Properties.Resources.down;
				loginForm = new FormLogin(SaveToken);
				loginForm.TopMost = this.TopMost;
				loginForm.Show();
				loginForm.Location = new Point(Location.X, Location.Y - loginForm.Height);
			}
			else
			{
				loginForm.Close();
				loginForm.Dispose();
				loginForm = null;
				picLogin.Image = Properties.Resources.up;
			}
		}

		int currentFrame = 0;
		bool isAnimating = false;
		object animationLock = new object();

		private async void SetFavouriteSprite(bool favourited)
		{
			picFavourite.Visible = true;
			if (favourited)
			{
				lock (animationLock)
				{
					currentFrame = 0;
					//Reset animation and exit
					if (isAnimating)
						return;
					isAnimating = true;
				}

				//Animate.
				while (currentFrame < favSprite.Frames.Length)
				{
					lock (animationLock)
					{
						if (!isAnimating)
							break;
					}
					picFavourite.Image = favSprite.Frames[currentFrame++];
					await Task.Delay(16);
				}

				isAnimating = false;
			}
			else
			{
				lock (animationLock)
					isAnimating = false;
				picFavourite.Image = favSprite.Frames[0];
			}
		}

		private async void picFavourite_Click(object sender, EventArgs e)
		{
			bool favouriteStatus = songInfoStream.currentInfo.extended?.favorite ?? false;
			picFavourite.Image = favouriteStatus ? fadedFavSprite.Frames[1] : fadedFavSprite.Frames[0];

			string result = await WebHelper.Post("/api/songs/favorite", Settings.GetStringSetting("Token"), new Dictionary<string, string>() {
				{ "song", songInfoStream.currentInfo.song_id.ToString() }
			});

			var response = Json.Parse<FavouritesResponse>(result);

			songInfoStream.Update();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Shift)
				shiftDown = true;
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Shift)
				shiftDown = false;
		}
	}
}
