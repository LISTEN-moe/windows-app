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
	class WebStreamPlayer
	{
		const int SAMPLE_RATE = 48000;

		AudioPlayer audioPlayer;
		
		Thread provideThread;

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
			audioPlayer = new AudioPlayer(SAMPLE_RATE);
			playing = true;

			provideThread = new Thread(() =>
			{
				HttpWebRequest req = WebRequest.CreateHttp(url);
				
				using (var stream = req.GetResponse().GetResponseStream())
				{
					var readFullyStream = new ReadFullyStream(stream);
					
					opened = true;

					while (playing)
					{
						byte[][] packets = ogg.GetAudioPackets(readFullyStream);

						for (int i = 0; i < packets.Length; i++)
						{
							var streamBytes = packets[i];
							try
							{
								int frameSize = OpusPacketInfo.GetNumSamplesPerFrame(streamBytes, 0, SAMPLE_RATE); //Get frame size from opus packet
								short[] rawBuffer = new short[frameSize * 2]; //2 channels
								var buffer = decoder.Decode(streamBytes, 0, streamBytes.Length, rawBuffer, 0, frameSize, false);
								audioPlayer.QueueBuffer(rawBuffer);
								audioPlayer.Process();
							}
							catch (Concentus.OpusException e)
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

			audioPlayer.BeginPlayback();
			audioPlayer.SetVolume(initialVolume);
		}

        public float AddVolume(float vol)
        {
			if (audioPlayer != null)
				return audioPlayer.AddVolume(vol);

			Console.WriteLine("OpenAL was not loaded!");
			return 1;
		}

		public void Stop()
		{
			if (playing)
			{
				opened = false;
				playing = false;

				audioPlayer.Dispose();

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
	}
}
