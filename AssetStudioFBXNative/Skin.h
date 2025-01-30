#pragma once

#include "api.h"

#include "Cluster.h"

using namespace System;

namespace FbxInterop {

public ref class Skin {
private:
	AsFbxSkinContext* _this;

public:
	Skin(AsFbxContext* context, FbxNode* node);
	~Skin();

public:
	void AddCluster(ClusterArray^ pClusterArray, int index, array<float>^ pBoneMatrix);

internal:
	AsFbxSkinContext* GetPtr();
};

}
