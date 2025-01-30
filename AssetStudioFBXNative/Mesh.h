#pragma once

#include <fbxsdk.h>
#define OHMS_CLR_TEMPORARY
#include "api.h"

#include "Cluster.h"

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
	void MeshInitControlPoints(int vertexCount);

	void MeshCreateElementNormal();

	void MeshCreateDiffuseUV(int uv);

	void MeshCreateNormalMapUV(int uv);

	void MeshCreateElementTangent();

	void MeshCreateElementVertexColor();

	void MeshCreateElementMaterial();

	void MeshSetControlPoint(int index, float x, float y, float z);

	void MeshAddPolygon(int materialIndex, int index0, int index1, int index2);

	void MeshElementNormalAdd(int elementIndex, float x, float y, float z);

	void MeshElementUVAdd(int elementIndex, float u, float v);

	void MeshElementTangentAdd(int elementIndex, float x, float y, float z, float w);

	void MeshElementVertexColorAdd(int elementIndex, float r, float g, float b, float a);

internal:
	FbxMesh* GetPtr();
};

}
