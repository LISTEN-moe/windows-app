using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenMoeClient
{
	class Globals
	{
		public static string VERSION = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.')); //Strip build number
		public static string USER_AGENT = "LISTEN.moe Desktop Client v" + VERSION + " (https://github.com/anonymousthing/ListenMoeClient)";
	}
}
