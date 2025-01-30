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
	void AnimPrepareStackAndLayer(Context^ context, String^ strTakeName);
	void AnimLoadCurves(Node^ pNode);
	void AnimBeginKeyModify();
	void AnimEndKeyModify();
	void AnimAddScalingKey(float time, float x, float y, float z);
	void AnimAddRotationKey(float time, float x, float y, float z);
	void AnimAddTranslationKey(float time, float x, float y, float z);
	void AnimApplyEulerFilter(float filterPrecision);
	int AnimGetCurrentBlendShapeChannelCount(Node^ pNode);
	bool AnimIsBlendShapeChannelMatch(int channelIndex, String^ strChannelName);
	void AnimBeginBlendShapeAnimCurve(int channelIndex);
	void AnimEndBlendShapeAnimCurve();
	void AnimAddBlendShapeKeyframe(float time, float value);

internal:
	AsFbxAnimContext* GetPtr();
};

}
