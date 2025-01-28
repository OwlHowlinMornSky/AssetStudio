using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AssetStudio;

namespace AssetStudioGUI {

	partial class AssetStudioGUIForm : Form {
		private AssetItem lastSelectedItem;

		private string tempClipboard;
		private readonly GUILogger m_logger;

		private readonly string m_defaultGuiTitle;

		private class Flag(bool init_val) {
			private bool _flag = init_val;
			public bool Active {
				get {
					return _flag;
				}
			}

			public void Set() {
				_flag = true;
			}
			public void Reset() {
				_flag = false;
			}
		}
		private readonly Flag m_usingStudio = new(false);

		private string openDirectoryBackup = string.Empty;
		private string saveDirectoryBackup = string.Empty;

		#region Member_LeftView
		//asset list sorting
		private int sortColumn = -1;
		private bool reverseSort;

		//tree search
		private int nextGObject;
		private List<TreeNode> treeSrcResults = [];
		#endregion

		public AssetStudioGUIForm() {
			LanguageOptions.initWhenOpenForm();

			InitializeComponent();
			ui_menuDebug.Visible = Debugger.IsAttached;

			/// 计算窗口默认标题。
			m_defaultGuiTitle = $"AssetStudio v1.{Application.ProductVersion}";
			Text = m_defaultGuiTitle;

			/// 将设置值应用到UI。
			ui_menuOptions_displayAllAssets.Checked = Properties.Settings.Default.displayAll;
			ui_menuOptions_displayInfo.Checked = Properties.Settings.Default.displayInfo;
			ui_menuOptions_enablePreview.Checked = Properties.Settings.Default.enablePreview;
			ui_menuOhmsExport_createANewFolder.Checked = Properties.SettingsOHMS.Default.ohmsCreateNew;

			/// 注册UI日志器。
			m_logger = new GUILogger(StatusStripUpdate);
			Logger.Default = m_logger;
			Progress.Default = new Progress<int>(SetProgressBarValue);
			StudioCore.StatusStripUpdate = StatusStripUpdate;

			StatusStripUpdate(Properties.Strings.Status_Ready);
		}

		private void SetProgressBarValue(int value) {
			void f(int newvalue) {
				ui_progressBar0_down.Value = newvalue;
			}
			if (InvokeRequired) {
				var r = BeginInvoke(f, value);
				EndInvoke(r);
			}
			else {
				f(value);
			}
		}

		private void StatusStripUpdate(string statusText) {
			void f(string text) {
				ui_statusLabel0_down.Text = text;
			}
			if (InvokeRequired) {
				var r = BeginInvoke(f, statusText);
				EndInvoke(r);
			}
			else {
				f(statusText);
			}
		}

		#region Process

		private async void BuildAssetStructures() {
			if (StudioCore.m_studio.assetsManager.assetsFileList.Count == 0) {
				//StatusStripUpdate("No Unity file can be loaded.");
				StatusStripUpdate(Properties.Strings.Load_NoThing);
				return;
			}

			(var productName, var treeNodeCollection) = await Task.Run(StudioCore.m_studio.BuildAssetData);
			var typeMap = await Task.Run(StudioCore.m_studio.BuildClassStructure);

			Text = m_defaultGuiTitle +
				$" - {(string.IsNullOrEmpty(productName) ? Properties.Strings.Load_NoProductName : productName)}" +
				$" - {StudioCore.m_studio.assetsManager.assetsFileList[0].unityVersion}" +
				$" - {StudioCore.m_studio.assetsManager.assetsFileList[0].m_TargetPlatform}";

			ui_tabLeft_page1_listView.VirtualListSize = StudioCore.m_studio.visibleAssets.Count;

			ui_tabLeft_page0_treeView.BeginUpdate();
			ui_tabLeft_page0_treeView.Nodes.AddRange(treeNodeCollection.ToArray());
			ui_tabLeft_page0_treeView.EndUpdate();
			treeNodeCollection.Clear();

			ui_tabLeft_page2_classesListView.BeginUpdate();
			foreach (var version in typeMap) {
				var versionGroup = new ListViewGroup(version.Key);
				ui_tabLeft_page2_classesListView.Groups.Add(versionGroup);

				foreach (var uclass in version.Value) {
					uclass.Value.Group = versionGroup;
					ui_tabLeft_page2_classesListView.Items.Add(uclass.Value);
				}
			}
			typeMap.Clear();
			ui_tabLeft_page2_classesListView.EndUpdate();

			var types = StudioCore.m_studio.exportableAssets.Select(x => x.Type).Distinct().OrderBy(x => x.ToString()).ToArray();
			foreach (var type in types) {
				var typeItem = new ToolStripMenuItem
				{
					CheckOnClick = true,
					Name = type.ToString(),
					Size = new Size(180, 22),
					Text = type.ToString()
				};
				typeItem.Click += ui_menuFilter_0_all_Click;
				ui_menuFilter.DropDownItems.Add(typeItem);
			}
			ui_menuFilter_0_all.Checked = true;
			var log = String.Format(Properties.Strings.Load_FinishLoading,
				StudioCore.m_studio.assetsManager.assetsFileList.Count, ui_tabLeft_page1_listView.Items.Count);
			var m_ObjectsCount = StudioCore.m_studio.assetsManager.assetsFileList.Sum(x => x.m_Objects.Count);
			var objectsCount = StudioCore.m_studio.assetsManager.assetsFileList.Sum(x => x.Objects.Count);
			if (m_ObjectsCount != objectsCount) {
				//log += $" and {m_ObjectsCount - objectsCount} assets failed to read";
				log += String.Format(Properties.Strings.Load_AndFailRead, m_ObjectsCount - objectsCount);
			}
			StatusStripUpdate(log);
		}

