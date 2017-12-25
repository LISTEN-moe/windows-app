using DequeNet;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ListenMoeClient
{
	public class AudioVisualiser
	{
		DateTime anchor;
		Deque<short> sampleBuffer = new Deque<short>();

		int fftSize = Settings.Get<int>(Setting.FftSize);
		const int exponent = 11;

		float[] lastFftPoints;
		bool logarithmic = false;
		float bias = 0.3f;
		float ScaleFactor = 1f;
		float normalisationFactor = 0.9f;

		int resolutionFactor = Settings.Get<int>(Setting.VisualiserResolutionFactor); //higher = lower resolution, number is the number of samples to skip
		float barWidth = Settings.Get<float>(Setting.VisualiserBarWidth);

		bool bars = true;
		bool stopped = true;

		Rectangle Bounds = Rectangle.Empty;
		Color visualiserColor;
		Brush barBrush;
		Pen linePen;

		public AudioVisualiser()
		{
			lastFftPoints = new float[fftSize];
		}

		public void SetBounds(Rectangle bounds)
		{
			this.Bounds = bounds;
		}

		public void ReloadSettings()
		{
			bars = Settings.Get<bool>(Setting.VisualiserBars);
			var opacity = (int)Math.Min(Math.Max(Settings.Get<float>(Setting.VisualiserTransparency) * 255, 0), 255);
			visualiserColor = Color.FromArgb(opacity, Settings.Get<Color>(Setting.VisualiserColor));

			if (Bounds.Width == 0 || Bounds.Height == 0)
				return;

			if (Settings.Get<bool>(Setting.VisualiserFadeEdges))
			{
				Color baseColor = Settings.Get<Color>(Setting.BaseColor);
				barBrush = new LinearGradientBrush(new Rectangle(Point.Empty, Bounds.Size), baseColor, visualiserColor, LinearGradientMode.Horizontal);
				ColorBlend blend = new ColorBlend
				{
					Colors = new Color[] { baseColor, baseColor, visualiserColor, visualiserColor, visualiserColor, baseColor, baseColor },
					Positions = new float[] { 0.0f, 0.05f, 0.2f, 0.5f, 0.8f, 0.95f, 1.0f }
				};
				((LinearGradientBrush)barBrush).InterpolationColors = blend;
				linePen = new Pen(barBrush, 1);
			}
			else
			{
				barBrush = new SolidBrush(visualiserColor);
				linePen = new Pen(visualiserColor, 1);
			}
		}

		public void AddSamples(short[] samples)
		{
			if (stopped)
				return;

			if (anchor == DateTime.MinValue)
				anchor = DateTime.Now;

			foreach (var s in samples)
				sampleBuffer.PushRight(s);

			if (sampleBuffer.Count > fftSize * 4)
			{
				DateTime now = DateTime.Now;
				int currentPos = (int)(Globals.SAMPLE_RATE * ((now - anchor).TotalMilliseconds / 1000)) * 2;
				anchor = now;
				for (int i = 0; i < currentPos && sampleBuffer.Count > 0; i++)
					sampleBuffer.PopLeft();
			}
		}

		public void ClearBuffers()
		{
			anchor = DateTime.MinValue;
			sampleBuffer.Clear();
		}

		public void Stop()
		{
			stopped = true;
			sampleBuffer.Clear();
			anchor = DateTime.MinValue;
		}

		public void Start()
		{
			stopped = false;
		}

		public void IncreaseBarWidth(float amount)
		{
			barWidth += amount;
			Settings.Set(Setting.VisualiserBarWidth, barWidth);
			Settings.WriteSettings();
		}

		public void IncreaseResolution(int amount)
		{
			if (resolutionFactor - amount > 0)
			{
				resolutionFactor -= amount;
				Settings.Set(Setting.VisualiserResolutionFactor, resolutionFactor);
				Settings.WriteSettings();
			}
		}

		private float[] CalculateNextFftFrame()
		{
			if (sampleBuffer == null)
				return null;

			int currentPos = (int)(Globals.SAMPLE_RATE * ((DateTime.Now - anchor).TotalMilliseconds / 1000)) * 2;
			if (sampleBuffer.Count - currentPos < fftSize || currentPos < 0)
				return null;

			short[] window = new short[fftSize];
			for (int i = 0; i < fftSize; i++)
				window[i] = sampleBuffer[currentPos + i];

			applyWindowFunction(window);
			float[] bins = FFT.Fft(window, exponent);
			bins = bins.Take(bins.Length / 4).ToArray();
			bins = bins.Select(f => (float)Math.Log10(f * 10) * 2 + 1).Select(f => ((f - 0.3f) * 1.5f) + 1.5f).ToArray();
			return bins;
		}

		private void applyWindowFunction(short[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (short)(data[i] * 0.5 * (1 - Math.Cos(2 * Math.PI * i / data.Length)));
			}
		}

		public void Render(Graphics g)
		{
			float[] fftPoints;
			if (stopped)
			{
				fftPoints = new float[lastFftPoints.Length];
			}
			else
			{
				fftPoints = CalculateNextFftFrame();
				if (fftPoints == null)
				{
					fftPoints = lastFftPoints;
				}
			}

			//Process points
			int noPoints = fftPoints.Length / resolutionFactor;
			PointF[] points = new PointF[noPoints];

			for (int i = 1; i < fftPoints.Length; i++)
				fftPoints[i] = fftPoints[i] * bias + lastFftPoints[i] * (1 - bias);

			int j = 0;
			if (logarithmic)
			{
				float binWidth = (Globals.SAMPLE_RATE / 2) / fftPoints.Length;
				float maxNote = (float)(12 * Math.Log(((fftPoints.Length - 1) * binWidth) / 16.35, 10));
				for (int i = 1; j < noPoints && i < fftPoints.Length; i += resolutionFactor)
				{
					var yVal = fftPoints[i];
					yVal *= Bounds.Height * ScaleFactor * 0.1f;
					yVal = yVal + ((yVal * normalisationFactor * j / noPoints) - (yVal * normalisationFactor / 2));
					if (float.IsInfinity(yVal) || float.IsNaN(yVal))
						yVal = 0;

					//TODO: cache this
					float frequency = i * binWidth;
					float notePos = (float)(12 * Math.Log(frequency / 16.35f, 10));
					float xVal = Bounds.Width * (notePos / maxNote);

					points[j] = new PointF(xVal, yVal);
					j++;
				}
				points[0] = new PointF(0, points[1].Y);
			}
			else
			{
				float spacing = Bounds.Width / ((float)noPoints - 1);
				for (int i = 0; j < noPoints && i < fftPoints.Length; i += resolutionFactor)
				{
					var yVal = fftPoints[i];
					yVal *= Bounds.Height * ScaleFactor * 0.1f;
					yVal = yVal + ((yVal * normalisationFactor * j / noPoints) - (yVal * normalisationFactor / 2));
					if (float.IsInfinity(yVal) || float.IsNaN(yVal))
						yVal = 0;
					points[j] = new PointF(spacing * j, yVal);
					j++;
				}
				points[0] = new PointF(0, points[1].Y);
			}

			g.SmoothingMode = SmoothingMode.HighQuality;

			var scale = Settings.Get<float>(Setting.Scale);

			//Bins go from bottom to top
			g.TranslateTransform(0, Bounds.Height);
			g.ScaleTransform(1, -1);

			g.TranslateTransform(Bounds.X, 0);
			if (bars)
			{
				RectangleF[] rectangles = new RectangleF[points.Length - 1];
				for (int i = 0; i < points.Length - 1; i++)
				{
					var next = points[i + 1];
					var current = points[i];

					float pos = Math.Max(current.X, current.X + (next.X - current.X - barWidth * scale) / 2);
					float width = Math.Min(barWidth * scale, next.X - current.X);
					rectangles[i] = new RectangleF(pos, 0, width, current.Y);
				}
				g.FillRectangles(barBrush, rectangles);
			}
			else
			{
				g.DrawCurve(linePen, points);
			}
			g.TranslateTransform(-Bounds.X, 0);

			g.ScaleTransform(1, -1);
			g.TranslateTransform(0, -(Bounds.Height));

			lastFftPoints = fftPoints.Select(x => (float.IsInfinity(x) || float.IsNaN(x)) ? 0 : x).ToArray();
		}
	}
}
