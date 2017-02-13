using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
    class OpenSans
    {
        private static PrivateFontCollection fonts = new PrivateFontCollection();

		private static Dictionary<float, Font> fontCache = new Dictionary<float, Font>();

        static OpenSans()
        {
            byte[] fontData = Properties.Resources.OpenSans_Regular;
			var handle = GCHandle.Alloc(fontData, GCHandleType.Pinned);
			IntPtr pointer = handle.AddrOfPinnedObject();
			try
			{
				fonts.AddMemoryFont(pointer, fontData.Length);
			}
			finally
			{
				handle.Free();
			}
        }

        public static Font GetFont(float size)
        {
			if (fontCache.ContainsKey(size))
				return fontCache[size];

			var font = new Font(fonts.Families[0], size);
			fontCache.Add(size, font);
			return font;
        }

		public static FontFamily GetFontFamily()
		{
			return fonts.Families[0];
		}
    }
}