		private void ResetForm() {
			foreach (var previewForm in MdiChildren) {
				previewForm.Close();
			}

			Text = m_defaultGuiTitle;

			// Studio things
			StudioCore.m_studio.assetsManager.Clear();
			StudioCore.m_studio.assemblyLoader.Clear();
			StudioCore.m_studio.exportableAssets.Clear();
			StudioCore.m_studio.visibleAssets.Clear();

			ui_tabLeft_page0_treeView.Nodes.Clear();
			ui_tabLeft_page1_listView.VirtualListSize = 0;
			ui_tabLeft_page1_listView.Items.Clear();
			ui_tabLeft_page2_classesListView.Items.Clear();
			ui_tabLeft_page2_classesListView.Groups.Clear();

			lastSelectedItem = null;
			sortColumn = -1;
			reverseSort = false;
			ui_tabLeft_page1_listSearch.Text = null;
			ui_tabLeft_page0_treeSearch.Text = null;

			var count = ui_menuFilter.DropDownItems.Count;
			for (var i = 1; i < count; i++) {
				ui_menuFilter.DropDownItems.RemoveAt(1);
			}

			StatusStripUpdate(Properties.Strings.Status_Ready);
		}

		private List<AssetItem> GetSelectedAssets() {
			var selectedAssets = new List<AssetItem>(ui_tabLeft_page1_listView.SelectedIndices.Count);
			foreach (int index in ui_tabLeft_page1_listView.SelectedIndices) {
				selectedAssets.Add((AssetItem)ui_tabLeft_page1_listView.Items[index]);
			}

			return selectedAssets;
		}

		private void FilterAssetList() {
			ui_tabLeft_page1_listView.BeginUpdate();
			ui_tabLeft_page1_listView.SelectedIndices.Clear();
			var show = new List<ClassIDType>();
			if (!ui_menuFilter_0_all.Checked) {
				for (var i = 1; i < ui_menuFilter.DropDownItems.Count; i++) {
					var item = (ToolStripMenuItem)ui_menuFilter.DropDownItems[i];
					if (item.Checked) {
						show.Add((ClassIDType)Enum.Parse(typeof(ClassIDType), item.Text));
					}
				}
				StudioCore.m_studio.visibleAssets = StudioCore.m_studio.exportableAssets.FindAll(x => show.Contains(x.Type));
			}
			else {
				StudioCore.m_studio.visibleAssets = StudioCore.m_studio.exportableAssets;
			}
			StudioCore.m_studio.visibleAssets = StudioCore.m_studio.visibleAssets.FindAll(
				x => x.Text.Contains(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase) ||
				x.SubItems[1].Text.Contains(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase) ||
				x.SubItems[3].Text.Contains(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase)
				);
			ui_tabLeft_page1_listView.VirtualListSize = StudioCore.m_studio.visibleAssets.Count;
			ui_tabLeft_page1_listView.EndUpdate();
		}

		private void TreeNodeSearch(TreeNode treeNode) {
			if (treeNode.Text.IndexOf(ui_tabLeft_page0_treeSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) {
				treeSrcResults.Add(treeNode);
			}

			foreach (TreeNode node in treeNode.Nodes) {
				TreeNodeSearch(node);
			}
		}

		#endregion // Process

		#region Export
		private void ExportObjects(bool animation) {
			lock (m_usingStudio) {
				m_usingStudio.Set();
			}
			if (ui_tabLeft_page0_treeView.Nodes.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					saveDirectoryBackup = saveFolderDialog.Folder;
					var exportPath = Path.Combine(saveFolderDialog.Folder, "GameObject") + Path.DirectorySeparatorChar;
					List<AssetItem> animationList = null;
					if (animation) {
						animationList = GetSelectedAssets().Where(x => x.Type == ClassIDType.AnimationClip).ToList();
						if (animationList.Count == 0) {
							animationList = null;
						}
					}
					StudioCore.ExportObjectsWithAnimationClip(exportPath, ui_tabLeft_page0_treeView.Nodes, animationList);
				}
			}
			else {
				//StatusStripUpdate("No Objects available for export");
				StatusStripUpdate(Properties.Strings.Export_Object_Nothing);
			}
			lock (m_usingStudio) {
				m_usingStudio.Reset();
			}
		}

