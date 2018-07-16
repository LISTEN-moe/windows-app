using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

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

			LoadAndBindColorSetting(panelVisualiserColor, "CustomVisualiserColor");
			LoadAndBindColorSetting(panelBaseColor, "CustomBaseColor");
			LoadAndBindColorSetting(panelAccentColor, "CustomAccentColor");

			numericUpdateInterval.Value = Settings.Get<int>(Setting.UpdateInterval) / 60;
			numericUpdateInterval.ValueChanged += NumericUpdateInterval_ValueChanged;

			float scale = Settings.Get<float>(Setting.Scale);
			tbResolutionScale.Value = (int)(scale * 10);
			lblResolutionScale.Text = scale.ToString("N1");

			float visualiserOpacity = Settings.Get<float>(Setting.VisualiserOpacity);
			tbVisualiserOpacity.Value = (int)(visualiserOpacity * 255);
			lblVisualiserOpacity.Text = visualiserOpacity.ToString("N1");

			float opacity = Settings.Get<float>(Setting.FormOpacity);
			tbOpacity.Value = (int)(opacity * 255);
			lblOpacity.Text = opacity.ToString("N1");

			panelNotLoggedIn.Visible = !User.LoggedIn;
			panelLoggedIn.Visible = User.LoggedIn;
			lblLoginStatus.Text = "Logged in as " + Settings.Get<string>(Setting.Username);
			lblLoginStatus.Location = new Point((this.Width / 2) - (lblLoginStatus.Width / 2), lblLoginStatus.Location.Y);

			StreamType st = Settings.Get<StreamType>(Setting.StreamType);
			rbKpop.Checked = st == StreamType.Kpop;

			User.OnLoginComplete += () =>
			{
				lblLoginStatus.Text = "Logged in as " + Settings.Get<string>(Setting.Username);
				lblLoginStatus.Location = new Point((this.Width / 2) - (lblLoginStatus.Width / 2), lblLoginStatus.Location.Y);
				txtUsername.Clear();
				txtPassword.Clear();
				panelNotLoggedIn.Visible = false;
				panelTwoFactorAuth.Visible = false;
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
			Settings.Set(Setting.UpdateInterval, (int)numericUpdateInterval.Value * 60);
			Settings.WriteSettings();
		}

		private void LoadAndBindCheckboxSetting(CheckBox checkbox, string settingsKey)
		{
			Setting key = (Setting)Enum.Parse(typeof(Setting), settingsKey);
			checkbox.Checked = Settings.Get<bool>(key);
			checkbox.CheckStateChanged += (sender, e) =>
			{
				Settings.Set(key, checkbox.Checked);
				Settings.WriteSettings();
				mainForm.ReloadSettings();
			};
		}

		private void LoadAndBindColorSetting(Panel panel, string settingsKey)
		{
			Setting key = (Setting)Enum.Parse(typeof(Setting), settingsKey);
			panel.BackColor = Settings.Get<Color>(key);
			panel.MouseClick += (sender, e) =>
			{
				ColorDialog dialog = new ColorDialog();
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Color c = dialog.Color;
					panel.BackColor = c;

					Settings.Set(key, c);
					Settings.Set(Setting.CustomColors, true);
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
			Settings.Set(Setting.Scale, newScale);
			Settings.WriteSettings();

			//Reload form scaling
			mainForm.ReloadScale();
		}

		private void FormSettings_FormClosing(object sender, FormClosingEventArgs e) =>
			//Should probably use a mutex for this, but oh well
			mainForm.SettingsForm = null;

		private async void btnLogin_Click(object sender, EventArgs e)
		{
			btnLogin.Enabled = false;
			AuthenticateResponse response = await User.Login(txtUsername.Text, txtPassword.Text);
			btnLogin.Enabled = true;
			if (response.mfa)
			{
				panelNotLoggedIn.Visible = false;
				panelLoggedIn.Visible = false;
				panelTwoFactorAuth.Visible = true;
				panelTwoFactorAuth.BringToFront();
				txtTwoFactorAuthCode.Focus();
			}
		}

		private void btnLogout_Click(object sender, EventArgs e) => User.Logout();

		private void tbVisualiserOpacity_Scroll(object sender, EventArgs e)
		{
			float newVal = tbVisualiserOpacity.Value / 255f;
			lblVisualiserOpacity.Text = newVal.ToString("N1");
			Settings.Set(Setting.VisualiserOpacity, newVal);
			Settings.WriteSettings();

			mainForm.ReloadSettings();
		}

		private void tbOpacity_Scroll(object sender, EventArgs e)
		{
			float newVal = tbOpacity.Value / 255f;
			lblOpacity.Text = newVal.ToString("N1");
			Settings.Set(Setting.FormOpacity, newVal);
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

		private void btnRefreshAudioDevices_Click(object sender, EventArgs e) => reloadAudioDevices();

		private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;

				btnLogin.PerformClick();
			}
		}

		private async void btnTwoFactorAuthSubmit_Click(object sender, EventArgs e)
		{
			btnTwoFactorAuthSubmit.Enabled = false;
			bool success = await User.LoginMfa(txtTwoFactorAuthCode.Text);
			btnTwoFactorAuthSubmit.Enabled = true;
			if (!success)
			{
				lblIncorrectTwoFactorAuth.Visible = true;
				await Task.Delay(2000);
				lblIncorrectTwoFactorAuth.Visible = false;
			}
		}

		private void txtTwoFactorAuthCode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				btnTwoFactorAuthSubmit.PerformClick();
			}
		}

		private async Task ReloadStreamType()
		{
			StreamType current = Settings.Get<StreamType>(Setting.StreamType);
			StreamType next = rbJpop.Checked ? StreamType.Jpop : StreamType.Kpop;

			if (current != next)
			{
				Settings.Set(Setting.StreamType, next);
				Settings.WriteSettings();

				mainForm.ReloadSettings();
				await mainForm.ReloadStream();
			}
		}

		private async void rbKpop_CheckedChanged(object sender, EventArgs e) => await ReloadStreamType();

		private async void rbJpop_CheckedChanged(object sender, EventArgs e) => await ReloadStreamType();
	}
}
