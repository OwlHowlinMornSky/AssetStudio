using System;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AssetStudio;
using Object = AssetStudio.Object;
using static AssetStudioGUI.Exporter;

namespace AssetStudioGUI {
	internal class StudioShell {
		public AssetsManager assetsManager = new AssetsManager();
		public AssemblyLoader assemblyLoader = new AssemblyLoader();
		public List<AssetItem> exportableAssets = new List<AssetItem>();
		public List<AssetItem> visibleAssets = new List<AssetItem>();

		public (string, List<TreeNode>) BuildAssetData() {
			StudioCore.StatusStripUpdate(Properties.Strings.Studio_BuildingAssetList);

			string productName = null;
			var objectCount = assetsManager.assetsFileList.Sum(x => x.Objects.Count);
			var objectAssetItemDic = new Dictionary<Object, AssetItem>(objectCount);
			var containers = new List<(PPtr<Object>, string)>();
			int i = 0;
			Progress.Reset();
			foreach (var assetsFile in assetsManager.assetsFileList) {
				foreach (var asset in assetsFile.Objects) {
					var assetItem = new AssetItem(asset);
					objectAssetItemDic.Add(asset, assetItem);
					assetItem.UniqueID = " #" + i;
					assetItem.ID = i;
					var exportable = false;
					switch (asset) {
					case GameObject m_GameObject:
						assetItem.Text = m_GameObject.m_Name;
						break;
					case Texture2D m_Texture2D:
						if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
							assetItem.FullSize = asset.byteSize + m_Texture2D.m_StreamData.size;
						assetItem.Text = m_Texture2D.m_Name;
						exportable = true;
						break;
					case AudioClip m_AudioClip:
						if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
							assetItem.FullSize = asset.byteSize + m_AudioClip.m_Size;
						assetItem.Text = m_AudioClip.m_Name;
						exportable = true;
						break;
					case VideoClip m_VideoClip:
						if (!string.IsNullOrEmpty(m_VideoClip.m_OriginalPath))
							assetItem.FullSize = asset.byteSize + (long)m_VideoClip.m_ExternalResources.m_Size;
						assetItem.Text = m_VideoClip.m_Name;
						exportable = true;
						break;
					case Shader m_Shader:
						assetItem.Text = m_Shader.m_ParsedForm?.m_Name ?? m_Shader.m_Name;
						exportable = true;
						break;
					case Mesh _:
					case TextAsset _:
					case AnimationClip _:
					case Font _:
					case MovieTexture _:
					case Sprite _:
						assetItem.Text = ((NamedObject)asset).m_Name;
						exportable = true;
						break;
					case Animator m_Animator:
						if (m_Animator.m_GameObject.TryGet(out var gameObject)) {
							assetItem.Text = gameObject.m_Name;
						}
						exportable = true;
						break;
					case MonoBehaviour m_MonoBehaviour:
						if (m_MonoBehaviour.m_Name == "" && m_MonoBehaviour.m_Script.TryGet(out var m_Script)) {
							assetItem.Text = m_Script.m_ClassName;
						}
						else {
							assetItem.Text = m_MonoBehaviour.m_Name;
						}
						exportable = true;
						break;
					case PlayerSettings m_PlayerSettings:
						productName = m_PlayerSettings.productName;
						break;
					case AssetBundle m_AssetBundle:
						foreach (var m_Container in m_AssetBundle.m_Container) {
							var preloadIndex = m_Container.Value.preloadIndex;
							var preloadSize = m_Container.Value.preloadSize;
							var preloadEnd = preloadIndex + preloadSize;
							for (int k = preloadIndex; k < preloadEnd; k++) {
								containers.Add((m_AssetBundle.m_PreloadTable[k], m_Container.Key));
							}
						}
						assetItem.Text = m_AssetBundle.m_Name;
						break;
					case ResourceManager m_ResourceManager:
						foreach (var m_Container in m_ResourceManager.m_Container) {
							containers.Add((m_Container.Value, m_Container.Key));
						}
						break;
					case NamedObject m_NamedObject:
						assetItem.Text = m_NamedObject.m_Name;
						break;
					}
					if (assetItem.Text == "") {
						assetItem.Text = assetItem.TypeString + assetItem.UniqueID; // OHMS IMPORTANT DEBUG
																					//assetItem.Text = "#" + assetItem.ID;
					}
					if (Properties.Settings.Default.displayAll || exportable) {
						exportableAssets.Add(assetItem);
					}
					Progress.Report(++i, objectCount);
				}
			}
			foreach ((var pptr, var container) in containers) {
				if (pptr.TryGet(out var obj)) {
					objectAssetItemDic[obj].Container = container;
				}
			}
			foreach (var tmp in exportableAssets) {
				tmp.SetSubItems();
			}
			containers.Clear();

			visibleAssets = exportableAssets;

			StudioCore.StatusStripUpdate(Properties.Strings.Studio_BuildingTreeStructure);

			var treeNodeCollection = new List<TreeNode>();
			var treeNodeDictionary = new Dictionary<GameObject, GameObjectTreeNode>();
			var assetsFileCount = assetsManager.assetsFileList.Count;
			int j = 0;
			Progress.Reset();
			foreach (var assetsFile in assetsManager.assetsFileList) {
				var fileNode = new TreeNode(assetsFile.fileName); //RootNode

				foreach (var obj in assetsFile.Objects) {
					if (obj is GameObject m_GameObject) {
						if (!treeNodeDictionary.TryGetValue(m_GameObject, out var currentNode)) {
							currentNode = new GameObjectTreeNode(m_GameObject);
							treeNodeDictionary.Add(m_GameObject, currentNode);
						}

						foreach (var pptr in m_GameObject.m_Components) {
							if (pptr.TryGet(out var m_Component)) {
								objectAssetItemDic[m_Component].TreeNode = currentNode;
								if (m_Component is MeshFilter m_MeshFilter) {
									if (m_MeshFilter.m_Mesh.TryGet(out var m_Mesh)) {
										objectAssetItemDic[m_Mesh].TreeNode = currentNode;
									}
								}
								else if (m_Component is SkinnedMeshRenderer m_SkinnedMeshRenderer) {
									if (m_SkinnedMeshRenderer.m_Mesh.TryGet(out var m_Mesh)) {
										objectAssetItemDic[m_Mesh].TreeNode = currentNode;
									}
								}
							}
						}

						var parentNode = fileNode;

						if (m_GameObject.m_Transform != null) {
							if (m_GameObject.m_Transform.m_Father.TryGet(out var m_Father)) {
								if (m_Father.m_GameObject.TryGet(out var parentGameObject)) {
									if (!treeNodeDictionary.TryGetValue(parentGameObject, out var parentGameObjectNode)) {
										parentGameObjectNode = new GameObjectTreeNode(parentGameObject);
										treeNodeDictionary.Add(parentGameObject, parentGameObjectNode);
									}
									parentNode = parentGameObjectNode;
								}
							}
						}

						parentNode.Nodes.Add(currentNode);
					}
				}

				if (fileNode.Nodes.Count > 0) {
					treeNodeCollection.Add(fileNode);
				}

				Progress.Report(++j, assetsFileCount);
			}
			treeNodeDictionary.Clear();

			objectAssetItemDic.Clear();

			return (productName, treeNodeCollection);
		}

