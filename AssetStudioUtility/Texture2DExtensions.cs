using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace AssetStudio {
	public static class Texture2DExtensions {
		public static Image<Bgra32> ConvertToImage(this Texture2D m_Texture2D, bool flip) {
			var converter = new Texture2DConverter(m_Texture2D);
			using TempBuffer<byte> buff = new(m_Texture2D.m_Width * m_Texture2D.m_Height * 4);
			if (converter.DecodeTexture2D(buff)) {
				var image = Image.LoadPixelData<Bgra32>((byte[])buff, m_Texture2D.m_Width, m_Texture2D.m_Height);
				if (flip) {
					image.Mutate(x => x.Flip(FlipMode.Vertical));
				}
				return image;
			}
			return null;
		}

		public static MemoryStream ConvertToStream(this Texture2D m_Texture2D, ImageFormat imageFormat, bool flip) {
			var image = ConvertToImage(m_Texture2D, flip);
			if (image != null) {
				using (image) {
					return image.ConvertToStream(imageFormat);
				}
			}
			return null;
		}
	}
}
