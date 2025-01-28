using System;
using System.Windows.Forms;
using AssetStudio;

namespace AssetStudioGUI {
	internal class GUILogger(Action<string> action) : ILogger {
		public bool ShowErrorMessage = true;
		private readonly Action<string> action = action;

		public void Log(LoggerEvent loggerEvent, string message) {
			switch (loggerEvent) {
			case LoggerEvent.Error:
				if (ShowErrorMessage) {
					MessageBox.Show(message);
				}
				break;
			default:
				action(message);
				break;
			}
		}

	}
}
