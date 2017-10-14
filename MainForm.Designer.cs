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
			this.lblVol = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemCopySongInfo = new System.Windows.Forms.MenuItem();
			this.panelPlayBtn = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItemPlayPause = new System.Windows.Forms.MenuItem();
			this.menuItemShow = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.picFavourite = new ListenMoeClient.BetterPictureBox();
			this.picClose = new ListenMoeClient.BetterPictureBox();
			this.picSettings = new ListenMoeClient.BetterPictureBox();
			this.picPlayPause = new ListenMoeClient.BetterPictureBox();
			this.panelPlayBtn.SuspendLayout();
			this.panelRight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picSettings)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).BeginInit();
			this.SuspendLayout();
			// 
			// lblVol
			// 
			this.lblVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVol.AutoSize = true;
			this.lblVol.BackColor = System.Drawing.Color.Transparent;
			this.lblVol.ForeColor = System.Drawing.Color.White;
			this.lblVol.Location = new System.Drawing.Point(35, 31);
			this.lblVol.Name = "lblVol";
			this.lblVol.Size = new System.Drawing.Size(33, 13);
			this.lblVol.TabIndex = 4;
			this.lblVol.Text = "100%";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopySongInfo});
			// 
			// menuItemCopySongInfo
			// 
			this.menuItemCopySongInfo.Index = 0;
			this.menuItemCopySongInfo.Text = "Copy song info";
			this.menuItemCopySongInfo.Click += new System.EventHandler(this.menuItemCopySongInfo_Click);
			// 
			// panelPlayBtn
			// 
			this.panelPlayBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panelPlayBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.panelPlayBtn.Controls.Add(this.picPlayPause);
			this.panelPlayBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panelPlayBtn.Location = new System.Drawing.Point(0, 0);
			this.panelPlayBtn.Name = "panelPlayBtn";
			this.panelPlayBtn.Size = new System.Drawing.Size(48, 48);
			this.panelPlayBtn.TabIndex = 5;
			this.panelPlayBtn.Click += new System.EventHandler(this.playPause_Click);
			this.panelPlayBtn.MouseEnter += new System.EventHandler(this.panelPlayBtn_MouseEnter);
			this.panelPlayBtn.MouseLeave += new System.EventHandler(this.panelPlayBtn_MouseLeave);
			// 
			// panelRight
			// 
			this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
			this.panelRight.Controls.Add(this.picFavourite);
			this.panelRight.Controls.Add(this.picClose);
			this.panelRight.Controls.Add(this.picSettings);
			this.panelRight.Controls.Add(this.lblVol);
			this.panelRight.Location = new System.Drawing.Point(385, 0);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(75, 48);
			this.panelRight.TabIndex = 8;
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
            this.menuItemExit});
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
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Text = "Exit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// picFavourite
			// 
			this.picFavourite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.picFavourite.BackColor = System.Drawing.Color.Transparent;
			this.picFavourite.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picFavourite.Location = new System.Drawing.Point(4, 8);
			this.picFavourite.Name = "picFavourite";
			this.picFavourite.Size = new System.Drawing.Size(32, 32);
			this.picFavourite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picFavourite.TabIndex = 7;
			this.picFavourite.TabStop = false;
			this.picFavourite.Visible = false;
			this.picFavourite.Click += new System.EventHandler(this.picFavourite_Click);
			// 
			// picClose
			// 
			this.picClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picClose.BackColor = System.Drawing.Color.Transparent;
			this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picClose.Image = global::ListenMoeClient.Properties.Resources.close;
			this.picClose.Location = new System.Drawing.Point(57, 5);
			this.picClose.Name = "picClose";
			this.picClose.Size = new System.Drawing.Size(12, 12);
			this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picClose.TabIndex = 2;
			this.picClose.TabStop = false;
			this.picClose.Click += new System.EventHandler(this.picClose_Click);
			// 
			// picSettings
			// 
			this.picSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picSettings.BackColor = System.Drawing.Color.Transparent;
			this.picSettings.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picSettings.Image = global::ListenMoeClient.Properties.Resources.up;
			this.picSettings.Location = new System.Drawing.Point(39, 5);
			this.picSettings.Name = "picSettings";
			this.picSettings.Size = new System.Drawing.Size(12, 12);
			this.picSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picSettings.TabIndex = 6;
			this.picSettings.TabStop = false;
			this.picSettings.Click += new System.EventHandler(this.picSettings_Click);
			// 
			// picPlayPause
			// 
			this.picPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.picPlayPause.BackColor = System.Drawing.Color.Transparent;
			this.picPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPlayPause.Image = global::ListenMoeClient.Properties.Resources.pause;
			this.picPlayPause.Location = new System.Drawing.Point(16, 16);
			this.picPlayPause.Name = "picPlayPause";
			this.picPlayPause.Size = new System.Drawing.Size(16, 16);
			this.picPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPlayPause.TabIndex = 0;
			this.picPlayPause.TabStop = false;
			this.picPlayPause.Click += new System.EventHandler(this.playPause_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.ClientSize = new System.Drawing.Size(460, 48);
			this.Controls.Add(this.panelRight);
			this.Controls.Add(this.panelPlayBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Listen.moe";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.panelPlayBtn.ResumeLayout(false);
			this.panelRight.ResumeLayout(false);
			this.panelRight.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSettings)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BetterPictureBox picPlayPause;
		private BetterPictureBox picClose;
        private System.Windows.Forms.Label lblVol;
        private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.Panel panelPlayBtn;
		private System.Windows.Forms.MenuItem menuItemCopySongInfo;
		private BetterPictureBox picSettings;
		private BetterPictureBox picFavourite;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemShow;
		private System.Windows.Forms.MenuItem menuItemPlayPause;
	}
}

