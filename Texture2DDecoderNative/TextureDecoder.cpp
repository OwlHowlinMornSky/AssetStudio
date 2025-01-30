#include "TextureDecoder.h"

#include "dllmain.h"

namespace Texture2DDecoder {

bool TextureDecoder::DecodeDXT1(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeDXT1(pData, width, height, pImage);
}

bool TextureDecoder::DecodeDXT5(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeDXT5(pData, width, height, pImage);
}

bool TextureDecoder::DecodePVRTC(array<Byte>^ data, int width, int height, array<Byte>^ image, bool is2bpp) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodePVRTC(pData, width, height, pImage, is2bpp);
}

bool TextureDecoder::DecodeETC1(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeETC1(pData, width, height, pImage);
}

bool TextureDecoder::DecodeETC2(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeETC2(pData, width, height, pImage);
}

bool TextureDecoder::DecodeETC2A1(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeETC2A1(pData, width, height, pImage);
}

bool TextureDecoder::DecodeETC2A8(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeETC2A8(pData, width, height, pImage);
}

bool TextureDecoder::DecodeEACR(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeEACR(pData, width, height, pImage);
}

bool TextureDecoder::DecodeEACRSigned(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeEACRSigned(pData, width, height, pImage);
}

bool TextureDecoder::DecodeEACRG(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeEACRG(pData, width, height, pImage);
}

bool TextureDecoder::DecodeEACRGSigned(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeEACRGSigned(pData, width, height, pImage);
}

bool TextureDecoder::DecodeBC4(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeBC4(pData, width, height, pImage);
}

bool TextureDecoder::DecodeBC5(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeBC5(pData, width, height, pImage);
}

bool TextureDecoder::DecodeBC6(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeBC6(pData, width, height, pImage);
}

bool TextureDecoder::DecodeBC7(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeBC7(pData, width, height, pImage);
}

bool TextureDecoder::DecodeATCRGB4(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeATCRGB4(pData, width, height, pImage);
}

bool TextureDecoder::DecodeATCRGBA8(array<Byte>^ data, int width, int height, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeATCRGBA8(pData, width, height, pImage);
}

bool TextureDecoder::DecodeASTC(array<Byte>^ data, int width, int height, int blockWidth, int blockHeight, array<Byte>^ image) {
	pin_ptr<Byte> pData = &data[0];
	pin_ptr<Byte> pImage = &image[0];
	return T2D_DecodeASTC(pData, width, height, blockWidth, blockHeight, pImage);
}

array<Byte>^ TextureDecoder::UnpackCrunch(array<Byte>^ data) {
	void* pBuffer = nullptr;
	uint32_t bufferSize = 0;

	{
		pin_ptr<Byte> pData = &data[0];
		T2D_UnpackCrunch(pData, (uint32_t)data->Length, &pBuffer, &bufferSize);
	}

	if (pBuffer == nullptr || bufferSize == 0) {
		return nullptr;
	}

	array<Byte>^ result = gcnew array<Byte>(bufferSize);

	Runtime::InteropServices::Marshal::Copy((IntPtr)pBuffer, result, 0, (int)bufferSize);

	T2D_DisposeBuffer(&pBuffer);

	return result;
}

array<Byte>^ TextureDecoder::UnpackUnityCrunch(array<Byte>^ data) {
	void* pBuffer = nullptr;
	uint32_t bufferSize = 0;

	{
		pin_ptr<Byte> pData = &data[0];
		T2D_UnpackUnityCrunch(pData, (uint32_t)data->Length, &pBuffer, &bufferSize);
	}

	if (pBuffer == nullptr || bufferSize == 0) {
		return nullptr;
	}

	array<Byte>^ result = gcnew array<Byte>(bufferSize);

	Runtime::InteropServices::Marshal::Copy((IntPtr)pBuffer, result, 0, (int)bufferSize);

	T2D_DisposeBuffer(&pBuffer);

	return result;
}

}
