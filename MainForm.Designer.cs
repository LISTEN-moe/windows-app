namespace ListenMoeClient
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemCopySongInfo = new System.Windows.Forms.MenuItem();
			this.menuItemOptions = new System.Windows.Forms.MenuItem();
			this.menuItem_Close = new System.Windows.Forms.MenuItem();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItemPlayPause = new System.Windows.Forms.MenuItem();
			this.menuItemShow = new System.Windows.Forms.MenuItem();
			this.menuItemResetLocation = new System.Windows.Forms.MenuItem();
			this.menuItemClose2 = new System.Windows.Forms.MenuItem();
			this.gridPanel = new CsGrid.GridPanel();
			this.panelPlayBtn = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.lblVol = new System.Windows.Forms.Label();
			this.picPlayPause = new ListenMoeClient.BetterPictureBox();
			this.centerPanel = new ListenMoeClient.CenterPanel();
			this.coverImage = new ListenMoeClient.BetterPictureBox();
			this.picFavourite = new ListenMoeClient.BetterPictureBox();
			this.gridPanel.SuspendLayout();
			this.panelPlayBtn.SuspendLayout();
			this.panelRight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).BeginInit();
			this.centerPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.coverImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).BeginInit();
			this.SuspendLayout();
			//
			// contextMenu1
			//
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopySongInfo,
            this.menuItemOptions,
            this.menuItem_Close});
			//
			// menuItemCopySongInfo
			//
			this.menuItemCopySongInfo.Index = 0;
			this.menuItemCopySongInfo.Text = "Copy song info";
			this.menuItemCopySongInfo.Click += new System.EventHandler(this.menuItemCopySongInfo_Click);
			//
			// menuItemOptions
			//
			this.menuItemOptions.Index = 1;
			this.menuItemOptions.Text = "Options";
			this.menuItemOptions.Click += new System.EventHandler(this.menuItemOptions_Click);
			//
			// menuItem_Close
			//
			this.menuItem_Close.Index = 2;
			this.menuItem_Close.Text = "Close";
			this.menuItem_Close.Click += new System.EventHandler(this.menuItemClose_Click);
			//
			// notifyIcon1
			//
			this.notifyIcon1.Text = "Listen.moe";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			//
			// contextMenu2
			//
			this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPlayPause,
            this.menuItemShow,
            this.menuItemResetLocation,
            this.menuItemClose2});
			//
			// menuItemPlayPause
			//
			this.menuItemPlayPause.Index = 0;
			this.menuItemPlayPause.Text = "Pause";
			this.menuItemPlayPause.Click += new System.EventHandler(this.playPause_Click);
			//
			// menuItemShow
			//
			this.menuItemShow.Index = 1;
			this.menuItemShow.Text = "Show";
			this.menuItemShow.Click += new System.EventHandler(this.menuItemShow_Click);
			//
			// menuItemResetLocation
			//
			this.menuItemResetLocation.Index = 2;
			this.menuItemResetLocation.Text = "Reset location";
			this.menuItemResetLocation.Click += new System.EventHandler(this.menuItemResetLocation_Click);
			//
			// menuItemClose2
			//
			this.menuItemClose2.Index = 3;
			this.menuItemClose2.Text = "Close";
			this.menuItemClose2.Click += new System.EventHandler(this.menuItemClose2_Click);
			//
			// gridPanel
			//
			this.gridPanel.Controls.Add(this.panelPlayBtn);
			this.gridPanel.Controls.Add(this.panelRight);
			this.gridPanel.Controls.Add(this.centerPanel);
			this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridPanel.Location = new System.Drawing.Point(0, 0);
			this.gridPanel.Name = "gridPanel";
			this.gridPanel.RenderInvalidControls = false;
			this.gridPanel.Size = new System.Drawing.Size(591, 294);
			this.gridPanel.TabIndex = 10;
			//
			// panelPlayBtn
			//
			this.panelPlayBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.panelPlayBtn.Controls.Add(this.picPlayPause);
			this.panelPlayBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panelPlayBtn.Location = new System.Drawing.Point(12, 78);
			this.panelPlayBtn.Name = "panelPlayBtn";
			this.panelPlayBtn.Size = new System.Drawing.Size(64, 64);
			this.panelPlayBtn.TabIndex = 5;
			this.panelPlayBtn.Tag = "playPause";
			this.panelPlayBtn.Click += new System.EventHandler(this.playPause_Click);
			this.panelPlayBtn.MouseEnter += new System.EventHandler(this.panelPlayBtn_MouseEnter);
			this.panelPlayBtn.MouseLeave += new System.EventHandler(this.panelPlayBtn_MouseLeave);
			this.panelPlayBtn.Resize += new System.EventHandler(this.panelPlayBtn_Resize);
			//
			// panelRight
			//
			this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
			this.panelRight.Controls.Add(this.lblVol);
			this.panelRight.Location = new System.Drawing.Point(507, 78);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(64, 64);
			this.panelRight.TabIndex = 8;
			this.panelRight.Tag = "rightPanel";
			//
			// lblVol
			//
			this.lblVol.BackColor = System.Drawing.Color.Transparent;
			this.lblVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lblVol.ForeColor = System.Drawing.Color.White;
			this.lblVol.Location = new System.Drawing.Point(0, 0);
			this.lblVol.Name = "lblVol";
			this.lblVol.Size = new System.Drawing.Size(64, 64);
			this.lblVol.TabIndex = 4;
			this.lblVol.Text = "100%";
			this.lblVol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// picPlayPause
			//
			this.picPlayPause.BackColor = System.Drawing.Color.Transparent;
			this.picPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPlayPause.Image = global::ListenMoeClient.Properties.Resources.pause;
			this.picPlayPause.Location = new System.Drawing.Point(20, 20);
			this.picPlayPause.Name = "picPlayPause";
			this.picPlayPause.Size = new System.Drawing.Size(24, 24);
			this.picPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPlayPause.TabIndex = 0;
			this.picPlayPause.TabStop = false;
			this.picPlayPause.Click += new System.EventHandler(this.playPause_Click);
			//
			// centerPanel
			//
			this.centerPanel.Controls.Add(this.coverImage);
			this.centerPanel.Controls.Add(this.picFavourite);
			this.centerPanel.Location = new System.Drawing.Point(97, 90);
			this.centerPanel.Name = "centerPanel";
			this.centerPanel.Size = new System.Drawing.Size(354, 48);
			this.centerPanel.TabIndex = 9;
			this.centerPanel.Tag = "centerPanel";
			this.centerPanel.Resize += new System.EventHandler(this.centerPanel_Resize);
			//
			// coverImage
			//
			this.coverImage.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.coverImage.BackColor = System.Drawing.Color.Transparent;
			this.coverImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.coverImage.Location = new System.Drawing.Point(312, -11);
			this.coverImage.Name = "coverImage";
			this.coverImage.Size = new System.Drawing.Size(64, 64);
			this.coverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.coverImage.TabIndex = 8;
			this.coverImage.TabStop = false;
			this.coverImage.Click += new System.EventHandler(this.coverImage_Click);
			//
			// picFavourite
			//
			this.picFavourite.BackColor = System.Drawing.Color.Transparent;
			this.picFavourite.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picFavourite.Location = new System.Drawing.Point(149, 0);
			this.picFavourite.Name = "picFavourite";
			this.picFavourite.Size = new System.Drawing.Size(48, 48);
			this.picFavourite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picFavourite.TabIndex = 7;
			this.picFavourite.TabStop = false;
			this.picFavourite.Visible = false;
			this.picFavourite.Click += new System.EventHandler(this.picFavourite_Click);
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.ClientSize = new System.Drawing.Size(591, 294);
			this.Controls.Add(this.gridPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Listen.moe";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.gridPanel.ResumeLayout(false);
			this.panelPlayBtn.ResumeLayout(false);
			this.panelRight.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).EndInit();
			this.centerPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.coverImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BetterPictureBox picPlayPause;
		private System.Windows.Forms.Label lblVol;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.Panel panelPlayBtn;
		private System.Windows.Forms.MenuItem menuItemCopySongInfo;
		private BetterPictureBox picFavourite;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItemShow;
		private System.Windows.Forms.MenuItem menuItemPlayPause;
		private System.Windows.Forms.MenuItem menuItemResetLocation;
		private CenterPanel centerPanel;
		private CsGrid.GridPanel gridPanel;
		private BetterPictureBox coverImage;
		private System.Windows.Forms.MenuItem menuItemOptions;
		private System.Windows.Forms.MenuItem menuItem_Close;
		private System.Windows.Forms.MenuItem menuItemClose2;
	}
}

