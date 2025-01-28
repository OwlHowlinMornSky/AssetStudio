using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Font = AssetStudio.Font;

namespace AssetStudioGUI.Controls {
	public partial class PreviewFontControl : UserControl, IPreviewControl {
		public PreviewFontControl() {
			InitializeComponent();
		}

		public void ResetPreview() {
			return;
		}

		[DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

		internal void PreviewFont(Font m_Font) {
			if (m_Font.m_FontData != null) {
				var data = Marshal.AllocCoTaskMem(m_Font.m_FontData.Length);
				Marshal.Copy(m_Font.m_FontData, 0, data, m_Font.m_FontData.Length);

				uint cFonts = 0;
				var re = AddFontMemResourceEx(data, (uint)m_Font.m_FontData.Length, IntPtr.Zero, ref cFonts);
				if (re != IntPtr.Zero) {
					using var pfc = new PrivateFontCollection();
					pfc.AddMemoryFont(data, m_Font.m_FontData.Length);
					Marshal.FreeCoTaskMem(data);
					if (pfc.Families.Length > 0) {
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 0;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 80;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 16, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 81;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 12, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 138;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 18, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 195;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 24, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 252;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 36, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 309;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 48, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 366;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 60, FontStyle.Regular);
						ui_tabRight_page0_fontPreviewBox.SelectionStart = 423;
						ui_tabRight_page0_fontPreviewBox.SelectionLength = 55;
						ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 72, FontStyle.Regular);
					}
					return;
				}
			}
			//StatusStripUpdate("Unsupported font for preview. Try to export.");
			AssetStudio.Logger.Default.Log(AssetStudio.LoggerEvent.Info, Properties.Strings.Preview_Font_Unsupported);
		}
	}
}
