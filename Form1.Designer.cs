namespace ListenMoeClient
{
	partial class Form1
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.picPlayPause = new ListenMoeClient.BetterPictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.picFavourite = new ListenMoeClient.BetterPictureBox();
			this.picClose = new ListenMoeClient.BetterPictureBox();
			this.picLogin = new ListenMoeClient.BetterPictureBox();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItemPlayPause = new System.Windows.Forms.MenuItem();
			this.menuItemShow = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLogin)).BeginInit();
			this.SuspendLayout();
			// 
			// lblVol
			// 
			this.lblVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVol.AutoSize = true;
			this.lblVol.BackColor = System.Drawing.Color.Transparent;
			this.lblVol.ForeColor = System.Drawing.Color.White;
			this.lblVol.Location = new System.Drawing.Point(40, 31);
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
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.panel1.Controls.Add(this.picPlayPause);
			this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(48, 48);
			this.panel1.TabIndex = 5;
			this.panel1.Click += new System.EventHandler(this.playPause_Click);
			this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
			this.panel1.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
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
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
			this.panel2.Controls.Add(this.picFavourite);
			this.panel2.Controls.Add(this.picClose);
			this.panel2.Controls.Add(this.picLogin);
			this.panel2.Controls.Add(this.lblVol);
			this.panel2.Location = new System.Drawing.Point(385, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(75, 48);
			this.panel2.TabIndex = 8;
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
			// picLogin
			// 
			this.picLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picLogin.BackColor = System.Drawing.Color.Transparent;
			this.picLogin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picLogin.Image = global::ListenMoeClient.Properties.Resources.up;
			this.picLogin.Location = new System.Drawing.Point(39, 5);
			this.picLogin.Name = "picLogin";
			this.picLogin.Size = new System.Drawing.Size(12, 12);
			this.picLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picLogin.TabIndex = 6;
			this.picLogin.TabStop = false;
			this.picLogin.Click += new System.EventHandler(this.picLogin_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "Listen.moe";
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.ClientSize = new System.Drawing.Size(460, 48);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Listen.moe";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFavourite)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLogin)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BetterPictureBox picPlayPause;
		private BetterPictureBox picClose;
        private System.Windows.Forms.Label lblVol;
        private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuItem menuItemCopySongInfo;
		private BetterPictureBox picLogin;
		private BetterPictureBox picFavourite;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemShow;
		private System.Windows.Forms.MenuItem menuItemPlayPause;
	}
}

