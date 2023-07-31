using AssetStudio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using static AssetStudioGUI.Exporter;
using Object = AssetStudio.Object;

namespace AssetStudioGUI {
	internal enum ExportType {
		Convert,
		Raw,
		Dump,
		ConvertOHMS,
		DumpJson
	}

	internal enum ExportFilter {
		All,
		Selected,
		Filtered
	}

	internal enum ExportArknightsFilter {
		Scene,
		CharArt
	}

	internal enum ExportListType {
		XML,
		JSON
	}

	internal static class Studio {
		public static AssetsManager assetsManager = new AssetsManager();
		public static AssemblyLoader assemblyLoader = new AssemblyLoader();
		public static List<AssetItem> exportableAssets = new List<AssetItem>();
		public static List<AssetItem> visibleAssets = new List<AssetItem>();
		internal static Action<string> StatusStripUpdate = x => { };

		public static int ExtractFolder(string path, string savePath) {
			int extractedCount = 0;
			Progress.Reset();
			var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++) {
				var file = files[i];
				var fileOriPath = Path.GetDirectoryName(file);
				var fileSavePath = fileOriPath.Replace(path, savePath);
				extractedCount += ExtractFile(file, fileSavePath);
				Progress.Report(i + 1, files.Length);
			}
			return extractedCount;
		}

		public static int ExtractFile(string[] fileNames, string savePath) {
			int extractedCount = 0;
			Progress.Reset();
			for (var i = 0; i < fileNames.Length; i++) {
				var fileName = fileNames[i];
				extractedCount += ExtractFile(fileName, savePath);
				Progress.Report(i + 1, fileNames.Length);
			}
			return extractedCount;
		}

		public static int ExtractFile(string fileName, string savePath) {
			int extractedCount = 0;
			var reader = new FileReader(fileName);
			if (reader.FileType == FileType.BundleFile)
				extractedCount += ExtractBundleFile(reader, savePath);
			else if (reader.FileType == FileType.WebFile)
				extractedCount += ExtractWebDataFile(reader, savePath);
			else
				reader.Dispose();
			return extractedCount;
		}

		private static int ExtractBundleFile(FileReader reader, string savePath) {
			//StatusStripUpdate($"Decompressing {reader.FileName} ...");
			StatusStripUpdate(Properties.Strings.Studio_Decompressing + $" {reader.FileName} ...");
			var bundleFile = new BundleFile(reader);
			reader.Dispose();
			if (bundleFile.fileList.Length > 0) {
				var extractPath = Path.Combine(savePath, reader.FileName + "_unpacked");
				return ExtractStreamFile(extractPath, bundleFile.fileList);
			}
			return 0;
		}

		private static int ExtractWebDataFile(FileReader reader, string savePath) {
			//StatusStripUpdate($"Decompressing {reader.FileName} ...");
			StatusStripUpdate(Properties.Strings.Studio_Decompressing + $" {reader.FileName} ...");
			var webFile = new WebFile(reader);
			reader.Dispose();
			if (webFile.fileList.Length > 0) {
				var extractPath = Path.Combine(savePath, reader.FileName + "_unpacked");
				return ExtractStreamFile(extractPath, webFile.fileList);
			}
			return 0;
		}

