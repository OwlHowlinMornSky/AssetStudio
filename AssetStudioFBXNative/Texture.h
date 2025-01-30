#pragma once

#include "api.h"

using namespace System;
using namespace fbxsdk;

namespace FbxInterop {

public ref class Texture {
private:
	FbxFileTexture* _ref;

internal:
	Texture(FbxFileTexture* _ptr);
public:
	~Texture();

	bool IsValid();

internal:
	FbxFileTexture* GetPtr();

};

}
