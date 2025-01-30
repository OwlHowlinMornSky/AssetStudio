#pragma once

#include "api.h"

#include "Context.h"

using namespace System;

namespace FbxInterop {

public ref class Anim {
private:
	AsFbxAnimContext* _this;

public:
	Anim(bool eulerFilter);
	~Anim();

public:
	void PrepareStackAndLayer(Context^ context, String^ stackName);
	void LoadCurves(Node^ node);
	void BeginKeyModify();
	void EndKeyModify();
	void AddScalingKey(float time, float x, float y, float z);
	void AddRotationKey(float time, float x, float y, float z);
	void AddTranslationKey(float time, float x, float y, float z);
	void ApplyEulerFilter(float filterPrecision);
	int GetCurrentBlendShapeChannelCount(Node^ node);
	bool IsBlendShapeChannelMatch(int channelIndex, String^ channelName);
	void BeginBlendShapeAnimCurve(int channelIndex);
	void EndBlendShapeAnimCurve();
	void AddBlendShapeKeyframe(float time, float value);

internal:
	AsFbxAnimContext* GetPtr();
};

}
