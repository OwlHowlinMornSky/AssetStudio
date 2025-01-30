#include "FbxContent.h"

#include <string>
#include <vector>

#include <fbxsdk.h>
#define OHMS_CLR_TEMPORARY
#include "api.h"

#include "asfbx_context.h"
#include "asfbx_skin_context.h"
#include "asfbx_anim_context.h"
#include "asfbx_morph_context.h"
#include "utils.h"

namespace {

std::string StringToUnmanagedUtf8(String^% managedStr) {
	std::string str;
	array<Byte>^ arr = Text::Encoding::UTF8->GetBytes(managedStr);
	int len = arr->Length;
	str.resize(len);
	IntPtr pcstr((void*)str.data());
	Runtime::InteropServices::Marshal::Copy(arr, 0, pcstr, len);
	return str;
}

}

namespace FbxNative {

IntPtr FbxContent::CreateContext() {
	return IntPtr(AsFbxCreateContext());
}

bool FbxContent::InitializeContext(
	IntPtr context, String^ fileName, float scaleFactor,
	int versionIndex, bool isAscii, bool is60Fps,
	String^% errorMessage
) {
	bool b;
	const char* pErrMsg = nullptr;

	std::string cstr = StringToUnmanagedUtf8(fileName);

	b = AsFbxInitializeContext(
		(AsFbxContext*)(void*)context, cstr.c_str(),
		scaleFactor, versionIndex, isAscii, is60Fps, &pErrMsg
	);

	errorMessage = gcnew String(pErrMsg ? pErrMsg : "");

	return b;
}

void FbxContent::DisposeContext(IntPtr% ppContext) {
	AsFbxContext* res = (AsFbxContext*)(void*)ppContext;
	AsFbxDisposeContext(&res);
	ppContext = IntPtr(res);
}

void FbxContent::SetFramePaths(IntPtr context, array<String^>^ framePaths) {
	int framePathCount = framePaths->Length;

	if (framePathCount == 0) {
		AsFbxSetFramePaths((AsFbxContext*)(void*)context, nullptr, 0);
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

	AsFbxSetFramePaths((AsFbxContext*)(void*)context, pPaths.data(), framePathCount);
}

void FbxContent::ExportScene(IntPtr context) {
	AsFbxExportScene((AsFbxContext*)(void*)context);
}

IntPtr FbxContent::GetSceneRootNode(IntPtr context) {
	return IntPtr(AsFbxGetSceneRootNode((AsFbxContext*)(void*)context));
}

IntPtr FbxContent::ExportSingleFrame(
	IntPtr context, IntPtr parentNode,
	String^ strFramePath, String^ strFrameName,
	float localPositionX, float localPositionY, float localPositionZ,
	float localRotationX, float localRotationY, float localRotationZ,
	float localScaleX, float localScaleY, float localScaleZ
) {
	std::string cstr0 = StringToUnmanagedUtf8(strFramePath);
	std::string cstr1 = StringToUnmanagedUtf8(strFrameName);
	return IntPtr(AsFbxExportSingleFrame(
		(AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)parentNode,
		cstr0.c_str(), cstr1.c_str(),
		localPositionX, localPositionY, localPositionZ,
		localRotationX, localRotationY, localRotationZ,
		localScaleX, localScaleY, localScaleZ
	));
}

void FbxContent::SetJointsNode_CastToBone(IntPtr context, IntPtr node, float boneSize) {
	return AsFbxSetJointsNode_CastToBone(
		(AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)node, boneSize
	);
}

void FbxContent::SetJointsNode_BoneInPath(IntPtr context, IntPtr node, float boneSize) {
	return AsFbxSetJointsNode_BoneInPath(
		(AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)node, boneSize
	);
}

void FbxContent::SetJointsNode_Generic(IntPtr context, IntPtr node) {
	return AsFbxSetJointsNode_Generic(
		(AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)node
	);
}

void FbxContent::PrepareMaterials(IntPtr context, int materialCount, int textureCount) {
	return AsFbxPrepareMaterials(
		(AsFbxContext*)(void*)context, materialCount, textureCount
	);
}

IntPtr FbxContent::CreateTexture(IntPtr context, String^ strMatTexName) {
	std::string cstr = StringToUnmanagedUtf8(strMatTexName);
	return IntPtr(AsFbxCreateTexture(
		(AsFbxContext*)(void*)context, cstr.c_str()
	));
}

void FbxContent::LinkTexture(int dest, IntPtr texture, IntPtr material, float offsetX, float offsetY, float scaleX, float scaleY) {
	return AsFbxLinkTexture(
		dest, (fbxsdk::FbxFileTexture*)(void*)texture, (fbxsdk::FbxSurfacePhong*)(void*)material, offsetX, offsetY, scaleX, scaleY
	);
}

IntPtr FbxContent::MeshCreateClusterArray(int boneCount) {
	return IntPtr(AsFbxMeshCreateClusterArray(boneCount));
}

void FbxContent::MeshDisposeClusterArray(IntPtr% ppArray) {
	fbxsdk::FbxArray<fbxsdk::FbxCluster*>* res = (fbxsdk::FbxArray<fbxsdk::FbxCluster*>*)(void*)ppArray;
	AsFbxMeshDisposeClusterArray(&res);
	ppArray = IntPtr(res);
}

IntPtr FbxContent::MeshCreateCluster(IntPtr context, IntPtr boneNode) {
	return IntPtr(AsFbxMeshCreateCluster(
		(AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)boneNode
	));
}

void FbxContent::MeshAddCluster(IntPtr array, IntPtr cluster) {
	fbxsdk::FbxArray<fbxsdk::FbxCluster*>* res = (fbxsdk::FbxArray<fbxsdk::FbxCluster*>*)(void*)array;
	AsFbxMeshAddCluster(res, (fbxsdk::FbxCluster*)(void*)cluster);
}

IntPtr FbxContent::MeshCreateMesh(IntPtr context, IntPtr frameNode) {
	return IntPtr(AsFbxMeshCreateMesh((AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)frameNode));
}

void FbxContent::MeshInitControlPoints(IntPtr mesh, int vertexCount) {
	AsFbxMeshInitControlPoints(
		(fbxsdk::FbxMesh*)(void*)mesh, vertexCount
	);
}

void FbxContent::MeshCreateElementNormal(IntPtr mesh) {
	AsFbxMeshCreateElementNormal((fbxsdk::FbxMesh*)(void*)mesh);
}

void FbxContent::MeshCreateDiffuseUV(IntPtr mesh, int uv) {
	AsFbxMeshCreateDiffuseUV((fbxsdk::FbxMesh*)(void*)mesh, uv);
}

void FbxContent::MeshCreateNormalMapUV(IntPtr mesh, int uv) {
	AsFbxMeshCreateNormalMapUV((fbxsdk::FbxMesh*)(void*)mesh, uv);
}

void FbxContent::MeshCreateElementTangent(IntPtr mesh) {
	AsFbxMeshCreateElementTangent((fbxsdk::FbxMesh*)(void*)mesh);
}

void FbxContent::MeshCreateElementVertexColor(IntPtr mesh) {
	AsFbxMeshCreateElementVertexColor((fbxsdk::FbxMesh*)(void*)mesh);
}

void FbxContent::MeshCreateElementMaterial(IntPtr mesh) {
	AsFbxMeshCreateElementMaterial((fbxsdk::FbxMesh*)(void*)mesh);
}

IntPtr FbxContent::CreateMaterial(IntPtr pContext, String^ pMatName, float diffuseR, float diffuseG, float diffuseB, float ambientR, float ambientG, float ambientB, float emissiveR, float emissiveG, float emissiveB, float specularR, float specularG, float specularB, float reflectR, float reflectG, float reflectB, float shininess, float transparency) {
	std::string cstr = StringToUnmanagedUtf8(pMatName);
	return IntPtr(AsFbxCreateMaterial(
		(AsFbxContext*)(void*)pContext, cstr.c_str(),
		diffuseR, diffuseG, diffuseB,
		ambientR, ambientG, ambientB,
		emissiveR, emissiveG, emissiveB,
		specularR, specularG, specularB,
		reflectR, reflectG, reflectB,
		shininess, transparency
	));
}

int FbxContent::AddMaterialToFrame(IntPtr frameNode, IntPtr material) {
	return AsFbxAddMaterialToFrame((fbxsdk::FbxNode*)(void*)frameNode, (fbxsdk::FbxSurfacePhong*)(void*)material);
}

void FbxContent::SetFrameShadingModeToTextureShading(IntPtr frameNode) {
	AsFbxSetFrameShadingModeToTextureShading((fbxsdk::FbxNode*)(void*)frameNode);
}

void FbxContent::MeshSetControlPoint(IntPtr mesh, int index, float x, float y, float z) {
	AsFbxMeshSetControlPoint((fbxsdk::FbxMesh*)(void*)mesh, index, x, y, z);
}

void FbxContent::MeshAddPolygon(IntPtr mesh, int materialIndex, int index0, int index1, int index2) {
	AsFbxMeshAddPolygon((fbxsdk::FbxMesh*)(void*)mesh, materialIndex, index0, index1, index2);
}

void FbxContent::MeshElementNormalAdd(IntPtr mesh, int elementIndex, float x, float y, float z) {
	AsFbxMeshElementNormalAdd((fbxsdk::FbxMesh*)(void*)mesh, elementIndex, x, y, z);
}

void FbxContent::MeshElementUVAdd(IntPtr mesh, int elementIndex, float u, float v) {
	AsFbxMeshElementUVAdd((fbxsdk::FbxMesh*)(void*)mesh, elementIndex, u, v);
}

void FbxContent::MeshElementTangentAdd(IntPtr mesh, int elementIndex, float x, float y, float z, float w) {
	AsFbxMeshElementTangentAdd((fbxsdk::FbxMesh*)(void*)mesh, elementIndex, x, y, z, w);
}

void FbxContent::MeshElementVertexColorAdd(IntPtr mesh, int elementIndex, float r, float g, float b, float a) {
	AsFbxMeshElementVertexColorAdd((fbxsdk::FbxMesh*)(void*)mesh, elementIndex, r, g, b, a);
}

void FbxContent::MeshSetBoneWeight(IntPtr pClusterArray, int boneIndex, int vertexIndex, float weight) {
	AsFbxMeshSetBoneWeight((fbxsdk::FbxArray<fbxsdk::FbxCluster*>*)(void*)pClusterArray, boneIndex, vertexIndex, weight);
}

IntPtr FbxContent::MeshCreateSkinContext(IntPtr context, IntPtr frameNode) {
	return IntPtr(AsFbxMeshCreateSkinContext((AsFbxContext*)(void*)context, (fbxsdk::FbxNode*)(void*)frameNode));
}

void FbxContent::MeshDisposeSkinContext(IntPtr% ppSkinContext) {
	AsFbxSkinContext* res = (AsFbxSkinContext*)(void*)ppSkinContext;
	AsFbxMeshDisposeSkinContext(&res);
	ppSkinContext = (IntPtr)res;
}

bool FbxContent::ClusterArray_HasItemAt(IntPtr pClusterArray, int index) {
	return FbxClusterArray_HasItemAt((fbxsdk::FbxArray<fbxsdk::FbxCluster*>*)(void*)pClusterArray, index);
}

void FbxContent::MeshSkinAddCluster(IntPtr pSkinContext, IntPtr pClusterArray, int index, float* pBoneMatrix) {
	AsFbxMeshSkinAddCluster(
		(AsFbxSkinContext*)(void*)pSkinContext,
		(fbxsdk::FbxArray<fbxsdk::FbxCluster*>*)(void*)pClusterArray,
		index,
		pBoneMatrix
	);
}

void FbxContent::MeshAddDeformer(IntPtr pSkinContext, IntPtr pMesh) {
	AsFbxMeshAddDeformer((AsFbxSkinContext*)(void*)pSkinContext, (fbxsdk::FbxMesh*)(void*)pMesh);
}

IntPtr FbxContent::AnimCreateContext(bool eulerFilter) {
	return IntPtr(AsFbxAnimCreateContext(eulerFilter));
}

void FbxContent::AnimDisposeContext(IntPtr% ppAnimContext) {
	AsFbxAnimContext* res = (AsFbxAnimContext*)(void*)ppAnimContext;
	AsFbxAnimDisposeContext(&res);
	ppAnimContext = (IntPtr)res;
}

void FbxContent::AnimPrepareStackAndLayer(IntPtr pContext, IntPtr pAnimContext, String^ strTakeName) {
	std::string cstr = StringToUnmanagedUtf8(strTakeName);
	AsFbxAnimPrepareStackAndLayer(
		(AsFbxContext*)(void*)pContext,
		(AsFbxAnimContext*)(void*)pAnimContext,
		cstr.c_str()
	);
}

void FbxContent::AnimLoadCurves(IntPtr pNode, IntPtr pAnimContext) {
	AsFbxAnimLoadCurves((fbxsdk::FbxNode*)(void*)pNode, (AsFbxAnimContext*)(void*)pAnimContext);
}

void FbxContent::AnimBeginKeyModify(IntPtr pAnimContext) {
	AsFbxAnimBeginKeyModify((AsFbxAnimContext*)(void*)pAnimContext);
}

void FbxContent::AnimEndKeyModify(IntPtr pAnimContext) {
	AsFbxAnimEndKeyModify((AsFbxAnimContext*)(void*)pAnimContext);
}

void FbxContent::AnimAddScalingKey(IntPtr pAnimContext, float time, float x, float y, float z) {
	AsFbxAnimAddScalingKey(
		(AsFbxAnimContext*)(void*)pAnimContext,
		time, x, y, z
	);
}

void FbxContent::AnimAddRotationKey(IntPtr pAnimContext, float time, float x, float y, float z) {
	AsFbxAnimAddRotationKey(
		(AsFbxAnimContext*)(void*)pAnimContext,
		time, x, y, z
	);
}

void FbxContent::AnimAddTranslationKey(IntPtr pAnimContext, float time, float x, float y, float z) {
	AsFbxAnimAddTranslationKey(
		(AsFbxAnimContext*)(void*)pAnimContext,
		time, x, y, z
	);
}

void FbxContent::AnimApplyEulerFilter(IntPtr pAnimContext, float filterPrecision) {
	AsFbxAnimApplyEulerFilter((AsFbxAnimContext*)(void*)pAnimContext, filterPrecision);
}

int FbxContent::AnimGetCurrentBlendShapeChannelCount(IntPtr pAnimContext, IntPtr pNode) {
	return AsFbxAnimGetCurrentBlendShapeChannelCount((AsFbxAnimContext*)(void*)pAnimContext, (fbxsdk::FbxNode*)(void*)pNode);
}

bool FbxContent::AnimIsBlendShapeChannelMatch(IntPtr pAnimContext, int channelIndex, String^ strChannelName) {
	std::string cstr = StringToUnmanagedUtf8(strChannelName);
	return AsFbxAnimIsBlendShapeChannelMatch(
		(AsFbxAnimContext*)(void*)pAnimContext,
		channelIndex, cstr.c_str()
	);
}

void FbxContent::AnimBeginBlendShapeAnimCurve(IntPtr pAnimContext, int channelIndex) {
	AsFbxAnimBeginBlendShapeAnimCurve((AsFbxAnimContext*)(void*)pAnimContext, channelIndex);
}

void FbxContent::AnimEndBlendShapeAnimCurve(IntPtr pAnimContext) {
	AsFbxAnimEndBlendShapeAnimCurve((AsFbxAnimContext*)(void*)pAnimContext);
}

void FbxContent::AnimAddBlendShapeKeyframe(IntPtr pAnimContext, float time, float value) {
	AsFbxAnimAddBlendShapeKeyframe((AsFbxAnimContext*)(void*)pAnimContext, time, value);
}

IntPtr FbxContent::MorphCreateContext() {
	return IntPtr(AsFbxMorphCreateContext());
}

void FbxContent::MorphInitializeContext(IntPtr pContext, IntPtr pMorphContext, IntPtr pNode) {
	AsFbxMorphInitializeContext((AsFbxContext*)(void*)pContext, (AsFbxMorphContext*)(void*)pMorphContext, (fbxsdk::FbxNode*)(void*)pNode);
}

void FbxContent::MorphDisposeContext(IntPtr% ppMorphContext) {
	AsFbxMorphContext* res = (AsFbxMorphContext*)(void*)ppMorphContext;
	AsFbxMorphDisposeContext(&res);
	ppMorphContext = (IntPtr)res;
}

void FbxContent::MorphAddBlendShapeChannel(IntPtr pContext, IntPtr pMorphContext, String^ strChannelName) {
	std::string cstr = StringToUnmanagedUtf8(strChannelName);
	AsFbxMorphAddBlendShapeChannel((AsFbxContext*)(void*)pContext, (AsFbxMorphContext*)(void*)pMorphContext, cstr.c_str());
}

void FbxContent::MorphAddBlendShapeChannelShape(IntPtr pContext, IntPtr pMorphContext, float weight, String^ shapeName) {
	std::string cstr = StringToUnmanagedUtf8(shapeName);
	AsFbxMorphAddBlendShapeChannelShape((AsFbxContext*)(void*)pContext, (AsFbxMorphContext*)(void*)pMorphContext, weight, cstr.c_str());
}

void FbxContent::MorphCopyBlendShapeControlPoints(IntPtr pMorphContext) {
	AsFbxMorphCopyBlendShapeControlPoints((AsFbxMorphContext*)(void*)pMorphContext);
}

void FbxContent::MorphSetBlendShapeVertex(IntPtr pMorphContext, unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertex((AsFbxMorphContext*)(void*)pMorphContext, index, x, y, z);
}

void FbxContent::MorphCopyBlendShapeControlPointsNormal(IntPtr pMorphContext) {
	AsFbxMorphCopyBlendShapeControlPointsNormal((AsFbxMorphContext*)(void*)pMorphContext);
}

void FbxContent::MorphSetBlendShapeVertexNormal(IntPtr pMorphContext, unsigned int index, float x, float y, float z) {
	AsFbxMorphSetBlendShapeVertexNormal((AsFbxMorphContext*)(void*)pMorphContext, index, x, y, z);
}

}
