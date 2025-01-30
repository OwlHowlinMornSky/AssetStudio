#pragma once

#include "api.h"

#include "Mesh.h"

using namespace System;

namespace FbxInterop {

public ref class Skin {
private:
	AsFbxSkinContext* _this;

public:
	Skin(AsFbxContext* context, FbxNode* node);
	~Skin();

public:
	void MeshSkinAddCluster(ClusterArray^ pClusterArray, int index, array<float>^ pBoneMatrix);
	void MeshAddDeformer(Mesh^ pMesh);

internal:
	AsFbxSkinContext* GetPtr();
};

}
