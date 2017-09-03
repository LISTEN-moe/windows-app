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
		private string text;
		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				OnTextChanged();
			}
		}

        private float currentPosition = 0;
        private float spacing = 0.7f; //As a multiple of the width of the label

		private DateTime last;

        private float stringWidth;

        bool scrolling = false;

		public Rectangle Bounds;
		public Font Font;

		private bool textChanged = true;
		
        private void UpdateTextPosition()
        {
			DateTime current = DateTime.Now;
			double ms = (current - last).TotalMilliseconds;

            if (scrolling)
            {
                float distance = (float)(ScrollSpeed * (ms / 1000));
                currentPosition -= distance;
                if (currentPosition < -stringWidth)
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
				stringWidth = g.MeasureString(text, Font).Width + 2;
				if (stringWidth > Bounds.Width)
					scrolling = true;
				else
				{
					scrolling = false;
					currentPosition = 0;
				}
				textChanged = false;
			}

			UpdateTextPosition();
			g.TranslateTransform(currentPosition, 0);
            RectangleF rect = new RectangleF(Bounds.Location, new SizeF(stringWidth, Bounds.Height));
			g.DrawString(text, Font, Brushes.White, rect.Location);

            if (scrolling)
            {
				//Draw it on the other side for seamless looping
				float secondPosition = stringWidth + Bounds.Width * spacing;
				g.TranslateTransform(secondPosition, 0);
                g.DrawString(text, Font, Brushes.White, rect.Location);

				g.TranslateTransform(-secondPosition, 0);
			}

			g.TranslateTransform(-currentPosition, 0);
        }
    }
}
