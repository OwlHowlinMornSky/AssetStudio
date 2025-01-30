#include "Anim.h"

#include "StringConvert.h"

namespace FbxInterop {

Anim::Anim(bool eulerFilter) {
	_this = new AsFbxAnimContext(eulerFilter);
}

Anim::~Anim() {
	delete _this;
	_this = nullptr;
}

void Anim::AnimPrepareStackAndLayer(Context^ context, String^ strTakeName) {
	std::string cstr = StringToUnmanagedUtf8(strTakeName);
	AsFbxAnimPrepareStackAndLayer(context->GetPtr(), _this, cstr.c_str());
}

void Anim::AnimLoadCurves(Node^ pNode) {
	AsFbxAnimLoadCurves(pNode->GetPtr(), _this);
}

void Anim::AnimBeginKeyModify() {
	AsFbxAnimBeginKeyModify(_this);
}

void Anim::AnimEndKeyModify() {
	AsFbxAnimEndKeyModify(_this);
}

void Anim::AnimAddScalingKey(float time, float x, float y, float z) {
	AsFbxAnimAddScalingKey(
		_this,
		time, x, y, z
	);
}

void Anim::AnimAddRotationKey(float time, float x, float y, float z) {
	AsFbxAnimAddRotationKey(
		_this,
		time, x, y, z
	);
}

void Anim::AnimAddTranslationKey(float time, float x, float y, float z) {
	AsFbxAnimAddTranslationKey(
		_this,
		time, x, y, z
	);
}

void Anim::AnimApplyEulerFilter(float filterPrecision) {
	AsFbxAnimApplyEulerFilter(_this, filterPrecision);
}

int Anim::AnimGetCurrentBlendShapeChannelCount(Node^ pNode) {
	return AsFbxAnimGetCurrentBlendShapeChannelCount(_this, pNode->GetPtr());
}

bool Anim::AnimIsBlendShapeChannelMatch(int channelIndex, String^ strChannelName) {
	std::string cstr = StringToUnmanagedUtf8(strChannelName);
	return AsFbxAnimIsBlendShapeChannelMatch(
		_this,
		channelIndex, cstr.c_str()
	);
}

void Anim::AnimBeginBlendShapeAnimCurve(int channelIndex) {
	AsFbxAnimBeginBlendShapeAnimCurve(_this, channelIndex);
}

void Anim::AnimEndBlendShapeAnimCurve() {
	AsFbxAnimEndBlendShapeAnimCurve(_this);
}

void Anim::AnimAddBlendShapeKeyframe(float time, float value) {
	AsFbxAnimAddBlendShapeKeyframe(_this, time, value);
}

AsFbxAnimContext* Anim::GetPtr() {
	return _this;
}

}

