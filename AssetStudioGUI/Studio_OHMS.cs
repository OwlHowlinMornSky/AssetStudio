using AssetStudio;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static AssetStudioGUI.Exporter;

namespace AssetStudioGUI {
	internal static class Studio_OHMS {

		#region Scene
		private struct SubMeshRendererThings {
			public ushort m_mapIndex;
			public Vector4 m_mapOffset;

			public SubMeshRendererThings(ushort i, float x, float y, float z, float w) {
				m_mapIndex = i;
				m_mapOffset = new Vector4(x, y, z, w);
			}
		}

		private struct Mesh_OHMS {
			public Mesh m_mesh;
			public List<SubMeshRendererThings> m_renderers;
			public int[] m_rendererRef;
			public Mesh_OHMS(in Mesh m) {
				m_mesh = m;
				m_renderers = new();
				m_rendererRef = new int[m.m_SubMeshes.Length];
			}
		}

		private static bool Export_Scene_ForLightingTex(in string savePath, in List<AssetItem> allAssets) {
			var allMeshes = allAssets.FindAll(x => x.Type == ClassIDType.LightmapSettings);
			return true;
		}

		public static bool Export_Scene(in string savePath, in List<AssetItem> allAssets) {
			Export_Scene_ForLightingTex(in savePath, in allAssets);

			Dictionary<long, Mesh_OHMS> l_meshes = new();
			{
				var allMeshes = allAssets.FindAll(x => x.Type == ClassIDType.Mesh);
				foreach (var mesh in allMeshes) {
					l_meshes.Add(mesh.m_PathID, new Mesh_OHMS((Mesh)mesh.Asset));
				}
			}
			var allGameObjects = allAssets.FindAll(x => x.Type == ClassIDType.GameObject);
			var allMeshRenderers = allAssets.FindAll(x => x.Type == ClassIDType.MeshRenderer);
			var allMeshFilters = allAssets.FindAll(x => x.Type == ClassIDType.MeshFilter);

			foreach (var rendererItem in allMeshRenderers) {
				var ren = (MeshRenderer)rendererItem.Asset;
				long m_Mesh_m_PathID = 0;
				{
					var objectItems = allGameObjects.FindAll(x => x.m_PathID == ren.m_GameObject.m_PathID);
					if (objectItems.Count != 1) {
						//throw new Exception($"0");
						continue;
					}
					var obj = (GameObject)objectItems[0].Asset;

					foreach (var component in obj.m_Components) {
						var filters = allMeshFilters.FindAll(x => x.m_PathID == component.m_PathID);
						if (filters.Count != 1) {
							continue;
						}
						var filter = (MeshFilter)filters[0].Asset;
						m_Mesh_m_PathID = filter.m_Mesh.m_PathID;
						if (m_Mesh_m_PathID != 0) {
							break;
						}
					}
				}
				if (m_Mesh_m_PathID == 0) {
					continue;
				}

				var tree = ren.ToType();
				var m_Enabled = tree["m_Enabled"];
				if (m_Enabled == null || !m_Enabled.GetType().Equals(typeof(bool)) || !(bool)m_Enabled) {
					//throw new Exception("1");
					continue;
				}
				var m_LightmapIndex = tree["m_LightmapIndex"];

				if (m_LightmapIndex == null) {
					//throw new Exception("2");
					continue;
				}
				if (!m_LightmapIndex.GetType().Equals(typeof(UInt16))) {
					//throw new Exception("3");
					continue;
				}

				var m_LightmapTilingOffset = tree["m_LightmapTilingOffset"];

				if (m_LightmapTilingOffset == null) {
					//throw new Exception("4");
					continue;
				}
				if (!m_LightmapTilingOffset.GetType().Equals(typeof(OrderedDictionary))) {
					//throw new Exception("5");
					continue;
				}
				var LightmapTilingOffset = (OrderedDictionary)m_LightmapTilingOffset;
				var x = (float)LightmapTilingOffset["x"];
				var y = (float)LightmapTilingOffset["y"];
				var z = (float)LightmapTilingOffset["z"];
				var w = (float)LightmapTilingOffset["w"];
				SubMeshRendererThings rendererThings = new((ushort)m_LightmapIndex, x, y, z, w);

				Mesh_OHMS? l_m_n = l_meshes[m_Mesh_m_PathID];
				if (l_m_n == null) {
					//throw new Exception("6");
					continue;
				}
				Mesh_OHMS l_m = (Mesh_OHMS)l_m_n;
				l_m.m_renderers.Add(rendererThings);

				for (uint i = 0; i < ren.m_StaticBatchInfo.subMeshCount; ++i) {
					l_m.m_rendererRef[ren.m_StaticBatchInfo.firstSubMesh + i] = l_m.m_renderers.Count - 1;
				}
			}
			return true;
		}
		#endregion Scene

	}
}
