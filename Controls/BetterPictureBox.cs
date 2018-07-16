using System.Windows.Forms;

namespace ListenMoeClient
{
	class BetterPictureBox : PictureBox
	{
		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			base.OnPaint(pe);
		}
	}
}
