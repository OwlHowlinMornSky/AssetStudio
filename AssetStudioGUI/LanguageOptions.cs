using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AssetStudioGUI {
	public partial class LanguageOptions : Form {
		public enum Language : uint {
			auto = 0,
			en_US,
			zh_CN
		}

		private static Language m_veryFirstLangChoice;
		private static Language m_veryFirstLang;
		private static string m_detectedLangString;
		private static Language m_detectedLangChoice;
		private static Language m_detectedLang;

		private Language m_oldLangChoice;
		private Language m_oldLang;
		private Language m_newLangChoice;

		public LanguageOptions() {
			InitializeComponent();

			if (m_detectedLangChoice == Language.auto) {
				ui_lang_auto.Checked = false;
				ui_lang_auto.Enabled = false;
			}
			else {
				string strDetected = "";
				switch (m_detectedLangChoice) {
				case Language.en_US:
					strDetected = ui_lang_en_us.Text;
					break;
				case Language.zh_CN:
					strDetected = ui_lang_zh_cn.Text;
					break;
				}
				ui_label_detected.Text = strDetected + " [" + m_detectedLangString + "]";
			}
			ui_button_ok.Enabled = false;

			m_oldLangChoice = (Language)Properties.SettingsOHMS.Default.language;
			m_oldLang = computeLanguage(m_oldLangChoice);
			m_newLangChoice = m_oldLangChoice;

			ui_label_restart_note.Visible = (m_oldLang != m_veryFirstLang);

			switch (m_oldLangChoice) {
			case Language.auto:
				if (m_detectedLang != Language.auto) {
					ui_lang_auto.Checked = true;
				}
				break;
			case Language.en_US:
				ui_lang_en_us.Checked = true;
				break;
			case Language.zh_CN:
				ui_lang_zh_cn.Checked = true;
				break;
			}
		}

		public static void update(Language lang) {
			lang = computeLanguage(lang);
			switch (lang) {
			case Language.en_US:
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
				break;
			case Language.zh_CN:
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
				break;
			default:
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
				break;
			}
		}

		public static Language computeLanguage(Language i) {
			switch (i) {
			case Language.auto:
				return m_detectedLang;
			case Language.en_US:
			case Language.zh_CN:
				return i;
			default:
				return Language.en_US;
			}
		}

		public static void initWhenOpenForm() {
			detectAuto();

			m_veryFirstLangChoice = (Language)Properties.SettingsOHMS.Default.language;
			m_veryFirstLang = computeLanguage(m_veryFirstLangChoice);

			update(m_veryFirstLang);
		}

		private static void detectAuto() {
			m_detectedLangString = CultureInfo.CurrentUICulture.ToString();
			var lang = m_detectedLangString.ToLower();

			Language res = Language.auto;
			if (lang.Contains("en")) {
				res = Language.en_US;
			}
			else if (lang.Contains("zh") /*&& lang.Contains("cn")*/) {
				res = Language.zh_CN;
			}

			m_detectedLangChoice = res;
			if (res == Language.auto) {
				m_detectedLang = Language.en_US;
			}
			else {
				m_detectedLang = res;
			}
			return;
		}

		#region Exit
		private void changeSetting(Language target) {
			Properties.SettingsOHMS.Default.language = (uint)target;
			Properties.SettingsOHMS.Default.Save();
		}

		private void ui_button_ok_Click(object sender, EventArgs e) {
			if (m_newLangChoice != m_oldLangChoice) {
				Language newLang = computeLanguage(m_newLangChoice);

				if (newLang != m_oldLang) {
					update(newLang);

					string title = Properties.Strings.LangSet_OkMessageboxTitle;
					string text = Properties.Strings.LangSet_OkMessageboxText;

					MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				changeSetting(m_newLangChoice);
				Close();
			}
		}

		private void ui_button_cancle_Click(object sender, EventArgs e) {
			Close();
		}
		#endregion Exit

		#region Changes
		private void changeChoose(Language n) {
			m_newLangChoice = n;
			ui_button_ok.Enabled = (m_newLangChoice != m_oldLangChoice);
		}

		private void ui_lang_auto_CheckedChanged(object sender, EventArgs e) {
			if (ui_lang_auto.Checked) {
				changeChoose(Language.auto);
			}
		}

		private void ui_lang_en_us_CheckedChanged(object sender, EventArgs e) {
			if (ui_lang_en_us.Checked) {
				changeChoose(Language.en_US);
			}
		}

		private void ui_lang_zh_cn_CheckedChanged(object sender, EventArgs e) {
			if (ui_lang_zh_cn.Checked) {
				changeChoose(Language.zh_CN);
			}
		}
		#endregion Changes
	}
}
