#pragma once

using namespace System;

namespace LzhamHelper {

public ref class Decode {
public:
	// Return status codes. LZHAM_Z_PARAM_ERROR is non-standard.
	enum class ReturnCode {
		LZHAM_Z_OK = 0,
		LZHAM_Z_STREAM_END = 1,
		LZHAM_Z_NEED_DICT = 2,
		LZHAM_Z_ERRNO = -1,
		LZHAM_Z_STREAM_ERROR = -2,
		LZHAM_Z_DATA_ERROR = -3,
		LZHAM_Z_MEM_ERROR = -4,
		LZHAM_Z_BUF_ERROR = -5,
		LZHAM_Z_VERSION_ERROR = -6,
		LZHAM_Z_PARAM_ERROR = -10000
	};

	/**
	* @brief 解码。
	* @return 状态码。
	*/
	static System::Int32 TryDecodeBuffer(array<System::Byte>^% input, array<System::Byte>^% output);

	static int DecodeBuffer(array<System::Byte>^% input, array<System::Byte>^% output);

	static int DecodeBuffer(array<System::Byte>^% input, int inputLength, array<System::Byte>^% output);
};

}
