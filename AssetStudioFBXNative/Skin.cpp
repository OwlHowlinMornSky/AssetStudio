#include "Skin.h"

namespace FbxInterop {

Skin::Skin(AsFbxContext* context, FbxNode* node) {
	_this = new AsFbxSkinContext(context, node);
}

Skin::~Skin() {
	delete _this;
	_this = nullptr;
}

void Skin::AddCluster(ClusterArray^ clusterArray, int index, array<float>^ boneMatrix) {
	pin_ptr<float> mat = &boneMatrix[0];
	AsFbxMeshSkinAddCluster(_this, clusterArray->GetPtr(), index, mat);
}

AsFbxSkinContext* Skin::GetPtr() {
	return _this;
}

}

