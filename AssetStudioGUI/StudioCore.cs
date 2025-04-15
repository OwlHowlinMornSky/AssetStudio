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

	internal static class StudioCore {
		internal static Action<string> StatusStripUpdate = x => { };
		internal static StudioShell m_studio = new();

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

		public static void ExportAssetsArknightsScene(string savePath, List<AssetItem> toExportAssets, ExportListType exportListType) {
			MessageBox.Show(Properties.Strings.Global_NotImplemented, "Scene Bundle", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
			//try {
			//	Studio_OHMS.Export_Scene(savePath, toExportAssets);
			//}
			//catch (Exception ex) {
			//	MessageBox.Show(ex.ToString());
			//}
			//StatusStripUpdate("Finished exporting as [Arknights]Scene Bundle.");
			//return;
		}

		public static async void ExportAssetsArknightsCharart(string savePath, List<AssetItem> allAssets) {
			try {
				await System.Threading.Tasks.Task.Run(() => {
					Progress.Reset();
					ExporterArknightsCharArt e = new(allAssets);
					StatusStripUpdate("Exporting Spine Animations.");
					e.ExportAllSpineAnimations(savePath);
					Progress.Report(1, 2);
					StudioCore.StatusStripUpdate("Exporting Illustrations.");
					e.ExportAllIllustrations(savePath);
					Progress.Report(2, 2);
				});
			}
			catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			StatusStripUpdate("Finished exporting as [Arknights]CharArt Bundle.");
			return;
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

		public static void OpenFolderInExplorer(string path) {
			var info = new ProcessStartInfo(path);
			info.UseShellExecute = true;
			Process.Start(info);
		}
	}

}
