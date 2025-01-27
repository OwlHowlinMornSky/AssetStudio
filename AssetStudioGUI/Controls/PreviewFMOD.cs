using AssetStudio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AssetStudioGUI.Controls {
	public partial class PreviewFMOD : UserControl {
		public PreviewFMOD() {
			InitializeComponent();
			m_prevSize = ClientSize;
		}

		internal void PreviewAudioClip(AssetItem assetItem, AudioClip m_AudioClip) {
			//Info
			assetItem.InfoText = Properties.Strings.Preview_Audio_formatHead;
			if (m_AudioClip.version[0] < 5) {
				switch (m_AudioClip.m_Type) {
				case FMODSoundType.ACC:
					assetItem.InfoText += "ACC";
					break;
				case FMODSoundType.AIFF:
					assetItem.InfoText += "AIFF";
					break;
				case FMODSoundType.IT:
					assetItem.InfoText += "Impulse tracker";
					break;
				case FMODSoundType.MOD:
					assetItem.InfoText += "Protracker / Fasttracker MOD";
					break;
				case FMODSoundType.MPEG:
					assetItem.InfoText += "MP2/MP3 MPEG";
					break;
				case FMODSoundType.OGGVORBIS:
					assetItem.InfoText += "Ogg vorbis";
					break;
				case FMODSoundType.S3M:
					assetItem.InfoText += "ScreamTracker 3";
					break;
				case FMODSoundType.WAV:
					assetItem.InfoText += "Microsoft WAV";
					break;
				case FMODSoundType.XM:
					assetItem.InfoText += "FastTracker 2 XM";
					break;
				case FMODSoundType.XMA:
					assetItem.InfoText += "Xbox360 XMA";
					break;
				case FMODSoundType.VAG:
					assetItem.InfoText += "PlayStation Portable ADPCM";
					break;
				case FMODSoundType.AUDIOQUEUE:
					assetItem.InfoText += "iPhone";
					break;
				default:
					assetItem.InfoText += "Unknown";
					break;
				}
			}
			else {
				switch (m_AudioClip.m_CompressionFormat) {
				case AudioCompressionFormat.PCM:
					assetItem.InfoText += "PCM";
					break;
				case AudioCompressionFormat.Vorbis:
					assetItem.InfoText += "Vorbis";
					break;
				case AudioCompressionFormat.ADPCM:
					assetItem.InfoText += "ADPCM";
					break;
				case AudioCompressionFormat.MP3:
					assetItem.InfoText += "MP3";
					break;
				case AudioCompressionFormat.PSMVAG:
					assetItem.InfoText += "PlayStation Portable ADPCM";
					break;
				case AudioCompressionFormat.HEVAG:
					assetItem.InfoText += "PSVita ADPCM";
					break;
				case AudioCompressionFormat.XMA:
					assetItem.InfoText += "Xbox360 XMA";
					break;
				case AudioCompressionFormat.AAC:
					assetItem.InfoText += "AAC";
					break;
				case AudioCompressionFormat.GCADPCM:
					assetItem.InfoText += "Nintendo 3DS/Wii DSP";
					break;
				case AudioCompressionFormat.ATRAC9:
					assetItem.InfoText += "PSVita ATRAC9";
					break;
				default:
					assetItem.InfoText += "Unknown";
					break;
				}
			}
			FMODreset();

			var m_AudioData = m_AudioClip.m_AudioData.GetData();
			if (m_AudioData == null || m_AudioData.Length == 0)
				return;
			var exinfo = new FMOD.CREATESOUNDEXINFO();

			exinfo.cbsize = Marshal.SizeOf(exinfo);
			exinfo.length = (uint)m_AudioClip.m_Size;

			var result = system.createSound(m_AudioData, FMOD.MODE.OPENMEMORY | loopMode, ref exinfo, out sound);
			if (FMOD_CHECK(result))
				return;

			sound.getNumSubSounds(out var numsubsounds);

			if (numsubsounds > 0) {
				result = sound.getSubSound(0, out var subsound);
				if (result == FMOD.RESULT.OK) {
					sound = subsound;
				}
			}

			result = sound.getLength(out FMODlenms, FMOD.TIMEUNIT.MS);
			if (FMOD_CHECK(result))
				return;

			result = system.playSound(sound, null, true, out channel);
			if (FMOD_CHECK(result))
				return;

			//SwitchPreviewPage(PreviewType.FMOD);

			result = channel.getFrequency(out var frequency);
			if (FMOD_CHECK(result))
				return;
		}

		private Size m_prevSize;
		private void FMOD_Preview_ClientSizeChanged(object sender, EventArgs e) {
			int dx = ClientSize.Width - m_prevSize.Width;
			int dy = ClientSize.Height - m_prevSize.Height;

			m_prevSize = ClientSize;

			m_prevSize.Width -= dx % 2;
			m_prevSize.Height -= dy % 2;
			dx /= 2;
			dy /= 2;

			SuspendLayout();

			foreach (Control item in Controls) {
				item.Left += dx;
				item.Top += dy;
			}

			ResumeLayout();
		}



		private FMOD.System system;
		private FMOD.Sound sound;
		private FMOD.Channel channel;
		private FMOD.SoundGroup masterSoundGroup;
		private FMOD.MODE loopMode = FMOD.MODE.LOOP_OFF;
		private uint FMODlenms;
		private float FMODVolume = 0.8f;

		private void FMODinit() {
			FMODreset();

			var result = FMOD.Factory.System_Create(out system);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = system.getVersion(out var version);
			FMOD_CHECK(result);
			if (version < FMOD.VERSION.number) {
				MessageBox.Show($"Error!  You are using an old version of FMOD {version:X}.  This program requires {FMOD.VERSION.number:X}.");
				Application.Exit();
			}

			result = system.init(2, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = system.getMasterSoundGroup(out masterSoundGroup);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = masterSoundGroup.setVolume(FMODVolume);
			if (FMOD_CHECK(result)) {
				return;
			}
		}

		private void FMODreset() {
			ui_tabRight_page0_FMODtimer.Stop();
			ui_tabRight_page0_FMODprogressBar.Value = 0;
			ui_tabRight_page0_FMODtimerLabel.Text = "00:00.00";
			ui_tabRight_page0_FMODdurationLabel.Text = "00:00.00";

			//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Stopped";
			ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
			ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
			ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

			ui_tabRight_page0_FMODinfoLabel.Text = "";

			if (sound != null && sound.isValid()) {
				var result = sound.release();
				FMOD_CHECK(result);
				sound = null;
			}
		}

		private bool FMOD_CHECK(FMOD.RESULT result) {
			if (result != FMOD.RESULT.OK) {
				FMODreset();
				//StatusStripUpdate($"FMOD error! {result} - {FMOD.Error.String(result)}");
				return true;
			}
			return false;
		}

		private void ui_tabRight_page0_FMODplayButton_Click(object sender, EventArgs e) {
			if (sound != null && channel != null) {
				ui_tabRight_page0_FMODtimer.Start();
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.stop();
					if (FMOD_CHECK(result)) {
						return;
					}

					result = system.playSound(sound, null, false, out channel);
					if (FMOD_CHECK(result)) {
						return;
					}

					//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
					ui_tabRight_page0_FMODpauseButton.Visible = true;
					ui_tabRight_page0_FMODresumeButton.Visible = false;
				}
				else {
					result = system.playSound(sound, null, false, out channel);
					if (FMOD_CHECK(result)) {
						return;
					}
					//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Playing";
					ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
					ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
					ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

					if (ui_tabRight_page0_FMODprogressBar.Value > 0) {
						uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;

						result = channel.setPosition(newms, FMOD.TIMEUNIT.MS);
						if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
							if (FMOD_CHECK(result)) {
								return;
							}
						}

					}
				}
			}
		}

		private void ui_tabRight_page0_FMODpauseButton_Click(object sender, EventArgs e) {
			if (sound != null && channel != null) {
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.getPaused(out var paused);
					if (FMOD_CHECK(result)) {
						return;
					}
					result = channel.setPaused(!paused);
					if (FMOD_CHECK(result)) {
						return;
					}

					if (paused) {
						//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Playing";
						//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
						ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
						ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
						ui_tabRight_page0_FMODpauseButton.Visible = true;
						ui_tabRight_page0_FMODresumeButton.Visible = false;

						ui_tabRight_page0_FMODtimer.Start();
					}
					else {
						//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Paused";
						//ui_tabRight_page0_FMODpauseButton.Text = "Resume";
						ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Paused.Visible = true;
						ui_tabRight_page0_FMODpauseButton.Visible = false;
						ui_tabRight_page0_FMODresumeButton.Visible = true;

						ui_tabRight_page0_FMODtimer.Stop();
					}
				}
			}
		}

		private void ui_tabRight_page0_FMODstopButton_Click(object sender, EventArgs e) {
			if (channel != null) {
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.stop();
					if (FMOD_CHECK(result)) {
						return;
					}
					//channel = null;
					//don't FMODreset, it will nullify the sound
					ui_tabRight_page0_FMODtimer.Stop();
					ui_tabRight_page0_FMODprogressBar.Value = 0;
					ui_tabRight_page0_FMODtimerLabel.Text = "00:00.00";
					//FMODdurationLabel.Text = "0:0.0";

					//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Stopped";
					ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
					ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
					ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

					//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
					ui_tabRight_page0_FMODpauseButton.Visible = true;
					ui_tabRight_page0_FMODresumeButton.Visible = false;
				}
			}
		}

		private void ui_tabRight_page0_FMODloopButton_CheckedChanged(object sender, EventArgs e) {
			FMOD.RESULT result;

			loopMode = ui_tabRight_page0_FMODloopButton.Checked ? FMOD.MODE.LOOP_NORMAL : FMOD.MODE.LOOP_OFF;

			if (sound != null) {
				result = sound.setMode(loopMode);
				if (FMOD_CHECK(result)) {
					return;
				}
			}

			if (channel != null) {
				result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				result = channel.getPaused(out var paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing || paused) {
					result = channel.setMode(loopMode);
					if (FMOD_CHECK(result)) {
						return;
					}
				}
			}
		}

		private void ui_tabRight_page0_FMODvolumeBar_ValueChanged(object sender, EventArgs e) {
			FMODVolume = Convert.ToSingle(ui_tabRight_page0_FMODvolumeBar.Value) / 10;

			var result = masterSoundGroup.setVolume(FMODVolume);
			if (FMOD_CHECK(result)) {
				return;
			}
		}

		private void ui_tabRight_page0_FMODprogressBar_Scroll(object sender, EventArgs e) {
			if (channel != null) {
				uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;
				//ui_tabRight_page0_FMODtimerLabel.Text = $"{newms / 1000 / 60}:{newms / 1000 % 60}.{newms / 10 % 100}";
				//FMODdurationLabel.Text = $"{FMODlenms / 1000 / 60}:{FMODlenms / 1000 % 60}.{FMODlenms / 10 % 100}";
				ui_tabRight_page0_FMODtimerLabel.Text = String.Format("{0:D2}:{1:D2}.{2:D2}", newms / 1000 / 60, newms / 1000 % 60, newms / 10 % 100);
			}
		}

		private void ui_tabRight_page0_FMODprogressBar_MouseDown(object sender, MouseEventArgs e) {
			ui_tabRight_page0_FMODtimer.Stop();
		}

		private void ui_tabRight_page0_FMODprogressBar_MouseUp(object sender, MouseEventArgs e) {
			if (channel != null) {
				uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;

				var result = channel.setPosition(newms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}


				result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					ui_tabRight_page0_FMODtimer.Start();
				}
			}
		}

		private void Ui_tabRight_page0_FMODtimer_Tick(object sender, EventArgs e) {
			uint ms = 0;
			bool playing = false;
			bool paused = false;

			if (channel != null) {
				var result = channel.getPosition(out ms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}

				result = channel.isPlaying(out playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}

				result = channel.getPaused(out paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}
			}

			//ui_tabRight_page0_FMODtimerLabel.Text = $"{ms / 1000 / 60}:{ms / 1000 % 60}.{ms / 10 % 100}";
			//FMODdurationLabel.Text = $"{FMODlenms / 1000 / 60}:{FMODlenms / 1000 % 60}.{FMODlenms / 10 % 100}";
			ui_tabRight_page0_FMODtimerLabel.Text = String.Format("{0:D2}:{1:D2}.{2:D2}", ms / 1000 / 60, ms / 1000 % 60, ms / 10 % 100);
			ui_tabRight_page0_FMODprogressBar.Value = (int)(ms * 1000 / FMODlenms);

			//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = paused ? "Paused " : playing ? "Playing" : "Stopped";
			if (paused) {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = true;
			}
			else if (playing) {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
			}
			else {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
			}

			if (system != null && channel != null) {
				system.update();
			}
		}

		private void PreviewFMOD_Load(object sender, EventArgs e) {
			if (Program.Runtime)
				FMODinit();

			ui_tabRight_page0_FMODresumeButton.Top = ui_tabRight_page0_FMODpauseButton.Top;
			ui_tabRight_page0_FMODstatusLabel_Playing.Top = ui_tabRight_page0_FMODstatusLabel_Stopped.Top;
			ui_tabRight_page0_FMODstatusLabel_Paused.Top = ui_tabRight_page0_FMODstatusLabel_Stopped.Top;
		}
	}
}
