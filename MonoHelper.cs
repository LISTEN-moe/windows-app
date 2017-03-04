using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
	class MonoHelper
	{
		public static bool IsWindows()
		{
			PlatformID p = Environment.OSVersion.Platform;
			return p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows || p == PlatformID.WinCE;
		}
	}
}
