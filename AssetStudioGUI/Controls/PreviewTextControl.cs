using System.Windows.Forms;

namespace AssetStudioGUI.Controls {
	public partial class PreviewTextControl : UserControl, IPreviewControl {
		public PreviewTextControl() {
			InitializeComponent();
		}

		public void ResetPreview() {
			return;
		}

		public void PreviewText(string text) {
			textBox1.Text = text;
		}

	}
}
