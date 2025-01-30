#include "Skin.h"

namespace FbxInterop {

Skin::Skin(AsFbxContext* context, FbxNode* node) {
	_this = new AsFbxSkinContext(context, node);
}

Skin::~Skin() {
	delete _this;
	_this = nullptr;
}

void Skin::AddCluster(ClusterArray^ pClusterArray, int index, array<float>^ pBoneMatrix) {
	pin_ptr<float> mat = &pBoneMatrix[0];
	AsFbxMeshSkinAddCluster(_this, pClusterArray->GetPtr(), index, mat);
}

AsFbxSkinContext* Skin::GetPtr() {
    return _this;
}

}

