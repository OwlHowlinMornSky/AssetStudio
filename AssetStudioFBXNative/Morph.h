#pragma once

#include "api.h"

#include "Context.h"
#include "Node.h"

using namespace System;

namespace FbxInterop {

public ref class Morph {
private:
	AsFbxMorphContext* _this;

public:
	Morph();
	~Morph();

public:
	void MorphInitializeContext(Context^ pContext, Node^ pNode);
	void MorphAddBlendShapeChannel(Context^ pContext, String^ strChannelName);
	void MorphAddBlendShapeChannelShape(Context^ pContext, float weight, String^ shapeName);
	void MorphCopyBlendShapeControlPoints();
	void MorphSetBlendShapeVertex(unsigned int index, float x, float y, float z);
	void MorphCopyBlendShapeControlPointsNormal();
	void MorphSetBlendShapeVertexNormal(unsigned int index, float x, float y, float z);

internal:
	AsFbxMorphContext* GetPtr();
};

}
