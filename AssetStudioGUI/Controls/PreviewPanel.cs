using AssetStudio;
using Newtonsoft.Json;
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
	public partial class PreviewPanel : UserControl {
		public PreviewPanel() {
			InitializeComponent();
		}

		private int m_previewLoaded;

		enum PreviewType {
			None = 0,
			FMOD,
			Font,
			GL,
			Text,
			ClassText,
			Dump,
			DumpJson,
			COUNT
		}

		private void ResetPreview() {
			ui_tabRight_page0.BackgroundImage = Properties.Resources.preview;
			ui_tabRight_page0.BackgroundImageLayout = ImageLayout.Center;

			SwitchPreviewPage(PreviewType.None);

			ui_tabRight_page0_assetInfoLabel.Visible = false;
			ui_tabRight_page0_assetInfoLabel.Text = null;

			ui_tabRight_page1_dumpTextBox.Text = null;
			ui_tabRight_page2_dumpJsonTextBox.Text = null;

			m_previewLoaded = 0;

			//StatusStripUpdate("");
		}

		private void SetVisiblePreviewFont(bool visible) {
			if (visible) {
				ui_tabRight_page0_fontPreviewBox.Dock = DockStyle.Fill;
				ui_tabRight_page0_fontPreviewBox.Visible = false;
			}
			else {
				ui_tabRight_page0_fontPreviewBox.Visible = true;
				ui_tabRight_page0_fontPreviewBox.Dock = DockStyle.None;
			}
		}

		private void SetVisiblePreviewText(bool visible) {
			if (visible) {
				ui_tabRight_page0_textPreviewBox.Dock = DockStyle.Fill;
				ui_tabRight_page0_textPreviewBox.Visible = false;
			}
			else {
				ui_tabRight_page0_textPreviewBox.Visible = true;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_textPreviewBox.Dock = DockStyle.None;
			}
		}

		private void SetVisiblePreviewClassText(bool visible) {
			if (visible) {
				ui_tabRight_page0_classTextPreview.Dock = DockStyle.Fill;
				ui_tabRight_page0_classTextPreview.Visible = false;
			}
			else {
				ui_tabRight_page0_classTextPreview.Visible = true;
				ui_tabRight_page0_classTextPreview.Text = null;
				ui_tabRight_page0_classTextPreview.Dock = DockStyle.None;
			}
		}

		private void SwitchPreviewPage(PreviewType type) {
			switch (type) {
			case PreviewType.None:
				/*ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_glPreview.Visible = false;*/
				SetVisiblePreviewFont(false);
				SetVisiblePreviewText(false);
				SetVisiblePreviewClassText(false);
				ui_tabRight_page1_dumpTextBox.Text = null;
				ui_tabRight_page2_dumpJsonTextBox.Text = null;
				break;
			case PreviewType.FMOD:
				/*ui_tabRight_page0_FMODpanel.Visible = true;
				ui_tabRight_page0_glPreview.Visible = false;*/
				SetVisiblePreviewFont(false);
				SetVisiblePreviewText(false);
				SetVisiblePreviewClassText(false);
				break;
			case PreviewType.Font:
				SetVisiblePreviewFont(true);
				/*ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_glPreview.Visible = false;*/
				SetVisiblePreviewText(false);
				SetVisiblePreviewClassText(false);
				break;
			case PreviewType.GL:
				/*ui_tabRight_page0_glPreview.Visible = true;
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();*/
				SetVisiblePreviewFont(false);
				SetVisiblePreviewText(false);
				SetVisiblePreviewClassText(false);
				break;
			case PreviewType.Text:
				/*ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_glPreview.Visible = false;*/
				SetVisiblePreviewFont(false);
				SetVisiblePreviewText(true);
				SetVisiblePreviewClassText(false);
				break;
			case PreviewType.ClassText:
				/*ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_glPreview.Visible = false;*/
				ui_tabRight_page0_assetInfoLabel.Visible = false;
				ui_tabRight_page0_assetInfoLabel.Text = null;
				SetVisiblePreviewFont(false);
				SetVisiblePreviewText(false);
				SetVisiblePreviewClassText(true);
				ui_tabRight_page1_dumpTextBox.Text = null;
				ui_tabRight_page2_dumpJsonTextBox.Text = null;
				break;
			}
		}

#nullable enable
		internal void PreviewAsset(AssetItem? assetItem) {
			if (assetItem == null) {
				ResetPreview();
				return;
			}
			if ((m_previewLoaded & (1 << 0)) != 0) {
				return;
			}
			try {
				switch (assetItem.Asset) {
				case Texture2D m_Texture2D:
					previewImage1.PreviewTexture2D(assetItem, m_Texture2D);
					break;
				case AudioClip m_AudioClip:
					//PreviewAudioClip(assetItem, m_AudioClip);
					break;
				case Shader m_Shader:
					PreviewShader(m_Shader);
					break;
				case TextAsset m_TextAsset:
					PreviewTextAsset(m_TextAsset);
					break;
				case MonoBehaviour m_MonoBehaviour:
					PreviewMonoBehaviour(m_MonoBehaviour);
					break;
				case Font m_Font:
					PreviewFont(m_Font);
					break;
				case Mesh m_Mesh:
					PreviewMesh(m_Mesh);
					break;
				case VideoClip _:
				case MovieTexture _:
					//StatusStripUpdate("Only supported export.");
					//StatusStripUpdate(Properties.Strings.Preview_OnlyExport);
					break;
				case Sprite m_Sprite:
					previewImage1.PreviewSprite(assetItem, m_Sprite);
					break;
				case Animator _:
					//StatusStripUpdate("Can be exported to FBX file.");
					//StatusStripUpdate(Properties.Strings.Preview_OnlyExport_FBX);
					break;
				case AnimationClip _:
					//StatusStripUpdate("Can be exported with Animator or Objects");
					//StatusStripUpdate(Properties.Strings.Preview_OnlyExport_Animator);
					break;
				default:
					//PreviewText(m_studio.DumpAsset(assetItem.Asset));
					break;
				}
				/*if (ui_menuOptions_displayInfo.Checked && lastSelectedItem.InfoText != null) {
					ui_tabRight_page0_assetInfoLabel.Text = lastSelectedItem.InfoText;
					ui_tabRight_page0_assetInfoLabel.Visible = true;
				}
				else {
					ui_tabRight_page0_assetInfoLabel.Text = null;
					ui_tabRight_page0_assetInfoLabel.Visible = false;
				}*/
				m_previewLoaded |= 1 << 0;
			}
			catch (Exception e) {
				//MessageBox.Show($"Preview {assetItem.Type}:{assetItem.Text} error\r\n{e.Message}\r\n{e.StackTrace}");
				MessageBox.Show(
					String.Format(Properties.Strings.Preview_Exception, assetItem.Type, assetItem.Text)
					+ "\n" + e.Message + "\n" + e.StackTrace);
			}
		}
#nullable disable


		private void PreviewShader(Shader m_Shader) {
			var str = ShaderConverter.Convert(m_Shader);
			//PreviewText(str == null ? "Serialized Shader can't be read" : str.Replace("\n", "\r\n"));
			PreviewText(str == null ? Properties.Strings.Preview_Shader_Serialized : str.Replace("\n", "\r\n"));
		}

		private void PreviewTextAsset(TextAsset m_TextAsset) {
			var text = Encoding.UTF8.GetString(m_TextAsset.m_Script);
			text = text.Replace("\n", "\r\n").Replace("\0", "");
			PreviewText(text);
		}

		private void PreviewMonoBehaviour(MonoBehaviour m_MonoBehaviour) {
			var obj = m_MonoBehaviour.ToType();
			if (obj == null) {
				//var type = m_studio.MonoBehaviourToTypeTree(m_MonoBehaviour);
				//obj = m_MonoBehaviour.ToType(type);
				return;
			}
			var str = JsonConvert.SerializeObject(obj, Formatting.Indented);
			PreviewText(str);
		}

		[DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

		private void PreviewFont(Font m_Font) {
			if (m_Font.m_FontData != null) {
				var data = Marshal.AllocCoTaskMem(m_Font.m_FontData.Length);
				Marshal.Copy(m_Font.m_FontData, 0, data, m_Font.m_FontData.Length);

				uint cFonts = 0;
				var re = AddFontMemResourceEx(data, (uint)m_Font.m_FontData.Length, IntPtr.Zero, ref cFonts);
				if (re != IntPtr.Zero) {
					using (var pfc = new PrivateFontCollection()) {
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
							SwitchPreviewPage(PreviewType.Font);
						}
					}
					return;
				}
			}
			//StatusStripUpdate("Unsupported font for preview. Try to export.");
			//StatusStripUpdate(Properties.Strings.Preview_Font_Unsupported);
		}

		private void PreviewMesh(Mesh m_Mesh) {
			SwitchPreviewPage(PreviewType.GL);
		}

		private void PreviewText(string text) {
			ui_tabRight_page0_textPreviewBox.Text = text;
			SwitchPreviewPage(PreviewType.Text);
		}

		private void PreviewDump(AssetItem assetItem) {
			if ((m_previewLoaded & (1 << 1)) != 0)
				return;
			//ui_tabRight_page1_dumpTextBox.Text = m_studio.DumpAsset(assetItem.Asset);
			m_previewLoaded |= 1 << 1;
			SwitchPreviewPage(PreviewType.Dump);
		}

		private void PreviewDumpJSON(AssetItem assetItem) {
			if ((m_previewLoaded & (1 << 2)) != 0)
				return;
			//ui_tabRight_page2_dumpJsonTextBox.Text = m_studio.DumpAssetJson(assetItem.Asset);
			m_previewLoaded |= 1 << 2;
			SwitchPreviewPage(PreviewType.DumpJson);
		}

		private void Ui_tabRight_tab_SelectedIndexChanged(object sender, EventArgs e) {
			if ((m_previewLoaded & (1 << 3)) != 0) {
				return;
			}
			/*if (lastSelectedItem == null) {
				ResetPreview();
				return;
			}
			switch (ui_tabRight_tab.SelectedIndex) {
			case 0:
				if (Properties.Settings.Default.enablePreview)
					PreviewAsset(lastSelectedItem);
				break;
			case 1:
				PreviewDump(lastSelectedItem);
				break;
			case 2:
				PreviewDumpJSON(lastSelectedItem);
				break;
			}*/
		}
	}
}
