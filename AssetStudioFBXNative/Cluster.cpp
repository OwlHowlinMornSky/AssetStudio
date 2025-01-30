#include "Cluster.h"

namespace FbxInterop {

Cluster::Cluster(FbxCluster* _ptr) {
	_ref = _ptr;
}

Cluster::~Cluster() {
	_ref = nullptr;
}

FbxCluster* Cluster::GetPtr() {
	return _ref;
}

ClusterArray::ClusterArray(int boneCount) {
	_this = new FbxArray<FbxCluster*>(boneCount);
}

ClusterArray::~ClusterArray() {
	delete _this;
	_this = nullptr;
}

void ClusterArray::MeshAddCluster(Cluster^ cluster) {
	AsFbxMeshAddCluster(_this, cluster != nullptr ? cluster->GetPtr() : nullptr);
}

void ClusterArray::MeshSetBoneWeight(int boneIndex, int vertexIndex, float weight) {
	AsFbxMeshSetBoneWeight(_this, boneIndex, vertexIndex, weight);
}

bool ClusterArray::ClusterArray_HasItemAt(int index) {
	return FbxClusterArray_HasItemAt(_this, index);
}

FbxArray<fbxsdk::FbxCluster*>* ClusterArray::GetPtr() {
	return _this;
}

}
