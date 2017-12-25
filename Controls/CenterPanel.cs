using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	enum UpdateState
	{
		None,
		InProgress,
		Complete
	}

	class CenterPanel : Panel
	{
		public AudioVisualiser Visualiser = new AudioVisualiser();

		MarqueeLabel lblArtist = new MarqueeLabel();
		MarqueeLabel lblTitle = new MarqueeLabel();
		MarqueeLabel lblEvent = new MarqueeLabel();
		int eventBarHeight = 16;
		bool isEventOrRequest = false;

		float updatePercent = 0;
		UpdateState updateState;

		public CenterPanel()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

			lblArtist.Text = "Connecting...";
			lblEvent.Centered = true;
			RecalculateMarqueeBounds();

			Visualiser.SetBounds(new Rectangle(0, 0, this.Width, this.Height));
			Visualiser.ReloadSettings();
		}

		private void RecalculateMarqueeBounds()
		{
			float scale = Settings.Get<float>(Setting.Scale);

			lblEvent.Bounds = new Rectangle(0, (int)(scale + this.Height - eventBarHeight * scale), this.Width, (int)(eventBarHeight * scale));
			if (isEventOrRequest)
			{
				lblArtist.Bounds = new Rectangle((int)(8 * scale), (int)(4 * scale), this.Width, this.Height);
				lblTitle.Bounds = new Rectangle((int)(6 * scale), (int)(18 * scale), this.Width, this.Height);
			}
			else
			{
				lblArtist.Bounds = new Rectangle((int)(8 * scale), (int)(13 * scale), this.Width, this.Height);
				lblTitle.Bounds = new Rectangle((int)(6 * scale), (int)(28 * scale), this.Width, this.Height);
			}

			lblEvent.RecalculateBounds();
			lblArtist.RecalculateBounds();
			lblTitle.RecalculateBounds();
		}

		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);

			RecalculateMarqueeBounds();
			Visualiser.SetBounds(new Rectangle(0, 0, this.Width, this.Height));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			this.SuspendLayout();

			Visualiser.Render(e.Graphics);
			lblTitle.Render(e.Graphics);
			lblArtist.Render(e.Graphics);

			if (isEventOrRequest)
			{
				float scale = Settings.Get<float>(Setting.Scale);

				Brush brush = new SolidBrush(Color.FromArgb(64, 0, 0, 0));
				e.Graphics.FillRectangle(brush, 0, this.Height - eventBarHeight * scale, this.Width, eventBarHeight * scale);
				lblEvent.Render(e.Graphics);
			}

			if (updateState != 0)
			{
				Brush brush = new SolidBrush(updateState == UpdateState.InProgress ? Color.Yellow : Color.LimeGreen);
				//Height for pause/play button
				e.Graphics.FillRectangle(brush, Settings.DEFAULT_HEIGHT, this.Height - 3, (this.Width - Settings.DEFAULT_HEIGHT- Settings.DEFAULT_RIGHT_PANEL_WIDTH) * updatePercent, 3);
			}

			this.ResumeLayout();
		}

		public void SetUpdateState(UpdateState state)
		{
			updateState = state;
			this.Invalidate();
		}

		public void SetUpdatePercent(float updatePercent)
		{
			this.updatePercent = updatePercent;
			this.Invalidate();
		}

		public void SetLabelText(string titleText, string artistText, string albumText, string eventText, bool isEventOrRequest)
		{
			lblTitle.Text = titleText;
			lblArtist.Text = artistText;
			if (!string.IsNullOrWhiteSpace(albumText))
				lblArtist.Text += " - " + albumText;
			lblEvent.Text = eventText;

			this.isEventOrRequest = isEventOrRequest;
			RecalculateMarqueeBounds();
		}

		public void SetLabelBrush(Brush brush)
		{
			lblTitle.FontBrush = brush;
			lblArtist.FontBrush = brush;
			lblEvent.FontBrush = brush;
		}

		public void SetFonts(Font titleFont, Font albumFont)
		{
			lblTitle.Font = titleFont;
			lblTitle.Subfont = albumFont;
			lblArtist.Font = albumFont;
			lblEvent.Font = albumFont;

			lblTitle.RecalculateBounds();
			lblArtist.RecalculateBounds();
			lblEvent.RecalculateBounds();
		}

		public void StartVisualiser(WebStreamPlayer player)
		{
			Visualiser.Start();
			player.SetVisualiser(Visualiser);
		}

		public void StopVisualiser(WebStreamPlayer player)
		{
			player.SetVisualiser(null);
			Visualiser.Stop();
		}

		public void ReloadVisualiser()
		{
			Visualiser.ReloadSettings();
		}
	}
}
