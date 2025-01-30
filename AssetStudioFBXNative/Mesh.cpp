#include "Mesh.h"

namespace FbxInterop {

Mesh::Mesh(FbxMesh* _ptr) {
	_ref = _ptr;
}

Mesh::~Mesh() {
	_ref = nullptr;
}

void Mesh::InitControlPoints(int vertexCount) {
	AsFbxMeshInitControlPoints(_ref, vertexCount);
}

void Mesh::CreateElementNormal() {
	AsFbxMeshCreateElementNormal(_ref);
}

void Mesh::CreateDiffuseUV(int uv) {
	AsFbxMeshCreateDiffuseUV(_ref, uv);
}

void Mesh::CreateNormalMapUV(int uv) {
	AsFbxMeshCreateNormalMapUV(_ref, uv);
}

void Mesh::CreateElementTangent() {
	AsFbxMeshCreateElementTangent(_ref);
}

void Mesh::CreateElementVertexColor() {
	AsFbxMeshCreateElementVertexColor(_ref);
}

void Mesh::CreateElementMaterial() {
	AsFbxMeshCreateElementMaterial(_ref);
}

void Mesh::SetControlPoint(int index, float x, float y, float z) {
	AsFbxMeshSetControlPoint(_ref, index, x, y, z);
}

void Mesh::AddPolygon(int materialIndex, int index0, int index1, int index2) {
	AsFbxMeshAddPolygon(_ref, materialIndex, index0, index1, index2);
}

void Mesh::AddDeformer(Skin^ skin) {
	AsFbxMeshAddDeformer(skin->GetPtr(), _ref);
}

void Mesh::ElementAddNormal(int elementIndex, float x, float y, float z) {
	AsFbxMeshElementNormalAdd(_ref, elementIndex, x, y, z);
}

void Mesh::ElementAddUV(int elementIndex, float u, float v) {
	AsFbxMeshElementUVAdd(_ref, elementIndex, u, v);
}

void Mesh::ElementAddTangent(int elementIndex, float x, float y, float z, float w) {
	AsFbxMeshElementTangentAdd(_ref, elementIndex, x, y, z, w);
}

void Mesh::ElementAddVertexColor(int elementIndex, float r, float g, float b, float a) {
	AsFbxMeshElementVertexColorAdd(_ref, elementIndex, r, g, b, a);
}

FbxMesh* Mesh::GetPtr() {
	return _ref;
}

}
