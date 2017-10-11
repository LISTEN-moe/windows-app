namespace ListenMoeClient
{
	partial class FormSettings
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
			this.btnLogin = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.lblResolutionScaleDesc = new System.Windows.Forms.Label();
			this.lblResolutionScale = new System.Windows.Forms.Label();
			this.tbResolutionScale = new System.Windows.Forms.TrackBar();
			this.cbHideFromAltTab = new System.Windows.Forms.CheckBox();
			this.cbCloseToTray = new System.Windows.Forms.CheckBox();
			this.cbIgnoreUpdates = new System.Windows.Forms.CheckBox();
			this.cbTopmost = new System.Windows.Forms.CheckBox();
			this.tpVisualiser = new System.Windows.Forms.TabPage();
			this.panelVisualiserColor = new System.Windows.Forms.Panel();
			this.cbEnableVisualiser = new System.Windows.Forms.CheckBox();
			this.tpAccount = new System.Windows.Forms.TabPage();
			this.panelNotLoggedIn = new System.Windows.Forms.Panel();
			this.panelLoggedIn = new System.Windows.Forms.Panel();
			this.lblLoginStatus = new System.Windows.Forms.Label();
			this.btnLogout = new System.Windows.Forms.Button();
			this.cbVisualiserBars = new System.Windows.Forms.CheckBox();
			this.txtUsername = new ListenMoeClient.GhostTextbox();
			this.txtPassword = new ListenMoeClient.GhostTextbox();
			this.tbVisualiserOpacity = new System.Windows.Forms.TrackBar();
			this.lblVisualiserOpacity = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbResolutionScale)).BeginInit();
			this.tpVisualiser.SuspendLayout();
			this.tpAccount.SuspendLayout();
			this.panelNotLoggedIn.SuspendLayout();
			this.panelLoggedIn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbVisualiserOpacity)).BeginInit();
			this.SuspendLayout();
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(147, 67);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(75, 23);
			this.btnLogin.TabIndex = 0;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tpGeneral);
			this.tabControl.Controls.Add(this.tpVisualiser);
			this.tabControl.Controls.Add(this.tpAccount);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(379, 134);
			this.tabControl.TabIndex = 2;
			// 
			// tpGeneral
			// 
			this.tpGeneral.Controls.Add(this.lblResolutionScaleDesc);
			this.tpGeneral.Controls.Add(this.lblResolutionScale);
			this.tpGeneral.Controls.Add(this.tbResolutionScale);
			this.tpGeneral.Controls.Add(this.cbHideFromAltTab);
			this.tpGeneral.Controls.Add(this.cbCloseToTray);
			this.tpGeneral.Controls.Add(this.cbIgnoreUpdates);
			this.tpGeneral.Controls.Add(this.cbTopmost);
			this.tpGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpGeneral.Size = new System.Drawing.Size(371, 108);
			this.tpGeneral.TabIndex = 2;
			this.tpGeneral.Text = "General";
			this.tpGeneral.UseVisualStyleBackColor = true;
			// 
			// lblResolutionScaleDesc
			// 
			this.lblResolutionScaleDesc.AutoSize = true;
			this.lblResolutionScaleDesc.Location = new System.Drawing.Point(278, 3);
			this.lblResolutionScaleDesc.Name = "lblResolutionScaleDesc";
			this.lblResolutionScaleDesc.Size = new System.Drawing.Size(85, 13);
			this.lblResolutionScaleDesc.TabIndex = 6;
			this.lblResolutionScaleDesc.Text = "Resolution scale";
			// 
			// lblResolutionScale
			// 
			this.lblResolutionScale.AutoSize = true;
			this.lblResolutionScale.Location = new System.Drawing.Point(240, 24);
			this.lblResolutionScale.Name = "lblResolutionScale";
			this.lblResolutionScale.Size = new System.Drawing.Size(13, 13);
			this.lblResolutionScale.TabIndex = 5;
			this.lblResolutionScale.Text = "1";
			// 
			// tbResolutionScale
			// 
			this.tbResolutionScale.BackColor = System.Drawing.Color.White;
			this.tbResolutionScale.Location = new System.Drawing.Point(259, 19);
			this.tbResolutionScale.Maximum = 50;
			this.tbResolutionScale.Minimum = 10;
			this.tbResolutionScale.Name = "tbResolutionScale";
			this.tbResolutionScale.Size = new System.Drawing.Size(104, 45);
			this.tbResolutionScale.TabIndex = 4;
			this.tbResolutionScale.TickStyle = System.Windows.Forms.TickStyle.None;
			this.tbResolutionScale.Value = 10;
			this.tbResolutionScale.Scroll += new System.EventHandler(this.tbResolutionScale_Scroll);
			// 
			// cbHideFromAltTab
			// 
			this.cbHideFromAltTab.AutoSize = true;
			this.cbHideFromAltTab.Location = new System.Drawing.Point(9, 78);
			this.cbHideFromAltTab.Name = "cbHideFromAltTab";
			this.cbHideFromAltTab.Size = new System.Drawing.Size(236, 17);
			this.cbHideFromAltTab.TabIndex = 3;
			this.cbHideFromAltTab.Text = "Hide from Alt-Tab menu (requires app restart)";
			this.cbHideFromAltTab.UseVisualStyleBackColor = true;
			// 
			// cbCloseToTray
			// 
			this.cbCloseToTray.AutoSize = true;
			this.cbCloseToTray.Location = new System.Drawing.Point(9, 54);
			this.cbCloseToTray.Name = "cbCloseToTray";
			this.cbCloseToTray.Size = new System.Drawing.Size(84, 17);
			this.cbCloseToTray.TabIndex = 2;
			this.cbCloseToTray.Text = "Close to tray";
			this.cbCloseToTray.UseVisualStyleBackColor = true;
			// 
			// cbIgnoreUpdates
			// 
			this.cbIgnoreUpdates.AutoSize = true;
			this.cbIgnoreUpdates.Location = new System.Drawing.Point(9, 30);
			this.cbIgnoreUpdates.Name = "cbIgnoreUpdates";
			this.cbIgnoreUpdates.Size = new System.Drawing.Size(97, 17);
			this.cbIgnoreUpdates.TabIndex = 1;
			this.cbIgnoreUpdates.Text = "Ignore updates";
			this.cbIgnoreUpdates.UseVisualStyleBackColor = true;
			// 
			// cbTopmost
			// 
			this.cbTopmost.AutoSize = true;
			this.cbTopmost.Location = new System.Drawing.Point(9, 7);
			this.cbTopmost.Name = "cbTopmost";
			this.cbTopmost.Size = new System.Drawing.Size(117, 17);
			this.cbTopmost.TabIndex = 0;
			this.cbTopmost.Text = "Keep form top most";
			this.cbTopmost.UseVisualStyleBackColor = true;
			// 
			// tpVisualiser
			// 
			this.tpVisualiser.Controls.Add(this.lblVisualiserOpacity);
			this.tpVisualiser.Controls.Add(this.tbVisualiserOpacity);
			this.tpVisualiser.Controls.Add(this.cbVisualiserBars);
			this.tpVisualiser.Controls.Add(this.panelVisualiserColor);
			this.tpVisualiser.Controls.Add(this.cbEnableVisualiser);
			this.tpVisualiser.Location = new System.Drawing.Point(4, 22);
			this.tpVisualiser.Name = "tpVisualiser";
			this.tpVisualiser.Padding = new System.Windows.Forms.Padding(3);
			this.tpVisualiser.Size = new System.Drawing.Size(371, 108);
			this.tpVisualiser.TabIndex = 1;
			this.tpVisualiser.Text = "Visualiser";
			this.tpVisualiser.UseVisualStyleBackColor = true;
			// 
			// panelVisualiserColor
			// 
			this.panelVisualiserColor.BackColor = System.Drawing.Color.Black;
			this.panelVisualiserColor.Cursor = System.Windows.Forms.Cursors.Hand;
			this.panelVisualiserColor.Location = new System.Drawing.Point(8, 53);
			this.panelVisualiserColor.Name = "panelVisualiserColor";
			this.panelVisualiserColor.Size = new System.Drawing.Size(50, 33);
			this.panelVisualiserColor.TabIndex = 1;
			this.panelVisualiserColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelVisualiserColor_MouseClick);
			// 
			// cbEnableVisualiser
			// 
			this.cbEnableVisualiser.AutoSize = true;
			this.cbEnableVisualiser.Location = new System.Drawing.Point(8, 6);
			this.cbEnableVisualiser.Name = "cbEnableVisualiser";
			this.cbEnableVisualiser.Size = new System.Drawing.Size(105, 17);
			this.cbEnableVisualiser.TabIndex = 0;
			this.cbEnableVisualiser.Text = "Enable visualiser";
			this.cbEnableVisualiser.UseVisualStyleBackColor = true;
			// 
			// tpAccount
			// 
			this.tpAccount.Controls.Add(this.panelLoggedIn);
			this.tpAccount.Controls.Add(this.panelNotLoggedIn);
			this.tpAccount.Location = new System.Drawing.Point(4, 22);
			this.tpAccount.Name = "tpAccount";
			this.tpAccount.Padding = new System.Windows.Forms.Padding(3);
			this.tpAccount.Size = new System.Drawing.Size(371, 108);
			this.tpAccount.TabIndex = 0;
			this.tpAccount.Text = "Account";
			this.tpAccount.UseVisualStyleBackColor = true;
			// 
			// panelNotLoggedIn
			// 
			this.panelNotLoggedIn.Controls.Add(this.txtUsername);
			this.panelNotLoggedIn.Controls.Add(this.txtPassword);
			this.panelNotLoggedIn.Controls.Add(this.btnLogin);
			this.panelNotLoggedIn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelNotLoggedIn.Location = new System.Drawing.Point(3, 3);
			this.panelNotLoggedIn.Name = "panelNotLoggedIn";
			this.panelNotLoggedIn.Size = new System.Drawing.Size(365, 102);
			this.panelNotLoggedIn.TabIndex = 4;
			// 
			// panelLoggedIn
			// 
			this.panelLoggedIn.Controls.Add(this.lblLoginStatus);
			this.panelLoggedIn.Controls.Add(this.btnLogout);
			this.panelLoggedIn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLoggedIn.Location = new System.Drawing.Point(3, 3);
			this.panelLoggedIn.Name = "panelLoggedIn";
			this.panelLoggedIn.Size = new System.Drawing.Size(365, 102);
			this.panelLoggedIn.TabIndex = 4;
			// 
			// lblLoginStatus
			// 
			this.lblLoginStatus.AutoSize = true;
			this.lblLoginStatus.Location = new System.Drawing.Point(94, 32);
			this.lblLoginStatus.Name = "lblLoginStatus";
			this.lblLoginStatus.Size = new System.Drawing.Size(68, 13);
			this.lblLoginStatus.TabIndex = 1;
			this.lblLoginStatus.Text = "Logged in as";
			// 
			// btnLogout
			// 
			this.btnLogout.Location = new System.Drawing.Point(149, 67);
			this.btnLogout.Name = "btnLogout";
			this.btnLogout.Size = new System.Drawing.Size(75, 23);
			this.btnLogout.TabIndex = 0;
			this.btnLogout.Text = "Logout";
			this.btnLogout.UseVisualStyleBackColor = true;
			this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
			// 
			// cbVisualiserBars
			// 
			this.cbVisualiserBars.AutoSize = true;
			this.cbVisualiserBars.Location = new System.Drawing.Point(8, 29);
			this.cbVisualiserBars.Name = "cbVisualiserBars";
			this.cbVisualiserBars.Size = new System.Drawing.Size(68, 17);
			this.cbVisualiserBars.TabIndex = 2;
			this.cbVisualiserBars.Text = "Use bars";
			this.cbVisualiserBars.UseVisualStyleBackColor = true;
			// 
			// txtUsername
			// 
			this.txtUsername.GhostText = "Username";
			this.txtUsername.Location = new System.Drawing.Point(94, 15);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(181, 20);
			this.txtUsername.TabIndex = 2;
			// 
			// txtPassword
			// 
			this.txtPassword.GhostText = "Password";
			this.txtPassword.Location = new System.Drawing.Point(94, 41);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '⚫';
			this.txtPassword.Size = new System.Drawing.Size(181, 20);
			this.txtPassword.TabIndex = 3;
			this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
			// 
			// tbVisualiserOpacity
			// 
			this.tbVisualiserOpacity.BackColor = System.Drawing.Color.White;
			this.tbVisualiserOpacity.Location = new System.Drawing.Point(64, 52);
			this.tbVisualiserOpacity.Maximum = 255;
			this.tbVisualiserOpacity.Name = "tbVisualiserOpacity";
			this.tbVisualiserOpacity.Size = new System.Drawing.Size(104, 45);
			this.tbVisualiserOpacity.TabIndex = 3;
			this.tbVisualiserOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
			this.tbVisualiserOpacity.Value = 1;
			this.tbVisualiserOpacity.Scroll += new System.EventHandler(this.tbVisualiserOpacity_Scroll);
			// 
			// lblVisualiserOpacity
			// 
			this.lblVisualiserOpacity.AutoSize = true;
			this.lblVisualiserOpacity.Location = new System.Drawing.Point(95, 78);
			this.lblVisualiserOpacity.Name = "lblVisualiserOpacity";
			this.lblVisualiserOpacity.Size = new System.Drawing.Size(43, 13);
			this.lblVisualiserOpacity.TabIndex = 4;
			this.lblVisualiserOpacity.Text = "Opacity";
			// 
			// FormSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(379, 134);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSettings";
			this.Text = "FormSettings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
			this.tabControl.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tpGeneral.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbResolutionScale)).EndInit();
			this.tpVisualiser.ResumeLayout(false);
			this.tpVisualiser.PerformLayout();
			this.tpAccount.ResumeLayout(false);
			this.panelNotLoggedIn.ResumeLayout(false);
			this.panelNotLoggedIn.PerformLayout();
			this.panelLoggedIn.ResumeLayout(false);
			this.panelLoggedIn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbVisualiserOpacity)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tpAccount;
		private System.Windows.Forms.TabPage tpVisualiser;
		private System.Windows.Forms.TabPage tpGeneral;
		private System.Windows.Forms.CheckBox cbTopmost;
		private System.Windows.Forms.CheckBox cbIgnoreUpdates;
		private System.Windows.Forms.CheckBox cbCloseToTray;
		private System.Windows.Forms.CheckBox cbHideFromAltTab;
		private System.Windows.Forms.CheckBox cbEnableVisualiser;
		private System.Windows.Forms.Panel panelVisualiserColor;
		private System.Windows.Forms.TrackBar tbResolutionScale;
		private System.Windows.Forms.Label lblResolutionScaleDesc;
		private System.Windows.Forms.Label lblResolutionScale;
		private GhostTextbox txtUsername;
		private GhostTextbox txtPassword;
		private System.Windows.Forms.Panel panelNotLoggedIn;
		private System.Windows.Forms.Panel panelLoggedIn;
		private System.Windows.Forms.Label lblLoginStatus;
		private System.Windows.Forms.Button btnLogout;
		private System.Windows.Forms.CheckBox cbVisualiserBars;
		private System.Windows.Forms.TrackBar tbVisualiserOpacity;
		private System.Windows.Forms.Label lblVisualiserOpacity;
	}
}