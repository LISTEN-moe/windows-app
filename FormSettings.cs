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
		Form1 parent;

		//Empty constructor only so the designer can be used. Do not put anything in here, it won't be called.
		public FormSettings()
		{
			InitializeComponent();
		}

		public FormSettings(Form1 parent)
		{
			InitializeComponent();
			this.Icon = Properties.Resources.icon;
			this.parent = parent;
			button1.Font = Meiryo.GetFont(11);
			textBox1.Font = Meiryo.GetFont(9);
			textBox2.Font = Meiryo.GetFont(9);
            checkBox1.Font = Meiryo.GetFont(9);
            checkBox2.Font = Meiryo.GetFont(9);
            checkBox3.Font = Meiryo.GetFont(9);
			checkBox4.Font = Meiryo.GetFont(9);
            label1.Font = Meiryo.GetFont(8);

            checkBox1.Checked = Settings.Get<bool>("TopMost");
            checkBox2.Checked = Settings.Get<bool>("IgnoreUpdates");
            checkBox3.Checked = Settings.Get<bool>("CloseToTray");
			checkBox4.Checked = Settings.Get<bool>("EnableVisualiser");

			User.AddLoginCallback(RecheckLoginStatus);
			RecheckLoginStatus();
		}

		public void RecheckLoginStatus()
		{
			if (User.LoggedIn)
			{
				var username = Settings.Get<string>("Username").Trim();
				var loginString = "Logged in as " + username;
				label1.Text = loginString;
			}
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			var response = await User.Login(textBox1.Text, textBox2.Text);
			parent.AfterLogin(response.success, response.token, textBox1.Text, response.message ?? "");
			this.Close();
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
			parent.SetTopMost(checkBox1.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Set("IgnoreUpdates", checkBox2.Checked);
            Settings.WriteSettings();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Set("CloseToTray", checkBox3.Checked);
            Settings.WriteSettings();
			parent.SetNotifyIconVisible(checkBox3.Checked);
        }

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Set("EnableVisualiser", checkBox4.Checked);
			Settings.WriteSettings();
			if (checkBox4.Checked)
				parent.StartVisualiser();
			else
				parent.StopVisualiser();
		}
	}
}
