using System;
using System.Drawing;
using System.Drawing.Text;

namespace ListenMoeClient
{
	class MarqueeLabel
	{
		public float ScrollSpeed { get; set; } = 50; //In pixels per second
		public bool Centered { get; set; } = false;
		private bool renderBounds = false; //For debugging
		Pen boundsPen = new Pen(new SolidBrush(Globals.RandomColor()));

		private string text = "";
		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				RecalculateBounds();
			}
		}

		private string subtext = "";
		public string Subtext
		{
			get { return subtext; }
			set
			{
				subtext = value;
				RecalculateBounds();
			}
		}

		private float currentPosition = 0;
		private float spacing = 0.7f; //As a multiple of the width of the label
		private float subtextDistance = 3; //In pixels

		private DateTime last;

		private SizeF mainTextSize;
		private SizeF subTextSize;
		private float totalStringWidth;

		bool scrolling = false;

		public Rectangle Bounds = Rectangle.Empty;
		public Font Font = new Font("Segoe UI", 9);
		public Font Subfont = new Font("Segoe UI", 8);
		public Brush FontBrush = Brushes.White;

		private bool textChanged = true;

		private void UpdateTextPosition()
		{
			DateTime current = DateTime.Now;
			double ms = (current - last).TotalMilliseconds;

			if (scrolling)
			{
				float distance = (float)(ScrollSpeed * (ms / 1000));
				currentPosition -= distance;
				if (currentPosition < -totalStringWidth)
					currentPosition = Bounds.Width * spacing;
			}

			last = current;
		}

		public void RecalculateBounds()
		{
			textChanged = true;
		}

		public void Render(Graphics g)
		{
			var scale = Settings.Get<float>(Setting.Scale);
			if (textChanged)
			{
				mainTextSize = g.MeasureString(text, Font);
				subTextSize = g.MeasureString(subtext, Subfont);

				totalStringWidth = mainTextSize.Width;
				if (subtext.Trim() != "")
				{
					totalStringWidth += subtextDistance; //Spacing between main and subtext
					totalStringWidth += subTextSize.Width;
				}
				totalStringWidth += 2; //Padding

				if (totalStringWidth > Bounds.Width)
					scrolling = true;
				else
				{
					scrolling = false;
					currentPosition = 0;
				}
				textChanged = false;
			}

			if (scale < 2)
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			else
				g.TextRenderingHint = TextRenderingHint.AntiAlias;

			UpdateTextPosition();

			g.TranslateTransform((int)currentPosition, 0);
			float x;
			if (Centered)
				x = (Bounds.Width / 2 - totalStringWidth / 2);
			else
				x = Bounds.Location.X;
			RectangleF rect = new RectangleF(new PointF(x, Bounds.Location.Y), new SizeF(totalStringWidth, Bounds.Height));

			void DrawText()
			{
				g.DrawString(text, Font, FontBrush, rect.Location);
				if (subtext.Trim() != "")
				{
					g.DrawString(subtext, Subfont, FontBrush, new PointF(x + mainTextSize.Width + subtextDistance * scale, rect.Location.Y + ((mainTextSize.Height - subTextSize.Height) / 2)));
				}
			}

			DrawText();

			if (scrolling)
			{
				//Draw it on the other side for seamless looping
				float secondPosition = x + totalStringWidth + Bounds.Width * spacing;
				g.TranslateTransform(secondPosition, 0);

				DrawText();

				g.TranslateTransform(-secondPosition, 0);
			}

			g.TranslateTransform(-(int)currentPosition, 0);

			if (renderBounds)
			{
				g.DrawRectangle(boundsPen, Bounds);
			}
		}
	}
}
