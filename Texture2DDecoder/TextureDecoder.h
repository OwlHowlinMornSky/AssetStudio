#pragma once

using namespace System;

namespace Texture2DDecoder {

public ref class TextureDecoder {
public:
	static bool DecodeDXT1(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeDXT5(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodePVRTC(array<Byte>^ data, int width, int height, array<Byte>^ image, bool is2bpp);

	static bool DecodeETC1(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeETC2(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeETC2A1(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeETC2A8(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeEACR(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeEACRSigned(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeEACRG(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeEACRGSigned(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeBC4(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeBC5(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeBC6(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeBC7(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeATCRGB4(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeATCRGBA8(array<Byte>^ data, int width, int height, array<Byte>^ image);

	static bool DecodeASTC(array<Byte>^ data, int width, int height, int blockWidth, int blockHeight, array<Byte>^ image);

	static array<Byte>^ UnpackCrunch(array<Byte>^ data);

	static array<Byte>^ UnpackUnityCrunch(array<Byte>^ data);

};

}
