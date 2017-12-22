using System;
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

		public static Color RandomColor()
		{
			return Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
		}
	}
}
