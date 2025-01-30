#include "Context.h"

#include "StringConvert.h"

namespace FbxInterop {

Context::Context() {
	_this = new AsFbxContext();
}

Context::~Context() {
	delete _this;
	_this = nullptr;
}

bool Context::Initialize(String^ fileName, float scaleFactor, int versionIndex, bool isAscii, bool is60Fps, String^% errorMessage) {
	bool b;
	const char* pErrMsg = nullptr;

	std::string cstr = StringToUnmanagedUtf8(fileName);

	b = AsFbxInitializeContext(
		_this, cstr.c_str(),
		scaleFactor, versionIndex, isAscii, is60Fps, &pErrMsg
	);

	errorMessage = gcnew String(pErrMsg ? pErrMsg : "");

	return b;
}

void Context::SetFramePaths(array<String^>^ framePaths) {
	int framePathCount = framePaths->Length;

	if (framePathCount == 0) {
		AsFbxSetFramePaths(_this, nullptr, 0);
		return;
	}

	std::vector<std::string> strings;
	strings.resize(framePathCount);
	std::vector<const char*> pPaths;
	pPaths.resize(framePathCount);

	for (int i = 0; i < framePathCount; i++) {
		strings[i] = StringToUnmanagedUtf8(framePaths[i]);
		pPaths[i] = strings[i].c_str();
	}

	AsFbxSetFramePaths(_this, pPaths.data(), framePathCount);
}

void Context::ExportScene() {
	AsFbxExportScene(_this);
}

Node^ Context::GetSceneRootNode() {
	return gcnew Node(AsFbxGetSceneRootNode(_this));
}

Node^ Context::ExportSingleFrame(
	Node^ parentNode, String^ strFramePath, String^ strFrameName,
	float localPositionX, float localPositionY, float localPositionZ,
	float localRotationX, float localRotationY, float localRotationZ,
	float localScaleX, float localScaleY, float localScaleZ
) {
	std::string cstr0 = StringToUnmanagedUtf8(strFramePath);
	std::string cstr1 = StringToUnmanagedUtf8(strFrameName);
	return gcnew Node(AsFbxExportSingleFrame(
		_this, parentNode->GetPtr(),
		cstr0.c_str(), cstr1.c_str(),
		localPositionX, localPositionY, localPositionZ,
		localRotationX, localRotationY, localRotationZ,
		localScaleX, localScaleY, localScaleZ
	));
}

void Context::SetJointsNode_CastToBone(Node^ node, float boneSize) {
	return AsFbxSetJointsNode_CastToBone(_this, node->GetPtr(), boneSize);
}

void Context::SetJointsNode_BoneInPath(Node^ node, float boneSize) {
	return AsFbxSetJointsNode_BoneInPath(_this, node->GetPtr(), boneSize);
}

void Context::SetJointsNode_Generic(Node^ node) {
	return AsFbxSetJointsNode_Generic(_this, node->GetPtr());
}

void Context::PrepareMaterials(int materialCount, int textureCount) {
	return AsFbxPrepareMaterials(_this, materialCount, textureCount);
}

Texture^ Context::CreateTexture(String^ strMatTexName) {
	std::string cstr = StringToUnmanagedUtf8(strMatTexName);
	return gcnew Texture(AsFbxCreateTexture(_this, cstr.c_str()));
}

Cluster^ Context::CreateCluster(Node^ boneNode) {
	return gcnew Cluster(AsFbxMeshCreateCluster(_this, boneNode->GetPtr()));
}

Mesh^ Context::CreateMesh(Node^ frameNode) {
	return gcnew Mesh(AsFbxMeshCreateMesh(_this, frameNode->GetPtr()));
}

Material^ Context::CreateMaterial(String^ pMatName, float diffuseR, float diffuseG, float diffuseB, float ambientR, float ambientG, float ambientB, float emissiveR, float emissiveG, float emissiveB, float specularR, float specularG, float specularB, float reflectR, float reflectG, float reflectB, float shininess, float transparency) {
	std::string cstr = StringToUnmanagedUtf8(pMatName);
	return gcnew Material(AsFbxCreateMaterial(
		_this, cstr.c_str(),
		diffuseR, diffuseG, diffuseB,
		ambientR, ambientG, ambientB,
		emissiveR, emissiveG, emissiveB,
		specularR, specularG, specularB,
		reflectR, reflectG, reflectB,
		shininess, transparency
	));
}

Skin^ Context::CreateSkinContext(Node^ frameNode) {
	return gcnew Skin(_this, frameNode->GetPtr());
}

AsFbxContext* Context::GetPtr() {
    return _this;
}

}
