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
	void Initialize(Context^ context, Node^ node);
	void AddBlendShapeChannel(Context^ context, String^ channelName);
	void AddBlendShapeChannelShape(Context^ context, float weight, String^ shapeName);
	void CopyBlendShapeControlPoints();
	void SetBlendShapeVertex(unsigned int index, float x, float y, float z);
	void CopyBlendShapeControlPointsNormal();
	void SetBlendShapeVertexNormal(unsigned int index, float x, float y, float z);

internal:
	AsFbxMorphContext* GetPtr();
};

}
