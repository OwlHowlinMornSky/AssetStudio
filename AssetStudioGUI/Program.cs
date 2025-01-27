using System;
using System.Windows.Forms;

namespace AssetStudioGUI {
	static class Program {
		internal static bool Runtime = false;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
#if !NETFRAMEWORK
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Runtime = true;
			Application.Run(new AssetStudioGUIForm());
		}
	}
}
