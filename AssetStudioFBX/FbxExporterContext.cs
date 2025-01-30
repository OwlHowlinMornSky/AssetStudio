using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using AssetStudio;
using FbxInterop;
using Node = FbxInterop.Node;
using Texture = FbxInterop.Texture;
using Material = FbxInterop.Material;

namespace AssetStudioFBX {
	public sealed class FbxExporterContext() : IDisposable {

		private readonly ContextS _pContext = new();
		private readonly Dictionary<ImportedFrame, Node> _frameToNode = [];
		private readonly List<KeyValuePair<string, Material>> _createdMaterials = [];
		private readonly Dictionary<string, Texture> _createdTextures = [];

		~FbxExporterContext() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed = false;
		private void Dispose(bool _) {
			if (_disposed)
				return;
			_disposed = true;

			_frameToNode.Clear();
			_createdMaterials.Clear();
			_createdTextures.Clear();

			_pContext.Dispose();
		}

		private void EnsureNotDisposed() {
			ObjectDisposedException.ThrowIf(_disposed, nameof(FbxExporterContext));
		}

		internal void Initialize(string fileName, float scaleFactor, int versionIndex, bool isAscii, bool is60Fps) {
			EnsureNotDisposed();

			var b = _pContext.Initialize(fileName, scaleFactor, versionIndex, isAscii, is60Fps, out var errorMessage);

			if (!b) {
				var fullMessage = $"Failed to initialize FbxExporter: {errorMessage}";
				throw new ApplicationException(fullMessage);
			}
		}

		internal void SetFramePaths(HashSet<string> framePaths) {
			EnsureNotDisposed();

			if (framePaths == null || framePaths.Count == 0) {
				return;
			}

			_pContext.SetFramePaths([.. framePaths]);
		}

		internal void ExportScene() {
			EnsureNotDisposed();

			_pContext.ExportScene();
		}

		internal void ExportFrame(List<ImportedMesh> meshList, List<ImportedFrame> meshFrames, ImportedFrame rootFrame) {
			var rootNode = _pContext.GetSceneRootNode();

#warning the way of checking here is not good.
			Debug.Assert(rootNode.IsValid());

			var nodeStack = new Stack<Node>();
			var frameStack = new Stack<ImportedFrame>();

			nodeStack.Push(rootNode);
			frameStack.Push(rootFrame);

			while (nodeStack.Count > 0) {
				var parentNode = nodeStack.Pop();
				var frame = frameStack.Pop();

				var childNode = _pContext.ExportSingleFrame(parentNode, frame.Path, frame.Name, frame.LocalPosition, frame.LocalRotation, frame.LocalScale);

				if (meshList != null && ImportedHelpers.FindMesh(frame.Path, meshList) != null) {
					meshFrames.Add(frame);
				}

				_frameToNode.Add(frame, childNode);

				for (var i = frame.Count - 1; i >= 0; i -= 1) {
					nodeStack.Push(childNode);
					frameStack.Push(frame[i]);
				}
			}
		}

		internal void SetJointsNode(ImportedFrame rootFrame, HashSet<string> bonePaths, bool castToBone, float boneSize) {
			var frameStack = new Stack<ImportedFrame>();

			frameStack.Push(rootFrame);

			while (frameStack.Count > 0) {
				var frame = frameStack.Pop();

				if (_frameToNode.TryGetValue(frame, out var node)) {
#warning the way of checking here is not good.
					Debug.Assert(node.IsValid());

					if (castToBone) {
						_pContext.SetJointsNode_CastToBone(node, boneSize);
					}
					else {
						Debug.Assert(bonePaths != null);

						if (bonePaths.Contains(frame.Path)) {
							_pContext.SetJointsNode_BoneInPath(node, boneSize);
						}
						else {
							_pContext.SetJointsNode_Generic(node);
						}
					}
				}

				for (var i = frame.Count - 1; i >= 0; i -= 1) {
					frameStack.Push(frame[i]);
				}
			}
		}

