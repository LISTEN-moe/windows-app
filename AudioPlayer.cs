using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	class AudioPlayer : IDisposable
	{
		const int MIN_BUFFER_COUNT = 50;
		int sampleRate;

		int source;
		AudioContext context;
		float volume;

		bool begunPlayback = false;

		Queue<short[]> samplesToPlay = new Queue<short[]>();

		public AudioPlayer(int sampleRate)
		{
			this.sampleRate = sampleRate;
			context = new AudioContext();
			source = AL.GenSource();
		}

		public void BeginPlayback()
		{
			begunPlayback = true;
		}

		public void Dispose()
		{
			AL.SourceStop(source);
			AL.DeleteSource(source);
			source = 0;
			context.Dispose();
		}

		public void QueueBuffer(short[] samples)
		{
			samplesToPlay.Enqueue(samples);
		}

		private byte[] GetAudioBuffer()
		{
			if (samplesToPlay.Count == 0)
				return null;

			short[] buffer = samplesToPlay.Dequeue();

			byte[] result = new byte[buffer.Length * 2];
			for (int i = 0; i < buffer.Length; i++)
			{
				byte[] val = BitConverter.GetBytes(buffer[i]);
				Array.Copy(val, 0, result, i * 2, 2);
			}

			return result;
		}

		public void Process()
		{
			int state;
			AL.GetSource(source, ALGetSourcei.SourceState, out state);

			//Clean up processed buffers
			int processed;
			AL.GetSource(source, ALGetSourcei.BuffersProcessed, out processed);
			if (processed > 0)
			{
				int[] buffersToDispose = AL.SourceUnqueueBuffers(source, processed);
				AL.DeleteBuffers(buffersToDispose);
			}

			//Shouldn't need this since our stream is real-time, but leaving it here just in case
			//If we still have at least half of our buffers available, just return
			//int queued;
			//AL.GetSource(source, ALGetSourcei.BuffersQueued, out queued);
			//if (queued > (int)(0.5f * MIN_BUFFER_COUNT))
			//	return;

			//Load the next set of buffers
			List<int> buffers = new List<int>();
			for (int i = 0; i < MIN_BUFFER_COUNT; i++)
			{
				byte[] buffer = GetAudioBuffer();
				if (buffer == null)
					break;

				buffers.Add(AL.GenBuffer());
				AL.BufferData(buffers[buffers.Count - 1], ALFormat.Stereo16, buffer, buffer.Length, sampleRate);
			}
			int[] availableBuffers = buffers.ToArray();
			AL.SourceQueueBuffers(source, Math.Min(availableBuffers.Length, MIN_BUFFER_COUNT), availableBuffers);

			if ((ALSourceState)state != ALSourceState.Playing && begunPlayback)
				AL.SourcePlay(source);
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
			AL.Source(source, ALSourcef.Gain, volume);
		}
	}
}
