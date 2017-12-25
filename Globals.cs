using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ListenMoeClient
{
	static class Globals
	{
		public static string VERSION = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.')); //Strip build number
		public static string USER_AGENT = "LISTEN.moe Desktop Client v" + VERSION + " (https://github.com/anonymousthing/ListenMoeClient)";
		public static int SAMPLE_RATE = 48000;

		static Random r = new Random();

		public static Point Subtract(this Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}

		public static float Bound(this float f, float min, float max)
		{
			return Math.Max(Math.Min(f, max), min);
		}

		public static byte BoundToByte(this float f)
		{
			return (byte)(Math.Min(Math.Max(0, f), 255));
		}

		public static Color Scale(this Color color, float multiplier)
		{
			return Color.FromArgb(
				(color.R * multiplier).BoundToByte(),
				(color.G * multiplier).BoundToByte(),
				(color.B * multiplier).BoundToByte()
			);
		}

		public static Rectangle Scale(this Rectangle r, float f)
		{
			return new Rectangle((int)(r.X * f), (int)(r.Y * f), (int)(r.Width * f), (int)(r.Height * f));
		}

		public static Color RandomColor()
		{
			return Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
		}

		public static Rectangle ToRectangle(this RectangleF r)
		{
			return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
		}

		public static Point ToPoint(this PointF p)
		{
			return new Point((int)p.X, (int)p.Y);
		}

		static Dictionary<Control, Rectangle> originalRect = new Dictionary<Control, Rectangle>();
		static Dictionary<Control, Size> originalMinSize = new Dictionary<Control, Size>();
		public static void BetterScale(this Control c, float f)
		{
			if (!originalRect.ContainsKey(c))
			{
				originalRect[c] = new Rectangle(c.Location.X, c.Location.Y, c.Width, c.Height);
				originalMinSize[c] = c.MinimumSize;
			}

			Size origMinSize = originalMinSize[c];
			c.MinimumSize = new SizeF(origMinSize.Width * f, origMinSize.Height * f).ToSize();

			Rectangle origRect = originalRect[c];
			c.Width = (int)(origRect.Width * f);
			c.Height = (int)(origRect.Height * f);
			if (!(c is Form))
				c.Location = new PointF(origRect.X * f, origRect.Y * f).ToPoint();

			//Scale children
			foreach (Control c2 in c.Controls)
				BetterScale(c2, f);
		}

		public static void ResetScale(this Control c)
		{
			if (originalRect.ContainsKey(c))
			{
				c.MinimumSize = originalMinSize[c];

				Rectangle origRect = originalRect[c];
				c.Width = origRect.Width;
				c.Height = origRect.Height;
				c.Location = origRect.Location;


				//Reset children
				foreach (Control c2 in c.Controls)
					ResetScale(c2);
			}
		}
	}
}
