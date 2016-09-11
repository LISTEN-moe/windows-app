using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

using NAudio;
using NAudio.Wave;
using System.Threading;

namespace CrappyListenMoe
{
	public partial class Form1 : Form
	{
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

		private PrivateFontCollection fonts = new PrivateFontCollection();

		Font titleFont;
		Font artistFont;

		Player player;

		//Drag form to move
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private const int SnapDist = 10;
		private bool DoSnap(int pos, int edge)
		{
			int delta = pos - edge;
			return delta > 0 && delta <= SnapDist;
		}
		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			Screen scn = Screen.FromPoint(this.Location);
			if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
			if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
			if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
			if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;
		}

		public Form1()
		{
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
			GetStats();

			this.Icon = Properties.Resources.icon;

			LoadOpenSans();

			lblTitle.Font = titleFont;
			lblArtist.Font = artistFont;


			player = new Player();
			player.Open();
			player.Play();
		}

		private void LoadOpenSans()
		{
			byte[] fontData = Properties.Resources.OpenSans_Regular;
			IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
			Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
			uint dummy = 0;
			fonts.AddMemoryFont(fontPtr, Properties.Resources.OpenSans_Regular.Length);
			AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.OpenSans_Regular.Length, IntPtr.Zero, ref dummy);
			Marshal.FreeCoTaskMem(fontPtr);

			titleFont = new Font(fonts.Families[0], 11.0F);
			artistFont = new Font(fonts.Families[0], 8.0F);
		}

		private void picPlayPause_Click(object sender, EventArgs e)
		{
			if (player.IsPlaying())
			{
				player.Stop();
				picPlayPause.Image = Properties.Resources.play;
			}
			else
			{
				player.Open();
				player.Play();
				picPlayPause.Image = Properties.Resources.pause;
			}
		}

		private void picClose_Click(object sender, EventArgs e)
		{
			this.Hide();
			player.Dispose();
			Environment.Exit(0);
		}

		private Stats DownloadStats()
		{
			var url = "https://listen.moe/stats.json";
			var data = new WebClient().DownloadString(url);

			DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(Stats));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				return (Stats)s.ReadObject(stream);
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			GetStats();
		}

		void GetStats()
		{
			Thread thread = new Thread(() =>
			{
				Stats stats = DownloadStats();
				lblTitle.Text = stats.song_name;
				string middle = string.IsNullOrWhiteSpace(stats.artist_name) ? "Requested by " : "; Requested by ";
				lblArtist.Text = stats.artist_name.Trim() + middle + stats.requested_by;
			});
			thread.Start();
		}
	}

	public class Stats
	{
		public string requested_by { get; set; }
		public string listeners { get; set; }
		public string song_name { get; set; }
		public string artist_name { get; set; }
	}

	class Player
	{
		BufferedWaveProvider provider;
		IMp3FrameDecompressor decompressor;
		IWavePlayer waveOut;
		Thread provideThread;

		bool playing = false;
		bool opened = false;

		public Player()
		{
			
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
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://listen.moe:9999/stream");
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

		public void Play()
		{
			while (!opened)
				Thread.Sleep(5);

			waveOut = new WaveOut();
			waveOut.Init(provider);
			waveOut.Play();
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
