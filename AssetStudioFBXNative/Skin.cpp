#include "Skin.h"

#include "asfbx_skin_context.h"

namespace FbxInterop {

Skin::Skin(AsFbxContext* context, FbxNode* node) {
	_this = new AsFbxSkinContext(context, node);
}

Skin::~Skin() {
	delete _this;
	_this = nullptr;
}

void Skin::MeshSkinAddCluster(ClusterArray^ pClusterArray, int index, float* pBoneMatrix) {
	AsFbxMeshSkinAddCluster(_this, pClusterArray->GetPtr(), index, pBoneMatrix);
}

void Skin::MeshAddDeformer(Mesh^ pMesh) {
	AsFbxMeshAddDeformer(_this, pMesh->GetPtr());
}

AsFbxSkinContext* Skin::GetPtr() {
    return _this;
}

}

