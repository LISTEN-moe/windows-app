using System.Drawing;
using System.Windows.Forms;

namespace ListenMoeClient.Controls
{
	class BorderedPanel : Panel
	{
		public Color BorderColor { get; set; } = Color.Black;

		private Pen borderPen = new Pen(Color.Black, 1);
		private int borderWidth = 1;
		public int BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				borderWidth = value;
				borderPen = new Pen(BorderColor, borderWidth);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawRectangle(borderPen, borderWidth / 2, borderWidth / 2, ClientRectangle.Width - borderWidth, ClientRectangle.Height - borderWidth);
		}
	}
}
