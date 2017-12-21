using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace ListenMoeClient
{


	public partial class FormSettings : Form
	{
		MainForm mainForm;
		AudioPlayer audioPlayer;

		public FormSettings(MainForm mainForm, AudioPlayer audioPlayer)
		{
			InitializeComponent();
			this.Icon = Properties.Resources.icon;
			this.mainForm = mainForm;
			this.audioPlayer = audioPlayer;

			LoadAndBindCheckboxSetting(cbCloseToTray, "CloseToTray");
			LoadAndBindCheckboxSetting(cbEnableVisualiser, "EnableVisualiser");
			LoadAndBindCheckboxSetting(cbHideFromAltTab, "HideFromAltTab");
			LoadAndBindCheckboxSetting(cbUpdateAutocheck, "UpdateAutocheck");
			LoadAndBindCheckboxSetting(cbThumbnailButton, "ThumbnailButton");
			LoadAndBindCheckboxSetting(cbTopmost, "TopMost");
			LoadAndBindCheckboxSetting(cbVisualiserBars, "VisualiserBars");
			LoadAndBindCheckboxSetting(cbVisualiserFadeEdges, "VisualiserFadeEdges");

			LoadAndBindColorSetting(panelVisualiserColor, "VisualiserColor");
			LoadAndBindColorSetting(panelBaseColor, "BaseColor");
			LoadAndBindColorSetting(panelAccentColor, "AccentColor");

			numericUpdateInterval.Value = Settings.Get<int>("UpdateInterval") / 60;
			numericUpdateInterval.ValueChanged += NumericUpdateInterval_ValueChanged;

			float scale = Settings.Get<float>("Scale");
			tbResolutionScale.Value = (int)(scale * 10);
			lblResolutionScale.Text = scale.ToString("N1");

			tbVisualiserOpacity.Value = (int)(Settings.Get<float>("VisualiserTransparency") * 255);
			float opacity = Settings.Get<float>("FormOpacity");
			tbOpacity.Value = (int)(opacity * 255);
			lblOpacity.Text = opacity.ToString("N1");

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
			
			reloadAudioDevices();
		}

		private void NumericUpdateInterval_ValueChanged(object sender, EventArgs e)
		{
			Settings.Set("UpdateInterval", (int)numericUpdateInterval.Value * 60);
			Settings.WriteSettings();
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

		private void LoadAndBindColorSetting(Panel panel, string settingsKey)
		{
			panel.BackColor = Settings.Get<Color>(settingsKey);
			panel.MouseClick += (sender, e) =>
			{
				ColorDialog dialog = new ColorDialog();
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Color c = dialog.Color;
					panel.BackColor = c;

					Settings.Set(settingsKey, c);
					Settings.WriteSettings();

					mainForm.ReloadSettings();
				}
			};
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

		private void tbOpacity_Scroll(object sender, EventArgs e)
		{
			float newVal = tbOpacity.Value / 255f;
			lblOpacity.Text = newVal.ToString("N1");
			Settings.Set("FormOpacity", newVal);
			Settings.WriteSettings();

			mainForm.ReloadSettings();
		}

		private void reloadAudioDevices()
		{
			dropdownAudioDevices.DataSource = audioPlayer.GetAudioOutputDevices();
			dropdownAudioDevices.SelectedIndex = Math.Max(0, Array.IndexOf(audioPlayer.GetAudioOutputDevices().Select(a => a.DeviceInfo.Guid).ToArray(), audioPlayer.CurrentDeviceGuid));
		}

		private void cbAudioDevices_SelectionChangeCommitted(object sender, EventArgs e)
		{
			AudioDevice selected = (AudioDevice)dropdownAudioDevices.SelectedItem;
			audioPlayer.SetAudioOutputDevice(selected.DeviceInfo.Guid);
		}

		private void btnRefreshAudioDevices_Click(object sender, EventArgs e)
		{
			reloadAudioDevices();
		}
	}
}
