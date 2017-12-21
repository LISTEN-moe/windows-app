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
		public AudioVisualiser Visualiser { get; set; }

		MarqueeLabel lblAlbum = new MarqueeLabel();
		MarqueeLabel lblTitle = new MarqueeLabel();

		float updatePercent = 0;
		UpdateState updateState;

		public CenterPanel()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

			lblTitle.Text = "Connecting...";
			RecalculateMarqueeBounds();
		}

		private void RecalculateMarqueeBounds()
		{
			lblAlbum.Bounds = new Rectangle(5, 26, this.Width, this.Height);
			lblTitle.Bounds = new Rectangle(5, 5, this.Width, this.Height);
			lblAlbum.RecalculateBounds();
			lblTitle.RecalculateBounds();
		}

		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);

			RecalculateMarqueeBounds();
			if (Visualiser != null)
				Visualiser.SetBounds(new Rectangle(0, 0, this.Width, this.Height));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			this.SuspendLayout();

			if (Visualiser != null)
			{
				Visualiser.Render(e.Graphics);
			}
			lblTitle.Render(e.Graphics);
			lblAlbum.Render(e.Graphics);

			if (updateState != 0)
			{
				Brush brush = new SolidBrush(updateState == UpdateState.InProgress ? Color.Yellow : Color.LimeGreen);
				//48px for pause/play button, 75 for the RHS area
				e.Graphics.FillRectangle(brush, 48, this.Height - 3, (this.Width - 48 - 75) * updatePercent, 3);
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

		public void SetLabelText(string titleText, string subtitleText, string albumText)
		{
			lblTitle.Text = titleText;
			lblTitle.Subtext = subtitleText;
			lblAlbum.Text = albumText;
		}

		public void SetLabelBrush(Brush brush)
		{
			lblTitle.FontBrush = brush;
			lblAlbum.FontBrush = brush;
		}

		public void SetFonts(Font titleFont, Font albumFont)
		{
			lblTitle.Font = titleFont;
			lblTitle.Subfont = albumFont;
			lblAlbum.Font = albumFont;

			lblTitle.RecalculateBounds();
			lblAlbum.RecalculateBounds();
		}
		
		public void StartVisualiser(WebStreamPlayer player)
		{
			if (Visualiser == null)
			{
				Visualiser = new AudioVisualiser();
				Visualiser.SetBounds(new Rectangle(0, 0, this.Width, this.Height));
				Visualiser.ReloadSettings();
				Visualiser.Start();
				player.SetVisualiser(Visualiser);
			}
		}

		public void StopVisualiser(WebStreamPlayer player)
		{
			if (Visualiser != null)
			{
				player.SetVisualiser(null);
				Visualiser.Stop();
				Visualiser = null;
			}
		}

		public void ReloadVisualiser()
		{
			Visualiser.ReloadSettings();
		}
	}
}
