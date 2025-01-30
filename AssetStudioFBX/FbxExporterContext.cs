using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AssetStudio;
using FbxInterop;
using Node = FbxInterop.Node;
using Texture = FbxInterop.Texture;
using Material = FbxInterop.Material;

namespace AssetStudioFBX {
	public sealed class FbxExporterContext() : IDisposable {

		private readonly ContextS m_context = new();
		private readonly Dictionary<ImportedFrame, Node> m_frameToNode = [];
		private readonly List<KeyValuePair<string, Material>> m_createdMaterials = [];
		private readonly Dictionary<string, Texture> m_createdTextures = [];

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

			m_frameToNode.Clear();
			m_createdMaterials.Clear();
			m_createdTextures.Clear();

			m_context.Dispose();
		}

		private void EnsureNotDisposed() {
			ObjectDisposedException.ThrowIf(_disposed, nameof(FbxExporterContext));
		}

		internal void Initialize(string fileName, float scaleFactor, int versionIndex, bool isAscii, bool is60Fps) {
			EnsureNotDisposed();

			var res = m_context.Initialize(fileName, scaleFactor, versionIndex, isAscii, is60Fps, out var errorMessage);

			if (!res) {
				var fullMessage = $"Failed to initialize FbxExporter: {errorMessage}";
				throw new ApplicationException(fullMessage);
			}
		}

		internal void SetFramePaths(HashSet<string> framePaths) {
			EnsureNotDisposed();

			if (framePaths == null || framePaths.Count == 0) {
				return;
			}

			m_context.SetFramePaths([.. framePaths]);
		}

		internal void ExportScene() {
			EnsureNotDisposed();

			m_context.ExportScene();
		}

		internal void ExportFrame(List<ImportedMesh> meshList, List<ImportedFrame> meshFrames, ImportedFrame rootFrame) {
			var rootNode = m_context.GetSceneRootNode();

#warning the way of checking here is not good.
			Debug.Assert(rootNode.IsValid());

			var nodeStack = new Stack<Node>();
			var frameStack = new Stack<ImportedFrame>();

			nodeStack.Push(rootNode);
			frameStack.Push(rootFrame);

			while (nodeStack.Count > 0) {
				var parentNode = nodeStack.Pop();
				var frame = frameStack.Pop();

				var childNode = m_context.ExportSingleFrame(parentNode, frame.Path, frame.Name, frame.LocalPosition, frame.LocalRotation, frame.LocalScale);

				if (meshList != null && ImportedHelpers.FindMesh(frame.Path, meshList) != null) {
					meshFrames.Add(frame);
				}

				m_frameToNode.Add(frame, childNode);

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

				if (m_frameToNode.TryGetValue(frame, out var node)) {
#warning the way of checking here is not good.
					Debug.Assert(node.IsValid());

					if (castToBone) {
						m_context.SetJointsNode_CastToBone(node, boneSize);
					}
					else {
						Debug.Assert(bonePaths != null);

						if (bonePaths.Contains(frame.Path)) {
							m_context.SetJointsNode_BoneInPath(node, boneSize);
						}
						else {
							m_context.SetJointsNode_Generic(node);
						}
					}
				}

				for (var i = frame.Count - 1; i >= 0; i -= 1) {
					frameStack.Push(frame[i]);
				}
			}
		}

		internal void PrepareMaterials(int materialCount, int textureCount) {
			m_context.PrepareMaterials(materialCount, textureCount);
		}

		internal void ExportMeshFromFrame(ImportedFrame rootFrame, ImportedFrame meshFrame, List<ImportedMesh> meshList, List<ImportedMaterial> materialList, List<ImportedTexture> textureList, bool exportSkins, bool exportAllUvsAsDiffuseMaps) {
			var meshNode = m_frameToNode[meshFrame];
			var mesh = ImportedHelpers.FindMesh(meshFrame.Path, meshList);

			ExportMesh(rootFrame, materialList, textureList, meshNode, mesh, exportSkins, exportAllUvsAsDiffuseMaps);
		}

