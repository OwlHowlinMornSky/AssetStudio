using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetStudioGUI {
	public partial class LanguageOptions : Form {
		static public int m_veryFirstSetting;

		private int m_oldLang;
		private int m_newLang;

		public LanguageOptions() {
			InitializeComponent();
			label_restart_note.Visible = (m_veryFirstSetting != Properties.SettingsOHMS.Default.language);
#if DEBUG
			lang_0.Visible = true;
#else
			lang_0.Visible = false;
			lang_0.Checked = false;
			lang_0.Enabled = false;
#endif
			button_ok.Enabled = false;
			m_newLang = m_oldLang = Properties.SettingsOHMS.Default.language;
			switch (m_oldLang) {
#if DEBUG
			case 0:
				lang_0.Checked = true;
				break;
#endif
			case 1:
				lang_en_us.Checked = true;
				break;
			case 2:
				lang_zh_cn.Checked = true;
				break;
			}
		}

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

		private void button_ok_Click(object sender, EventArgs e) {
			if (m_newLang != m_oldLang) {
				if (m_newLang != m_veryFirstSetting) {
					string title = "";
					string text = "";
					switch (m_newLang) {
					case 0:
						title += Properties.StringsMainForm.LangSet_OkMessageboxTitle;
						text += Properties.StringsMainForm.LangSet_OkMessageboxText;
						break;
					case 2:
						title += Properties.StringsMainForm.LangSet_OkMessageboxTitle;
						text += Properties.StringsMainForm.LangSet_OkMessageboxText;
						break;
					default:
						title += Properties.StringsMainForm.LangSet_OkMessageboxTitle;
						text += Properties.StringsMainForm.LangSet_OkMessageboxText;
						break;
					}
					switch (m_oldLang) {
					case 1:
						title += "  " + Properties.StringsMainForm.LangSet_OkMessageboxTitle;
						text += Properties.StringsMainForm.LangSet_OkMessageboxText;
						break;
					case 2:
						title += "  " + Properties.StringsMainForm.LangSet_OkMessageboxTitle;
						text += Properties.StringsMainForm.LangSet_OkMessageboxText;
						break;
					}
					var res = MessageBox.Show(text, title,
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Information
						);
					if (res == DialogResult.Yes || res == DialogResult.No) {
						Properties.SettingsOHMS.Default.language = m_newLang;
						Properties.SettingsOHMS.Default.Save();
						if (res == DialogResult.Yes) {
							DialogResult = DialogResult.OK;
						}
						else {
							DialogResult = DialogResult.Cancel;
						}
						update(m_newLang);
					}
				}
				else {
					Properties.SettingsOHMS.Default.language = m_veryFirstSetting;
					Properties.SettingsOHMS.Default.Save();
					DialogResult = DialogResult.Cancel;
					update(m_veryFirstSetting);
				}
			}
		}

		private void button_cancle_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
		}

		private void changeChoose(int n) {
			m_newLang = n;
			button_ok.Enabled = (m_newLang != m_oldLang);
			//MessageBox.Show(n.ToString());
		}

		private void lang_0_CheckedChanged(object sender, EventArgs e) {
#if DEBUG
			if (lang_0.Checked) {
				changeChoose(0);
			}
#endif
		}

		private void lang_en_us_CheckedChanged(object sender, EventArgs e) {
			if (lang_en_us.Checked) {
				changeChoose(1);
			}
		}

		private void lang_zh_cn_CheckedChanged(object sender, EventArgs e) {
			if (lang_zh_cn.Checked) {
				changeChoose(2);
			}
		}

	}
}
