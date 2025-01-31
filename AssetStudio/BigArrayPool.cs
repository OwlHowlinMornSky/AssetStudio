using System;
using System.Buffers;

namespace AssetStudio {
	public static class BigArrayPool<T> {
		private static readonly ArrayPool<T> s_shared = ArrayPool<T>.Create(64 * 1024 * 1024, 3);
		public static ArrayPool<T> Shared => s_shared;
	}
	public class TempBuffer<T>(int size) : IDisposable {

		~TempBuffer() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed = false;
		protected virtual void Dispose(bool _) {
			if (_disposed)
				return;
			_disposed = true;

			BigArrayPool<T>.Shared.Return(this);
		}

		private readonly T[] _buffer = BigArrayPool<T>.Shared.Rent(size);

		public static implicit operator T[](TempBuffer<T> _this) {
			return _this._buffer;
		}
	}
}
