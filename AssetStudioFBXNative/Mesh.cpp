#include "Mesh.h"

namespace FbxInterop {

Mesh::Mesh(FbxMesh* _ptr) {
	_ref = _ptr;
}

Mesh::~Mesh() {
	_ref = nullptr;
}

void Mesh::MeshInitControlPoints(int vertexCount) {
	AsFbxMeshInitControlPoints(_ref, vertexCount);
}

void Mesh::MeshCreateElementNormal() {
	AsFbxMeshCreateElementNormal(_ref);
}

void Mesh::MeshCreateDiffuseUV(int uv) {
	AsFbxMeshCreateDiffuseUV(_ref, uv);
}

void Mesh::MeshCreateNormalMapUV(int uv) {
	AsFbxMeshCreateNormalMapUV(_ref, uv);
}

void Mesh::MeshCreateElementTangent() {
	AsFbxMeshCreateElementTangent(_ref);
}

void Mesh::MeshCreateElementVertexColor() {
	AsFbxMeshCreateElementVertexColor(_ref);
}

void Mesh::MeshCreateElementMaterial() {
	AsFbxMeshCreateElementMaterial(_ref);
}

void Mesh::MeshSetControlPoint(int index, float x, float y, float z) {
	AsFbxMeshSetControlPoint(_ref, index, x, y, z);
}

void Mesh::MeshAddPolygon(int materialIndex, int index0, int index1, int index2) {
	AsFbxMeshAddPolygon(_ref, materialIndex, index0, index1, index2);
}

void Mesh::MeshElementNormalAdd(int elementIndex, float x, float y, float z) {
	AsFbxMeshElementNormalAdd(_ref, elementIndex, x, y, z);
}

void Mesh::MeshElementUVAdd(int elementIndex, float u, float v) {
	AsFbxMeshElementUVAdd(_ref, elementIndex, u, v);
}

void Mesh::MeshElementTangentAdd(int elementIndex, float x, float y, float z, float w) {
	AsFbxMeshElementTangentAdd(_ref, elementIndex, x, y, z, w);
}

void Mesh::MeshElementVertexColorAdd(int elementIndex, float r, float g, float b, float a) {
	AsFbxMeshElementVertexColorAdd(_ref, elementIndex, r, g, b, a);
}

FbxMesh* Mesh::GetPtr() {
	return _ref;
}

}
