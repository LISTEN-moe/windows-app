namespace CrappyListenMoe
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
			this.lblVol = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemTopmost = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.picPlayPause = new CrappyListenMoe.BetterPictureBox();
			this.picClose = new CrappyListenMoe.BetterPictureBox();
			this.lblArtist = new CrappyListenMoe.MarqueeLabel();
			this.lblTitle = new CrappyListenMoe.MarqueeLabel();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
			this.SuspendLayout();
			// 
			// lblVol
			// 
			this.lblVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVol.AutoSize = true;
			this.lblVol.BackColor = System.Drawing.Color.Transparent;
			this.lblVol.ForeColor = System.Drawing.Color.White;
			this.lblVol.Location = new System.Drawing.Point(366, 28);
			this.lblVol.Name = "lblVol";
			this.lblVol.Size = new System.Drawing.Size(33, 13);
			this.lblVol.TabIndex = 4;
			this.lblVol.Text = "100%";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTopmost});
			// 
			// menuItemTopmost
			// 
			this.menuItemTopmost.Index = 0;
			this.menuItemTopmost.Text = "Always on top";
			this.menuItemTopmost.Click += new System.EventHandler(this.menuItemTopmost_Click);
			// 
			// panel1
			// 
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
			this.picPlayPause.BackColor = System.Drawing.Color.Transparent;
			this.picPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPlayPause.Image = global::CrappyListenMoe.Properties.Resources.pause;
			this.picPlayPause.Location = new System.Drawing.Point(16, 16);
			this.picPlayPause.Name = "picPlayPause";
			this.picPlayPause.Size = new System.Drawing.Size(16, 16);
			this.picPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPlayPause.TabIndex = 0;
			this.picPlayPause.TabStop = false;
			this.picPlayPause.Click += new System.EventHandler(this.playPause_Click);
			// 
			// picClose
			// 
			this.picClose.BackColor = System.Drawing.Color.Transparent;
			this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picClose.Image = global::CrappyListenMoe.Properties.Resources.close;
			this.picClose.Location = new System.Drawing.Point(374, 8);
			this.picClose.Name = "picClose";
			this.picClose.Size = new System.Drawing.Size(16, 16);
			this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picClose.TabIndex = 2;
			this.picClose.TabStop = false;
			this.picClose.Click += new System.EventHandler(this.picClose_Click);
			// 
			// lblArtist
			// 
			this.lblArtist.BackColor = System.Drawing.Color.Transparent;
			this.lblArtist.ForeColor = System.Drawing.Color.White;
			this.lblArtist.Location = new System.Drawing.Point(58, 26);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.ScrollSpeed = 50F;
			this.lblArtist.Size = new System.Drawing.Size(310, 22);
			this.lblArtist.TabIndex = 3;
			this.lblArtist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(58, 5);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.ScrollSpeed = 50F;
			this.lblTitle.Size = new System.Drawing.Size(310, 43);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "(No connection)";
			this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(46)))), ((int)(((byte)(59)))));
			this.ClientSize = new System.Drawing.Size(400, 48);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblVol);
			this.Controls.Add(this.picClose);
			this.Controls.Add(this.lblArtist);
			this.Controls.Add(this.lblTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Listen.moe";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BetterPictureBox picPlayPause;
		private MarqueeLabel lblTitle;
		private BetterPictureBox picClose;
		private MarqueeLabel lblArtist;
        private System.Windows.Forms.Label lblVol;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItemTopmost;
		private System.Windows.Forms.Panel panel1;
	}
}

