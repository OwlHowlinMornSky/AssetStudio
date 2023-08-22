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

		private static bool Export_Textures_CombineRGBA(in string savePath, in AssetItem mainTex, in AssetItem alphaTex) {
			//return ExportTexture2D_Combine(mainTex, alphaTex, savePath);

			var res0 = ExportTexture2D_PNG(mainTex, savePath, out var rgbPath);
			var res1 = ExportTexture2D_PNG(alphaTex, savePath, out var alphaPath);
			if (res0 && res1) {
				/* Mat[] mat = new Mat[2];
				mat[0] = Cv2.ImRead(rgbPath);
				mat[1] = Cv2.ImRead(alphaPath);

				if (mat[0].Size() != mat[1].Size()) {
					Cv2.Resize(mat[1], mat[1], mat[0].Size(), 0.0, 0.0, InterpolationFlags.Cubic);
				}

				Mat[] res = new Mat[1];
				res[0] = new Mat(mat[0].Size(), MatType.CV_8UC4);

				int[] fromTo = {
					0, 0,
					1, 1,
					2, 2,
					3, 3
				};

				Cv2.MixChannels(mat, res, fromTo);

				string combPath = Path.Combine(savePath, mainTex.Text + "[c].png");
				if (Cv2.ImWrite(combPath, res)) {
					Directory.CreateDirectory(Path.Combine(savePath, "original"));
					File.Move(rgbPath, Path.Combine(savePath, "original", Path.GetFileName(rgbPath)));
					File.Move(alphaPath, Path.Combine(savePath, "original", Path.GetFileName(alphaPath)));
					File.Move(combPath, Path.Combine(savePath, mainTex.Text + ".png"));
				}*/
				return true;
			}
			return false;
		}

		private static bool Export_SpineTextAsset(in string savePath, in AssetItem item) {
			return ExportTextAsset_NoAppendingExtension(item, savePath);
		}

		#region CharArt
		private static bool Export_CharArt_GetMaterialTextures(in AssetItem item, out long mainTexID, out long alphaTexID) {
			mainTexID = 0;
			alphaTexID = 0;
			if (item.Type != ClassIDType.Material) {
				return false;
			}
			var asset = (Material)item.Asset;
			foreach (var pair in asset.m_SavedProperties.m_TexEnvs) {
				switch (pair.Key) {
				case "_AlphaTex":
					//MessageBox.Show(pair.Value.m_Texture.m_PathID.ToString());
					if (alphaTexID == 0) {
						alphaTexID = pair.Value.m_Texture.m_PathID;
					}
					else {
						return false;
					}
					break;
				case "_MainTex":
					//MessageBox.Show(pair.Value.m_Texture.m_PathID.ToString());
					if (mainTexID == 0) {
						mainTexID = pair.Value.m_Texture.m_PathID;
					}
					else {
						return false;
					}
					break;
				}
			}
			if (mainTexID == 0 || alphaTexID == 0) {
				return false;
			}
			return true;
		}

		private static bool Export_CharArt_Building_ForAtlasAsset(in string savePath, in List<AssetItem> allAssets, long atlasFile_pathid) {
			var atlasItems = allAssets.FindAll(x => x.m_PathID == atlasFile_pathid && x.Type == ClassIDType.MonoBehaviour);
			if (atlasItems.Count != 1) {
				return false;
			}
			var atlasAsset = (MonoBehaviour)atlasItems[0].Asset;
			var atlas = atlasAsset.ToType();
			var atlas_atlasFile0 = atlas["atlasFile"];
			if (atlas_atlasFile0 == null) {
				return false;
			}
			if (!atlas_atlasFile0.GetType().Equals(typeof(OrderedDictionary))) {
				return false;
			}
			var atlas_atlasFile = (OrderedDictionary)atlas_atlasFile0;
			var atlas_atlasFile_pathid0 = atlas_atlasFile["m_PathID"];
			if (atlas_atlasFile_pathid0 == null) {
				return false;
			}
			if (!atlas_atlasFile_pathid0.GetType().Equals(typeof(long))) {
				return false;
			}
			var atlas_atlasFile_pathid = (long)atlas_atlasFile_pathid0;
			{
				var atlasFiles = allAssets.FindAll(x => x.m_PathID == atlas_atlasFile_pathid && x.Type == ClassIDType.TextAsset);
				if (atlasFiles.Count != 1) {
					return false;
				}
				Export_SpineTextAsset(in savePath, atlasFiles[0]);
			}

			var atlas_materials0 = atlas["materials"];
			if (atlas_materials0 == null) {
				return false;
			}
			if (!atlas_materials0.GetType().Equals(typeof(List<object>))) {
				return false;
			}
			var atlas_materials = ((List<object>)atlas_materials0).Select(x => (OrderedDictionary)x).ToList();
			foreach (var mat in atlas_materials) {
				var mat_pathid0 = mat["m_PathID"];
				if (mat_pathid0 == null) {
					continue;
				}
				if (!mat_pathid0.GetType().Equals(typeof(long))) {
					continue;
				}
				var mat_pathid = (long)mat_pathid0;
				var matItems = allAssets.FindAll(x => (x.m_PathID == mat_pathid && x.Type == ClassIDType.Material));
				if (matItems.Count != 1) {
					continue;
				}
				var matItem = matItems[0];
				{
					if (!Export_CharArt_GetMaterialTextures(in matItem, out long mainTexID, out long alphaTexID)) {
						continue;
					}
					var mainTexs = allAssets.FindAll(x => x.m_PathID == mainTexID && x.Type == ClassIDType.Texture2D);
					if (mainTexs.Count != 1) {
						continue;
					}
					var alphaTexs = allAssets.FindAll(x => x.m_PathID == alphaTexID && x.Type == ClassIDType.Texture2D);
					if (alphaTexs.Count != 1) {
						continue;
					}
					//ExportTexture2D(mainTexs[0], savePath);
					//ExportTexture2D(alphaTexs[0], savePath);
					if (!Export_Textures_CombineRGBA(in savePath, mainTexs[0], alphaTexs[0])) {
						MessageBox.Show("Error");
					}
				}
			}
			return true;
		}

		private static bool Export_CharArt_Building_ForSkeletonData(in string savePath, in List<AssetItem> allAssets, long skeletonDataAsset_pathid) {
			var skeletonDataItems = allAssets.FindAll(x => x.m_PathID == skeletonDataAsset_pathid && x.Type == ClassIDType.MonoBehaviour);
			if (skeletonDataItems.Count != 1) {
				return false;
			}
			var skeletonDataAsset = (MonoBehaviour)skeletonDataItems[0].Asset;
			var skeletonData = skeletonDataAsset.ToType();
			var skeletonData_skeletonJSON0 = skeletonData["skeletonJSON"];
			if (skeletonData_skeletonJSON0 == null) {
				return false;
			}
			if (!skeletonData_skeletonJSON0.GetType().Equals(typeof(OrderedDictionary))) {
				return false;
			}
			var skeletonData_skeletonJSON = (OrderedDictionary)skeletonData_skeletonJSON0;
			var skeletonData_skeletonJSON_pathid0 = skeletonData_skeletonJSON["m_PathID"];
			if (skeletonData_skeletonJSON_pathid0 == null) {
				return false;
			}
			if (!skeletonData_skeletonJSON_pathid0.GetType().Equals(typeof(long))) {
				return false;
			}
			var skeletonJSON_pathid = (long)skeletonData_skeletonJSON_pathid0;
			//MessageBox.Show("skeletonJSON, m_PathID: " + skeletonData_skeletonJSON_pathid.ToString());
			{
				var skeletonJSONs = allAssets.FindAll(x => x.m_PathID == skeletonJSON_pathid && x.Type == ClassIDType.TextAsset);
				if (skeletonJSONs.Count != 1) {
					return false;
				}
				Export_SpineTextAsset(in savePath, skeletonJSONs[0]);
			}

			var skeletonData_atlasAssets0 = skeletonData["atlasAssets"];
			if (skeletonData_atlasAssets0 == null) {
				return false;
			}
			if (!skeletonData_atlasAssets0.GetType().Equals(typeof(List<object>))) {
				return false;
			}
			var skeletonData_atlasAssets = ((List<object>)skeletonData_atlasAssets0).Select(x => (OrderedDictionary)x).ToList();
			foreach (var atlas in skeletonData_atlasAssets) {
				var atlas_pathid0 = atlas["m_PathID"];
				if (atlas_pathid0 == null) {
					continue;
				}
				if (!atlas_pathid0.GetType().Equals(typeof(long))) {
					continue;
				}
				var atlas_pathid = (long)atlas_pathid0;
				//MessageBox.Show("atlas, m_PathID: " + atlas_pathid.ToString());
				Export_CharArt_Building_ForAtlasAsset(in savePath, in allAssets, atlas_pathid);
			}
			return true;
		}

		private static bool Export_CharArt_Building_ForSkeletonAnimation(in string savePath, in List<AssetItem> allAssets, long skeleton_pathid) {
			var SkeletonAnimationItems = allAssets.FindAll(x => x.m_PathID == skeleton_pathid && x.Type == ClassIDType.MonoBehaviour);
			if (SkeletonAnimationItems.Count != 1) {
				return false;
			}
			var SkeletonAnimationAsset = (MonoBehaviour)SkeletonAnimationItems[0].Asset;
			var SkeletonAnimation = SkeletonAnimationAsset.ToType();
			var SkeletonAnimation_skeletonDataAsset0 = SkeletonAnimation["skeletonDataAsset"];
			if (SkeletonAnimation_skeletonDataAsset0 == null) {
				return false;
			}
			if (!SkeletonAnimation_skeletonDataAsset0.GetType().Equals(typeof(OrderedDictionary))) {
				return false;
			}
			var SkeletonAnimation_skeletonDataAsset = (OrderedDictionary)SkeletonAnimation_skeletonDataAsset0;
			var SkeletonAnimation_skeletonDataAsset_pathid0 = SkeletonAnimation_skeletonDataAsset["m_PathID"];
			if (SkeletonAnimation_skeletonDataAsset_pathid0 == null) {
				return false;
			}
			if (!SkeletonAnimation_skeletonDataAsset_pathid0.GetType().Equals(typeof(long))) {
				return false;
			}
			var skeletonDataAsset_pathid = (long)SkeletonAnimation_skeletonDataAsset_pathid0;
			//MessageBox.Show("skeletonDataAsset, m_PathID: " + SkeletonAnimation_skeletonDataAsset_pathid.ToString());
			return Export_CharArt_Building_ForSkeletonData(in savePath, in allAssets, skeletonDataAsset_pathid);
		}

		private static bool Export_CharArt_Battle_ForCharacterAnimator_ForTwoFace(in AssetItem item, out long frontID, out long backID) {
			frontID = 0;
			backID = 0;
			var CharAnimatorAsset = (MonoBehaviour)item.Asset;
			var CharAnimator = CharAnimatorAsset.ToType();
			{
				var CharAnimator_front0 = CharAnimator["_front"];
				if (CharAnimator_front0 == null) {
					return false;
				}
				if (!CharAnimator_front0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var CharAnimator_front = (OrderedDictionary)CharAnimator_front0;
				var CharAnimator_front_skeleton0 = CharAnimator_front["skeleton"];
				if (CharAnimator_front_skeleton0 == null) {
					return false;
				}
				if (!CharAnimator_front_skeleton0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var CharAnimator_front_skeleton = (OrderedDictionary)CharAnimator_front_skeleton0;
				var CharAnimator_front_skeleton_pathid0 = CharAnimator_front_skeleton["m_PathID"];
				if (CharAnimator_front_skeleton_pathid0 == null) {
					return false;
				}
				if (!CharAnimator_front_skeleton_pathid0.GetType().Equals(typeof(long))) {
					return false;
				}
				frontID = (long)CharAnimator_front_skeleton_pathid0;
			}
			{
				var CharAnimator_back0 = CharAnimator["_back"];
				if (CharAnimator_back0 == null) {
					return false;
				}
				if (!CharAnimator_back0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var CharAnimator_back = (OrderedDictionary)CharAnimator_back0;
				var CharAnimator_back_skeleton0 = CharAnimator_back["skeleton"];
				if (CharAnimator_back_skeleton0 == null) {
					return false;
				}
				if (!CharAnimator_back_skeleton0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var CharAnimator_back_skeleton = (OrderedDictionary)CharAnimator_back_skeleton0;
				var CharAnimator_back_skeleton_pathid0 = CharAnimator_back_skeleton["m_PathID"];
				if (CharAnimator_back_skeleton_pathid0 == null) {
					return false;
				}
				if (!CharAnimator_back_skeleton_pathid0.GetType().Equals(typeof(long))) {
					return false;
				}
				backID = (long)CharAnimator_back_skeleton_pathid0;
			}
			if (frontID == 0 || backID == 0) {
				return false;
			}
			return true;
		}

		private static bool Export_CharArt_Battle_ForCharacterAnimator(in string savePath, in List<AssetItem> allAssets) {
			var CharAnimatorItems = allAssets.FindAll(x => (x.Type == ClassIDType.MonoBehaviour && x.Text == "CharacterAnimator"));
			if (CharAnimatorItems.Count < 1) {
				return false;
			}
			foreach (var CharAnimatorItem in CharAnimatorItems) {
				//var CharAnimatorItem = CharAnimatorItems[0];
				//MessageBox.Show("VCharacter, m_PathID: " + VCharacterItem.m_PathID.ToString());
				if (!Export_CharArt_Battle_ForCharacterAnimator_ForTwoFace(in CharAnimatorItem, out var frontID, out var backID)) {
					return false;
				}
				var frontres = Export_CharArt_Building_ForSkeletonAnimation(Path.Combine(savePath, "Battle-Front"), in allAssets, frontID);
				var backres = Export_CharArt_Building_ForSkeletonAnimation(Path.Combine(savePath, "Battle-Back"), in allAssets, backID);
			}
			return true;
			//frontres || backres;
		}

		private static bool Export_CharArt_Battle_ForSingleSpineAnimator(in string savePath, in List<AssetItem> allAssets) {
			var SingleAnimatorItems = allAssets.FindAll(x => (x.Type == ClassIDType.MonoBehaviour && x.Text == "SingleSpineAnimator"));
			if (SingleAnimatorItems.Count < 1) {
				return false;
			}
			foreach (var SingleAnimatorItem in SingleAnimatorItems) {
				//var SingleAnimatorItem = SingleAnimatorItems[0];
				var SingleAnimatorAsset = (MonoBehaviour)SingleAnimatorItem.Asset;
				var SingleAnimator = SingleAnimatorAsset.ToType();
				var SingleAnimator_skeleton0 = SingleAnimator["_skeleton"];
				if (SingleAnimator_skeleton0 == null) {
					return false;
				}
				if (!SingleAnimator_skeleton0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var SingleAnimator_skeleton = (OrderedDictionary)SingleAnimator_skeleton0;
				var SingleAnimator_skeleton_pathid0 = SingleAnimator_skeleton["m_PathID"];
				if (SingleAnimator_skeleton_pathid0 == null) {
					return false;
				}
				if (!SingleAnimator_skeleton_pathid0.GetType().Equals(typeof(long))) {
					return false;
				}
				var SingleAnimator_skeleton_pathid = (long)SingleAnimator_skeleton_pathid0;
				//MessageBox.Show("SkeletonAnimation, m_PathID: " + VCharacter_skeleton_m_PathID.ToString());
				Export_CharArt_Building_ForSkeletonAnimation(Path.Combine(savePath, "Battle"), in allAssets, SingleAnimator_skeleton_pathid);
			}
			return true; //Export_CharArt_Building_ForSkeletonAnimation(Path.Combine(savePath, "Battle"), in allAssets, SingleAnimator_skeleton_pathid);
		}

		private static bool Export_CharArt_Illust_ForOneIllustGetIDs(in AssetItem imageItem, out long material_pathid, out long sprite_pathid) {
			material_pathid = 0;
			sprite_pathid = 0;
			if (imageItem.Type != ClassIDType.MonoBehaviour) {
				return false;
			}
			var imageAsset = (MonoBehaviour)imageItem.Asset;
			var image = imageAsset.ToType();
			{
				var image_material0 = image["m_Material"];
				if (image_material0 == null) {
					return false;
				}
				if (!image_material0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var image_material = (OrderedDictionary)image_material0;
				var image_material_pathid0 = image_material["m_PathID"];
				if (image_material_pathid0 == null) {
					return false;
				}
				if (!image_material_pathid0.GetType().Equals(typeof(long))) {
					return false;
				}
				material_pathid = (long)image_material_pathid0;
			}
			{
				var image_sprite0 = image["m_Sprite"];
				if (image_sprite0 == null) {
					return false;
				}
				if (!image_sprite0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var image_sprite = (OrderedDictionary)image_sprite0;
				var image_sprite_pathid0 = image_sprite["m_PathID"];
				if (image_sprite_pathid0 == null) {
					return false;
				}
				if (!image_sprite_pathid0.GetType().Equals(typeof(long))) {
					return false;
				}
				sprite_pathid = (long)image_sprite_pathid0;
			}
			if (material_pathid == 0 || sprite_pathid == 0) {
				return false;
			}
			return true;
		}

		private static bool Export_CharArt_GetSpriteTextures(in AssetItem spriteItem, out long mainTexID, out long alphaTexID) {
			mainTexID = 0;
			alphaTexID = 0;
			if (spriteItem.Type != ClassIDType.Sprite) {
				return false;
			}
			var spriteAsset = (Sprite)spriteItem.Asset;
			mainTexID = spriteAsset.m_RD.texture.m_PathID;
			alphaTexID = spriteAsset.m_RD.alphaTexture.m_PathID;
			return true;
		}

		public static bool Export_CharArt_Building(in string savePath, in List<AssetItem> allAssets) {
			StudioCore.StatusStripUpdate("Exporting The Spine Animations of Building.");
			var VCharacterItems = allAssets.FindAll(x => (x.Type == ClassIDType.MonoBehaviour && x.Text == "VCharacter"));
			if (VCharacterItems.Count < 1) {
				return false;
			}
			foreach (var VCharacterItem in VCharacterItems) {
				//var VCharacterItem = VCharacterItems[0];
				//MessageBox.Show("VCharacter, m_PathID: " + VCharacterItem.m_PathID.ToString());
				var VCharacterAsset = (MonoBehaviour)VCharacterItem.Asset;
				var VCharacter = VCharacterAsset.ToType();
				var VCharacter_skeleton0 = VCharacter["_skeleton"];
				if (VCharacter_skeleton0 == null) {
					return false;
				}
				if (!VCharacter_skeleton0.GetType().Equals(typeof(OrderedDictionary))) {
					return false;
				}
				var VCharacter_skeleton = (OrderedDictionary)VCharacter_skeleton0;
				var VCharacter_skeleton_m_PathID0 = VCharacter_skeleton["m_PathID"];
				if (VCharacter_skeleton_m_PathID0 == null) {
					return false;
				}
				if (!VCharacter_skeleton_m_PathID0.GetType().Equals(typeof(long))) {
					return false;
				}
				var skeleton_pathid = (long)VCharacter_skeleton_m_PathID0;
				//MessageBox.Show("SkeletonAnimation, m_PathID: " + VCharacter_skeleton_m_PathID.ToString());
				if (!Export_CharArt_Building_ForSkeletonAnimation(Path.Combine(savePath, "Building"), in allAssets, skeleton_pathid)) {

				}
			}
			return true;
		}

		public static bool Export_CharArt_Battle(in string savePath, in List<AssetItem> allAssets) {
			StudioCore.StatusStripUpdate("Exporting The Spine Animations of Battle.");
			//return Export_CharArt_Battle_ForCharacterAnimator(in savePath, in allAssets) ||
			//	Export_CharArt_Battle_ForSingleSpineAnimator(in savePath, in allAssets);
			Export_CharArt_Battle_ForCharacterAnimator(in savePath, in allAssets);
			Export_CharArt_Battle_ForSingleSpineAnimator(in savePath, in allAssets);
			return true;
		}

		public static bool Export_CharArt_Pictures(in string outPath, in List<AssetItem> allAssets) {
			StudioCore.StatusStripUpdate("Exporting Pictures.");
			string savePath = Path.Combine(outPath, "Illust");
			var IllustsItems = allAssets.FindAll(x => (x.Type == ClassIDType.MonoBehaviour && x.Text == "Image"));
			int i = 0;
			int n = IllustsItems.Count;
			foreach (var illust in IllustsItems) {
				if (!Export_CharArt_Illust_ForOneIllustGetIDs(illust, out var material_pathid, out var sprite_pathid)) {
					continue;
				}
				var materialItems = allAssets.FindAll(x => (x.m_PathID == material_pathid && x.Type == ClassIDType.Material));
				if (materialItems.Count != 1) {
					continue;
				}
				var spriteItems = allAssets.FindAll(x => (x.m_PathID == sprite_pathid && x.Type == ClassIDType.Sprite));
				if (spriteItems.Count != 1) {
					continue;
				}
				Export_CharArt_GetMaterialTextures(materialItems[0], out var mainTexID, out var alphaTexID);
				Export_CharArt_GetSpriteTextures(spriteItems[0], out var mainTexID1, out var alphaTexID1);
				if (mainTexID != mainTexID1) {
					if (mainTexID == 0) {
						mainTexID = mainTexID1;
					}
					else if (mainTexID1 != 0) {
						continue;
					}
				}
				if (alphaTexID != alphaTexID1) {
					if (alphaTexID == 0) {
						alphaTexID = alphaTexID1;
					}
					else if (alphaTexID1 != 0) {
						continue;
					}
				}
				if (mainTexID == 0 || alphaTexID == 0) {
					continue;
				}
				//string name = ((Sprite)spriteItems[0].Asset).m_Name;
				//MessageBox.Show(name);
				{
					var mainTexs = allAssets.FindAll(x => x.m_PathID == mainTexID && x.Type == ClassIDType.Texture2D);
					if (mainTexs.Count != 1) {
						continue;
					}
					var alphaTexs = allAssets.FindAll(x => x.m_PathID == alphaTexID && x.Type == ClassIDType.Texture2D);
					if (alphaTexs.Count != 1) {
						continue;
					}
					var mainTex = mainTexs[0];
					var alphaTex = alphaTexs[0];
					if (!Export_Textures_CombineRGBA(in savePath, in mainTex, in alphaTex)) {
						MessageBox.Show("Error");
					}
				}
				Progress.Report(++i + n, n + n);
			}
			return true;
		}
		#endregion CharArt

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
