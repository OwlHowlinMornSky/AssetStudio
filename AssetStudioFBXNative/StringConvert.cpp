#include "StringConvert.h"

std::string StringToUnmanagedUtf8(String^% managedStr) {
	std::string str;
	array<Byte>^ arr = Text::Encoding::UTF8->GetBytes(managedStr);
	int len = arr->Length;
	str.resize(len);
	IntPtr pcstr((void*)str.data());
	Runtime::InteropServices::Marshal::Copy(arr, 0, pcstr, len);
	return str;
}
