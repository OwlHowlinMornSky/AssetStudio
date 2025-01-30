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
	void PrepareStackAndLayer(Context^ context, String^ strTakeName);
	void LoadCurves(Node^ pNode);
	void BeginKeyModify();
	void EndKeyModify();
	void AddScalingKey(float time, float x, float y, float z);
	void AddRotationKey(float time, float x, float y, float z);
	void AddTranslationKey(float time, float x, float y, float z);
	void ApplyEulerFilter(float filterPrecision);
	int GetCurrentBlendShapeChannelCount(Node^ pNode);
	bool IsBlendShapeChannelMatch(int channelIndex, String^ strChannelName);
	void BeginBlendShapeAnimCurve(int channelIndex);
	void EndBlendShapeAnimCurve();
	void AddBlendShapeKeyframe(float time, float value);

internal:
	AsFbxAnimContext* GetPtr();
};

}