		public Dictionary<string, SortedDictionary<int, TypeTreeItem>> BuildClassStructure() {
			var typeMap = new Dictionary<string, SortedDictionary<int, TypeTreeItem>>();
			foreach (var assetsFile in assetsManager.assetsFileList) {
				if (typeMap.TryGetValue(assetsFile.unityVersion, out var curVer)) {
					foreach (var type in assetsFile.m_Types.Where(x => x.m_Type != null)) {
						var key = type.classID;
						if (type.m_ScriptTypeIndex >= 0) {
							key = -1 - type.m_ScriptTypeIndex;
						}
						curVer[key] = new TypeTreeItem(key, type.m_Type);
					}
				}
				else {
					var items = new SortedDictionary<int, TypeTreeItem>();
					foreach (var type in assetsFile.m_Types.Where(x => x.m_Type != null)) {
						var key = type.classID;
						if (type.m_ScriptTypeIndex >= 0) {
							key = -1 - type.m_ScriptTypeIndex;
						}
						items[key] = new TypeTreeItem(key, type.m_Type);
					}
					typeMap.Add(assetsFile.unityVersion, items);
				}
			}

			return typeMap;
		}

		public void ExportAssetsStructuredLIST(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
			switch (exportListType) {
			case ExportListType.XML: {
				var filename = Path.Combine(savePath, "assets.xml");
				var doc = new XDocument(
							new XElement("Assets",
								new XAttribute("CreatedAt", DateTime.Now.ToString("s")),
								new XAttribute("CreatedAtUTC", DateTime.UtcNow.ToString("s")),
								new XElement("SourceFiles",
									new XAttribute("OpenType", assetsManager.m_lastLoadType.ToString()),
									assetsManager.m_lastOpenPaths.Select(
										x => new XElement("File",
											new XElement("Path", x)
										)
									)
								),
								toExportAssets.Select(
									asset => new XElement("Asset",
										new XAttribute("ID", asset.ID),
										new XElement("Name", asset.Text),
										new XElement("Type",
											new XAttribute("id", (int)asset.Type),
											asset.TypeString
										),
										new XElement("PathID", asset.m_PathID)
									)
								)
							)
						);

				doc.Save(filename);
				break;
			}
			case ExportListType.JSON: {
				var filename = Path.Combine(savePath, "assets.json");
				JObject doc = new JObject {
						new JProperty("createdAt", DateTime.Now.ToString("s")),
						new JProperty("createdAtUTC", DateTime.UtcNow.ToString("s")),
						new JProperty("SourceFiles",
							new JObject(
								new JProperty("OpenType", assetsManager.m_lastLoadType.ToString()),
								new JProperty("Files",
									new JArray(
										assetsManager.m_lastOpenPaths.Select(
											x => new JObject(
												new JProperty("Path", x)
											)
										)
									)
								)
							)
						),
						new JProperty("Assets",
							new JArray(
								toExportAssets.Select(
									asset => new JObject(
										new JProperty("ID", asset.ID),
										new JProperty("Name", asset.Text),
										new JProperty("Type", new JObject(
											new JProperty("id", (int)asset.Type),
											new JProperty("name", asset.TypeString))),
										new JProperty("PathID", asset.m_PathID)
									)
								)
							)
						)
					};
				if (Properties.SettingsOHMS.Default.indentedJson) {
					File.WriteAllText(filename, JsonConvert.SerializeObject(doc, Formatting.Indented));
				}
				else {
					File.WriteAllText(filename, JsonConvert.SerializeObject(doc, Formatting.None));
				}
				break;
			}
			}
		}

