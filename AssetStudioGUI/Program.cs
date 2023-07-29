using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AssetStudioGUI {
	static class Program {
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
			do {
				m_exitForChanges = false;
				LanguageOptions.m_veryFirstSetting = Properties.SettingsOHMS.Default.language;
				Application.Run(new AssetStudioGUIForm());
			}
			while (m_exitForChanges);
		}

		public static bool m_exitForChanges;
	}
}
