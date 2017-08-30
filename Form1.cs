using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	public partial class Form1 : Form
	{
		#region Magical form stuff
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr window, int index, int value);
		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr window, int index);
		const int GWL_EXSTYLE = -20;
		const int WS_EX_TOOLWINDOW = 0x00000080;
		const int WS_EX_APPWINDOW = 0x00040000;

		Point originalLocation;
		Point preMoveCursorLocation;
		int cursorLeftDiff, cursorRightDiff, cursorTopDiff, cursorBottomDiff;
		bool moving = false;

		//Screen edge snapping
		private const int snapDistance = 10;
		private bool CloseToEdge(int pos, int edge)
		{
			return Math.Abs(pos - edge) <= snapDistance;
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				preMoveCursorLocation = Cursor.Position;
				originalLocation = this.Location;
				moving = true;

				cursorLeftDiff = preMoveCursorLocation.X - this.Left;
				cursorRightDiff = this.Right - preMoveCursorLocation.X;
				cursorTopDiff = preMoveCursorLocation.Y - this.Top;
				cursorBottomDiff = this.Bottom - preMoveCursorLocation.Y;
			}
			else if (e.Button == MouseButtons.Right)
			{
				contextMenu1.Show(this, e.Location);
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (moving)
			{
				Point cursorDiff = new Point(Cursor.Position.X - preMoveCursorLocation.X, Cursor.Position.Y - preMoveCursorLocation.Y);
				Point newLocation = new Point(originalLocation.X + cursorDiff.X, originalLocation.Y + cursorDiff.Y);

				if (RawInput.IsPressed(VirtualKeys.Shift))
				{
					this.Location = newLocation;
				}
				else
				{
					Screen s = Screen.FromPoint(newLocation);

					bool hSnapped = false;
					bool vSnapped = false;
					if ((hSnapped = CloseToEdge(s.WorkingArea.Left, newLocation.X))) this.Left = s.WorkingArea.Left;
					if ((vSnapped = CloseToEdge(s.WorkingArea.Top, newLocation.Y))) this.Top = s.WorkingArea.Top;
					if (!hSnapped && (hSnapped = CloseToEdge(s.WorkingArea.Right, newLocation.X + Width))) this.Left = s.WorkingArea.Right - this.Width;
					if (!vSnapped && (vSnapped = CloseToEdge(s.WorkingArea.Bottom, newLocation.Y + Height))) this.Top = s.WorkingArea.Bottom - this.Height;

					int finalX = newLocation.X;
					int finalY = newLocation.Y;
					if (hSnapped)
						finalX = this.Location.X;
					if (vSnapped)
						finalY = this.Location.Y;

					this.Location = new Point(finalX, finalY);
					
					RecalculateMenuDirection();

					Settings.SetIntSetting("LocationX", this.Location.X);
					Settings.SetIntSetting("LocationY", this.Location.Y);
					Settings.WriteSettings();
				}

				if (loginForm != null)
				{
					if (openMenuUpwards)
						loginForm.Location = new Point(Location.X, Location.Y - loginForm.Height);
					else
						loginForm.Location = new Point(Location.X, Location.Y + this.Height);
				}
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (moving)
				moving = false;
		}
		
		#endregion

		WebStreamPlayer player;
		SongInfoStream songInfoStream;

		Font titleFont;
		Font artistFont;
		Font volumeFont;

		float updatePercent = 0;
		int updateState = 0; //0 = not updating, 1 = in progress, 2 = complete
		FormSettings loginForm;

		Sprite favSprite;
		Sprite fadedFavSprite;

		bool openMenuUpwards = true;

		public Form1()
		{
			InitializeComponent();
			RawInput.RegisterDevice(HIDUsagePage.Generic, HIDUsage.Keyboard, RawInputDeviceFlags.InputSink, this.Handle);
			Settings.LoadSettings();

			ApplyLoadedSettings();

			if (!Settings.GetBoolSetting("IgnoreUpdates"))
			{
				CheckForUpdates();
			}

			this.MouseWheel += Form1_MouseWheel;
			this.Icon = Properties.Resources.icon;

			LoadOpenSans();
			
			lblTitle.Font = titleFont;
			lblArtist.Font = artistFont;
			lblVol.Font = volumeFont;

			notifyIcon1.ContextMenu = contextMenu2;
			notifyIcon1.Icon = Properties.Resources.icon;

			if (Settings.GetBoolSetting("HideFromAltTab"))
			{
				this.ShowInTaskbar = false;
				int windowStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
				SetWindowLong(this.Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
				notifyIcon1.Visible = true;
			}

			favSprite = SpriteLoader.LoadFavSprite();
			fadedFavSprite = SpriteLoader.LoadFadedFavSprite();
			picFavourite.Image = favSprite.Frames[0];

			RawInput.RegisterCallback(VirtualKeys.MediaPlayPause, async () =>
			{
				await TogglePlayback();
			});
			
			Connect();
			RecalculateMenuDirection();

			player = new WebStreamPlayer("https://listen.moe/stream");
			player.Play();
		}

		private void RecalculateMenuDirection()
		{
			var screen = Screen.FromPoint(this.Location);
			bool previous = openMenuUpwards;
			//FormSettings height
			openMenuUpwards = Location.Y - screen.Bounds.Top > 365;
			if (openMenuUpwards != previous)
			{
				picLogin.Image = openMenuUpwards ? Properties.Resources.up : Properties.Resources.down;
			}
		}

		private async void Connect()
		{
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			var savedToken = Settings.GetStringSetting("Token");
			if (savedToken.Trim() != "")
			{
				picFavourite.Visible = await User.Login(savedToken);
			}
			await LoadWebSocket(scheduler);
		}

		private async Task LoadWebSocket(TaskScheduler scheduler)
		{
			await Task.Run(() =>
			{
				songInfoStream = new SongInfoStream(scheduler);
				songInfoStream.OnSongInfoReceived += ProcessSongInfo;
			});
		}

		protected override void WndProc(ref Message m)
		{
			WM message = (WM)m.Msg;
			if (message == WM.INPUT)
				RawInput.ProcessMessage(m.LParam);
			base.WndProc(ref m);
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
			SetTopMost(Settings.GetBoolSetting("TopMost"));
			float vol = Settings.GetFloatSetting("Volume");
			SetVolumeLabel(vol);
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
				if (RawInput.IsPressed(VirtualKeys.Shift))
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

		private async void playPause_Click(object sender, EventArgs e)
		{
			await TogglePlayback();
		}

		private async Task TogglePlayback()
		{
			if (player.IsPlaying())
			{
				picPlayPause.Image = Properties.Resources.play;
				menuItemPlayPause.Text = "Play";
				await player.Stop();
			}
			else
			{
				picPlayPause.Image = Properties.Resources.pause;
				menuItemPlayPause.Text = "Pause";
				if (songInfoStream != null)
					songInfoStream.ReconnectIfDead();
				player.Play();
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
			if (Settings.GetBoolSetting("CloseToTray"))
			{
				notifyIcon1.Visible = true;
				this.Hide();
				if (loginForm != null)
					loginForm.Hide();
			}
			else
			{
				this.Close();
			}
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

		private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			await Exit();
		}

		private async Task Exit()
		{
			this.Hide();
			await player.Dispose();
			Application.Exit();
		}

		public void SetTopMost(bool topMost)
		{
			this.TopMost = topMost;
			if (loginForm != null)
				loginForm.TopMost = topMost;
			Settings.SetBoolSetting("TopMost", topMost);
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

		public void AfterLogin(bool success, string token, string username, string message)
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
				picLogin.Image = openMenuUpwards ? Properties.Resources.down : Properties.Resources.up;
				loginForm = new FormSettings(this);
				loginForm.TopMost = this.TopMost;
				loginForm.Show();
				if (openMenuUpwards)
					loginForm.Location = new Point(Location.X, Location.Y - loginForm.Height);
				else
					loginForm.Location = new Point(Location.X, Location.Y + this.Height);
			}
			else
			{
				loginForm.Close();
				loginForm.Dispose();
				loginForm = null;
				picLogin.Image = openMenuUpwards ? Properties.Resources.up : Properties.Resources.down;
			}
		}

		int currentFrame = 0;
		bool isAnimating = false;

		private async void menuItemExit_Click(object sender, EventArgs e)
		{
			await Exit();
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				Restore();
		}

		private void menuItemShow_Click(object sender, EventArgs e)
		{
			Restore();
		}

		private void Restore()
		{
			this.Show();
			this.Activate();
			if (loginForm != null)
			{
				loginForm.Show();
				loginForm.Activate();
			}
			if (!Settings.GetBoolSetting("HideFromAltTab"))
				notifyIcon1.Visible = false;
		}

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

			string result = await WebHelper.Post("https://listen.moe/api/songs/favorite", Settings.GetStringSetting("Token"), new Dictionary<string, string>() {
				{ "song", songInfoStream.currentInfo.song_id.ToString() }
			});

			var response = Json.Parse<FavouritesResponse>(result);
			SetFavouriteSprite(response.favorite);
		}
	}
}
