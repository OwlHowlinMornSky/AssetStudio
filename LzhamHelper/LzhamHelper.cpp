#include "LzhamHelper.h"

// Define LZHAM_DEFINE_ZLIB_API causes lzham.h to remap the standard zlib.h functions/macro definitions to lzham's.
// This is totally optional - you can also directly use the lzham_* functions and macros instead.
//#define LZHAM_DEFINE_ZLIB_API
#include "lzham/lzham_static_lib.h"

System::Int32 LzhamHelper::Decode::TryDecodeBuffer(array<System::Byte>^% input, array<System::Byte>^% output) {
	pin_ptr<System::Byte> inputBuffer(&input[0]);
	unsigned long inputLength = input->Length;
	unsigned char* inbuffer = inputBuffer;

	pin_ptr<System::Byte> outputBuffer(&output[0]);
	unsigned long outputMaxLength = output->Length;
	unsigned char* outbuffer = outputBuffer;

	return lzham_z_uncompress(outbuffer, &outputMaxLength, inbuffer, inputLength);
}

int LzhamHelper::Decode::DecodeBuffer(array<System::Byte>^% input, array<System::Byte>^% output) {
	pin_ptr<System::Byte> inputBuffer(&input[0]);
	unsigned long inputLength = input->Length;
	unsigned char* inbuffer = inputBuffer;

	pin_ptr<System::Byte> outputBuffer(&output[0]);
	unsigned long outputMaxLength = output->Length;
	unsigned char* outbuffer = outputBuffer;

	int code = lzham_z_uncompress(outbuffer, &outputMaxLength, inbuffer, inputLength);

	switch (code) {
	case LZHAM_Z_OK:
		break;
	default:
		throw gcnew System::NotImplementedException();
	}

	return outputMaxLength;
}

int LzhamHelper::Decode::DecodeBuffer(array<System::Byte>^% input, int inputLength, array<System::Byte>^% output) {
	pin_ptr<System::Byte> inputBuffer(&input[0]);
	int inputLength0 = input->Length;
	unsigned char* inbuffer = inputBuffer;

	if (inputLength > inputLength0) {
		throw gcnew System::InsufficientMemoryException("Lzham: Buffer is smaller than specified.");
	}

	pin_ptr<System::Byte> outputBuffer(&output[0]);
	unsigned long outputMaxLength = output->Length + 5;
	unsigned char* outbuffer = outputBuffer;

	int code = lzham_z_uncompress(outbuffer, &outputMaxLength, inbuffer, inputLength);

	switch (code) {
	case LZHAM_Z_OK:
		break;
	default:
		throw gcnew System::NotImplementedException("Code: " + code);
	}

	return outputMaxLength;
}
