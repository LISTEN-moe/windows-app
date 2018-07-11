using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Concentus.Structs;

namespace ListenMoeClient
{
	class WebStreamPlayer
	{
		public AudioPlayer BasePlayer { get; set; } = new AudioPlayer();

		Thread provideThread;

		Ogg ogg = new Ogg();
		OpusDecoder decoder = OpusDecoder.Create(Globals.SAMPLE_RATE, 2);

		bool playing = false;
		string url;

		AudioVisualiser visualiser;

		public WebStreamPlayer(string url) => this.url = url;

		public async Task Dispose() => await Stop();

		public void SetVisualiser(AudioVisualiser visualiser) => this.visualiser = visualiser;

		public void SetStreamUrl(string url) => this.url = url;

		public void Play()
		{
			this.BasePlayer.Play();
			playing = true;

			provideThread = new Thread(() =>
			{
				try
				{
					WebClient wc = new WebClient();
					wc.Headers[HttpRequestHeader.UserAgent] = Globals.USER_AGENT;

					using (System.IO.Stream stream = wc.OpenRead(url))
					{
						ReadFullyStream readFullyStream = new ReadFullyStream(stream);

						int packetCounter = 0;
						while (playing)
						{
							byte[][] packets = ogg.GetAudioPackets(readFullyStream);

							packetCounter++;
							//Skip first 5 pages (control frames, etc)
							if (packetCounter <= 5)
								continue;

							for (int i = 0; i < packets.Length; i++)
							{
								byte[] streamBytes = packets[i];
								try
								{
									int frameSize = OpusPacketInfo.GetNumSamplesPerFrame(streamBytes, 0, Globals.SAMPLE_RATE); //Get frame size from opus packet
									short[] rawBuffer = new short[frameSize * 2]; //2 channels
									int buffer = decoder.Decode(streamBytes, 0, streamBytes.Length, rawBuffer, 0, frameSize, false);
									this.BasePlayer.QueueBuffer(rawBuffer);

									if (visualiser != null)
										visualiser.AddSamples(rawBuffer);
								}
								catch (Concentus.OpusException)
								{
									//Skip this frame
								}
							}
						}
					}
				}
				catch (Exception)
				{

				}
			});
			provideThread.Start();
		}

		public float AddVolume(float vol) => this.BasePlayer.AddVolume(vol);

		public async Task Stop()
		{
			if (playing)
			{
				playing = false;

				this.BasePlayer.Stop();

				if (provideThread != null)
				{
					provideThread.Abort();
					await Task.Run(() => provideThread.Join());
					provideThread = null;
				}

				decoder.ResetState();
			}
		}

		public bool IsPlaying() => playing;
	}
}
