using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	public partial class FormSettings : Form
	{
		MainForm mainForm;

		public FormSettings(MainForm mainForm)
		{
			InitializeComponent();
			this.Icon = Properties.Resources.icon;
			this.mainForm = mainForm;

			LoadAndBindCheckboxSetting(cbCloseToTray, "CloseToTray");
			LoadAndBindCheckboxSetting(cbEnableVisualiser, "EnableVisualiser");
			LoadAndBindCheckboxSetting(cbHideFromAltTab, "HideFromAltTab");
			LoadAndBindCheckboxSetting(cbIgnoreUpdates, "IgnoreUpdates");
			LoadAndBindCheckboxSetting(cbTopmost, "TopMost");
			LoadAndBindCheckboxSetting(cbVisualiserBars, "VisualiserBars");

			float scale = Settings.Get<float>("Scale");
			tbResolutionScale.Value = (int)(scale * 10);
			lblResolutionScale.Text = scale.ToString("N1");

			tbVisualiserOpacity.Value = (int)(Settings.Get<float>("VisualiserTransparency") * 255);

			panelVisualiserColor.BackColor = Settings.GetVisualiserColor();

			panelNotLoggedIn.Visible = !User.LoggedIn;
			panelLoggedIn.Visible = User.LoggedIn;
			lblLoginStatus.Text = "Logged in as " + Settings.Get<string>("Username");
			lblLoginStatus.Location = new Point((this.Width / 2) - (lblLoginStatus.Width / 2), lblLoginStatus.Location.Y);

			User.OnLoginComplete += () =>
			{
				lblLoginStatus.Text = "Logged in as " + Settings.Get<string>("Username");
				lblLoginStatus.Location = new Point((this.Width / 2) - (lblLoginStatus.Width / 2), lblLoginStatus.Location.Y);
				txtUsername.Clear();
				txtPassword.Clear();
				panelNotLoggedIn.Visible = false;
				panelLoggedIn.Visible = true;
				panelLoggedIn.BringToFront();
			};
			User.OnLogout += () =>
			{
				panelLoggedIn.Visible = false;
				panelNotLoggedIn.Visible = true;
				panelNotLoggedIn.BringToFront();
			};
		}

		private void LoadAndBindCheckboxSetting(CheckBox checkbox, string settingsKey)
		{
			checkbox.Checked = Settings.Get<bool>(settingsKey);
			checkbox.CheckStateChanged += (sender, e) =>
			{
				Settings.Set(settingsKey, checkbox.Checked);
				Settings.WriteSettings();
				mainForm.ReloadSettings();
			};
		}

		private void panelVisualiserColor_MouseClick(object sender, MouseEventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Color c = dialog.Color;
				panelVisualiserColor.BackColor = c;

				string hexColor = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
				Settings.Set("VisualiserColor", hexColor);
				Settings.WriteSettings();

				mainForm.ReloadSettings();
			}
		}

		private void tbResolutionScale_Scroll(object sender, EventArgs e)
		{
			//Set new scale
			float newScale = tbResolutionScale.Value / 10f;
			lblResolutionScale.Text = newScale.ToString("N1");
			Settings.Set("Scale", newScale);
			Settings.WriteSettings();

			//Reload form scaling
			mainForm.ReloadScale();
		}

		private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			//Should probably use a mutex for this, but oh well
			mainForm.SettingsForm = null;
		}

		private async void btnLogin_Click(object sender, EventArgs e)
		{
			btnLogin.Enabled = false;
			await User.Login(txtUsername.Text, txtPassword.Text);
			btnLogin.Enabled = true;
		}

		private void btnLogout_Click(object sender, EventArgs e)
		{
			User.Logout();
		}

		private void txtPassword_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				btnLogin.PerformClick();
		}

		private void tbVisualiserOpacity_Scroll(object sender, EventArgs e)
		{
			Settings.Set("VisualiserTransparency", tbVisualiserOpacity.Value / 255f);
			Settings.WriteSettings();
			mainForm.ReloadSettings();
		}
	}
}
