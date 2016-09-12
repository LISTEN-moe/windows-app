using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
    class MarqueeLabel : Label
    {
        public float ScrollSpeed { get; set; } = 25; //In pixels per second

        private float currentPosition = 0;
        private float spacing = 0.7f; //As a multiple of the width of the label

        private Stopwatch stopwatch = new Stopwatch();
        private Thread shiftThread;

        private float stringWidth;

        bool scrolling = false;

        public MarqueeLabel() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            stopwatch.Start();
            shiftThread = new Thread(Shift);
            shiftThread.Start();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            shiftThread.Abort();
        }

        long lastMs = 0;

        private void Shift()
        {
            //Super crappy animation timer!
            lastMs = stopwatch.ElapsedMilliseconds;
            while (true)
            {
                long now = stopwatch.ElapsedMilliseconds;
                long ms = now - lastMs;

                if (scrolling)
                {
                    float distance = ScrollSpeed * (ms / 1000f);
                    currentPosition -= distance;
                    if (currentPosition < -stringWidth)
                        currentPosition = ClientRectangle.Width * spacing;
                    this.Invalidate();
                }

                lastMs = now;
                Thread.Sleep(50); //yeah well
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (!this.IsDisposed) //We get issues with the graphics object when we're disposing, since it calls this TextChange method
            {
                stringWidth = this.CreateGraphics().MeasureString(Text, Font).Width;
                if (stringWidth > Width)
                    scrolling = true;
                else
                    scrolling = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(currentPosition, 0);
            RectangleF rect = new RectangleF(ClientRectangle.Location, new SizeF(stringWidth, ClientRectangle.Height));
            e.Graphics.DrawString(Text, Font, Brushes.White, rect);

            if (scrolling)
            {
                //Draw it on the other side for seamless looping
                e.Graphics.TranslateTransform(stringWidth + ClientRectangle.Width * spacing, 0);
                e.Graphics.DrawString(Text, Font, Brushes.White, rect);
            }
        }
    }
}