		private void ExportMergeObjects(bool animation) {
			lock (m_usingStudio) {
				m_usingStudio.Set();
			}
			if (ui_tabLeft_page0_treeView.Nodes.Count > 0) {
				var gameObjects = new List<GameObject>();
				StudioCore.GetSelectedParentNode(ui_tabLeft_page0_treeView.Nodes, gameObjects);
				if (gameObjects.Count > 0) {
					var saveFileDialog = new SaveFileDialog();
					saveFileDialog.FileName = gameObjects[0].m_Name + " (merge).fbx";
					saveFileDialog.AddExtension = false;
					saveFileDialog.Filter = "Fbx file (*.fbx)|*.fbx";
					saveFileDialog.InitialDirectory = saveDirectoryBackup;
					if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
						saveDirectoryBackup = Path.GetDirectoryName(saveFileDialog.FileName);
						var exportPath = saveFileDialog.FileName;
						List<AssetItem> animationList = null;
						if (animation) {
							animationList = GetSelectedAssets().Where(x => x.Type == ClassIDType.AnimationClip).ToList();
							if (animationList.Count == 0) {
								animationList = null;
							}
						}
						StudioCore.ExportObjectsMergeWithAnimationClip(exportPath, gameObjects, animationList);
					}
				}
				else {
					//StatusStripUpdate("No Object selected for export.");
					StatusStripUpdate(Properties.Strings.Export_Object_Nothing_Selected);
				}
			}
			lock (m_usingStudio) {
				m_usingStudio.Reset();
			}
		}

