using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace ListenMoeClient
{

    /// <summary>
    /// Audio Output Device Object
    /// </summary>
    public class AudioDevice
    {
        public int ID;
        public string Name;

        public AudioDevice(int _id, string _name)
        {
            this.ID = _id;
            this.Name = _name;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }

	public class AudioPlayer : IDisposable
	{
		BufferedWaveProvider provider;
		WaveOut waveOut;

		Queue<short[]> samplesToPlay = new Queue<short[]>();

		public AudioPlayer()
		{
			WaveFormat format = new WaveFormat(Globals.SAMPLE_RATE, 2);
			provider = new BufferedWaveProvider(format)
			{
				BufferDuration = TimeSpan.FromSeconds(5),
				DiscardOnBufferOverflow = true
			};

            Initialize();
        }

        /// <summary>
        /// Intialize the WaveOut Device and set Volume
        /// </summary>
        /// <param name="reinitialize">Whether to re-initialize the player.</param>
        public void Initialize(bool reinitialize = false)
        {
            PlaybackState prevState = PlaybackState.Stopped;

            if (reinitialize)
            {
                prevState = waveOut.PlaybackState;
                Dispose();
            }
                

            waveOut = new WaveOut();
            SetAudioOutputDevice(Settings.Get<int>("OutputDeviceNumer"));
            waveOut.Init(provider);
            waveOut.Volume = Settings.Get<float>("Volume");
            

            if (reinitialize)
                if(prevState == PlaybackState.Playing)
                    Play();
            
        }

		public void Play()
		{
			provider.ClearBuffer();
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


        /// <summary>
        /// Get an array of the available audio output devices.
        /// <para>Because of a limitation of WaveOut, device's names will be cut if they are too long.</para>
        /// </summary>
        public AudioDevice[] GetAudioOutputDevices()
        {
            AudioDevice[] devices = new AudioDevice[WaveOut.DeviceCount + 1];

            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                devices[n+1] = new AudioDevice(n, caps.ProductName);
            }

            return devices;
        }

        /// <summary>
        /// Get current audio output device.
        /// </summary>
        public AudioDevice GetCurrentAudioOutputDevice()
        {
            WaveOutCapabilities dev = WaveOut.GetCapabilities(waveOut.DeviceNumber);
            return new AudioDevice(waveOut.DeviceNumber, dev.ProductName);
        }

        /// <summary>
        /// Get the index of an Audio Device by providing the ID
        /// </summary>
        /// <param name="ID">Audio Device ID (From -1 to the amount of devices available)</param>
        /// <returns></returns>
        public int GetAudioDeviceIndex(int ID)
        {
            AudioDevice[] devices = GetAudioOutputDevices();

            int desired = -1;

            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].ID == ID)
                {
                    desired = i;
                    break;
                }
            }

            return desired;
        }

        /// <summary>
        /// Set the audio output device (if available); Returns current audio device (desired if valid).
        /// <para><strong>Important:</strong> Requires re-initialization of WaveOut to apply changes.</para>
        /// </summary>
        /// <param name="device">Device ID</param>
        /// <param name="reinitialize">Whether to re-initialize the player.</param>
        /// <returns></returns>
        public int SetAudioOutputDevice(int device, bool reinitialize = false)
        {
            int currentDevice = GetCurrentAudioOutputDevice().ID;

            if (device == currentDevice)
                return currentDevice;

            int validDevice = (device < WaveOut.DeviceCount) ? device : -1; //If invalid set the default device(-1)
            waveOut.DeviceNumber = validDevice;
            Settings.Set<int>("OutputDeviceNumer", validDevice);
            Settings.WriteSettings();

            if (reinitialize)
                Initialize(true);

            return currentDevice;
        }

        /// <summary>
        /// Set the audio output device (if available); Returns current audio device (desired if valid).
        /// <para><strong>Important:</strong> Requires re-initialization of WaveOut to apply changes.</para>
        /// </summary>
        /// <param name="device">Audio Device Object</param>
        /// <param name="reinitialize">Whether to re-initialize the player.</param>
        /// <returns></returns>
        public int SetAudioOutputDevice(AudioDevice device, bool reinitialize = false)
        {
            return SetAudioOutputDevice(device.ID, reinitialize);
        }
    }
}
