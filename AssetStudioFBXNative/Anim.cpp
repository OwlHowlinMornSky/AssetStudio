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

void Anim::PrepareStackAndLayer(Context^ context, String^ stackName) {
	std::string cstr = StringToUnmanagedUtf8(stackName);
	AsFbxAnimPrepareStackAndLayer(context->GetPtr(), _this, cstr.c_str());
}

void Anim::LoadCurves(Node^ node) {
	AsFbxAnimLoadCurves(node->GetPtr(), _this);
}

void Anim::BeginKeyModify() {
	AsFbxAnimBeginKeyModify(_this);
}

void Anim::EndKeyModify() {
	AsFbxAnimEndKeyModify(_this);
}

void Anim::AddScalingKey(float time, float x, float y, float z) {
	AsFbxAnimAddScalingKey(_this, time, x, y, z);
}

void Anim::AddRotationKey(float time, float x, float y, float z) {
	AsFbxAnimAddRotationKey(_this, time, x, y, z);
}

void Anim::AddTranslationKey(float time, float x, float y, float z) {
	AsFbxAnimAddTranslationKey(_this, time, x, y, z);
}

void Anim::ApplyEulerFilter(float filterPrecision) {
	AsFbxAnimApplyEulerFilter(_this, filterPrecision);
}

int Anim::GetCurrentBlendShapeChannelCount(Node^ node) {
	return AsFbxAnimGetCurrentBlendShapeChannelCount(_this, node->GetPtr());
}

bool Anim::IsBlendShapeChannelMatch(int channelIndex, String^ channelName) {
	std::string cstr = StringToUnmanagedUtf8(channelName);
	return AsFbxAnimIsBlendShapeChannelMatch(_this, channelIndex, cstr.c_str());
}

void Anim::BeginBlendShapeAnimCurve(int channelIndex) {
	AsFbxAnimBeginBlendShapeAnimCurve(_this, channelIndex);
}

void Anim::EndBlendShapeAnimCurve() {
	AsFbxAnimEndBlendShapeAnimCurve(_this);
}

void Anim::AddBlendShapeKeyframe(float time, float value) {
	AsFbxAnimAddBlendShapeKeyframe(_this, time, value);
}

AsFbxAnimContext* Anim::GetPtr() {
	return _this;
}

}