		private void ExportAssets(ExportFilter type, ExportType exportType) {
			lock (m_usingStudio) {
				m_usingStudio.Set();
			}
			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = StudioCore.m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = StudioCore.m_studio.visibleAssets;
						break;
					}
					StudioCore.ExportAssets(saveFolderDialog.Folder, toExportAssets, exportType);
				}
			}
			else {
				//StatusStripUpdate("No exportable assets loaded");
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
			lock (m_usingStudio) {
				m_usingStudio.Reset();
			}
		}

		private void ExportAssetsList(ExportFilter type, ExportListType filetype) {
			// XXX: Only exporting as XML for now, but would JSON(/CSV/other) be useful too?
			lock (m_usingStudio) {
				m_usingStudio.Set();
			}

			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = StudioCore.m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = StudioCore.m_studio.visibleAssets;
						break;
					}
					StudioCore.ExportAssetsList(saveFolderDialog.Folder, toExportAssets, filetype);
				}
			}
			else {
				//StatusStripUpdate("No exportable assets loaded");
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
			lock (m_usingStudio) {
				m_usingStudio.Reset();
			}
		}

		private void ExportAnimatorAndSelectedAnimationClips() {
			lock (m_usingStudio) {
				m_usingStudio.Set();
			}

			AssetItem animator = null;
			List<AssetItem> animationList = new List<AssetItem>();
			var selectedAssets = GetSelectedAssets();
			foreach (var assetPreloadData in selectedAssets) {
				if (assetPreloadData.Type == ClassIDType.Animator) {
					animator = assetPreloadData;
				}
				else if (assetPreloadData.Type == ClassIDType.AnimationClip) {
					animationList.Add(assetPreloadData);
				}
			}

			if (animator != null) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					saveDirectoryBackup = saveFolderDialog.Folder;
					var exportPath = Path.Combine(saveFolderDialog.Folder, "Animator") + Path.DirectorySeparatorChar;
					StudioCore.ExportAnimatorWithAnimationClip(animator, animationList, exportPath);
				}
			}

			lock (m_usingStudio) {
				m_usingStudio.Reset();
			}
		}
		#endregion // Export

		#region Form

		private void AssetStudioGUIForm_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			}
		}

		private async void AssetStudioGUIForm_DragDrop(object sender, DragEventArgs e) {
			lock (m_usingStudio) {
				if (m_usingStudio.Active) {
					MessageBox.Show(this, Properties.Strings.Studio_UsingStudio);
					return;
				}
			}
			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (paths.Length > 0) {
				ResetForm();
				StudioCore.m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;
				lock (m_usingStudio) {
					m_usingStudio.Set();
				}
				await Task.Run(() => StudioCore.m_studio.assetsManager.LoadDropIn(paths));
				BuildAssetStructures();
				lock (m_usingStudio) {
					m_usingStudio.Reset();
				}
			}
		}

		#endregion // Form

		#region ContextMenu
		private void Ui_conMenu_copyText_Click(object sender, EventArgs e) {
			Clipboard.SetDataObject(tempClipboard);
		}

		private void Ui_conMenu_exportSelectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.Convert);
		}

		private void Ui_conMenu_exportAnimatorAndSelectedAnimationClips_Click(object sender, EventArgs e) {
			ExportAnimatorAndSelectedAnimationClips();
		}

		private void Ui_conMenu_goToSceneHierarchy_Click(object sender, EventArgs e) {
			var selectasset = (AssetItem)ui_tabLeft_page1_listView.Items[ui_tabLeft_page1_listView.SelectedIndices[0]];
			if (selectasset.TreeNode != null) {
				ui_tabLeft_page0_treeView.SelectedNode = selectasset.TreeNode;
				ui_tabLeft_tab.SelectedTab = ui_tabLeft_page0;
			}
		}

		private void Ui_conMenu_showOriginalFile_Click(object sender, EventArgs e) {
			var selectasset = (AssetItem)ui_tabLeft_page1_listView.Items[ui_tabLeft_page1_listView.SelectedIndices[0]];
			var args = $"/select, \"{selectasset.SourceFile.originalPath ?? selectasset.SourceFile.fullName}\"";
			var pfi = new ProcessStartInfo("explorer.exe", args);
			Process.Start(pfi);
		}

		private void Ui_conMenu_OHMS_ExportSelected_Click(object sender, EventArgs e) {
			ExportAssetsOHMS(ExportFilter.Selected);
		}

		private void Ui_conMenu_OHMS_ExportSelected_Struct_XML_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.XML);
		}

		private void Ui_conMenu_OHMS_ExportSelected_Struct_JSON_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.JSON);
		}
		#endregion ContextMenu

		#region Menu_File
		private async void ui_menuFile_loadFile_Click(object sender, EventArgs e) {
			lock (m_usingStudio) {
				if (m_usingStudio.Active) {
					MessageBox.Show(this, Properties.Strings.Studio_UsingStudio);
					return;
				}
			}
			ui_openFileDialog0.InitialDirectory = openDirectoryBackup;
			if (ui_openFileDialog0.ShowDialog(this) == DialogResult.OK) {
				ResetForm();
				openDirectoryBackup = Path.GetDirectoryName(ui_openFileDialog0.FileNames[0]);
				StudioCore.m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;
				lock (m_usingStudio) {
					m_usingStudio.Set();
				}
				await Task.Run(() => StudioCore.m_studio.assetsManager.LoadFiles(ui_openFileDialog0.FileNames));
				BuildAssetStructures();
				lock (m_usingStudio) {
					m_usingStudio.Reset();
				}
			}
		}

		private async void ui_menuFile_loadFolder_Click(object sender, EventArgs e) {
			lock (m_usingStudio) {
				if (m_usingStudio.Active) {
					MessageBox.Show(this, Properties.Strings.Studio_UsingStudio);
					return;
				}
			}
			var openFolderDialog = new OpenFolderDialog();
			openFolderDialog.InitialFolder = openDirectoryBackup;
			if (openFolderDialog.ShowDialog(this) == DialogResult.OK) {
				ResetForm();
				openDirectoryBackup = openFolderDialog.Folder;
				StudioCore.m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;
				lock (m_usingStudio) {
					m_usingStudio.Set();
				}
				await Task.Run(() => StudioCore.m_studio.assetsManager.LoadFolder(openFolderDialog.Folder));
				BuildAssetStructures();
				lock (m_usingStudio) {
					m_usingStudio.Reset();
				}
			}
		}

		private async void ui_menuFile_extractFile_Click(object sender, EventArgs e) {
			if (ui_openFileDialog0.ShowDialog(this) == DialogResult.OK) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.Title = Properties.Strings.Export_SaveFolderDialog_Title;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					var fileNames = ui_openFileDialog0.FileNames;
					var savePath = saveFolderDialog.Folder;
					lock (m_usingStudio) {
						m_usingStudio.Set();
					}
					var extractedCount = await Task.Run(() => StudioCore.ExtractFile(fileNames, savePath));
					//StatusStripUpdate($"Finished extracting {extractedCount} files.");
					StatusStripUpdate(String.Format(Properties.Strings.Export_FinishExtracting, extractedCount));
					lock (m_usingStudio) {
						m_usingStudio.Reset();
					}
				}
			}
		}

		private async void ui_menuFile_extractFolder_Click(object sender, EventArgs e) {
			var openFolderDialog = new OpenFolderDialog();
			if (openFolderDialog.ShowDialog(this) == DialogResult.OK) {
				var saveFolderDialog = new OpenFolderDialog();
				//saveFolderDialog.Title = "Select the save folder";
				saveFolderDialog.Title = Properties.Strings.Export_SaveFolderDialog_Title;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					var path = openFolderDialog.Folder;
					var savePath = saveFolderDialog.Folder;
					lock (m_usingStudio) {
						m_usingStudio.Set();
					}
					var extractedCount = await Task.Run(() => StudioCore.ExtractFolder(path, savePath));
					//StatusStripUpdate($"Finished extracting {extractedCount} files.");
					StatusStripUpdate(String.Format(Properties.Strings.Export_FinishExtracting, extractedCount));
					lock (m_usingStudio) {
						m_usingStudio.Reset();
					}
				}
			}
		}

		private void ui_menuFile_clear_Click(object sender, EventArgs e) {
			lock (m_usingStudio) {
				if (m_usingStudio.Active) {
					MessageBox.Show(this, Properties.Strings.Studio_UsingStudio);
					return;
				}
			}
			ResetForm();
		}
		#endregion // Menu_File

		#region Menu_Options
		private void ui_menuOptions_language_Click(object sender, EventArgs e) {
			using var langOpt = new LanguageOptions();
			langOpt.ShowDialog(this);
		}

		private void ui_menuOptions_exportOptions_Click(object sender, EventArgs e) {
			using var exportOpt = new ExportOptions();
			exportOpt.ShowDialog(this);
		}

		private void ui_menuOptions_displayAllAssets_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.displayAll = ui_menuOptions_displayAllAssets.Checked;
			Properties.Settings.Default.Save();
		}

		private void ui_menuOptions_enablePreview_Check(object sender, EventArgs e) {
			////////previewPanel1.PreviewAsset(ui_menuOptions_enablePreview.Checked ? lastSelectedItem : null);

			Properties.Settings.Default.enablePreview = ui_menuOptions_enablePreview.Checked;
			Properties.Settings.Default.Save();
		}

		private void ui_menuOptions_displayAssetInfo_Check(object sender, EventArgs e) {
			Properties.Settings.Default.displayInfo = ui_menuOptions_displayInfo.Checked;
			Properties.Settings.Default.Save();
		}
		#endregion // Menu_Options

		#region Menu_Filter
		private void ui_menuFilter_0_all_Click(object sender, EventArgs e) {
			var typeItem = (ToolStripMenuItem)sender;
			if (typeItem != ui_menuFilter_0_all) {
				ui_menuFilter_0_all.Checked = false;
			}
			else if (ui_menuFilter_0_all.Checked) {
				for (var i = 1; i < ui_menuFilter.DropDownItems.Count; i++) {
					var item = (ToolStripMenuItem)ui_menuFilter.DropDownItems[i];
					item.Checked = false;
				}
			}
			FilterAssetList();
		}
		#endregion

		#region Menu_Model
		private void ui_menuModel_exportAllObjectsSplit_Click(object sender, EventArgs e) {
			if (ui_tabLeft_page0_treeView.Nodes.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					saveDirectoryBackup = saveFolderDialog.Folder;
					var savePath = saveFolderDialog.Folder + Path.DirectorySeparatorChar;
					StudioCore.ExportSplitObjects(savePath, ui_tabLeft_page0_treeView.Nodes);
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Object_Nothing);
			}
		}

		private void ui_menuModel_exportSelectedObjectsSplit_Click(object sender, EventArgs e) {
			ExportObjects(false);
		}

		private void ui_menuModel_exportSelectedObjectsWithAnimationClip_Click(object sender, EventArgs e) {
			ExportObjects(true);
		}

		private void ui_menuModel_exportSelectedObjectsMerge_Click(object sender, EventArgs e) {
			ExportMergeObjects(false);
		}

		private void ui_menuModel_exportSelectedObjectsMergeWithAnimationClips_Click(object sender, EventArgs e) {
			ExportMergeObjects(true);
		}
		#endregion

		#region Menu_Export
		private void ui_menuExport_allAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.All, ExportType.Convert);
		}

		private void ui_menuExport_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.Convert);
		}

		private void ui_menuExport_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Filtered, ExportType.Convert);
		}

		private void ui_menuExport_exportAnimatorAndSelectedAnimationClips_Click(object sender, EventArgs e) {
			ExportAnimatorAndSelectedAnimationClips();
		}

		private void ui_menuExport_Raw_allAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.All, ExportType.Raw);
		}

		private void ui_menuExport_Raw_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.Raw);
		}

		private void ui_menuExport_Raw_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Filtered, ExportType.Raw);
		}

		private void ui_menuExport_Dump_allAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.All, ExportType.Dump);
		}

		private void ui_menuExport_Dump_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.Dump);
		}

		private void ui_menuExport_Dump_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Filtered, ExportType.Dump);
		}

		private void ui_menuExport_dumpJson_allAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.All, ExportType.DumpJson);
		}

		private void ui_menuExport_dumpJson_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.DumpJson);
		}

		private void ui_menuExport_dumpJson_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Filtered, ExportType.DumpJson);
		}

		private void ui_menuExport_assetsListToXml_allAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.All, ExportListType.XML);
		}

		private void ui_menuExport_assetsListToXml_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.Selected, ExportListType.XML);
		}

		private void ui_menuExport_assetsListToXml_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.Filtered, ExportListType.XML);
		}

		private void ui_menuExport_assetListToJson_allAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.All, ExportListType.JSON);
		}

		private void ui_menuExport_assetListToJson_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.Selected, ExportListType.JSON);
		}

		private void ui_menuExport_assetListToJson_filteredAssets_Click(object sender, EventArgs e) {
			ExportAssetsList(ExportFilter.Filtered, ExportListType.JSON);
		}
		#endregion Menu_Export

		#region Menu_Debug
		private void ui_menuDebug_showErrorMessage_Click(object sender, EventArgs e) {
			m_logger.ShowErrorMessage = ui_menuDebug_showErrorMessage.Checked;
		}

		private void ui_menuDebug_exportClassStructures_Click(object sender, EventArgs e) {
			if (ui_tabLeft_page2_classesListView.Items.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					var savePath = saveFolderDialog.Folder;
					var count = ui_tabLeft_page2_classesListView.Items.Count;
					int i = 0;
					Progress.Reset();
					foreach (TypeTreeItem item in ui_tabLeft_page2_classesListView.Items) {
						var versionPath = Path.Combine(savePath, item.Group.Header);
						Directory.CreateDirectory(versionPath);

						var saveFile = $"{versionPath}{Path.DirectorySeparatorChar}{item.SubItems[1].Text} {item.Text}.txt";
						File.WriteAllText(saveFile, item.ToString());

						Progress.Report(++i, count);
					}

					//StatusStripUpdate("Finished exporting class structures");
					StatusStripUpdate(Properties.Strings.Debug_FinishExportClassStructure);
				}
			}
		}

		private void ui_TEST_ToolStripMenuItem_Click(object sender, EventArgs e) {
		}

		private void tESTToolStripMenuItem_Click(object sender, EventArgs e) {
			var test = new Studio_Special_Arknights();
			test.ShowDialog(this);
		}

		#endregion Menu_Debug

		#region Menu_OHMS_Export
		private void ui_menuOhmsExport_createANewFolder_CheckedChanged(object sender, EventArgs e) {
			Properties.SettingsOHMS.Default.ohmsCreateNew = ui_menuOhmsExport_createANewFolder.Checked;
			Properties.SettingsOHMS.Default.Save();
		}

		private void ui_menuOhmsExport_allAssets_Click(object sender, EventArgs e) {
			ExportAssetsOHMS(ExportFilter.All);
		}

		private void ui_menuOhmsExport_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssetsOHMS(ExportFilter.Selected);
		}

		private void ui_menuOhmsExport_displayedAssets_Click(object sender, EventArgs e) {
			ExportAssetsOHMS(ExportFilter.Filtered);
		}

		private void ui_menuOhmsExport_structuredJsonList_allAssets_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.All, ExportListType.JSON);
		}

		private void ui_menuOhmsExport_structuredJsonList_selectedAssets_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.JSON);
		}

		private void ui_menuOhmsExport_structuredJsonList_displayedAssets_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Filtered, ExportListType.JSON);
		}

		private void ui_menuOhmsExport_StructXml_All_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.All, ExportListType.XML);
		}

		private void ui_menuOhmsExport_StructXml_selected_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.XML);
		}

		private void ui_menuOhmsExport_StructXml_displayed_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Filtered, ExportListType.XML);
		}

		private void ui_menuOhmsExport_arknights_sceneBundle_Click(object sender, EventArgs e) {
			ExportAssetsArknights(ExportArknightsFilter.Scene, ExportListType.JSON);
		}

		private void ui_menuOhmsExport_arknights_charartBundle_Click(object sender, EventArgs e) {
			ExportAssetsArknights(ExportArknightsFilter.CharArt, ExportListType.JSON);
		}
		#endregion // Menu_OHMS_Export

		#region LeftTabPage
		private void ui_tabLeft_tab_Selected(object sender, TabControlEventArgs e) {
			switch (e.TabPageIndex) {
			case 0:
				ui_tabLeft_page0_treeSearch.Select();
				break;
			case 1:
				ui_tabLeft_page1_listSearch.Select();
				break;
			}
		}

		private void ui_tabLeft_page0_treeSearch_TextChanged(object sender, EventArgs e) {
			treeSrcResults.Clear();
			nextGObject = 0;
		}

		private void ui_tabLeft_page0_treeSearch_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode != Keys.Enter)
				return;
			if (treeSrcResults.Count == 0) {
				foreach (TreeNode node in ui_tabLeft_page0_treeView.Nodes) {
					TreeNodeSearch(node);
				}
			}
			if (treeSrcResults.Count > 0) {
				if (nextGObject >= treeSrcResults.Count) {
					nextGObject = 0;
				}
				treeSrcResults[nextGObject].EnsureVisible();
				ui_tabLeft_page0_treeView.SelectedNode = treeSrcResults[nextGObject];
				nextGObject++;
			}
		}

		private void ui_tabLeft_page0_treeView_AfterCheck(object sender, TreeViewEventArgs e) {
			foreach (TreeNode childNode in e.Node.Nodes) {
				childNode.Checked = e.Node.Checked;
			}
		}

		private void ui_tabLeft_page1_ListSearchTextChanged(object sender, EventArgs e) {
		}

		private void Ui_tabLeft_page1_listSearch_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode != Keys.Enter)
				return;
			FilterAssetList();
		}

		[DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
		private static extern int StrCmpLogicalW(string psz1, string psz2);

		private void ui_tabLeft_page1_listView_ColumnClick(object sender, ColumnClickEventArgs e) {
			if (sortColumn != e.Column) {
				reverseSort = false;
			}
			else {
				reverseSort = !reverseSort;
			}
			sortColumn = e.Column;
			ui_tabLeft_page1_listView.BeginUpdate();
			ui_tabLeft_page1_listView.SelectedIndices.Clear();
			if (sortColumn == 4) //FullSize
			{
				StudioCore.m_studio.visibleAssets.Sort((a, b) => {
					var asf = a.FullSize;
					var bsf = b.FullSize;
					return reverseSort ? bsf.CompareTo(asf) : asf.CompareTo(bsf);
				});
			}
			else if (sortColumn == 3) // PathID
			{
				StudioCore.m_studio.visibleAssets.Sort((x, y) => {
					long pathID_X = x.m_PathID;
					long pathID_Y = y.m_PathID;
					return reverseSort ? pathID_Y.CompareTo(pathID_X) : pathID_X.CompareTo(pathID_Y);
				});
			}
			else if (sortColumn == 2) { // Type
				StudioCore.m_studio.visibleAssets.Sort((a, b) => {
					var at = a.SubItems[sortColumn].Text;
					var bt = b.SubItems[sortColumn].Text;
					return reverseSort ? bt.CompareTo(at) : at.CompareTo(bt);
				});
			}
			else {
				StudioCore.m_studio.visibleAssets.Sort((a, b) => {
					var at = a.SubItems[sortColumn].Text;
					var bt = b.SubItems[sortColumn].Text;
					return reverseSort ? StrCmpLogicalW(bt, at) : StrCmpLogicalW(at, bt);
				});
			}
			ui_tabLeft_page1_listView.EndUpdate();
		}

		private void ui_tabLeft_page1_listView_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				if (ui_tabLeft_page1_listView.SelectedIndices.Count > 0) {
					ui_conMenu_exportAnimatorAndSelectedAnimationClips.Visible = false;
					ui_conMenu_goToSceneHierarchy.Visible = false;
					ui_conMenu_showOriginalFile.Visible = false;

					if (ui_tabLeft_page1_listView.SelectedIndices.Count >= 1) {
						var selectedAssets = GetSelectedAssets();
						if (selectedAssets.Any(x => x.Type == ClassIDType.Animator) && selectedAssets.Any(x => x.Type == ClassIDType.AnimationClip)) {
							ui_conMenu_exportAnimatorAndSelectedAnimationClips.Visible = true;
						}
					}
					if (ui_tabLeft_page1_listView.SelectedIndices.Count == 1) {
						ui_conMenu_goToSceneHierarchy.Visible = true;
						ui_conMenu_showOriginalFile.Visible = true;
					}

					tempClipboard = ui_tabLeft_page1_listView.HitTest(new Point(e.X, e.Y)).SubItem.Text;
					ui_contextMenuStrip0.Show(ui_tabLeft_page1_listView, e.X, e.Y);
				}
			}
		}

		private void Ui_tabLeft_page1_listView_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (lastSelectedItem != null) {
				var previewForm = new PreviewForm(lastSelectedItem) {
					MdiParent = this,
					WindowState = ActiveMdiChild == null ? FormWindowState.Maximized : FormWindowState.Normal
				};
				if (ActiveMdiChild?.WindowState == FormWindowState.Maximized) {
					ActiveMdiChild.WindowState = FormWindowState.Normal;
				}
				previewForm.Show();
			}
		}

		private void ui_tabLeft_page1_listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
			e.Item = StudioCore.m_studio.visibleAssets[e.ItemIndex];
		}

		private void ui_tabLeft_page1_listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			lastSelectedItem = e.IsSelected ? (AssetItem)e.Item : null;
		}

		private void ui_tabLeft_page2_classesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			//StatusStripUpdate("");
		}

		#endregion LeftTabPage

		#region OHMS
		private bool GetANewFolder(string iDir, out string oDir) {
			if (StudioCore.m_studio.assetsManager.m_lastOpenPaths.Length == 1) {
				oDir = Path.Combine(iDir, Path.GetFileName(StudioCore.m_studio.assetsManager.m_lastOpenPaths[0]));
				if (Directory.Exists(oDir)) {
					oDir += DateTime.Now.ToString(" yyyyMMdd-HHmmss");
				}
			}
			else {
				oDir = Path.Combine(iDir, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
			}
			if (Directory.Exists(oDir)) {
				oDir += " #";
				string t = null;
				for (uint i = 0; i < 1024; ++i) {
					var tt = oDir + i;
					if (!Directory.Exists(tt)) {
						t = tt;
					}
				}
				if (t == null) {
					MessageBox.Show(Properties.Strings.OHMS_new_folder_failed,
						Properties.Strings.OHMS_new_folder_failed_title);
					return false;
				}
				oDir = t;
			}
			return true;
		}

		private void ExportAssetsStructured(ExportFilter type, ExportListType listType) {
			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.ohmsLastFolder = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.Save();

					string outdir;
					if (Properties.SettingsOHMS.Default.ohmsCreateNew) {
						if (!GetANewFolder(saveFolderDialog.Folder, out outdir)) {
							return;
						}
					}
					else {
						outdir = saveFolderDialog.Folder;
					}

					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = StudioCore.m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = StudioCore.m_studio.visibleAssets;
						break;
					}
					StudioCore.m_studio.ExportAssetsStructured(outdir, toExportAssets, listType);
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsArknights(ExportArknightsFilter type, ExportListType listType) {
			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.ohmsLastFolder = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.Save();

					string outdir;
					if (Properties.SettingsOHMS.Default.ohmsCreateNew) {
						if (!GetANewFolder(saveFolderDialog.Folder, out outdir)) {
							return;
						}
					}
					else {
						outdir = saveFolderDialog.Folder;
					}
					switch (type) {
					case ExportArknightsFilter.Scene:
						StudioCore.ExportAssetsArknightsScene(outdir, StudioCore.m_studio.exportableAssets, listType);
						break;
					case ExportArknightsFilter.CharArt:
						StudioCore.ExportAssetsArknightsCharart(outdir, StudioCore.m_studio.exportableAssets);
						break;
					}
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsSprites(ExportFilter type) {
			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.ohmsLastFolder = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.Save();

					string outdir;
					if (Properties.SettingsOHMS.Default.ohmsCreateNew) {
						if (!GetANewFolder(saveFolderDialog.Folder, out outdir)) {
							return;
						}
					}
					else {
						outdir = saveFolderDialog.Folder;
					}

					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = StudioCore.m_studio.exportableAssets.Where(x => x.Type == ClassIDType.Sprite).ToList();
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets().Where(x => x.Type == ClassIDType.Sprite).ToList();
						break;
					case ExportFilter.Filtered:
						toExportAssets = StudioCore.m_studio.visibleAssets.Where(x => x.Type == ClassIDType.Sprite).ToList();
						break;
					}
					StudioCore.ExportAssets(outdir, toExportAssets, ExportType.Convert);
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsOHMS(ExportFilter type) {
			if (StudioCore.m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					//ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.ohmsLastFolder = saveFolderDialog.Folder;
					Properties.SettingsOHMS.Default.Save();

					string outdir;
					if (Properties.SettingsOHMS.Default.ohmsCreateNew) {
						if (!GetANewFolder(saveFolderDialog.Folder, out outdir)) {
							return;
						}
					}
					else {
						outdir = saveFolderDialog.Folder;
					}

					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = StudioCore.m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = StudioCore.m_studio.visibleAssets;
						break;
					}
					StudioCore.ExportAssets(outdir, toExportAssets, ExportType.ConvertOHMS);
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		#endregion OHMS

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e) {
			foreach (var previewForm in MdiChildren) {
				previewForm.Close();
			}
		}
	}
}
