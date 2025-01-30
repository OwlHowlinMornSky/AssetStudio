#pragma once

#include "api.h"

#include "Material.h"

using namespace System;
using namespace fbxsdk;

namespace FbxInterop {

public ref class Node {
private:
	FbxNode* _ref;

internal:
	Node(FbxNode* _ptr);
public:
	~Node();

public:
	int AddMaterialToFrame(Material^ material);

	void SetFrameShadingModeToTextureShading();

	bool IsValid();

internal:
	FbxNode* GetPtr();
};

}
