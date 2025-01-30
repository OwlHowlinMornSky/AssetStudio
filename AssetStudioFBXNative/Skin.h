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
	void AddCluster(ClusterArray^ clusterArray, int index, array<float>^ boneMatrix);

internal:
	AsFbxSkinContext* GetPtr();
};

}
