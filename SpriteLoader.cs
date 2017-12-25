using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ListenMoeClient
{
	class Sprite
	{
		public Image[] Frames;
	}

	class SpriteLoader
	{
		public static Sprite LoadFavSprite(Bitmap sheet, int frameSize)
		{
			Sprite result = new Sprite
			{
				Frames = new Image[sheet.Width / frameSize]
			};
			//Split into frameSizexframeSize
			for (int i = 0; i < sheet.Width / frameSize; i++)
			{
				Bitmap bitmap = new Bitmap(frameSize, frameSize, PixelFormat.Format32bppArgb);
				using (Graphics g = Graphics.FromImage(bitmap))
					g.DrawImage(sheet, new Rectangle(0, 0, frameSize, frameSize), new Rectangle(frameSize * i, 0, frameSize, frameSize), GraphicsUnit.Pixel);

				result.Frames[i] = bitmap;
			}

			return result;
		}

		public static void SetAlpha(Bitmap b, byte alpha)
		{
			BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			IntPtr p = bmpData.Scan0;
			byte[] pixels = new byte[b.Width * b.Height * 4]; //4 channels
			Marshal.Copy(p, pixels, 0, pixels.Length);

			for (int i = 0; i < pixels.Length; i += 4)
			{
				if (pixels[i + 3] == 0)
					continue;
				pixels[i + 3] = alpha;
			}

			Marshal.Copy(pixels, 0, p, pixels.Length);
			b.UnlockBits(bmpData);
		}

		public static Sprite LoadFadedFavSprite(Bitmap sheet, int frameSize)
		{
			Sprite result = new Sprite
			{
				Frames = new Image[2]
			};

			int n = sheet.Width / frameSize;

			Bitmap b0 = new Bitmap(frameSize, frameSize, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(b0))
				g.DrawImage(sheet, new Rectangle(0, 0, frameSize, frameSize), new Rectangle(0, 0, frameSize, frameSize), GraphicsUnit.Pixel);

			Bitmap b1 = new Bitmap(frameSize, frameSize, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(b1))
				g.DrawImage(sheet, new Rectangle(0, 0, frameSize, frameSize), new Rectangle(frameSize * (n - 1), 0, frameSize, frameSize), GraphicsUnit.Pixel);

			SetAlpha(b0, 128);
			SetAlpha(b1, 128);

			result.Frames[0] = b0;
			result.Frames[1] = b1;

			return result;
		}

		private static Image DarkenBitmap(Image b)
		{
			Bitmap darkened = new Bitmap(b);

			ColorMatrix mat = new ColorMatrix();
			mat.Matrix00 = 0.8f;
			mat.Matrix11 = 0.8f;
			mat.Matrix22 = 0.8f;

			ImageAttributes attr = new ImageAttributes();
			attr.SetColorMatrix(mat);

			using (Graphics g = Graphics.FromImage(darkened))
			{
				g.DrawImage(b, new Rectangle(Point.Empty, b.Size), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, attr);
			}

			return darkened;
		}

		public static Sprite LoadDarkFavSprite(Bitmap sheet, int frameSize)
		{
			Sprite result = new Sprite
			{
				Frames = new Image[sheet.Width / frameSize]
			};
			//Split into frameSizexframeSize
			for (int i = 0; i < sheet.Width / frameSize; i++)
			{
				Bitmap bitmap = new Bitmap(frameSize, frameSize, PixelFormat.Format32bppArgb);
				using (Graphics g = Graphics.FromImage(bitmap))
					g.DrawImage(sheet, new Rectangle(0, 0, frameSize, frameSize), new Rectangle(frameSize * i, 0, frameSize, frameSize), GraphicsUnit.Pixel);

				result.Frames[i] = bitmap;
			}

			result.Frames[0] = DarkenBitmap(result.Frames[0]);

			return result;
		}
	}
}
