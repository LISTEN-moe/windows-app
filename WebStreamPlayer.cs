using NAudio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Concentus.Structs;

namespace CrappyListenMoe
{
	//Somewhat taken from the NAudio sample code
	class WebStreamPlayer
	{
		const int SAMPLE_RATE = 48000;

		BufferedWaveProvider provider;
		IWavePlayer waveOut;
		Thread provideThread;
        VolumeWaveProvider16 volumeProvider;

		Ogg ogg = new Ogg();
		OpusDecoder decoder = OpusDecoder.Create(SAMPLE_RATE, 2);

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
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
				
				using (var stream = resp.GetResponseStream())
				{
					var readFullyStream = new ReadFullyStream(stream);

					provider = new BufferedWaveProvider(new WaveFormat(SAMPLE_RATE, 2));
					provider.BufferDuration = TimeSpan.FromSeconds(20);
					opened = true;

					while (playing)
					{
						if (BufferGettingFull())
							Thread.Sleep(500);

						byte[][] packets = ogg.GetAudioPackets(readFullyStream);

						for (int i = 0; i < packets.Length; i++)
						{
							var streamBytes = packets[i];
							try
							{
								int frameSize = OpusPacketInfo.GetNumSamplesPerFrame(streamBytes, 0, SAMPLE_RATE); //Get frame size from opus packet
								short[] outputBuffer = new short[frameSize * 2]; //2 channels
								var buffer = decoder.Decode(streamBytes, 0, streamBytes.Length, outputBuffer, 0, frameSize, false);
								for (int s = 0; s < outputBuffer.Length; s++)
								{
									byte[] tmp = BitConverter.GetBytes(outputBuffer[s]);
									provider.AddSamples(tmp, 0, 2);
								}
							}
							catch (Exception e)
							{
								//Skip this frame
								//Note: the first 2 frames will hit this exception (I'm pretty sure they're not audio data frames)
							}
						}
					}
				}
			});
			provideThread.Start();
		}

        public void Play() { Play(Settings.GetFloatSetting("Volume")); }
		public void Play(float initialVolume)
		{
			while (!opened)
				Thread.Sleep(1);

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
                return volumeProvider.Volume;
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
			if (volumeProvider != null)
				return SetVolume(volumeProvider.Volume + vol);
			return -1;
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

				decoder.ResetState();
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
