#pragma once

#include "dllexport.h"
#include "bool32_t.h"

T2D_API(bool32_t) T2D_DecodeDXT1(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeDXT5(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodePVRTC(const void* data, int32_t width, int32_t height, void* image, bool32_t is2bpp);

T2D_API(bool32_t) T2D_DecodeETC1(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeETC2(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeETC2A1(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeETC2A8(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeEACR(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeEACRSigned(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeEACRG(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeEACRGSigned(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeBC4(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeBC5(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeBC6(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeBC7(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeATCRGB4(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeATCRGBA8(const void* data, int32_t width, int32_t height, void* image);

T2D_API(bool32_t) T2D_DecodeASTC(const void* data, int32_t width, int32_t height, int32_t blockWidth, int32_t blockHeight, void* image);

T2D_API(void) T2D_DisposeBuffer(void** ppBuffer);

T2D_API(void) T2D_UnpackCrunch(const void* data, uint32_t dataSize, void** ppResult, uint32_t* pResultSize);

T2D_API(void) T2D_UnpackUnityCrunch(const void* data, uint32_t dataSize, void** ppResult, uint32_t* pResultSize);
