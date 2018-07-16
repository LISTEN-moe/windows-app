using System;

namespace ListenMoeClient
{
	public struct Complex
	{
		public float Real, Imaginary;

		public Complex(float r, float i)
		{
			Real = r;
			Imaginary = i;
		}

		public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);

		public static Complex operator *(Complex a, Complex b) => new Complex(a.Real * b.Real - a.Imaginary * b.Imaginary,
				a.Real * b.Imaginary + a.Imaginary * b.Real);

		public static Complex operator +(Complex a, Complex b) => new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);

		public static Complex operator -(Complex a, Complex b) => new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
	}

	class FFT
	{
		private static Complex[] ConvertToComplex(short[] buffer)
		{
			Complex[] complex = new Complex[buffer.Length];
			for (int i = 0; i < buffer.Length; i++)
			{
				complex[i] = new Complex(buffer[i], 0);
			}
			return complex;
		}

		public static float[] Fft(short[] buffer, int exponent)
		{
			Complex[] data = ConvertToComplex(buffer);
			Fft(data, buffer.Length, exponent);

			float[] resultBuffer = new float[data.Length / 2];
			for (int i = 1; i < resultBuffer.Length; i++)
			{
				resultBuffer[i] = (float)data[i].Magnitude / 64;
			}
			return resultBuffer;
		}

		public static void Fft(Complex[] data, int fftSize, int exponent)
		{
			int c = fftSize;

			//binary inversion
			Inverse(data, c);

			int j0, j1, j2 = 1;
			float n0, n1, tr, ti, m;
			float v0 = -1, v1 = 0;

			//move to outer scope to optimize performance
			int j, i;

			for (int l = 0; l < exponent; l++)
			{
				n0 = 1;
				n1 = 0;
				j1 = j2;
				j2 <<= 1; //j2 * 2

				for (j = 0; j < j1; j++)
				{
					for (i = j; i < c; i += j2)
					{
						j0 = i + j1;
						//--
						tr = n0 * data[j0].Real - n1 * data[j0].Imaginary;
						ti = n0 * data[j0].Imaginary + n1 * data[j0].Real;
						//--
						data[j0].Real = data[i].Real - tr;
						data[j0].Imaginary = data[i].Imaginary - ti;
						//add
						data[i].Real += tr;
						data[i].Imaginary += ti;
					}

					//calc coeff
					m = v0 * n0 - v1 * n1;
					n1 = v1 * n0 + v0 * n1;
					n0 = m;
				}

				v1 = (float)Math.Sqrt((1f - v0) / 2f);
				v0 = (float)Math.Sqrt((1f + v0) / 2f);
			}

			for (int k = 0; k < c; k++)
			{
				data[k].Real /= c;
				data[k].Imaginary /= c;
			}
		}

		private static void Inverse(Complex[] data, int c)
		{
			int z = 0;
			int n1 = c >> 1; //c / 2

			for (int n0 = 0; n0 < c - 1; n0++)
			{
				if (n0 < z)
				{
					Swap(data, n0, z);
				}
				int l = n1;

				while (l <= z)
				{
					z = z - l;
					l >>= 1;
				}
				z += l;
			}
		}

		private static void Swap(Complex[] data, int index, int index2)
		{
			Complex tmp = data[index];
			data[index] = data[index2];
			data[index2] = tmp;
		}
	}
}
