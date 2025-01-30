#pragma once

#include "api.h"

#include "Node.h"
#include "Mesh.h"

using namespace System;

namespace FbxInterop {

public ref class Context {
private:
	AsFbxContext* _this;

public:
	Context();
	~Context();

public:
	bool Initialize(
		String^ fileName, float scaleFactor,
		int versionIndex, bool isAscii, bool is60Fps,
		[Runtime::InteropServices::OutAttribute]String^% errorMessage
	);

	void SetFramePaths(array<String^>^ framePaths);

	void ExportScene();

	Node^ GetSceneRootNode();

	Node^ ExportSingleFrame(
		Node^ parentNode, String^ framePath, String^ frameName,
		float localPositionX, float localPositionY, float localPositionZ,
		float localRotationX, float localRotationY, float localRotationZ,
		float localScaleX, float localScaleY, float localScaleZ
	);

	void SetJointsNode_CastToBone(Node^ node, float boneSize);

	void SetJointsNode_BoneInPath(Node^ node, float boneSize);

	void SetJointsNode_Generic(Node^ node);

	void PrepareMaterials(int materialCount, int textureCount);

	Texture^ CreateTexture(String^ matTexName);

	Cluster^ CreateCluster(Node^ boneNode);

	Mesh^ CreateMesh(Node^ frameNode);

	Material^ CreateMaterial(
		String^ matName,
		float diffuseR, float diffuseG, float diffuseB,
		float ambientR, float ambientG, float ambientB,
		float emissiveR, float emissiveG, float emissiveB,
		float specularR, float specularG, float specularB,
		float reflectR, float reflectG, float reflectB,
		float shininess, float transparency
	);

	Skin^ CreateSkinContext(Node^ frameNode);

internal:
	AsFbxContext* GetPtr();
};

}
