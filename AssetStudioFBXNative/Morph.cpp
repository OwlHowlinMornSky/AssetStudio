#include "Morph.h"

#include "asfbx_morph_context.h"

#include "StringConvert.h"

namespace FbxInterop {

Morph::Morph() {
	_this = new AsFbxMorphContext();
}

Morph::~Morph() {
	delete _this;
	_this = nullptr;
}

void Morph::MorphInitializeContext(Context^ pContext, Node^ pNode) {
	AsFbxMorphInitializeContext(pContext->GetPtr(), _this, pNode->GetPtr());
}

void Morph::MorphAddBlendShapeChannel(Context^ pContext, String^ strChannelName) {
	std::string cstr = StringToUnmanagedUtf8(strChannelName);
	AsFbxMorphAddBlendShapeChannel(pContext->GetPtr(), _this, cstr.c_str());
}

void Morph::MorphAddBlendShapeChannelShape(Context^ pContext, float weight, String^ shapeName) {
	std::string cstr = StringToUnmanagedUtf8(shapeName);
	AsFbxMorphAddBlendShapeChannelShape(pContext->GetPtr(), _this, weight, cstr.c_str());
}

void Morph::MorphCopyBlendShapeControlPoints() {
	AsFbxMorphCopyBlendShapeControlPoints(_this);
}

void Morph::MorphSetBlendShapeVertex(unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertex(_this, index, x, y, z);
}

void Morph::MorphCopyBlendShapeControlPointsNormal() {
	AsFbxMorphCopyBlendShapeControlPointsNormal(_this);
}

void Morph::MorphSetBlendShapeVertexNormal(unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertexNormal(_this, index, x, y, z);
}

AsFbxMorphContext* Morph::GetPtr() {
	return _this;
}

}
