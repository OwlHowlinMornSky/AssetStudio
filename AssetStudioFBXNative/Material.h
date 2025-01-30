#pragma once

#include <fbxsdk.h>
#define OHMS_CLR_TEMPORARY
#include "api.h"

#include "Texture.h"

using namespace System;
using namespace fbxsdk;

namespace FbxInterop {

public ref class Material {
private:
	FbxSurfacePhong* _ref;

internal:
	Material(FbxSurfacePhong* _ptr);
public:
	~Material();

public:
	void LinkTexture(int dest, Texture^ texture, float offsetX, float offsetY, float scaleX, float scaleY);

internal:
	FbxSurfacePhong* GetPtr();
};

}