		public void ExportAssetsStructured(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
			ThreadPool.QueueUserWorkItem(state => {
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				int toExportCount = toExportAssets.Count + 1;
				int exportedCount = 0;
				int i = 0;

				Progress.Reset();

				Exporter.g_ohms_export_with_structure = true;

				foreach (var asset in toExportAssets) {
					string exportPath = Path.Combine(savePath, "things");
					switch (Properties.Settings.Default.assetGroupOption) {
					case 0: //type name
						exportPath = Path.Combine(exportPath, asset.TypeString);
						break;
					case 1: //container path
						if (!string.IsNullOrEmpty(asset.Container)) {
							exportPath = Path.Combine(exportPath, Path.GetDirectoryName(asset.Container));
						}
						break;
					case 2: //source file
						if (string.IsNullOrEmpty(asset.SourceFile.originalPath)) {
							exportPath = Path.Combine(exportPath, asset.SourceFile.fileName + "_export");
						}
						else {
							exportPath = Path.Combine(exportPath, Path.GetFileName(asset.SourceFile.originalPath) + "_export", asset.SourceFile.fileName);
						}
						break;
					}
					exportPath += Path.DirectorySeparatorChar;
					StudioCore.StatusStripUpdate($"[{exportedCount}/{toExportCount}] "
						+ Properties.Strings.Studio_Exporting
						+ $" {asset.TypeString}:{asset.Text}");
					try {
						if (ExportConvertFileOHMS(asset, exportPath)) {
							++exportedCount;
						}
					}
					catch (Exception ex) {
						//MessageBox.Show($"Export {asset.Type}:{asset.Text} error\r\n{ex.Message}\r\n{ex.StackTrace}");
						MessageBox.Show(
							String.Format(Properties.Strings.Studio_Export_Exception, asset.Type, asset.Text)
							+ $"\r\n{ex.Message}\r\n{ex.StackTrace}");
					}

					Progress.Report(++i, toExportCount);
				}

				Exporter.g_ohms_export_with_structure = false;

				ExportAssetsStructuredLIST(savePath, toExportAssets, exportListType);
				++exportedCount;
				Progress.Report(++i, toExportCount);

				//var statusText = "Finished OHMS_EXPORT_STRUCT" + $" [{exportedCount}/{toExportCount}].";
				var statusText = Properties.Strings.Studio_Finished_OHMS_EXPORT_STRUCT
				+ $" [{exportedCount}/{toExportCount}].";
				StudioCore.StatusStripUpdate(statusText);

				if (Properties.Settings.Default.openAfterExport) {
					StudioCore.OpenFolderInExplorer(savePath);
				}
			});
		}

		public TypeTree MonoBehaviourToTypeTree(MonoBehaviour m_MonoBehaviour) {
			if (!assemblyLoader.Loaded) {
				var openFolderDialog = new OpenFolderDialog();
				//openFolderDialog.Title = "Select Assembly Folder";
				openFolderDialog.Title = Properties.Strings.Studio_SelectAssemblyFolder;
				if (openFolderDialog.ShowDialog() == DialogResult.OK) {
					assemblyLoader.Load(openFolderDialog.Folder);
				}
				else {
					assemblyLoader.Loaded = true;
				}
			}
			return m_MonoBehaviour.ConvertToTypeTree(assemblyLoader);
		}

		public string DumpAsset(Object obj) {
			var str = obj.Dump();
			if (str == null && obj is MonoBehaviour m_MonoBehaviour) {
				var type = MonoBehaviourToTypeTree(m_MonoBehaviour);
				str = m_MonoBehaviour.Dump(type);
			}
			return str;
		}

		public string DumpAssetJson(Object obj) {
			var type = obj.ToType();
			if (type == null && obj is MonoBehaviour m_MonoBehaviour) {
				var m_Type = MonoBehaviourToTypeTree(m_MonoBehaviour);
				type = m_MonoBehaviour.ToType(m_Type);
			}
			var str = JsonConvert.SerializeObject(type, Formatting.Indented);
			return str;
		}

	}
}
