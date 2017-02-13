using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	class Sprite
	{
		public Image[] Frames;
	}

	class SpriteLoader
	{
		public static Sprite LoadFavSprite()
		{
			Bitmap sheet = Properties.Resources.fav_sprite;

			Sprite result = new Sprite();
			result.Frames = new Image[sheet.Width / 64];
			//Split into 64x64
			for (int i = 0; i < sheet.Width / 64; i++)
			{
				Bitmap bitmap = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					ImageAttributes attr = new ImageAttributes();
					g.DrawImage(sheet, new Rectangle(0, 0, 64, 64), new Rectangle(64 * i, 0, 64, 64), GraphicsUnit.Pixel);
				}
				result.Frames[i] = bitmap;
			}

			return result;
		}
	}
}
