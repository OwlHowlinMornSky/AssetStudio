using AssetStudio;
using AssetStudioGUI.Controls;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Windows.Forms;
using Font = AssetStudio.Font;

namespace AssetStudioGUI {
	public partial class PreviewForm : Form {
		public PreviewForm() {
			InitializeComponent();
		}

		internal void Preview(AssetItem assetItem) {
			if (assetItem == null)
				return;

			Text = $"{assetItem.Text} {{{assetItem.m_PathID}}}";
			textBox_info.Text =
				$"Name: '{assetItem.Text}'." + Environment.NewLine +
				$"PathID: {assetItem.m_PathID}." + Environment.NewLine +
				$"Type: {assetItem.TypeString}." + Environment.NewLine +
				$"Container: {assetItem.Container}." + Environment.NewLine +
				$"Size: {assetItem.FullSize} byte(s)."
				;

			try {
				Control control = null;
				switch (assetItem.Asset) {
				case Texture2D m_Texture2D: {
					var prev = new PreviewImageControl();
					prev.PreviewTexture2D(assetItem, m_Texture2D);
					control = prev;
					break;
				}
				case AudioClip m_AudioClip: {
					var prev = new PreviewAudioControl();
					prev.PreviewAudioClip(assetItem, m_AudioClip);
					control = prev;
					break;
				}
				case Shader m_Shader: {
					var prev = new PreviewTextControl();
					var str = ShaderConverter.Convert(m_Shader);
					//PreviewText(str == null ? "Serialized Shader can't be read" : str.Replace("\n", "\r\n"));
					prev.PreviewText(str == null ? Properties.Strings.Preview_Shader_Serialized : str.Replace("\n", "\r\n"));
					control = prev;
					break;
				}
				case TextAsset m_TextAsset: {
					var prev = new PreviewTextControl();
					var text = Encoding.UTF8.GetString(m_TextAsset.m_Script);
					text = text.Replace("\n", "\r\n").Replace("\0", "");
					prev.PreviewText(text);
					control = prev;
					break;
				}
				case MonoBehaviour m_MonoBehaviour: {
					var prev = new PreviewTextControl();
					var obj = m_MonoBehaviour.ToType();
					if (obj == null) {
						//var type = m_studio.MonoBehaviourToTypeTree(m_MonoBehaviour);
						//obj = m_MonoBehaviour.ToType(type);
						return;
					}
					var str = JsonConvert.SerializeObject(obj, Formatting.Indented);
					prev.PreviewText(str);
					control = prev;
					break;
				}
				case Font m_Font: {
					var prev = new PreviewFontControl();
					prev.PreviewFont(m_Font);
					control = prev;
					break;
				}
				case Mesh m_Mesh: {
					var prev = new PreviewMeshControl();
					//control = prev;
					prev.Dock = DockStyle.Fill;
					tabPage_Preview.Controls.Add(prev);
					prev.Preview(m_Mesh);
					break;
				}
				case VideoClip _:
				case MovieTexture _:
					//StatusStripUpdate("Only supported export.");
					Logger.Default.Log(LoggerEvent.Info, Properties.Strings.Preview_OnlyExport);
					break;
				case Sprite m_Sprite: {
					var prev = new PreviewImageControl();
					prev.PreviewSprite(assetItem, m_Sprite);
					control = prev;
					break;
				}
				case Animator _:
					//StatusStripUpdate("Can be exported to FBX file.");
					Logger.Default.Log(LoggerEvent.Info, Properties.Strings.Preview_OnlyExport_FBX);
					break;
				case AnimationClip _:
					//StatusStripUpdate("Can be exported with Animator or Objects");
					Logger.Default.Log(LoggerEvent.Info, Properties.Strings.Preview_OnlyExport_Animator);
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
				if (control != null) {
					control.Dock = DockStyle.Fill;
					tabPage_Preview.Controls.Add(control);
				}
			}
			catch (Exception e) {
				//MessageBox.Show($"Preview {assetItem.Type}:{assetItem.Text} error\r\n{e.Message}\r\n{e.StackTrace}");
				MessageBox.Show(
					string.Format(Properties.Strings.Preview_Exception, assetItem.Type, assetItem.Text)
					+ "\n" + e.Message + "\n" + e.StackTrace
					);
			}

			string xmlDump = StudioCore.m_studio.DumpAsset(assetItem.Asset);
			string jsonDump = StudioCore.m_studio.DumpAssetJson(assetItem.Asset);

			ui_tabRight_page1_dumpTextBox.Text = xmlDump.Length < ui_tabRight_page1_dumpTextBox.MaxLength ? xmlDump : "[Dump is too long to show. Please try export the dump.]";
			ui_tabRight_page2_dumpJsonTextBox.Text = jsonDump.Length < ui_tabRight_page2_dumpJsonTextBox.MaxLength ? jsonDump : "[Dump is too long to show. Please try export the dump.]";
		}

		private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e) {
			foreach (var control in tabPage_Preview.Controls) {
				if (control is not IPreviewControl pc)
					continue;
				pc.ResetPreview();
			}
			WindowState = FormWindowState.Normal;
		}

	}
}
