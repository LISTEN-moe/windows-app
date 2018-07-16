using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ListenMoeClient
{
	public class GhostTextbox : TextBox
	{
		[DllImport("user32.dll", EntryPoint = "SendMessageW")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		private string ghostText;
		/// <summary>
		/// The placeholder text to display in the textbox.
		/// </summary>
		public string GhostText
		{
			get { return ghostText; }
			set { ghostText = value; updateGhostText(); }
		}

		public GhostTextbox() { }

		public GhostTextbox(string ghostText)
		{
			GhostText = ghostText;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			updateGhostText();
		}

		private void updateGhostText()
		{
			if (!this.IsHandleCreated || string.IsNullOrWhiteSpace(ghostText))
				return;

			IntPtr mem = Marshal.StringToHGlobalUni(ghostText);
			SendMessage(this.Handle, 0x1501, (IntPtr)1, mem);
			Marshal.FreeHGlobal(mem);
		}
	}
}
