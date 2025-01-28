using AssetStudio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AssetStudioGUI.Controls {
	public partial class PreviewAudioControl : UserControl, IPreviewControl {
		public PreviewAudioControl() {
			InitializeComponent();
			/// 保存工作区初始大小。
			m_prevSize = ClientSize;
			/// 把 暂停、继续 的按钮设置到同一位置。
			button_resume.Top = button_pause.Top;
			/// 把 停止、暂停、播放 的标签设置到同一位置。
			label_playing.Top = label_stopped.Top;
			label_paused.Top = label_stopped.Top;
		}

		public void ResetPreview() {
			timer1.Stop();

			SuspendLayout();

			progressBar1.Value = 0;
			label_timer.Text = "00:00.00";
			label_duration.Text = "00:00.00";

			label_playing.Visible = false;
			label_paused.Visible = false;
			label_stopped.Visible = true;

			button_pause.Visible = true;
			button_resume.Visible = false;

			label_sampleRate.Text = "";

			ResumeLayout();

			FMOD_Reset();
		}

		internal void PreviewAudioClip(AssetItem assetItem, AudioClip audioClip) {
			if (Program.Runtime)
				FMOD_Init();

			/// 计算 Info。
			assetItem.InfoText = Properties.Strings.Preview_Audio_formatHead;
			if (audioClip.version[0] < 5) {
				assetItem.InfoText += audioClip.m_Type switch {
					FMODSoundType.ACC => "ACC",
					FMODSoundType.AIFF => "AIFF",
					FMODSoundType.IT => "Impulse tracker",
					FMODSoundType.MOD => "Protracker / Fasttracker MOD",
					FMODSoundType.MPEG => "MP2/MP3 MPEG",
					FMODSoundType.OGGVORBIS => "Ogg vorbis",
					FMODSoundType.S3M => "ScreamTracker 3",
					FMODSoundType.WAV => "Microsoft WAV",
					FMODSoundType.XM => "FastTracker 2 XM",
					FMODSoundType.XMA => "Xbox360 XMA",
					FMODSoundType.VAG => "PlayStation Portable ADPCM",
					FMODSoundType.AUDIOQUEUE => "iPhone",
					_ => "Unknown",
				};
			}
			else {
				assetItem.InfoText += audioClip.m_CompressionFormat switch {
					AudioCompressionFormat.PCM => "PCM",
					AudioCompressionFormat.Vorbis => "Vorbis",
					AudioCompressionFormat.ADPCM => "ADPCM",
					AudioCompressionFormat.MP3 => "MP3",
					AudioCompressionFormat.PSMVAG => "PlayStation Portable ADPCM",
					AudioCompressionFormat.HEVAG => "PSVita ADPCM",
					AudioCompressionFormat.XMA => "Xbox360 XMA",
					AudioCompressionFormat.AAC => "AAC",
					AudioCompressionFormat.GCADPCM => "Nintendo 3DS/Wii DSP",
					AudioCompressionFormat.ATRAC9 => "PSVita ATRAC9",
					_ => "Unknown",
				};
			}

			var audioData = audioClip.m_AudioData.GetData();
			if (audioData == null || audioData.Length == 0)
				return;
			var exinfo = new FMOD.CREATESOUNDEXINFO();

			exinfo.cbsize = Marshal.SizeOf(exinfo);
			exinfo.length = (uint)audioClip.m_Size;

			var result = m_system.createSound(audioData, FMOD.MODE.OPENMEMORY | m_loopMode, ref exinfo, out m_sound);
			if (FMOD_Check(result))
				return;

			m_sound.getNumSubSounds(out var numsubsounds);

			if (numsubsounds > 0) {
				result = m_sound.getSubSound(0, out var subsound);
				if (result == FMOD.RESULT.OK) {
					m_sound = subsound;
				}
			}

			result = m_sound.getLength(out m_durationInMilliseconds, FMOD.TIMEUNIT.MS);
			if (FMOD_Check(result))
				return;

			result = m_system.playSound(m_sound, null, true, out m_channel);
			if (FMOD_Check(result))
				return;

			result = m_channel.getFrequency(out var frequency);
			if (FMOD_Check(result))
				return;

			label_sampleRate.Text = $"{frequency} Hz";
		}

		/// <summary>
		/// 本控件加载时初始化FMOD。
		/// </summary>
		private void PreviewFMOD_Load(object sender, EventArgs e) {
		}

		/// <summary>
		/// 用 客户区大小变化量 来决定 内部控件的移动。
		/// </summary>
		private Size m_prevSize;
		private void PreviewFMOD_ClientSizeChanged(object sender, EventArgs e) {
			int dx = ClientSize.Width - m_prevSize.Width;
			int dy = ClientSize.Height - m_prevSize.Height;

			m_prevSize = ClientSize;

			/// 大小变化量直接除以2，会导致变化量误差，所以把误差加进 prevSize 保存起来。
			m_prevSize.Width -= dx % 2;
			m_prevSize.Height -= dy % 2;
			dx /= 2;
			dy /= 2;

			/// Update positions of children controls。
			SuspendLayout();
			foreach (Control item in Controls) {
				item.Left += dx;
				item.Top += dy;
			}
			ResumeLayout();
		}

		/// <summary>
		/// Click "Play" button.
		/// </summary>
		private void Button_Play_Click(object sender, EventArgs e) {
			if (m_sound == null && m_channel == null)
				return;

			timer1.Start();

			var result = m_channel.isPlaying(out var playing);
			if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
				if (FMOD_Check(result)) {
					return;
				}
			}

			if (playing) {
				result = m_channel.stop();
				if (FMOD_Check(result)) {
					return;
				}

				result = m_system.playSound(m_sound, null, false, out m_channel);
				if (FMOD_Check(result)) {
					return;
				}

				/// Switch to "Pause" button.
				button_pause.Visible = true;
				button_resume.Visible = false;
			}
			else {
				result = m_system.playSound(m_sound, null, false, out m_channel);
				if (FMOD_Check(result)) {
					return;
				}

				/// Show "Playing".
				label_playing.Visible = true;
				label_paused.Visible = false;
				label_stopped.Visible = false;

				if (progressBar1.Value > 0) {
					uint newms = m_durationInMilliseconds / 1000 * (uint)progressBar1.Value;

					result = m_channel.setPosition(newms, FMOD.TIMEUNIT.MS);
					if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
						if (FMOD_Check(result)) {
							return;
						}
					}

				}
			}
		}

		/// <summary>
		/// Click "Pause"/"Resume" button.
		/// </summary>
		private void Button_Pause_Click(object sender, EventArgs e) {
			if (m_sound != null && m_channel != null) {
				var result = m_channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}

				if (playing) {
					result = m_channel.getPaused(out var paused);
					if (FMOD_Check(result)) {
						return;
					}
					result = m_channel.setPaused(!paused);
					if (FMOD_Check(result)) {
						return;
					}

					if (paused) {
						/// Show "Playing".
						label_playing.Visible = true;
						label_paused.Visible = false;
						label_stopped.Visible = false;

						/// Show "Pause" button.
						button_pause.Visible = true;
						button_resume.Visible = false;

						timer1.Start();
					}
					else {
						/// Show "Paused".
						label_playing.Visible = false;
						label_paused.Visible = true;
						label_stopped.Visible = false;

						/// Show "Resume" button.
						button_pause.Visible = false;
						button_resume.Visible = true;

						timer1.Stop();
					}
				}
			}
		}

		/// <summary>
		/// Click "Stop" button.
		/// </summary>
		private void Button_Stop_Click(object sender, EventArgs e) {
			if (m_channel != null) {
				var result = m_channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}

				if (playing) {
					result = m_channel.stop();
					if (FMOD_Check(result)) {
						return;
					}
					//channel = null;
					/// don't FMOD_Reset, it will nullify the sound
					timer1.Stop();
					progressBar1.Value = 0;
					label_timer.Text = "00:00.00";

					/// Show "Stopped".
					label_playing.Visible = false;
					label_paused.Visible = false;
					label_stopped.Visible = true;

					/// Show "Pause" button.
					button_pause.Visible = true;
					button_resume.Visible = false;
				}
			}
		}

		private void Button_Loop_CheckedChanged(object sender, EventArgs e) {
			FMOD.RESULT result;

			m_loopMode = button_loop.Checked ? FMOD.MODE.LOOP_NORMAL : FMOD.MODE.LOOP_OFF;

			if (m_sound != null) {
				result = m_sound.setMode(m_loopMode);
				if (FMOD_Check(result)) {
					return;
				}
			}

			if (m_channel != null) {
				result = m_channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}

				result = m_channel.getPaused(out var paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}

				if (playing || paused) {
					result = m_channel.setMode(m_loopMode);
					if (FMOD_Check(result)) {
						return;
					}
				}
			}
		}

		private void VolumeBar_ValueChanged(object sender, EventArgs e) {
			m_volume = Convert.ToSingle(volumeBar.Value) / 10;

			var result = m_masterSoundGroup.setVolume(m_volume);
			if (FMOD_Check(result)) {
				return;
			}
		}

		private void ProgressBar_Scroll(object sender, EventArgs e) {
			if (m_channel != null) {
				uint newms = m_durationInMilliseconds / 1000 * (uint)progressBar1.Value;
				label_timer.Text = string.Format("{0:D2}:{1:D2}.{2:D2}", newms / 1000 / 60, newms / 1000 % 60, newms / 10 % 100);
			}
		}

		private void ProgressBar_MouseDown(object sender, MouseEventArgs e) {
			timer1.Stop();
		}

		private void ProgressBar_MouseUp(object sender, MouseEventArgs e) {
			if (m_channel != null) {
				uint newms = m_durationInMilliseconds / 1000 * (uint)progressBar1.Value;

				var result = m_channel.setPosition(newms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}


				result = m_channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_Check(result)) {
						return;
					}
				}

				if (playing) {
					timer1.Start();
				}
			}
		}

		private void Timer_Tick(object sender, EventArgs e) {
			uint ms = 0;
			bool playing = false;
			bool paused = false;

			if (m_channel != null) {
				var result = m_channel.getPosition(out ms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_Check(result);
				}

				result = m_channel.isPlaying(out playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_Check(result);
				}

				result = m_channel.getPaused(out paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_Check(result);
				}
			}

			label_timer.Text = string.Format("{0:D2}:{1:D2}.{2:D2}", ms / 1000 / 60, ms / 1000 % 60, ms / 10 % 100);
			progressBar1.Value = (int)(ms * 1000 / m_durationInMilliseconds);

			if (paused) {
				label_playing.Visible = false;
				label_paused.Visible = true;
				label_stopped.Visible = false;
			}
			else if (playing) {
				label_playing.Visible = true;
				label_paused.Visible = false;
				label_stopped.Visible = false;
			}
			else {
				label_playing.Visible = false;
				label_paused.Visible = false;
				label_stopped.Visible = true;
			}

			if (m_system != null && m_channel != null) {
				m_system.update();
			}
		}

		private FMOD.System m_system = null;
		private FMOD.SoundGroup m_masterSoundGroup = null;
		private FMOD.Channel m_channel = null;
		private FMOD.Sound m_sound = null;
		private FMOD.MODE m_loopMode = FMOD.MODE.LOOP_OFF;
		private uint m_durationInMilliseconds = 1000;
		private float m_volume = 0.8f;

		private void FMOD_Init() {
			FMOD_Reset();

			FMOD.RESULT result;

			result = FMOD.Factory.System_Create(out m_system);
			if (FMOD_Check(result)) {
				return;
			}

			result = m_system.getVersion(out var version);
			FMOD_Check(result);
			if (version < FMOD.VERSION.number) {
				MessageBox.Show($"Error!  You are using an old version of FMOD {version:X}.  This program requires {FMOD.VERSION.number:X}.");
				return;
			}

			result = m_system.init(2, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
			if (FMOD_Check(result)) {
				return;
			}

			result = m_system.getMasterSoundGroup(out m_masterSoundGroup);
			if (FMOD_Check(result)) {
				return;
			}

			result = m_masterSoundGroup.setVolume(m_volume);
			if (FMOD_Check(result)) {
				return;
			}
		}

		private void FMOD_Reset() {
			/*if (m_channel?.isValid() ?? false) {
				var result = m_channel.stop();
				FMOD_Check(result);
			}
			m_channel = null;*/

			if (m_sound?.isValid() ?? false) {
				var result = m_sound.release();
				FMOD_Check(result);
			}
			m_sound = null;

			/*if (m_masterSoundGroup?.isValid() ?? false) {
				var result = m_masterSoundGroup.release();
				FMOD_Check(result);
			}
			m_masterSoundGroup = null;*/

			if (m_system?.isValid() ?? false) {
				var result = m_system.close();
				FMOD_Check(result);
				result = m_system.release();
				FMOD_Check(result);
			}
			m_system = null;
		}

		private bool FMOD_Check(FMOD.RESULT result) {
			if (result != FMOD.RESULT.OK) {
				//FMOD_Reset();
				Logger.Default.Log(LoggerEvent.Error, $"FMOD error! {result} - {FMOD.Error.String(result)}");
				return true;
			}
			return false;
		}

	}
}
