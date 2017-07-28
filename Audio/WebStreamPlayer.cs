using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	class WebStreamPlayer
	{
		const int SAMPLE_RATE = 44100;
		const int VORBIS_BLOCK_SIZE = 16384;

		AudioPlayer audioPlayer = new AudioPlayer(SAMPLE_RATE);
		
		Thread provideThread;

		Ogg ogg = new Ogg();

		bool playing = false;
		string url;

		public WebStreamPlayer(string url)
		{
			this.url = url;
		}

		public void Dispose()
		{
			Stop();
		}

		public void Play()
		{
			audioPlayer.Play();
			playing = true;

			provideThread = new Thread(() =>
			{
				HttpWebRequest req = WebRequest.CreateHttp(url);

				var dll = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "OggVorbis.dll"));
				Type type = dll.GetExportedTypes().First(t => t.Name == "OggDecodeStream");
				
				using (var stream = req.GetResponse().GetResponseStream())
				{
					var readFullyStream = new ReadFullyStream(stream);
					var reader = Activator.CreateInstance(type, new object[] { readFullyStream });
					while (playing)
					{
						byte[] buffer = new byte[VORBIS_BLOCK_SIZE];
						type.InvokeMember("Read", BindingFlags.InvokeMethod, null, reader, new object[] { buffer, 0, VORBIS_BLOCK_SIZE });
						audioPlayer.QueueBuffer(buffer);
					}
				}
			});
			provideThread.Start();
		}

        public float AddVolume(float vol)
        {
			return audioPlayer.AddVolume(vol);
		}

		public void Stop()
		{
			if (playing)
			{
				playing = false;

				audioPlayer.Stop();

				if (provideThread != null)
				{
					provideThread.Abort();
					provideThread = null;
				}
			}
		}

		public bool IsPlaying()
		{
			return playing;
		}
	}
}
