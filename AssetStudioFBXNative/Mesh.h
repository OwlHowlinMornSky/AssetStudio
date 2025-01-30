#pragma once

#include "api.h"

#include "Cluster.h"
#include "Skin.h"

using namespace System;
using namespace fbxsdk;

namespace FbxInterop {

public ref class Mesh {
private:
	FbxMesh* _ref;

internal:
	Mesh(FbxMesh* _ptr);
public:
	~Mesh();

public:
	void InitControlPoints(int vertexCount);
	void CreateElementNormal();
	void CreateDiffuseUV(int uv);
	void CreateNormalMapUV(int uv);
	void CreateElementTangent();
	void CreateElementVertexColor();
	void CreateElementMaterial();
	void SetControlPoint(int index, float x, float y, float z);
	void AddPolygon(int materialIndex, int index0, int index1, int index2);
	void AddDeformer(Skin^ skin);
	void ElementAddNormal(int elementIndex, float x, float y, float z);
	void ElementAddUV(int elementIndex, float u, float v);
	void ElementAddTangent(int elementIndex, float x, float y, float z, float w);
	void ElementAddVertexColor(int elementIndex, float r, float g, float b, float a);

internal:
	FbxMesh* GetPtr();
};

}