		private static int ExtractStreamFile(string extractPath, StreamFile[] fileList) {
			int extractedCount = 0;
			foreach (var file in fileList) {
				var filePath = Path.Combine(extractPath, file.path);
				var fileDirectory = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(fileDirectory)) {
					Directory.CreateDirectory(fileDirectory);
				}
				if (!File.Exists(filePath)) {
					using (var fileStream = File.Create(filePath)) {
						file.stream.CopyTo(fileStream);
					}
					extractedCount += 1;
				}
				file.stream.Dispose();
			}
			return extractedCount;
		}

		public static (string, List<TreeNode>) BuildAssetData() {
			//StatusStripUpdate("Building asset list...");
			StatusStripUpdate(Properties.Strings.Studio_BuildingAssetList);

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

			//StatusStripUpdate("Building tree structure...");
			StatusStripUpdate(Properties.Strings.Studio_BuildingTreeStructure);

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

		public static Dictionary<string, SortedDictionary<int, TypeTreeItem>> BuildClassStructure() {
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

		public static void ExportAssets(string savePath, List<AssetItem> toExportAssets, ExportType exportType) {
			ThreadPool.QueueUserWorkItem(state => {
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				int toExportCount = toExportAssets.Count;
				int exportedCount = 0;
				int i = 0;
				Progress.Reset();
				foreach (var asset in toExportAssets) {
					string exportPath;
					switch (Properties.Settings.Default.assetGroupOption) {
					case 0: //type name
						exportPath = Path.Combine(savePath, asset.TypeString);
						break;
					case 1: //container path
						if (!string.IsNullOrEmpty(asset.Container)) {
							exportPath = Path.Combine(savePath, Path.GetDirectoryName(asset.Container));
						}
						else {
							exportPath = savePath;
						}
						break;
					case 2: //source file
						if (string.IsNullOrEmpty(asset.SourceFile.originalPath)) {
							exportPath = Path.Combine(savePath, asset.SourceFile.fileName + "_export");
						}
						else {
							exportPath = Path.Combine(savePath, Path.GetFileName(asset.SourceFile.originalPath) + "_export", asset.SourceFile.fileName);
						}
						break;
					default:
						exportPath = savePath;
						break;
					}
					exportPath += Path.DirectorySeparatorChar;
					//StatusStripUpdate($"[{exportedCount}/{toExportCount}] Exporting {asset.TypeString}: {asset.Text}");
					StatusStripUpdate($"[{exportedCount}/{toExportCount}] " +
						Properties.Strings.Studio_Exporting + $" {asset.TypeString}:{asset.Text}");
					try {
						switch (exportType) {
						case ExportType.Raw:
							if (ExportRawFile(asset, exportPath)) {
								exportedCount++;
							}
							break;
						case ExportType.Dump:
							if (ExportDumpFile(asset, exportPath)) {
								exportedCount++;
							}
							break;
						case ExportType.Convert:
							if (ExportConvertFile(asset, exportPath)) {
								exportedCount++;
							}
							break;
						case ExportType.ConvertOHMS:
							if (ExportConvertFileOHMS(asset, exportPath)) {
								exportedCount++;
							}
							break;
						case ExportType.DumpJson:
							if (ExportDumpFileJson(asset, exportPath)) {
								exportedCount++;
							}
							break;
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

				//var statusText = exportedCount == 0 ? "Nothing exported." : $"Finished exporting {exportedCount} assets.";
				var statusText = exportedCount == 0 ?
					Properties.Strings.Studio_Export_NothingExported :
					String.Format(Properties.Strings.Studio_Export_FinishedExporting, exportedCount);

				if (toExportCount > exportedCount) {
					statusText += " " +
						//$"{toExportCount - exportedCount} assets skipped (not extractable or files already exist)";
						String.Format(Properties.Strings.Studio_Export_FinishedExporting_Skipped, toExportCount - exportedCount);
				}

				StatusStripUpdate(statusText);

				if (Properties.Settings.Default.openAfterExport && exportedCount > 0) {
					OpenFolderInExplorer(savePath);
				}
			});
		}

		public static void ExportAssetsList(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
			ThreadPool.QueueUserWorkItem(state => {
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				Progress.Reset();

				switch (exportListType) {
				case ExportListType.XML: {
					var filename = Path.Combine(savePath, "assets.xml");
					var doc = new XDocument(
							new XElement("Assets",
								new XAttribute("filename", filename),
								new XAttribute("createdAt", DateTime.UtcNow.ToString("s")),
								toExportAssets.Select(
									asset => new XElement("Asset",
										new XElement("Name", asset.Text),
										new XElement("Container", asset.Container),
										new XElement("Type", new XAttribute("id", (int)asset.Type), asset.TypeString),
										new XElement("PathID", asset.m_PathID),
										new XElement("Source", asset.SourceFile.fullName),
										new XElement("Size", asset.FullSize)
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
						new JProperty("filename", filename),
						new JProperty("createdAt", DateTime.UtcNow.ToString("s")),
						new JProperty("Assets", new JArray(
						toExportAssets.Select(
						asset => new JObject(
							new JProperty("Name", asset.Text),
							new JProperty("Container", asset.Container),
							new JProperty("Type", new JObject(
								new JProperty("id", (int)asset.Type),
								new JProperty("name", asset.TypeString))),
							new JProperty("PathID", asset.m_PathID),
							new JProperty("Source", asset.SourceFile.fullName),
							new JProperty("Size", asset.FullSize)
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

				//var statusText = $"Finished exporting asset list with {toExportAssets.Count()} items.";
				var statusText = String.Format(Properties.Strings.Studio_Export_FinishedExporting_List, toExportAssets.Count());

				StatusStripUpdate(statusText);

				if (Properties.Settings.Default.openAfterExport && toExportAssets.Count() > 0) {
					OpenFolderInExplorer(savePath);
				}
			});
		}

		public static void ExportAssetsStructuredLIST(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
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

		public static void ExportAssetsStructured(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
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
					StatusStripUpdate($"[{exportedCount}/{toExportCount}] "
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
				StatusStripUpdate(statusText);

				if (Properties.Settings.Default.openAfterExport) {
					OpenFolderInExplorer(savePath);
				}
			});
		}

		public static void ExportAssetsArknightsScene(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
			try {
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			//StatusStripUpdate("Finished exporting as [Arknights]Scene Bundle.");
			MessageBox.Show(Properties.Strings.Global_NotImplemented);
			return;
		}

		public async static void ExportAssetsArknightsCharart(string savePath, List<AssetItem> allAssets) {
			//List<MonoBehaviour> monoBehaviours =
			//	toExportAssets.FindAll(x => x.Type == ClassIDType.MonoBehaviour).Select(x => (MonoBehaviour)x.Asset).ToList();
			try {
				await System.Threading.Tasks.Task.Run(() => {
					Progress.Reset();
					Studio_OHMS.Export_CharArt_Building(in savePath, in allAssets);
					Progress.Report(1, 4);
					Studio_OHMS.Export_CharArt_Battle(in savePath, in allAssets);
					Progress.Report(2, 4);
					Studio_OHMS.Export_CharArt_Pictures(in savePath, in allAssets);
					Progress.Report(4, 4);
				});
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			StatusStripUpdate("Finished exporting as [Arknights]CharArt Bundle.");
			return;
			/*List<long> longs = new List<long>();
			foreach (var item in toExportAssets) {
				if (item.Type == ClassIDType.Material) {
					var m = (Material)item.Asset;
					foreach (var pair in m.m_SavedProperties.m_TexEnvs) {
						switch (pair.Key) {
						case "_AlphaTex":
						case "_MainTex":
							longs.Add(pair.Value.m_Texture.m_PathID);
							break;
						}
					}
				}
			}
			var test = new string("");
			foreach (var n in longs) {
				test += n.ToString() + "\n";
			}
			MessageBox.Show(test, "test");*/
		}

		public static void ExportSplitObjects(string savePath, TreeNodeCollection nodes) {
			ThreadPool.QueueUserWorkItem(state => {
				var count = nodes.Cast<TreeNode>().Sum(x => x.Nodes.Count);
				int k = 0;
				Progress.Reset();
				foreach (TreeNode node in nodes) {
					//遍历一级子节点
					foreach (GameObjectTreeNode j in node.Nodes) {
						//收集所有子节点
						var gameObjects = new List<GameObject>();
						CollectNode(j, gameObjects);
						//跳过一些不需要导出的object
						if (gameObjects.All(x => x.m_SkinnedMeshRenderer == null && x.m_MeshFilter == null)) {
							Progress.Report(++k, count);
							continue;
						}
						//处理非法文件名
						var filename = FixFileName(j.Text);
						//每个文件存放在单独的文件夹
						var targetPath = $"{savePath}{filename}{Path.DirectorySeparatorChar}";
						//重名文件处理
						for (int i = 1; ; i++) {
							if (Directory.Exists(targetPath)) {
								targetPath = $"{savePath}{filename} ({i}){Path.DirectorySeparatorChar}";
							}
							else {
								break;
							}
						}
						Directory.CreateDirectory(targetPath);
						//导出FBX
						StatusStripUpdate($"Exporting {filename}.fbx");
						try {
							ExportGameObject(j.gameObject, targetPath);
						}
						catch (Exception ex) {
							MessageBox.Show($"Export GameObject:{j.Text} error\r\n{ex.Message}\r\n{ex.StackTrace}");
						}

						Progress.Report(++k, count);
						StatusStripUpdate($"Finished exporting {filename}.fbx");
					}
				}
				if (Properties.Settings.Default.openAfterExport) {
					OpenFolderInExplorer(savePath);
				}
				StatusStripUpdate("Finished");
			});
		}

		private static void CollectNode(GameObjectTreeNode node, List<GameObject> gameObjects) {
			gameObjects.Add(node.gameObject);
			foreach (GameObjectTreeNode i in node.Nodes) {
				CollectNode(i, gameObjects);
			}
		}

		public static void ExportAnimatorWithAnimationClip(AssetItem animator, List<AssetItem> animationList, string exportPath) {
			ThreadPool.QueueUserWorkItem(state => {
				Progress.Reset();
				StatusStripUpdate($"Exporting {animator.Text}");
				try {
					ExportAnimator(animator, exportPath, animationList);
					if (Properties.Settings.Default.openAfterExport) {
						OpenFolderInExplorer(exportPath);
					}
					Progress.Report(1, 1);
					StatusStripUpdate($"Finished exporting {animator.Text}");
				}
				catch (Exception ex) {
					MessageBox.Show($"Export Animator:{animator.Text} error\r\n{ex.Message}\r\n{ex.StackTrace}");
					StatusStripUpdate("Error in export");
				}
			});
		}

		public static void ExportObjectsWithAnimationClip(string exportPath, TreeNodeCollection nodes, List<AssetItem> animationList = null) {
			ThreadPool.QueueUserWorkItem(state => {
				var gameObjects = new List<GameObject>();
				GetSelectedParentNode(nodes, gameObjects);
				if (gameObjects.Count > 0) {
					var count = gameObjects.Count;
					int i = 0;
					Progress.Reset();
					foreach (var gameObject in gameObjects) {
						StatusStripUpdate($"Exporting {gameObject.m_Name}");
						try {
							ExportGameObject(gameObject, exportPath, animationList);
							StatusStripUpdate($"Finished exporting {gameObject.m_Name}");
						}
						catch (Exception ex) {
							MessageBox.Show($"Export GameObject:{gameObject.m_Name} error\r\n{ex.Message}\r\n{ex.StackTrace}");
							StatusStripUpdate("Error in export");
						}

						Progress.Report(++i, count);
					}
					if (Properties.Settings.Default.openAfterExport) {
						OpenFolderInExplorer(exportPath);
					}
				}
				else {
					StatusStripUpdate("No Object selected for export.");
				}
			});
		}

		public static void ExportObjectsMergeWithAnimationClip(string exportPath, List<GameObject> gameObjects, List<AssetItem> animationList = null) {
			ThreadPool.QueueUserWorkItem(state => {
				var name = Path.GetFileName(exportPath);
				Progress.Reset();
				StatusStripUpdate($"Exporting {name}");
				try {
					ExportGameObjectMerge(gameObjects, exportPath, animationList);
					Progress.Report(1, 1);
					StatusStripUpdate($"Finished exporting {name}");
				}
				catch (Exception ex) {
					MessageBox.Show($"Export Model:{name} error\r\n{ex.Message}\r\n{ex.StackTrace}");
					StatusStripUpdate("Error in export");
				}
				if (Properties.Settings.Default.openAfterExport) {
					OpenFolderInExplorer(Path.GetDirectoryName(exportPath));
				}
			});
		}

		public static void GetSelectedParentNode(TreeNodeCollection nodes, List<GameObject> gameObjects) {
			foreach (TreeNode i in nodes) {
				if (i is GameObjectTreeNode gameObjectTreeNode && i.Checked) {
					gameObjects.Add(gameObjectTreeNode.gameObject);
				}
				else {
					GetSelectedParentNode(i.Nodes, gameObjects);
				}
			}
		}

		public static TypeTree MonoBehaviourToTypeTree(MonoBehaviour m_MonoBehaviour) {
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

		public static string DumpAsset(Object obj) {
			var str = obj.Dump();
			if (str == null && obj is MonoBehaviour m_MonoBehaviour) {
				var type = MonoBehaviourToTypeTree(m_MonoBehaviour);
				str = m_MonoBehaviour.Dump(type);
			}
			return str;
		}

		public static string DumpAssetJson(Object obj) {
			var type = obj.ToType();
			if (type == null && obj is MonoBehaviour m_MonoBehaviour) {
				var m_Type = MonoBehaviourToTypeTree(m_MonoBehaviour);
				type = m_MonoBehaviour.ToType(m_Type);
			}
			var str = JsonConvert.SerializeObject(type, Formatting.Indented);
			return str;
		}

		public static void OpenFolderInExplorer(string path) {
			var info = new ProcessStartInfo(path);
			info.UseShellExecute = true;
			Process.Start(info);
		}
	}
}
