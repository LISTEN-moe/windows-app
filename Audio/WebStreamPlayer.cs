using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Concentus.Structs;

namespace ListenMoeClient
{
	class WebStreamPlayer
	{
		const int SAMPLE_RATE = 48000;

		AudioPlayer audioPlayer = new AudioPlayer(SAMPLE_RATE);
		
		Thread provideThread;

		Ogg ogg = new Ogg();
		OpusDecoder decoder = OpusDecoder.Create(SAMPLE_RATE, 2);

		bool playing = false;
		string url;

		public WebStreamPlayer(string url)
		{
			this.url = url;
		}

		public async Task Dispose()
		{
			await Stop();
		}

		public void Play()
		{
			audioPlayer.Play();
			playing = true;

			provideThread = new Thread(() =>
			{
				try
				{
					HttpWebRequest req = WebRequest.CreateHttp(url);
					//req.UserAgent = Globals.USER_AGENT;

					using (var stream = req.GetResponse().GetResponseStream())
					{
						var readFullyStream = new ReadFullyStream(stream);

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
								}
								catch (Concentus.OpusException e)
								{
									//Skip this frame
									//Note: the first 2 frames will hit this exception (I'm pretty sure they're not audio data frames)
								}
							}
						}
					}
				} catch (Exception e)
				{

				}
			});
			provideThread.Start();
		}

        public float AddVolume(float vol)
        {
			return audioPlayer.AddVolume(vol);
		}

		public async Task Stop()
		{
			if (playing)
			{
				playing = false;

				audioPlayer.Stop();

				if (provideThread != null)
				{
					await Task.Run(() => { provideThread.Abort(); });
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
