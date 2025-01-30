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
	void Initialize(Context^ pContext, Node^ pNode);
	void AddBlendShapeChannel(Context^ pContext, String^ strChannelName);
	void AddBlendShapeChannelShape(Context^ pContext, float weight, String^ shapeName);
	void CopyBlendShapeControlPoints();
	void SetBlendShapeVertex(unsigned int index, float x, float y, float z);
	void CopyBlendShapeControlPointsNormal();
	void SetBlendShapeVertexNormal(unsigned int index, float x, float y, float z);

internal:
	AsFbxMorphContext* GetPtr();
};

}
