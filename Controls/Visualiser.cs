using DequeNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	class Visualiser
	{
		DateTime anchor;
		Deque<short> sampleBuffer = new Deque<short>();

		const int fftSize = 2048;

		float[] lastFftPoints = new float[fftSize];
		bool logarithmic = false;
		float bias = 0.3f;
		float ScaleFactor = 1f;
		float normalisationFactor = 1.0f;

		int resolutionFactor = Settings.Get<int>("VisualiserResolutionFactor"); //higher = lower resolution, number is the number of samples to skip
		float barWidth = Settings.Get<float>("VisualiserBarWidth");

		bool bars = true;

		bool stopped = false;

		public Rectangle Bounds;

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
			Settings.Set("VisualiserBarWidth", barWidth);
			Settings.WriteSettings();
		}

		public void IncreaseResolution(int amount)
		{
			if (resolutionFactor - amount > 0)
			{
				resolutionFactor -= amount;
				Settings.Set("VisualiserResolutionFactor", resolutionFactor);
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
			float[] bins = FFT.Fft(window);
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
				for (int i = 1; j < noPoints && i < fftPoints.Length; i += resolutionFactor)
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
				
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			var scale = Settings.Get<float>("Scale");

			//Bins go from bottom to top
			g.TranslateTransform(0, Bounds.Height * scale);
			g.ScaleTransform(1, -1);
			g.ScaleTransform(scale, scale);

			g.TranslateTransform(Bounds.X, 0);
			if (bars)
			{
				for (int i = 0; i < points.Length - 1; i++)
				{
					var next = points[i + 1];
					var current = points[i];

					float pos = Math.Max(current.X, current.X + (next.X - current.X - barWidth) / 2);
					float width = Math.Min(barWidth, next.X - current.X);
					g.FillRectangle(new SolidBrush(Color.FromArgb(128, 236, 26, 85)), pos, 0, width, current.Y);
				}
			}
			else
			{
				g.DrawCurve(new Pen(Color.FromArgb(255, 236, 26, 85), 1), points);
			}
			g.TranslateTransform(-Bounds.X, 0);

			g.ScaleTransform(1 / scale, 1 / scale);
			g.ScaleTransform(1, -1);
			g.TranslateTransform(0, -(Bounds.Height * scale));

			lastFftPoints = fftPoints.Select(x => (float.IsInfinity(x) || float.IsNaN(x)) ? 0 : x).ToArray();
		}
	}
}