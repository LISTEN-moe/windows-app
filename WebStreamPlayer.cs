using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	//Somewhat taken from the NAudio sample code
	class WebStreamPlayer
	{
		BufferedWaveProvider provider;
		IMp3FrameDecompressor decompressor;
		IWavePlayer waveOut;
		Thread provideThread;
        VolumeWaveProvider16 volumeProvider;

		bool playing = false;
		bool opened = false;
		string url;

		public WebStreamPlayer(string url)
		{
			this.url = url;
		}

		public void Dispose()
		{
			Stop();
		}

		public void Open()
		{
			playing = true;

			provideThread = new Thread(() =>
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

				var buffer = new byte[16 * 1024 * 4];
				using (var stream = resp.GetResponseStream())
				{
					var readFullyStream = new ReadFullyStream(stream);
					while (playing)
					{
						if (BufferGettingFull())
							Thread.Sleep(500);

						Mp3Frame frame = Mp3Frame.LoadFromStream(readFullyStream);

						if (decompressor == null)
						{
							var waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, frame.BitRate);
							decompressor = new AcmMp3FrameDecompressor(waveFormat);
							provider = new BufferedWaveProvider(decompressor.OutputFormat);
							provider.BufferDuration = TimeSpan.FromSeconds(20);
							opened = true;
						}

						int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
						provider.AddSamples(buffer, 0, decompressed);
					}

					decompressor.Dispose();
				}
			});
			provideThread.Start();
		}

        public void Play() { Play(Settings.GetFloatSetting("Volume")); }
		public void Play(float initialVolume)
		{
			while (!opened)
				Thread.Sleep(5);

			waveOut = new WaveOut();
            volumeProvider = new VolumeWaveProvider16(provider);
            volumeProvider.Volume = BoundVolume(initialVolume);
			waveOut.Init(volumeProvider);
			waveOut.Play();
		}

        public float SetVolume(float vol)
        {
            if (volumeProvider != null)
            {
                volumeProvider.Volume = BoundVolume(vol);
                return vol;
            }
            return 1.0f;
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
            return SetVolume(volumeProvider.Volume + vol);
        }

		public void Stop()
		{
			if (playing)
			{
				opened = false;
				playing = false;

				if (waveOut != null)
				{
					waveOut.Stop();
					waveOut.Dispose();
					waveOut = null;
				}

				if (provideThread != null)
				{
					provideThread.Abort();
					provideThread = null;
				}

				if (decompressor != null)
				{
					decompressor.Dispose();
					decompressor = null;
				}
			}
		}

		public bool IsPlaying()
		{
			return playing;
		}

		bool BufferGettingFull()
		{
			return provider != null && provider.BufferLength - provider.BufferedBytes < provider.WaveFormat.AverageBytesPerSecond / 4;
		}
	}
}
