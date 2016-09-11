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
			this.components = new System.ComponentModel.Container();
			this.lblTitle = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.lblArtist = new System.Windows.Forms.Label();
			this.picClose = new BetterPictureBox();
			this.picPlayPause = new BetterPictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).BeginInit();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(33, 4);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(27, 13);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "Title";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 5000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblArtist
			// 
			this.lblArtist.AutoSize = true;
			this.lblArtist.ForeColor = System.Drawing.Color.White;
			this.lblArtist.Location = new System.Drawing.Point(34, 26);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.Size = new System.Drawing.Size(30, 13);
			this.lblArtist.TabIndex = 3;
			this.lblArtist.Text = "Artist";
			// 
			// picClose
			// 
			this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picClose.Image = global::CrappyListenMoe.Properties.Resources.close;
			this.picClose.Location = new System.Drawing.Point(364, 16);
			this.picClose.Name = "picClose";
			this.picClose.Size = new System.Drawing.Size(16, 16);
			this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picClose.TabIndex = 2;
			this.picClose.TabStop = false;
			this.picClose.Click += new System.EventHandler(this.picClose_Click);
			// 
			// picPlayPause
			// 
			this.picPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPlayPause.Image = global::CrappyListenMoe.Properties.Resources.pause;
			this.picPlayPause.Location = new System.Drawing.Point(12, 16);
			this.picPlayPause.Name = "picPlayPause";
			this.picPlayPause.Size = new System.Drawing.Size(16, 16);
			this.picPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPlayPause.TabIndex = 0;
			this.picPlayPause.TabStop = false;
			this.picPlayPause.Click += new System.EventHandler(this.picPlayPause_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(26)))), ((int)(((byte)(85)))));
			this.ClientSize = new System.Drawing.Size(400, 48);
			this.Controls.Add(this.lblArtist);
			this.Controls.Add(this.picClose);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.picPlayPause);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Listen.moe";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPlayPause)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BetterPictureBox picPlayPause;
		private System.Windows.Forms.Label lblTitle;
		private BetterPictureBox picClose;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblArtist;
	}
}

