#include "dllmain.h"

#include "bcn.h"
#include "pvrtc.h"
#include "etc.h"
#include "atc.h"
#include "astc.h"
#include "crunch.h"
#include "unitycrunch.h"

T2D_API(bool32_t) T2D_DecodeDXT1(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc1(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeDXT5(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc3(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodePVRTC(const void* data, int32_t width, int32_t height, void* image, bool32_t is2bpp) {
	return decode_pvrtc(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image), is2bpp ? 1 : 0);
}

T2D_API(bool32_t) T2D_DecodeETC1(const void* data, int32_t width, int32_t height, void* image) {
	return decode_etc1(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeETC2(const void* data, int32_t width, int32_t height, void* image) {
	return decode_etc2(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeETC2A1(const void* data, int32_t width, int32_t height, void* image) {
	return decode_etc2a1(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeETC2A8(const void* data, int32_t width, int32_t height, void* image) {
	return decode_etc2a8(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeEACR(const void* data, int32_t width, int32_t height, void* image) {
	return decode_eacr(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeEACRSigned(const void* data, int32_t width, int32_t height, void* image) {
	return decode_eacr_signed(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeEACRG(const void* data, int32_t width, int32_t height, void* image) {
	return decode_eacrg(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeEACRGSigned(const void* data, int32_t width, int32_t height, void* image) {
	return decode_eacrg_signed(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeBC4(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc4(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeBC5(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc5(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeBC6(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc6(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeBC7(const void* data, int32_t width, int32_t height, void* image) {
	return decode_bc7(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeATCRGB4(const void* data, int32_t width, int32_t height, void* image) {
	return decode_atc_rgb4(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeATCRGBA8(const void* data, int32_t width, int32_t height, void* image) {
	return decode_atc_rgba8(static_cast<const uint8_t*>(data), width, height, static_cast<uint32_t*>(image));
}

T2D_API(bool32_t) T2D_DecodeASTC(const void* data, int32_t width, int32_t height, int32_t blockWidth, int32_t blockHeight, void* image) {
	return decode_astc(static_cast<const uint8_t*>(data), width, height, blockWidth, blockHeight, static_cast<uint32_t*>(image));
}

T2D_API(void) T2D_DisposeBuffer(void** ppBuffer) {
	if (ppBuffer == nullptr) {
		return;
	}

	auto ppTypedBuffer = reinterpret_cast<uint8_t**>(ppBuffer);

	delete[](*ppTypedBuffer);

	*ppBuffer = nullptr;
}

T2D_API(void) T2D_UnpackCrunch(const void* data, uint32_t dataSize, void** ppResult, uint32_t* pResultSize) {
	void* result;
	uint32_t resultSize;

	if (ppResult != nullptr) {
		*ppResult = nullptr;
	}

	if (pResultSize != nullptr) {
		*pResultSize = 0;
	}

	if (!crunch_unpack_level(static_cast<const uint8_t*>(data), dataSize, 0, &result, &resultSize)) {
		return;
	}

	if (ppResult != nullptr) {
		*ppResult = result;
	}

	if (pResultSize != nullptr) {
		*pResultSize = resultSize;
	}
}

T2D_API(void) T2D_UnpackUnityCrunch(const void* data, uint32_t dataSize, void** ppResult, uint32_t* pResultSize) {
	void* result;
	uint32_t resultSize;

	if (ppResult != nullptr) {
		*ppResult = nullptr;
	}

	if (pResultSize != nullptr) {
		*pResultSize = 0;
	}

	if (!unity_crunch_unpack_level(static_cast<const uint8_t*>(data), dataSize, 0, &result, &resultSize)) {
		return;
	}

	if (ppResult != nullptr) {
		*ppResult = result;
	}

	if (pResultSize != nullptr) {
		*pResultSize = resultSize;
	}
}
