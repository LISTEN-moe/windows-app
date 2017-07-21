using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace CrappyListenMoe
{
	class AudioPlayer : IDisposable
	{
		const int MIN_BUFFER_COUNT = 50;
		int sampleRate;

		BufferedWaveProvider provider;
		WaveOut waveOut = new WaveOut();

		Queue<short[]> samplesToPlay = new Queue<short[]>();

		public AudioPlayer(int sampleRate)
		{
			this.sampleRate = sampleRate;
			WaveFormat format = new WaveFormat(sampleRate, 2);
			provider = new BufferedWaveProvider(format);
			provider.BufferDuration = TimeSpan.FromSeconds(10);
			provider.DiscardOnBufferOverflow = true;
			waveOut.Init(provider);
			waveOut.Volume = Settings.GetFloatSetting("Volume");
		}

		public void Play()
		{
			waveOut.Play();
		}

		public void Stop()
		{
			waveOut.Stop();
			provider.ClearBuffer();
		}

		public void Dispose()
		{
			waveOut.Stop();
			provider.ClearBuffer();
			waveOut.Dispose();
		}

		public void QueueBuffer(short[] samples)
		{
			byte[] bytes = new byte[samples.Length * 2];
			Buffer.BlockCopy(samples, 0, bytes, 0, bytes.Length);
			provider.AddSamples(bytes, 0, bytes.Length);
		}
		
		private float BoundVolume(float vol)
		{
			//Cap between 0 and 1
			vol = Math.Max(0, vol);
			vol = Math.Min(1, vol);
			return vol;
		}

		public float AddVolume(float vol)
		{
			SetVolume(waveOut.Volume + vol);
			return waveOut.Volume;
		}

		public void SetVolume(float vol)
		{
			waveOut.Volume = BoundVolume(vol);
		}
	}
}