		internal void PrepareMaterials(int materialCount, int textureCount) {
			_pContext.PrepareMaterials(materialCount, textureCount);
		}

		internal void ExportMeshFromFrame(ImportedFrame rootFrame, ImportedFrame meshFrame, List<ImportedMesh> meshList, List<ImportedMaterial> materialList, List<ImportedTexture> textureList, bool exportSkins, bool exportAllUvsAsDiffuseMaps) {
			var meshNode = _frameToNode[meshFrame];
			var mesh = ImportedHelpers.FindMesh(meshFrame.Path, meshList);

			ExportMesh(rootFrame, materialList, textureList, meshNode, mesh, exportSkins, exportAllUvsAsDiffuseMaps);
		}

		private Texture ExportTexture(ImportedTexture texture) {
			if (texture == null) {
				return null;
			}

			if (_createdTextures.TryGetValue(texture.Name, out Texture value)) {
				return value;
			}

			var pTex = _pContext.CreateTexture(texture.Name);

			_createdTextures.Add(texture.Name, pTex);

			var file = new FileInfo(texture.Name);

			using (var writer = new BinaryWriter(file.Create())) {
				writer.Write(texture.Data);
			}

			return pTex;
		}

		private void ExportMesh(
			ImportedFrame rootFrame,
			List<ImportedMaterial> materialList,
			List<ImportedTexture> textureList,
			Node frameNode,
			ImportedMesh importedMesh,
			bool exportSkins,
			bool exportAllUvsAsDiffuseMaps
		) {
			var boneList = importedMesh.BoneList;
			var totalBoneCount = 0;
			var hasBones = false;
			if (exportSkins && boneList?.Count > 0) {
				totalBoneCount = boneList.Count;
				hasBones = true;
			}

			ClusterArray pClusterArray = null;

			if (hasBones) {
				pClusterArray = new(totalBoneCount);

				foreach (var bone in boneList) {
					if (bone.Path != null) {
						var frame = rootFrame.FindFrameByPath(bone.Path);
						var boneNode = _frameToNode[frame];

						var cluster = _pContext.MeshCreateCluster(boneNode);

						pClusterArray.MeshAddCluster(cluster);
					}
					else {
						pClusterArray.MeshAddCluster(null);
					}
				}
			}

			var mesh = _pContext.MeshCreateMesh(frameNode);

			mesh.MeshInitControlPoints(importedMesh.VertexList.Count);

			if (importedMesh.hasNormal) {
				mesh.MeshCreateElementNormal();
			}

			for (int i = 0; i < importedMesh.hasUV.Length; i++) {
				if (!importedMesh.hasUV[i]) {
					continue;
				}

				if (i == 1 && !exportAllUvsAsDiffuseMaps) {
					mesh.MeshCreateNormalMapUV(1);
				}
				else {
					mesh.MeshCreateDiffuseUV(i);
				}
			}

			if (importedMesh.hasTangent) {
				mesh.MeshCreateElementTangent();
			}

			if (importedMesh.hasColor) {
				mesh.MeshCreateElementVertexColor();
			}

			mesh.MeshCreateElementMaterial();

			foreach (var meshObj in importedMesh.SubmeshList) {
				var materialIndex = 0;
				var mat = ImportedHelpers.FindMaterial(meshObj.Material, materialList);

				if (mat != null) {
					var foundMat = _createdMaterials.FindIndex(kv => kv.Key == mat.Name);
					Material pMat;

					if (foundMat >= 0) {
						pMat = _createdMaterials[foundMat].Value;
					}
					else {
						var diffuse = mat.Diffuse;
						var ambient = mat.Ambient;
						var emissive = mat.Emissive;
						var specular = mat.Specular;
						var reflection = mat.Reflection;

						pMat = _pContext.CreateMaterial(mat.Name, in diffuse, in ambient, in emissive, in specular, in reflection, mat.Shininess, mat.Transparency);

						_createdMaterials.Add(KeyValuePair.Create(mat.Name, pMat));
					}

					materialIndex = frameNode.AddMaterialToFrame(pMat);

					var hasTexture = false;

					foreach (var texture in mat.Textures) {
						var tex = ImportedHelpers.FindTexture(texture.Name, textureList);
						var pTexture = ExportTexture(tex);

						if (pTexture.IsValid()) {
							switch (texture.Dest) {
							case 0:
							case 1:
							case 2:
							case 3: {
								pMat.LinkTexture(texture.Dest, pTexture, texture.Offset.X, texture.Offset.Y, texture.Scale.X, texture.Scale.Y);
								hasTexture = true;
								break;
							}
							default:
								break;
							}
						}
					}

					if (hasTexture) {
						frameNode.SetFrameShadingModeToTextureShading();
					}
				}

				foreach (var face in meshObj.FaceList) {
					var index0 = face.VertexIndices[0] + meshObj.BaseVertex;
					var index1 = face.VertexIndices[1] + meshObj.BaseVertex;
					var index2 = face.VertexIndices[2] + meshObj.BaseVertex;

					mesh.MeshAddPolygon(materialIndex, index0, index1, index2);
				}
			}

			var vertexList = importedMesh.VertexList;

			var vertexCount = vertexList.Count;

			for (var j = 0; j < vertexCount; j += 1) {
				var importedVertex = vertexList[j];

				var vertex = importedVertex.Vertex;
				mesh.MeshSetControlPoint(j, vertex.X, vertex.Y, vertex.Z);

				if (importedMesh.hasNormal) {
					var normal = importedVertex.Normal;
					mesh.MeshElementNormalAdd(0, normal.X, normal.Y, normal.Z);
				}

				for (var uvIndex = 0; uvIndex < importedMesh.hasUV.Length; uvIndex += 1) {
					if (importedMesh.hasUV[uvIndex]) {
						var uv = importedVertex.UV[uvIndex];
						mesh.MeshElementUVAdd(uvIndex, uv[0], uv[1]);
					}
				}

				if (importedMesh.hasTangent) {
					var tangent = importedVertex.Tangent;
					mesh.MeshElementTangentAdd(0, tangent.X, tangent.Y, tangent.Z, tangent.W);
				}

				if (importedMesh.hasColor) {
					var color = importedVertex.Color;
					mesh.MeshElementVertexColorAdd(0, color.R, color.G, color.B, color.A);
				}

				if (hasBones && importedVertex.BoneIndices != null && pClusterArray != null) {
					var boneIndices = importedVertex.BoneIndices;
					var boneWeights = importedVertex.Weights;

					for (var k = 0; k < 4; k += 1) {
						if (boneIndices[k] < totalBoneCount && boneWeights[k] > 0) {
							pClusterArray.MeshSetBoneWeight(boneIndices[k], j, boneWeights[k]);
						}
					}
				}
			}


			if (hasBones) {
				Skin pSkinContext = _pContext.MeshCreateSkinContext(frameNode);

				for (var j = 0; j < totalBoneCount; j += 1) {
					if (!pClusterArray.ClusterArray_HasItemAt(j))
						continue;

					var m = boneList[j].Matrix;

					float[] array = [
						m[0], m[1], m[2], m[3],
						m[4], m[5], m[6], m[7],
						m[8], m[9], m[10], m[11],
						m[12], m[13], m[14], m[15]
					];

					pSkinContext.MeshSkinAddCluster(pClusterArray, j, array);
				}

				pSkinContext.MeshAddDeformer(mesh);
			}
		}

