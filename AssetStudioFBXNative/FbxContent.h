#pragma once

using namespace System;

namespace FbxNative {

public ref class FbxContent {
public:
	static IntPtr CreateContext();

	static bool InitializeContext(
		IntPtr context, String^ fileName, float scaleFactor,
		int versionIndex, bool isAscii, bool is60Fps,
		[Runtime::InteropServices::OutAttribute]String^% errorMessage
	);

	static void DisposeContext(IntPtr% ppContext);

	static void SetFramePaths(IntPtr context, array<String^>^ framePaths);

	static void ExportScene(IntPtr context);

	static IntPtr GetSceneRootNode(IntPtr context);

	static IntPtr ExportSingleFrame(
		IntPtr context, IntPtr parentNode,
		String^ strFramePath, String^ strFrameName,
		float localPositionX, float localPositionY, float localPositionZ,
		float localRotationX, float localRotationY, float localRotationZ,
		float localScaleX, float localScaleY, float localScaleZ
	);

	static void SetJointsNode_CastToBone(IntPtr context, IntPtr node, float boneSize);

	static void SetJointsNode_BoneInPath(IntPtr context, IntPtr node, float boneSize);

	static void SetJointsNode_Generic(IntPtr context, IntPtr node);

	static void PrepareMaterials(IntPtr context, int materialCount, int textureCount);

	static IntPtr CreateTexture(IntPtr context, String^ strMatTexName);

	static void LinkTexture(int dest, IntPtr texture, IntPtr material, float offsetX, float offsetY, float scaleX, float scaleY);

	static IntPtr MeshCreateClusterArray(int boneCount);

	static void MeshDisposeClusterArray(IntPtr% ppArray);

	static IntPtr MeshCreateCluster(IntPtr context, IntPtr boneNode);

	static void MeshAddCluster(IntPtr array, IntPtr cluster);

	static IntPtr MeshCreateMesh(IntPtr context, IntPtr frameNode);

	static void MeshInitControlPoints(IntPtr mesh, int vertexCount);

	static void MeshCreateElementNormal(IntPtr mesh);

	static void MeshCreateDiffuseUV(IntPtr mesh, int uv);

	static void MeshCreateNormalMapUV(IntPtr mesh, int uv);

	static void MeshCreateElementTangent(IntPtr mesh);

	static void MeshCreateElementVertexColor(IntPtr mesh);

	static void MeshCreateElementMaterial(IntPtr mesh);

	static IntPtr CreateMaterial(
		IntPtr pContext, String^ pMatName,
		float diffuseR, float diffuseG, float diffuseB,
		float ambientR, float ambientG, float ambientB,
		float emissiveR, float emissiveG, float emissiveB,
		float specularR, float specularG, float specularB,
		float reflectR, float reflectG, float reflectB,
		float shininess, float transparency
	);

	static int AddMaterialToFrame(IntPtr frameNode, IntPtr material);

	static void SetFrameShadingModeToTextureShading(IntPtr frameNode);

	static void MeshSetControlPoint(IntPtr mesh, int index, float x, float y, float z);

	static void MeshAddPolygon(IntPtr mesh, int materialIndex, int index0, int index1, int index2);

	static void MeshElementNormalAdd(IntPtr mesh, int elementIndex, float x, float y, float z);

	static void MeshElementUVAdd(IntPtr mesh, int elementIndex, float u, float v);

	static void MeshElementTangentAdd(IntPtr mesh, int elementIndex, float x, float y, float z, float w);

	static void MeshElementVertexColorAdd(IntPtr mesh, int elementIndex, float r, float g, float b, float a);

	static void MeshSetBoneWeight(IntPtr pClusterArray, int boneIndex, int vertexIndex, float weight);

	static IntPtr MeshCreateSkinContext(IntPtr context, IntPtr frameNode);

	static void MeshDisposeSkinContext(IntPtr% ppSkinContext);

	static bool ClusterArray_HasItemAt(IntPtr pClusterArray, int index);

	static void MeshSkinAddCluster(IntPtr pSkinContext, IntPtr pClusterArray, int index, float* pBoneMatrix);

	static void MeshAddDeformer(IntPtr pSkinContext, IntPtr pMesh);

	static IntPtr AnimCreateContext(bool eulerFilter);

	static void AnimDisposeContext(IntPtr% ppAnimContext);

	static void AnimPrepareStackAndLayer(IntPtr pContext, IntPtr pAnimContext, String^ strTakeName);

	static void AnimLoadCurves(IntPtr pNode, IntPtr pAnimContext);

	static void AnimBeginKeyModify(IntPtr pAnimContext);

	static void AnimEndKeyModify(IntPtr pAnimContext);

	static void AnimAddScalingKey(IntPtr pAnimContext, float time, float x, float y, float z);

	static void AnimAddRotationKey(IntPtr pAnimContext, float time, float x, float y, float z);

	static void AnimAddTranslationKey(IntPtr pAnimContext, float time, float x, float y, float z);

	static void AnimApplyEulerFilter(IntPtr pAnimContext, float filterPrecision);

	static int AnimGetCurrentBlendShapeChannelCount(IntPtr pAnimContext, IntPtr pNode);

	static bool AnimIsBlendShapeChannelMatch(IntPtr pAnimContext, int channelIndex, String^ strChannelName);

	static void AnimBeginBlendShapeAnimCurve(IntPtr pAnimContext, int channelIndex);

	static void AnimEndBlendShapeAnimCurve(IntPtr pAnimContext);

	static void AnimAddBlendShapeKeyframe(IntPtr pAnimContext, float time, float value);

	static IntPtr MorphCreateContext();

	static void MorphInitializeContext(IntPtr pContext, IntPtr pMorphContext, IntPtr pNode);

	static void MorphDisposeContext(IntPtr% ppMorphContext);




	static void MorphAddBlendShapeChannel(IntPtr pContext, IntPtr pMorphContext, String^ strChannelName);

	static void MorphAddBlendShapeChannelShape(IntPtr pContext, IntPtr pMorphContext, float weight, String^ shapeName);

	static void MorphCopyBlendShapeControlPoints(IntPtr pMorphContext);

	static void MorphSetBlendShapeVertex(IntPtr pMorphContext, unsigned int index, float x, float y, float z);

	static void MorphCopyBlendShapeControlPointsNormal(IntPtr pMorphContext);

	static void MorphSetBlendShapeVertexNormal(IntPtr pMorphContext, unsigned int index, float x, float y, float z);


};

}
