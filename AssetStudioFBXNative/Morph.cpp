#include "Morph.h"

#include "StringConvert.h"

namespace FbxInterop {

Morph::Morph() {
	_this = new AsFbxMorphContext();
}

Morph::~Morph() {
	delete _this;
	_this = nullptr;
}

void Morph::Initialize(Context^ context, Node^ node) {
	AsFbxMorphInitializeContext(context->GetPtr(), _this, node->GetPtr());
}

void Morph::AddBlendShapeChannel(Context^ context, String^ channelName) {
	std::string cstr = StringToUnmanagedUtf8(channelName);
	AsFbxMorphAddBlendShapeChannel(context->GetPtr(), _this, cstr.c_str());
}

void Morph::AddBlendShapeChannelShape(Context^ context, float weight, String^ shapeName) {
	std::string cstr = StringToUnmanagedUtf8(shapeName);
	AsFbxMorphAddBlendShapeChannelShape(context->GetPtr(), _this, weight, cstr.c_str());
}

void Morph::CopyBlendShapeControlPoints() {
	AsFbxMorphCopyBlendShapeControlPoints(_this);
}

void Morph::SetBlendShapeVertex(unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertex(_this, index, x, y, z);
}

void Morph::CopyBlendShapeControlPointsNormal() {
	AsFbxMorphCopyBlendShapeControlPointsNormal(_this);
}

void Morph::SetBlendShapeVertexNormal(unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertexNormal(_this, index, x, y, z);
}

AsFbxMorphContext* Morph::GetPtr() {
	return _this;
}

}
