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
			if (text.Length < textBox1.MaxLength)
				textBox1.Text = text;
			else
				textBox1.Text = "[The content of this text file is too long to preview. Please try to export it.]";
		}

	}
}
