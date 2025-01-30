#include "Skin.h"

namespace FbxInterop {

Skin::Skin(AsFbxContext* context, FbxNode* node) {
	_this = new AsFbxSkinContext(context, node);
}

Skin::~Skin() {
	delete _this;
	_this = nullptr;
}

void Skin::MeshSkinAddCluster(ClusterArray^ pClusterArray, int index, array<float>^ pBoneMatrix) {
	pin_ptr<float> mat = &pBoneMatrix[0];
	AsFbxMeshSkinAddCluster(_this, pClusterArray->GetPtr(), index, mat);
}

void Skin::MeshAddDeformer(Mesh^ pMesh) {
	AsFbxMeshAddDeformer(_this, pMesh->GetPtr());
}

AsFbxSkinContext* Skin::GetPtr() {
    return _this;
}

}

