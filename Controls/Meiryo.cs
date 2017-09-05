using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ListenMoeClient
{
    class Meiryo
    {
        private static PrivateFontCollection fonts = new PrivateFontCollection();

		private static Dictionary<float, Font> fontCache = new Dictionary<float, Font>();

        static Meiryo()
        {
            byte[] fontData = Properties.Resources.Meiryo;
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

			var font = new Font(fonts.Families[0], size, GraphicsUnit.Point);
			fontCache.Add(size, font);
			return font;
        }

		public static FontFamily GetFontFamily()
		{
			return fonts.Families[0];
		}
    }
}
