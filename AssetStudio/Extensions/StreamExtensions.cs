using System.IO;

namespace AssetStudio {
	public static class StreamExtensions {
		private const int BufferSize = 81920;

		public static int CopyTo(this Stream source, Stream destination, int size) {
			int sz = int.Min(BufferSize, size);
			var buffer = new byte[sz];
			int cnt = 0;
			for (int left = size, read = 1; left > 0 && read != 0;) {
				int toRead = int.Min(left, sz);
				read = source.Read(buffer, 0, toRead);
				destination.Write(buffer, 0, read);
				cnt += read;
				left -= read;
			}
			return cnt;
		}

		public static int CopyTo(this Stream source, Stream destination, long _size) => CopyTo(source, destination, (int)_size);
		public static int CopyTo(this Stream source, Stream destination, uint _size) => CopyTo(source, destination, (int)_size);
	}
}
