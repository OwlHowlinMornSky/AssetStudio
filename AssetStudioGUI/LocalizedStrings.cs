using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudioGUI {
	static internal class LocalizedStrings {
		static public void load(int lang) {
			m_strings = new string[(uint)Type.COUNT];
			switch (lang) {
			default:
				m_strings[(int)Type.Preview_GL_info0] = Properties.Resources.zls_preview_GL_info0;
				m_strings[(int)Type.Preview_GL_info1] = Properties.Resources.zls_preview_GL_info1;
				m_strings[(int)Type.Preview_GL_unable] = Properties.Resources.zls_preview_GL_cannotPreview;
				m_strings[(int)Type.Preview_Audio_formatHead] = Properties.Resources.zls_preview_audio_format_head;
				m_strings[(int)Type.Load_FinishLoading] = Properties.Resources.zls_load_finishLoading;
				m_strings[(int)Type.Export_Exporting] = Properties.Resources.zls_Exporting;
				break;
			case 2:
				m_strings[(int)Type.Preview_GL_info0] = Properties.Resources.zls_preview_GL_info0_zh_CN;
				m_strings[(int)Type.Preview_GL_info1] = Properties.Resources.zls_preview_GL_info1_zh_CN;
				m_strings[(int)Type.Preview_GL_unable] = Properties.Resources.zls_preview_GL_cannotPreview_zh_CN;
				m_strings[(int)Type.Preview_Audio_formatHead] = Properties.Resources.zls_preview_audio_format_head_zh_CN;
				m_strings[(int)Type.Load_FinishLoading] = Properties.Resources.zls_load_finishLoading_zh_CN;
				m_strings[(int)Type.Export_Exporting] = Properties.Resources.zls_Exporting_zh_CN;
				break;
			}
		}

		public enum Type: uint {
			Preview_GL_info0 = 0,
			Preview_GL_info1,
			Preview_GL_unable,
			Preview_Audio_formatHead,
			Load_FinishLoading,
			Export_Exporting,
			COUNT
		}

		static private string[] m_strings;

		static public string Get(Type key) {
			return m_strings[(int)key];
		}
	}
}
