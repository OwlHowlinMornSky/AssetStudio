#pragma once

#include <fbxsdk.h>
#define OHMS_CLR_TEMPORARY
#include "api.h"

#include "Cluster.h"
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
	void MeshSkinAddCluster(ClusterArray^ pClusterArray, int index, float* pBoneMatrix);
	void MeshAddDeformer(Mesh^ pMesh);

internal:
	AsFbxSkinContext* GetPtr();
};

}