		internal void ExportAnimations(ImportedFrame rootFrame, List<ImportedKeyframedAnimation> animationList, bool eulerFilter, float filterPrecision) {
			if (animationList == null || animationList.Count == 0) {
				return;
			}

			var pAnimContext = new Anim(eulerFilter);

			for (int i = 0; i < animationList.Count; i++) {
				var importedAnimation = animationList[i];
				string takeName;

				if (importedAnimation.Name != null) {
					takeName = importedAnimation.Name;
				}
				else {
					takeName = $"Take{i}";
				}

				pAnimContext.AnimPrepareStackAndLayer(_pContext, takeName);

				ExportKeyframedAnimation(rootFrame, importedAnimation, pAnimContext, filterPrecision);
			}
		}

		private void ExportKeyframedAnimation(ImportedFrame rootFrame, ImportedKeyframedAnimation parser, Anim pAnimContext, float filterPrecision) {
			foreach (var track in parser.TrackList) {
				if (track.Path == null)
					continue;

				var frame = rootFrame.FindFrameByPath(track.Path);

				if (frame == null)
					continue;

				var pNode = _frameToNode[frame];

				pAnimContext.AnimLoadCurves(pNode);

				pAnimContext.AnimBeginKeyModify();

				foreach (var scaling in track.Scalings) {
					var value = scaling.value;
					pAnimContext.AnimAddScalingKey(scaling.time, value.X, value.Y, value.Z);
				}

				foreach (var rotation in track.Rotations) {
					var value = rotation.value;
					pAnimContext.AnimAddRotationKey(rotation.time, value.X, value.Y, value.Z);
				}

				foreach (var translation in track.Translations) {
					var value = translation.value;
					pAnimContext.AnimAddTranslationKey(translation.time, value.X, value.Y, value.Z);
				}

				pAnimContext.AnimEndKeyModify();

				pAnimContext.AnimApplyEulerFilter(filterPrecision);

				var blendShape = track.BlendShape;

				if (blendShape == null)
					continue;

				var channelCount = pAnimContext.AnimGetCurrentBlendShapeChannelCount(pNode);

				if (channelCount <= 0)
					continue;

				for (var channelIndex = 0; channelIndex < channelCount; channelIndex += 1) {
					if (!pAnimContext.AnimIsBlendShapeChannelMatch(channelIndex, blendShape.ChannelName))
						continue;

					pAnimContext.AnimBeginBlendShapeAnimCurve(channelIndex);

					foreach (var keyframe in blendShape.Keyframes) {
						pAnimContext.AnimAddBlendShapeKeyframe(keyframe.time, keyframe.value);
					}

					pAnimContext.AnimEndBlendShapeAnimCurve();
				}
			}
		}

