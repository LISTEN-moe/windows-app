using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	public partial class MainForm : Form
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
					
					Settings.Set("LocationX", this.Location.X);
					Settings.Set("LocationY", this.Location.Y);
					Settings.WriteSettings();
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
		float currentScale = 1f;

		float updatePercent = 0;
		int updateState = 0; //0 = not updating, 1 = in progress, 2 = complete
		public FormSettings SettingsForm;

		Sprite favSprite;
		Sprite fadedFavSprite;

		MarqueeLabel lblAlbum = new MarqueeLabel();
		MarqueeLabel lblTitle = new MarqueeLabel();
		Visualiser visualiser;

		CancellationTokenSource cts;
		CancellationToken ct;
		Task updaterTask;
		Task renderLoop;

		public MainForm()
		{
			InitializeComponent();
			contextMenu1.MenuItems.Add(new MenuItem("LISTEN.moe Desktop Client v" + Globals.VERSION.ToString()) { Enabled = false });
			Settings.LoadSettings();
			//Write immediately after loading to flush any new default settings
			Settings.WriteSettings();
			if (Settings.Get<bool>("HideFromAltTab"))
			{
				this.ShowInTaskbar = false;
				int windowStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
				SetWindowLong(this.Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
				notifyIcon1.Visible = true;
			}

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
			RawInput.RegisterDevice(HIDUsagePage.Generic, HIDUsage.Keyboard, RawInputDeviceFlags.InputSink, this.Handle);


			cts = new CancellationTokenSource();
			ct = cts.Token;
#pragma warning disable CS4014
			CheckForUpdates(new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()));
#pragma warning restore CS4014
			StartUpdateAutochecker();

			this.MouseWheel += Form1_MouseWheel;
			this.Icon = Properties.Resources.icon;
			
			lblAlbum.Bounds = new Rectangle(58, 26, 321, 22);
			lblTitle.Bounds = new Rectangle(56, 5, 321, 43);
			lblTitle.Text = "Connecting...";

			notifyIcon1.ContextMenu = contextMenu2;
			notifyIcon1.Icon = Properties.Resources.icon;

			favSprite = SpriteLoader.LoadFavSprite();
			fadedFavSprite = SpriteLoader.LoadFadedFavSprite();
			picFavourite.Image = favSprite.Frames[0];

			RawInput.RegisterCallback(VirtualKeys.MediaPlayPause, async () =>
			{
				await TogglePlayback();
			});
			
			Connect();

			player = new WebStreamPlayer("https://listen.moe/stream");
			player.SetVisualiser(visualiser);
			player.Play();

			renderLoop = Task.Run(() =>
			{
				while (!ct.IsCancellationRequested)
				{
					this.Invalidate();
					Thread.Sleep(33);
				}
			});
			
			ReloadScale();
			ReloadSettings();
		}

		public void ReloadSettings()
		{
			if (Settings.Get<bool>("EnableVisualiser"))
				StartVisualiser();
			else
				StopVisualiser();
			
			if (visualiser != null)
				visualiser.ReloadSettings();
			this.TopMost = Settings.Get<bool>("TopMost");

			this.Location = new Point(Settings.Get<int>("LocationX"), Settings.Get<int>("LocationY"));
			float vol = Settings.Get<float>("Volume");
			panelPlayBtn.BackColor = Settings.Get<Color>("AccentColor");
			this.BackColor = Settings.Get<Color>("BaseColor");
			panelRight.BackColor = Color.FromArgb((int)((BackColor.R * 1.1f).Bound(0, 255)),
												  (int)((BackColor.G * 1.1f).Bound(0, 255)),
												  (int)((BackColor.B * 1.1f).Bound(0, 255)));
			SetVolumeLabel(vol);
			this.Opacity = Settings.Get<float>("FormOpacity");
		}

		public void ReloadScale()
		{
			float scaleFactor = Settings.Get<float>("Scale");
			this.Scale(new SizeF(scaleFactor / currentScale, scaleFactor / currentScale));
			currentScale = scaleFactor;
			
			//Reload fonts to get newly scaled font sizes
			LoadFonts();
			SetPlayPauseSize(false);
		}

		public void StartVisualiser()
		{
			if (visualiser == null)
			{
				visualiser = new Visualiser();
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
			var scaleFactor = Settings.Get<float>("Scale");
			titleFont = new Font(family, 12 * scaleFactor);
			albumFont = Meiryo.GetFont(8 * scaleFactor);
			volumeFont = Meiryo.GetFont(8 * scaleFactor);

			lblTitle.Font = titleFont;
			lblTitle.Subfont = albumFont;
			lblAlbum.Font = albumFont;
			lblVol.Font = volumeFont;

			lblTitle.OnTextChanged();
			lblAlbum.OnTextChanged();
		}

		private async void Connect()
		{
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			await LoadWebSocket(scheduler);
		}

		private async Task LoadWebSocket(TaskScheduler scheduler)
		{
			await Task.Run(async () =>
			{
				TaskFactory factory = new TaskFactory(scheduler);
				songInfoStream = new SongInfoStream(factory);
				songInfoStream.OnSongInfoReceived += ProcessSongInfo;

				User.OnLoginComplete += () =>
				{
					factory.StartNew(() => picFavourite.Visible = true);
					songInfoStream.Authenticate();
				};
				User.OnLogout += async () =>
				{
					await factory.StartNew(() => picFavourite.Visible = false);
					await Task.Run(() => songInfoStream.Reconnect());
				};
				string savedToken = Settings.Get<string>("Token").Trim();
				if (savedToken != "")
					await User.Login(savedToken);
			});
		}

		protected override void WndProc(ref Message m)
		{
			WM message = (WM)m.Msg;
			if (message == WM.INPUT)
				RawInput.ProcessMessage(m.LParam);
			if (m.Msg == Program.WM_SHOWME)
			{
				Restore();
			}
			base.WndProc(ref m);
		}

		private async Task CheckForUpdates(TaskFactory factory)
		{
			if (await Updater.CheckGithubVersion())
			{
				System.Media.SystemSounds.Beep.Play(); //DING
				DialogResult result = await factory.StartNew(() => MessageBox.Show(this, "An update is available for the Listen.moe player. Do you want to update and restart the application now?",
						"Listen.moe client - Update available - current version " + Globals.VERSION, MessageBoxButtons.YesNo));
				if (result == DialogResult.Yes)
				{
					updateState = 1;
					await Updater.UpdateToNewVersion(Wc_DownloadProgressChanged, Wc_DownloadFileCompleted);
				}
			}
		}

		private void StartUpdateAutochecker()
		{
			TaskFactory factory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
			updaterTask = Task.Run(async () =>
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				long last = stopwatch.ElapsedMilliseconds;
				while (!ct.IsCancellationRequested)
				{
					if (stopwatch.ElapsedMilliseconds - last > Settings.Get<int>("UpdateInterval") * 1000)
					{
						//We only check for the setting here, because I'm lazy to dispose/recreate this checker thread when they change the setting
						if (!Settings.Get<bool>("UpdateAutocheck"))
						{
							last = stopwatch.ElapsedMilliseconds;
							continue;
						}

						await CheckForUpdates(factory);
						last = stopwatch.ElapsedMilliseconds;
					}
					else
					{
						Thread.Sleep(100);
					}
				}
			});
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
			this.SuspendLayout();

			if (visualiser != null)
			{
				visualiser.Render(e.Graphics);
			}
			lblTitle.Render(e.Graphics);
			lblAlbum.Render(e.Graphics);

			if (updateState != 0)
			{
				Brush brush = new SolidBrush(updateState == 1 ? Color.Yellow : Color.LimeGreen);
				//48px for pause/play button, 75 for the RHS area
				e.Graphics.FillRectangle(brush, 48, this.Height - 3, (this.Width - 48 - 75) * updatePercent, 3);
			}

			this.ResumeLayout();
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
						Settings.Set("Volume", newVol);
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
				visualiser.Start();
				player.Play();
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
			if (Settings.Get<bool>("CloseToTray"))
				this.Hide();
			else
				this.Close();
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
			cts.Cancel();
			if (SettingsForm != null)
			{
				SettingsForm.Close();
			}
			this.Hide();
			notifyIcon1.Visible = false;
			await player.Dispose();
			Environment.Exit(0);
		}

		private void panelPlayBtn_MouseEnter(object sender, EventArgs e)
		{
			SetPlayPauseSize(true);
		}

		private void panelPlayBtn_MouseLeave(object sender, EventArgs e)
		{
			if (panelPlayBtn.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
				return;
			SetPlayPauseSize(false);
		}

		private void SetPlayPauseSize(bool bigger)
		{
			var scale = Settings.Get<float>("Scale");
			if (bigger)
			{
				picPlayPause.Size = new Size((int)(18 * scale), (int)(18 * scale));
				picPlayPause.Location = new Point((int)(15 * scale), (int)(15 * scale));
			}
			else
			{
				picPlayPause.Size = new Size((int)(16 * scale), (int)(16 * scale));
				picPlayPause.Location = new Point((int)(16 * scale), (int)(16 * scale));
			}
		}

		private void menuItemCopySongInfo_Click(object sender, EventArgs e)
		{
			SongInfo info = songInfoStream.currentInfo;
			Clipboard.SetText(info.song_name + " \n" + info.artist_name + " \n" + info.anime_name);
		}

		private void picSettings_Click(object sender, EventArgs e)
		{
			if (SettingsForm == null)
			{
				SettingsForm = new FormSettings(this);
				SettingsForm.Show();
			}
			else
			{
				SettingsForm.Activate();
			}
		}

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
			WindowState = FormWindowState.Normal;
			this.Show();
			this.Activate();
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

			string result = await WebHelper.Post("https://listen.moe/api/songs/favorite", Settings.Get<string>("Token"), new Dictionary<string, string>() {
				{ "song", songInfoStream.currentInfo.song_id.ToString() }
			});

			var response = Json.Parse<FavouritesResponse>(result);
			SetFavouriteSprite(response.favorite);
		}

		private void menuItemResetLocation_Click(object sender, EventArgs e)
		{
			Settings.Set("LocationX", 0);
			Settings.Set("LocationY", 0);
			Settings.WriteSettings();
			this.Location = new Point(0, 0);
		}
	}
}
