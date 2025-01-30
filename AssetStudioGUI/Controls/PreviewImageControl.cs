using System;
using System.Windows.Forms;
using AssetStudio;

namespace AssetStudioGUI.Controls {
	public partial class PreviewImageControl : UserControl, IPreviewControl {
		public PreviewImageControl() {
			InitializeComponent();
		}

		private SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Bgra32> m_image;
		private DirectBitmap m_bitmap;
		private static readonly char[] m_textureChannelNames = ['B', 'G', 'R', 'A'];
		private bool[] m_textureChannels = [true, true, true, true];

		internal void PreviewTexture2D(AssetItem assetItem, Texture2D texture) {
			m_image = texture.ConvertToImage(true);
			if (m_image != null) {
				m_bitmap = new DirectBitmap(m_image.ConvertToBytes(), texture.m_Width, texture.m_Height);

				assetItem.InfoText = String.Format(Properties.Strings.Preview_Tex2D_info,
					texture.m_Width, texture.m_Height, texture.m_TextureFormat);
				switch (texture.m_TextureSettings.m_FilterMode) {
				case 0:
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Point);
					break;
				case 1:
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Bilinear);
					break;
				case 2:
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Trilinear);
					break;
				}
				assetItem.InfoText += "\n" +
					String.Format(Properties.Strings.Preview_Tex2D_info_mipmap,
					texture.m_TextureSettings.m_Aniso, texture.m_TextureSettings.m_MipBias);
				switch (texture.m_TextureSettings.m_WrapMode) {
				case 0:
					assetItem.InfoText += "\n" +
						//	"Wrap mode: Repeat";
						Properties.Strings.Preview_Tex2D_info_wrap + Properties.Strings.Preview_Tex2D_info_wrap_repeat;
					break;
				case 1:
					assetItem.InfoText += "\n" +
						//	"Wrap mode: Clamp";
						Properties.Strings.Preview_Tex2D_info_wrap + Properties.Strings.Preview_Tex2D_info_wrap_clamp;
					break;
				}
				//assetItem.InfoText += "\n" + "Channels: ";
				assetItem.InfoText += "\n" + Properties.Strings.Preview_Tex2D_info_channels;
				int validChannel = 0;
				for (int i = 0; i < 4; i++) {
					if (m_textureChannels[i]) {
						assetItem.InfoText += m_textureChannelNames[i];
						validChannel++;
					}
				}
				if (validChannel == 0)
					//assetItem.InfoText += "None";
					assetItem.InfoText += Properties.Strings.Preview_Tex2D_info_channels_none;
				if (validChannel != 4) {
					var bytes = m_bitmap.Bits;
					for (int i = 0; i < m_bitmap.Height; i++) {
						int offset = Math.Abs(m_bitmap.Stride) * i;
						for (int j = 0; j < m_bitmap.Width; j++) {
							bytes[offset] = m_textureChannels[0] ? bytes[offset] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 1] = m_textureChannels[1] ? bytes[offset + 1] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 2] = m_textureChannels[2] ? bytes[offset + 2] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 3] = m_textureChannels[3] ? bytes[offset + 3] : byte.MaxValue;
							offset += 4;
						}
					}
				}
				Preview(m_bitmap);

				//StatusStripUpdate("'Ctrl' + 'R'/'G'/'B'/'A' " + "for Channel Toggle");
				//StatusStripUpdate("'Ctrl' + 'R'/'G'/'B'/'A' " + Properties.Strings.Preview_Tex2D_Channel_Toggle);
			}
			else {
				//StatusStripUpdate("Unsupported image for preview");
				//StatusStripUpdate(Properties.Strings.Preview_Tex2D_unsupported);
			}
		}

		internal void PreviewSprite(AssetItem assetItem, Sprite m_Sprite) {
			using var image = m_Sprite.GetImage();
			if (image != null) {
				var bitmap = new DirectBitmap(image.ConvertToBytes(), image.Width, image.Height);

				//assetItem.InfoText = $"Width: {bitmap.Width}\nHeight: {bitmap.Height}\n";
				assetItem.InfoText =
					String.Format(Properties.Strings.Preview_Sprite_info + "\n", bitmap.Width, bitmap.Height);
				Preview(bitmap);
			}
			else {
				//StatusStripUpdate("Unsupported sprite for preview.");
				//StatusStripUpdate(Properties.Strings.Preview_Sprite_unsupported);
			}
		}

		private bool _reset = false;
		public void ResetPreview() {
			_reset = true;
			m_image?.Dispose();
			m_image = null;
			m_bitmap?.Dispose();
			m_bitmap = null;
		}

		private void Preview(DirectBitmap img) {
			pictureBox1.Image = img.Bitmap;
			pictureBox1.ClientSize = new System.Drawing.Size(img.Width, img.Height);
			//SwitchPreviewPage(PreviewType.None);
		}

		private void PreviewImage_KeyDown(object sender, KeyEventArgs e) {
			/*if (e.Control) {
				var changed = false;
				switch (e.KeyCode) {
				case Keys.B:
					m_textureChannels[0] = !m_textureChannels[0];
					changed = true;
					break;
				case Keys.G:
					m_textureChannels[1] = !m_textureChannels[1];
					changed = true;
					break;
				case Keys.R:
					m_textureChannels[2] = !m_textureChannels[2];
					changed = true;
					break;
				case Keys.A:
					m_textureChannels[3] = !m_textureChannels[3];
					changed = true;
					break;
				}
				if (changed && m_image != null) {
					Preview(m_image);
				}
			}*/
		}

		private void PreviewImageControl_ClientSizeChanged(object sender, EventArgs e) {
			if (_reset)
				return;
			var sz = ClientSize - pictureBox1.Size;
			sz /= 2;
			pictureBox1.Location = (System.Drawing.Point)sz;
		}
	}
}
