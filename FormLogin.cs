using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
	public partial class FormLogin : Form
	{
		Form1 parent;

		//Empty constructor only so the designer can be used. Do not put anything in here, it won't be called.
		public FormLogin()
		{
			InitializeComponent();
		}

		public FormLogin(Form1 parent)
		{
			InitializeComponent();
			this.parent = parent;
			button1.Font = OpenSans.GetFont(11);
			textBox1.Font = OpenSans.GetFont(9);
			textBox2.Font = OpenSans.GetFont(9);
            checkBox1.Font = OpenSans.GetFont(9);
            checkBox2.Font = OpenSans.GetFont(9);
            checkBox3.Font = OpenSans.GetFont(9);
            label1.Font = OpenSans.GetFont(8);

            checkBox1.Checked = Settings.GetBoolSetting("TopMost");
            checkBox2.Checked = Settings.GetBoolSetting("IgnoreUpdates");
            checkBox3.Checked = Settings.GetBoolSetting("CloseToTray");

			User.AddLoginCallback(RecheckLoginStatus);
			RecheckLoginStatus();
		}

		public void RecheckLoginStatus()
		{
			if (User.LoggedIn)
			{
				var username = Settings.GetStringSetting("Username").Trim();
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
            Settings.SetBoolSetting("IgnoreUpdates", checkBox2.Checked);
            Settings.WriteSettings();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SetBoolSetting("CloseToTray", checkBox3.Checked);
            Settings.WriteSettings();
        }
    }
}
