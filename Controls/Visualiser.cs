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
		
		float[] lastFftPoints;
		bool logarithmic = false;
		float bias = 0.5f;
		float ScaleFactor = 1f;
		float normalisationFactor = 1.0f;

		public Rectangle Bounds;

		int fftSize = 2048;

		public void AddSamples(short[] samples)
		{
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
			if (lastFftPoints == null)
				lastFftPoints = bins;
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
			float[] fftPoints = CalculateNextFftFrame();
			if (fftPoints == null)
				fftPoints = lastFftPoints;

			if (fftPoints != null)
			{				
				//Process points
				PointF[] points = new PointF[fftPoints.Length];
				int j = 0;

				if (logarithmic)
				{
					float binWidth = (Globals.SAMPLE_RATE / 2) / points.Length;
					float maxNote = (float)(12 * Math.Log(((points.Length - 1) * binWidth) / 16.35, 2));
					for (int i = 1; i < points.Length; i++)
					{
						var yVal = fftPoints[i] * bias + lastFftPoints[i] * (1 - bias);
						yVal *= Bounds.Height * ScaleFactor * 0.1f;
						if (float.IsInfinity(yVal) || float.IsNaN(yVal))
							yVal = points[i - 1].Y;

						//TODO: cache this
						float frequency = i * binWidth;
						float notePos = (float)(12 * Math.Log(frequency / 16.35f, 2));
						float xVal = Bounds.Width * (notePos / maxNote);

						points[i] = new PointF(xVal, yVal);
					}
					points[0] = new PointF(0, points[1].Y);
				}
				else
				{
					float spacing = Bounds.Width / (float)points.Length;
					for (int i = 1; i < points.Length; i++)
					{
						var yVal = fftPoints[i] * bias + lastFftPoints[i] * (1 - bias);
						yVal *= Bounds.Height * ScaleFactor * 0.1f;
						yVal = yVal + ((yVal * normalisationFactor * i / points.Length) - (yVal * normalisationFactor / 2));
						if (float.IsInfinity(yVal) || float.IsNaN(yVal))
							yVal = points[i - 1].Y;
						points[i] = new PointF(spacing * i, yVal);
					}
					points[0] = new PointF(0, points[1].Y);
				}
				
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				//Bins go from bottom to top
				g.TranslateTransform(0, Bounds.Height);
				g.ScaleTransform(1, -1);

				g.TranslateTransform(Bounds.X, 0);
				g.DrawCurve(new Pen(Color.FromArgb(160, 236, 26, 85), 1), points);
				g.TranslateTransform(-Bounds.X, 0);

				g.ScaleTransform(1, -1);
				g.TranslateTransform(0, -Bounds.Height);

				lastFftPoints = fftPoints;
			}
		}
	}
}