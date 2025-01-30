#pragma once

#include "api.h"

using namespace System;
using namespace fbxsdk;

namespace FbxInterop {

public ref class Cluster {
private:
	FbxCluster* _ref;

internal:
	Cluster(FbxCluster* _ptr);
public:
	~Cluster();

internal:
	FbxCluster* GetPtr();
};

public ref class ClusterArray {
private:
	FbxArray<fbxsdk::FbxCluster*>* _this;

public:
	ClusterArray(int boneCount);
	~ClusterArray();

public:
	void AddCluster(Cluster^ cluster);
	void SetBoneWeight(int boneIndex, int vertexIndex, float weight);
	bool HasItemAt(int index);

internal:
	FbxArray<fbxsdk::FbxCluster*>* GetPtr();
};

}
