using System.Globalization;
using System.Threading;

namespace AssetStudioGUI {
	static class CultureSwitcher {
		public static void update(int lang) {
			switch (lang) {
#if DEBUG
			case 0:
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
				break;
#endif
			case 2:
				Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
				break;
			default:
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
				break;
			}
		}
	}
}