		private Texture ExportTexture(ImportedTexture imTexture) {
			if (imTexture == null) {
				return null;
			}

			if (m_createdTextures.TryGetValue(imTexture.Name, out Texture value)) {
				return value;
			}

			var texture = m_context.CreateTexture(imTexture.Name);

			m_createdTextures.Add(imTexture.Name, texture);

			var file = new FileInfo(imTexture.Name);

			using (var writer = new BinaryWriter(file.Create())) {
				writer.Write(imTexture.Data);
			}

			return texture;
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

			ClusterArray clusterArray = null;

			if (hasBones) {
				clusterArray = new(totalBoneCount);

				foreach (var bone in boneList) {
					if (bone.Path != null) {
						var frame = rootFrame.FindFrameByPath(bone.Path);
						var boneNode = m_frameToNode[frame];

						var cluster = m_context.CreateCluster(boneNode);

						clusterArray.AddCluster(cluster);
					}
					else {
						clusterArray.AddCluster(null);
					}
				}
			}

			var mesh = m_context.CreateMesh(frameNode);

			mesh.InitControlPoints(importedMesh.VertexList.Count);

			if (importedMesh.hasNormal) {
				mesh.CreateElementNormal();
			}

			for (int i = 0; i < importedMesh.hasUV.Length; i++) {
				if (!importedMesh.hasUV[i]) {
					continue;
				}

				if (i == 1 && !exportAllUvsAsDiffuseMaps) {
					mesh.CreateNormalMapUV(1);
				}
				else {
					mesh.CreateDiffuseUV(i);
				}
			}

			if (importedMesh.hasTangent) {
				mesh.CreateElementTangent();
			}

			if (importedMesh.hasColor) {
				mesh.CreateElementVertexColor();
			}

			mesh.CreateElementMaterial();

			foreach (var meshObj in importedMesh.SubmeshList) {
				var materialIndex = 0;
				var imMat = ImportedHelpers.FindMaterial(meshObj.Material, materialList);

				if (imMat != null) {
					var foundMat = m_createdMaterials.FindIndex(kv => kv.Key == imMat.Name);
					Material mat;

					if (foundMat >= 0) {
						mat = m_createdMaterials[foundMat].Value;
					}
					else {
						var diffuse = imMat.Diffuse;
						var ambient = imMat.Ambient;
						var emissive = imMat.Emissive;
						var specular = imMat.Specular;
						var reflection = imMat.Reflection;

						mat = m_context.CreateMaterial(imMat.Name, in diffuse, in ambient, in emissive, in specular, in reflection, imMat.Shininess, imMat.Transparency);

						m_createdMaterials.Add(KeyValuePair.Create(imMat.Name, mat));
					}

					materialIndex = frameNode.AddMaterialToFrame(mat);

					var hasTexture = false;

					foreach (var imTexture in imMat.Textures) {
						var imTex = ImportedHelpers.FindTexture(imTexture.Name, textureList);
						var tex = ExportTexture(imTex);

						if (tex.IsValid()) {
							switch (imTexture.Dest) {
							case 0:
							case 1:
							case 2:
							case 3: {
								mat.LinkTexture(imTexture.Dest, tex, imTexture.Offset.X, imTexture.Offset.Y, imTexture.Scale.X, imTexture.Scale.Y);
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

					mesh.AddPolygon(materialIndex, index0, index1, index2);
				}
			}

			var vertexList = importedMesh.VertexList;

			var vertexCount = vertexList.Count;

			for (var j = 0; j < vertexCount; j += 1) {
				var importedVertex = vertexList[j];

				var vertex = importedVertex.Vertex;
				mesh.SetControlPoint(j, vertex.X, vertex.Y, vertex.Z);

				if (importedMesh.hasNormal) {
					var normal = importedVertex.Normal;
					mesh.ElementAddNormal(0, normal.X, normal.Y, normal.Z);
				}

				for (var uvIndex = 0; uvIndex < importedMesh.hasUV.Length; uvIndex += 1) {
					if (importedMesh.hasUV[uvIndex]) {
						var uv = importedVertex.UV[uvIndex];
						mesh.ElementAddUV(uvIndex, uv[0], uv[1]);
					}
				}

				if (importedMesh.hasTangent) {
					var tangent = importedVertex.Tangent;
					mesh.ElementAddTangent(0, tangent.X, tangent.Y, tangent.Z, tangent.W);
				}

				if (importedMesh.hasColor) {
					var color = importedVertex.Color;
					mesh.ElementAddVertexColor(0, color.R, color.G, color.B, color.A);
				}

				if (hasBones && importedVertex.BoneIndices != null && clusterArray != null) {
					var boneIndices = importedVertex.BoneIndices;
					var boneWeights = importedVertex.Weights;

					for (var k = 0; k < 4; k += 1) {
						if (boneIndices[k] < totalBoneCount && boneWeights[k] > 0) {
							clusterArray.SetBoneWeight(boneIndices[k], j, boneWeights[k]);
						}
					}
				}
			}


			if (hasBones) {
				Skin skin = m_context.CreateSkinContext(frameNode);

				for (var j = 0; j < totalBoneCount; j += 1) {
					if (!clusterArray.HasItemAt(j))
						continue;

					var m = boneList[j].Matrix;

					float[] array = [
						m[0], m[1], m[2], m[3],
						m[4], m[5], m[6], m[7],
						m[8], m[9], m[10], m[11],
						m[12], m[13], m[14], m[15]
					];

					skin.AddCluster(clusterArray, j, array);
				}

				mesh.AddDeformer(skin);
			}
		}

		internal void ExportAnimations(ImportedFrame rootFrame, List<ImportedKeyframedAnimation> animationList, bool eulerFilter, float filterPrecision) {
			if (animationList == null || animationList.Count == 0) {
				return;
			}

			var anim = new Anim(eulerFilter);

			for (int i = 0; i < animationList.Count; i++) {
				var importedAnimation = animationList[i];
				string takeName;

				if (importedAnimation.Name != null) {
					takeName = importedAnimation.Name;
				}
				else {
					takeName = $"Take{i}";
				}

				anim.PrepareStackAndLayer(m_context, takeName);

				ExportKeyframedAnimation(rootFrame, importedAnimation, anim, filterPrecision);
			}
		}

		private void ExportKeyframedAnimation(ImportedFrame rootFrame, ImportedKeyframedAnimation parser, Anim anim, float filterPrecision) {
			foreach (var track in parser.TrackList) {
				if (track.Path == null)
					continue;

				var frame = rootFrame.FindFrameByPath(track.Path);

				if (frame == null)
					continue;

				var node = m_frameToNode[frame];

				anim.LoadCurves(node);

				anim.BeginKeyModify();

				foreach (var scaling in track.Scalings) {
					var value = scaling.value;
					anim.AddScalingKey(scaling.time, value.X, value.Y, value.Z);
				}

				foreach (var rotation in track.Rotations) {
					var value = rotation.value;
					anim.AddRotationKey(rotation.time, value.X, value.Y, value.Z);
				}

				foreach (var translation in track.Translations) {
					var value = translation.value;
					anim.AddTranslationKey(translation.time, value.X, value.Y, value.Z);
				}

				anim.EndKeyModify();

				anim.ApplyEulerFilter(filterPrecision);

				var blendShape = track.BlendShape;

				if (blendShape == null)
					continue;

				var channelCount = anim.GetCurrentBlendShapeChannelCount(node);

				if (channelCount <= 0)
					continue;

				for (var channelIndex = 0; channelIndex < channelCount; channelIndex += 1) {
					if (!anim.IsBlendShapeChannelMatch(channelIndex, blendShape.ChannelName))
						continue;

					anim.BeginBlendShapeAnimCurve(channelIndex);

					foreach (var keyframe in blendShape.Keyframes) {
						anim.AddBlendShapeKeyframe(keyframe.time, keyframe.value);
					}

					anim.EndBlendShapeAnimCurve();
				}
			}
		}

		internal void ExportMorphs(ImportedFrame rootFrame, List<ImportedMorph> morphList) {
			if (morphList == null || morphList.Count == 0) {
				return;
			}

			foreach (var imMorph in morphList) {
				var frame = rootFrame.FindFrameByPath(imMorph.Path);

				if (frame == null)
					continue;

				var node = m_frameToNode[frame];

				var morph = new Morph();

				morph.Initialize(m_context, node);

				foreach (var channel in imMorph.Channels) {
					morph.AddBlendShapeChannel(m_context, channel.Name);

					for (var i = 0; i < channel.KeyframeList.Count; i++) {
						var keyframe = channel.KeyframeList[i];

						morph.AddBlendShapeChannelShape(m_context, keyframe.Weight, i == 0 ? channel.Name : $"{channel.Name}_{i + 1}");

						morph.CopyBlendShapeControlPoints();

						foreach (var vertex in keyframe.VertexList) {
							var v = vertex.Vertex.Vertex;
							morph.SetBlendShapeVertex(vertex.Index, v.X, v.Y, v.Z);
						}

						if (!keyframe.hasNormals)
							continue;

						morph.CopyBlendShapeControlPointsNormal();

						foreach (var vertex in keyframe.VertexList) {
							var v = vertex.Vertex.Normal;
							morph.SetBlendShapeVertexNormal(vertex.Index, v.X, v.Y, v.Z);
						}
					}
				}
			}
		}

	}
}
