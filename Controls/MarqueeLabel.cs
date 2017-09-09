using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
    class MarqueeLabel
    {
        public float ScrollSpeed { get; set; } = 50; //In pixels per second
		private string text = "";
		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				OnTextChanged();
			}
		}

		private string subtext = "";
		public string Subtext
		{
			get { return subtext; }
			set
			{
				subtext = value;
				OnTextChanged();
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

		public Rectangle Bounds;
		public Font Font;
		public Font Subfont;

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

        public void OnTextChanged()
        {
			textChanged = true;
        }

		public void Render(Graphics g)
        {
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

			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			UpdateTextPosition();

			var scale = Settings.Get<float>("Scale");

			g.TranslateTransform((int)currentPosition, 0);
            RectangleF rect = new RectangleF(new PointF(Bounds.Location.X * scale, Bounds.Location.Y * scale), new SizeF(totalStringWidth, Bounds.Height));
			g.DrawString(text, Font, Brushes.White, rect.Location);
			if (subtext.Trim() != "")
			{
				g.DrawString(subtext, Subfont, Brushes.White, new PointF(rect.Location.X + mainTextSize.Width + subtextDistance * scale, rect.Location.Y + ((mainTextSize.Height - subTextSize.Height) / 2)));
			}

            if (scrolling)
            {
				//Draw it on the other side for seamless looping
				float secondPosition = totalStringWidth + Bounds.Width * spacing;
				g.TranslateTransform(secondPosition, 0);
                g.DrawString(text, Font, Brushes.White, rect.Location);
				if (subtext.Trim() != "")
				{
					g.DrawString(subtext, Subfont, Brushes.White, new PointF(rect.Location.X + mainTextSize.Width + subtextDistance * scale, rect.Location.Y + ((mainTextSize.Height - subTextSize.Height) / 2)));
				}

				g.TranslateTransform(-secondPosition, 0);
			}

			g.TranslateTransform(-(int)currentPosition, 0);
        }
    }
}
