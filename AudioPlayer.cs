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
		
		float volume;

		Queue<short[]> samplesToPlay = new Queue<short[]>();

		public AudioPlayer(int sampleRate)
		{
			this.sampleRate = sampleRate;
			WaveFormat format = new WaveFormat(sampleRate, 2);
			provider = new BufferedWaveProvider(format);
			provider.DiscardOnBufferOverflow = true;
			waveOut.Init(provider);
		}

		public void BeginPlayback()
		{
			waveOut.Play();
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

		public void Process()
		{
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
			SetVolume(volume + vol);
			return volume;
		}

		public void SetVolume(float vol)
		{
			volume = BoundVolume(vol);
			waveOut.Volume = volume;
		}
	}
}
