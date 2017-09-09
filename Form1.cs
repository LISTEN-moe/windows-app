using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
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
		Font albumFont;
		Font volumeFont;

		float updatePercent = 0;
		int updateState = 0; //0 = not updating, 1 = in progress, 2 = complete
		FormSettings loginForm;

		Sprite favSprite;
		Sprite fadedFavSprite;

		bool openMenuUpwards = true;

		MarqueeLabel lblAlbum = new MarqueeLabel();
		MarqueeLabel lblTitle = new MarqueeLabel();
		Visualiser visualiser;

		Thread renderLoop;

		public Form1()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			RawInput.RegisterDevice(HIDUsagePage.Generic, HIDUsage.Keyboard, RawInputDeviceFlags.InputSink, this.Handle);
			Settings.LoadSettings();
			
			ApplyLoadedSettings();

			if (!Settings.GetBoolSetting("IgnoreUpdates"))
			{
				CheckForUpdates();
			}

			this.MouseWheel += Form1_MouseWheel;
			this.Icon = Properties.Resources.icon;

			LoadFonts();

			lblTitle.Font = titleFont;
			lblTitle.Subfont = albumFont;
			lblAlbum.Font = albumFont;
			lblVol.Font = volumeFont;

			lblAlbum.Bounds = new Rectangle(58, 26, 321, 22);
			lblTitle.Bounds = new Rectangle(56, 5, 321, 43);
			lblTitle.Text = "Connecting...";

			if (Settings.GetBoolSetting("EnableVisualiser"))
			{
				StartVisualiser();
			}

			notifyIcon1.ContextMenu = contextMenu2;
			notifyIcon1.Icon = Properties.Resources.icon;

			if (Settings.GetBoolSetting("HideFromAltTab"))
			{
				this.ShowInTaskbar = false;
				int windowStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
				SetWindowLong(this.Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
			}
			if (Settings.GetBoolSetting("CloseToTray"))
			{
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
			player.SetVisualiser(visualiser);
			player.Play();

			renderLoop = new Thread(() =>
			{
				while (true)
				{
					this.Invalidate();
					Thread.Sleep(33);
				}
			});
			renderLoop.Start();
		}

		public void StartVisualiser()
		{
			if (visualiser == null)
			{
				visualiser = new Visualiser();
				visualiser.Bounds = new Rectangle(48, 48, 337, 48);
				visualiser.Start();
				player.SetVisualiser(visualiser);
			}
		}

		public void StopVisualiser()
		{
			if (visualiser != null)
			{
				player.SetVisualiser(null);
				visualiser.Stop();
				visualiser = null;
			}
		}

		private void LoadFonts()
		{
			var family = Meiryo.GetFontFamily();
			titleFont = new Font(family, 12);
			albumFont = Meiryo.GetFont(8.0f);
			volumeFont = Meiryo.GetFont(8.0f);
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
			if (m.Msg == Program.WM_SHOWME)
			{
				WindowState = FormWindowState.Normal;
				this.Activate();
			}
			base.WndProc(ref m);
		}

		private async void CheckForUpdates()
		{
			if (await Updater.CheckGithubVersion())
			{
				System.Media.SystemSounds.Beep.Play(); //DING
				if (MessageBox.Show(this, "An update is available for the Listen.moe player. Do you want to update and restart the application now?",
						"Listen.moe client - Update available - current version " + Globals.VERSION, MessageBoxButtons.YesNo) == DialogResult.Yes)
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

			if (updateState != 0)
			{
				Brush brush = new SolidBrush(updateState == 1 ? Color.Yellow : Color.LimeGreen);
				//48px for pause/play button, 75 for the RHS area
				e.Graphics.FillRectangle(brush, 48, this.Height - 3, (this.Width - 48 - 75) * updatePercent, 3);
			}

			if (visualiser != null)
			{
				visualiser.Render(e.Graphics);
			}
			lblTitle.Render(e.Graphics);
			lblAlbum.Render(e.Graphics);
		}

		private void ApplyLoadedSettings()
		{
			this.Location = new Point(Settings.GetIntSetting("LocationX"), Settings.GetIntSetting("LocationY"));
			SetTopMost(Settings.GetBoolSetting("TopMost"));
			float vol = Settings.GetFloatSetting("Volume");
			SetVolumeLabel(vol);
		}

		private void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta != 0)
			{
				if (RawInput.IsPressed(VirtualKeys.Control))
				{
					visualiser.IncreaseBarWidth(0.5f * e.Delta / (float)SystemInformation.MouseWheelScrollDelta);
				}
				else if (RawInput.IsPressed(VirtualKeys.Menu))
				{
					visualiser.IncreaseResolution(e.Delta / SystemInformation.MouseWheelScrollDelta);
				}
				else
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
				visualiser.Stop();
				await player.Stop();
			}
			else
			{
				picPlayPause.Image = Properties.Resources.pause;
				menuItemPlayPause.Text = "Pause";
				if (songInfoStream != null)
					songInfoStream.ReconnectIfDead();
				visualiser.Start();
				player.Play();
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
			if (Settings.GetBoolSetting("CloseToTray"))
			{
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
			lblTitle.Subtext = songInfo.artist_name.Trim();
			string albumName = songInfo.anime_name;
			string middle = "";
			if (!string.IsNullOrEmpty(songInfo.requested_by))
			{
				middle = songInfo.requested_by.Contains(" ") ? "" : "Requested by ";
				if (!string.IsNullOrWhiteSpace(albumName))
					middle = "; " + middle;
			}
			lblAlbum.Text = albumName + middle + songInfo.requested_by;

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
			renderLoop.Abort();
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

		public void SetNotifyIconVisible(bool visible)
		{
			notifyIcon1.Visible = visible;
		}
	}
}