		internal void ExportMorphs(ImportedFrame rootFrame, List<ImportedMorph> morphList) {
			if (morphList == null || morphList.Count == 0) {
				return;
			}

			foreach (var morph in morphList) {
				var frame = rootFrame.FindFrameByPath(morph.Path);

				if (frame == null)
					continue;

				var pNode = _frameToNode[frame];

				var pMorphContext = new Morph();

				pMorphContext.MorphInitializeContext(_pContext, pNode);

				foreach (var channel in morph.Channels) {
					pMorphContext.MorphAddBlendShapeChannel(_pContext, channel.Name);

					for (var i = 0; i < channel.KeyframeList.Count; i++) {
						var keyframe = channel.KeyframeList[i];

						pMorphContext.MorphAddBlendShapeChannelShape(_pContext, keyframe.Weight, i == 0 ? channel.Name : $"{channel.Name}_{i + 1}");

						pMorphContext.MorphCopyBlendShapeControlPoints();

						foreach (var vertex in keyframe.VertexList) {
							var v = vertex.Vertex.Vertex;
							pMorphContext.MorphSetBlendShapeVertex(vertex.Index, v.X, v.Y, v.Z);
						}

						if (!keyframe.hasNormals)
							continue;

						pMorphContext.MorphCopyBlendShapeControlPointsNormal();

						foreach (var vertex in keyframe.VertexList) {
							var v = vertex.Vertex.Normal;
							pMorphContext.MorphSetBlendShapeVertexNormal(vertex.Index, v.X, v.Y, v.Z);
						}
					}
				}
			}
		}

	}
}
