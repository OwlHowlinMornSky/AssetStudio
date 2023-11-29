using AssetStudio;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Font = AssetStudio.Font;
#if NET472
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;
#else
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;
using Matrix4 = OpenTK.Mathematics.Matrix4;
#endif

namespace AssetStudioGUI {

	partial class AssetStudioGUIForm : Form {
		private AssetItem lastSelectedItem;
		private string tempClipboard;

		public static StudioShell m_studio = new();

		#region Member_LeftView
		//asset list sorting
		private int sortColumn = -1;
		private bool reverseSort;

		//asset list filter
		private System.Timers.Timer m_delayTimer;
		private bool enableFiltering;

		//tree search
		private int nextGObject;
		private List<TreeNode> treeSrcResults = new List<TreeNode>();

		private string openDirectoryBackup = string.Empty;
		private string saveDirectoryBackup = string.Empty;

		private GUILogger logger;

		private string m_TextBase; // OHMS
		#endregion

		private DirectBitmap m_imageTexture;

		public AssetStudioGUIForm() {
			LanguageOptions.initWhenOpenForm();

			InitializeComponent();
#if DEBUG
			ui_menuDebug.Visible = true;
#endif
			m_page0_search_default = ui_tabLeft_page0_treeSearch.Text;
			m_page1_filter_default = ui_tabLeft_page1_listSearch.Text;

			ui_tabRight_page0_FMODresumeButton.Top = ui_tabRight_page0_FMODpauseButton.Top;
			ui_tabRight_page0_FMODstatusLabel_Playing.Top = ui_tabRight_page0_FMODstatusLabel_Stopped.Top;
			ui_tabRight_page0_FMODstatusLabel_Paused.Top = ui_tabRight_page0_FMODstatusLabel_Stopped.Top;

			m_TextBase = $"AssetStudio v0.16.47-OHMS-v{Application.ProductVersion}";

			Text = m_TextBase;
			m_delayTimer = new System.Timers.Timer(800);
			m_delayTimer.Elapsed += new ElapsedEventHandler(m_delayTimer_Elapsed);
			ui_menuOptions_displayAllAssets.Checked = Properties.Settings.Default.displayAll;
			ui_menuOptions_displayInfo.Checked = Properties.Settings.Default.displayInfo;
			ui_menuOptions_enablePreview.Checked = Properties.Settings.Default.enablePreview;
			ui_menuOhmsExport_createANewFolder.Checked = Properties.SettingsOHMS.Default.ohmsCreateNew;

			FMODinit();
			GLInit();

			logger = new GUILogger(StatusStripUpdate);
			Logger.Default = logger;
			Progress.Default = new Progress<int>(SetProgressBarValue);

			StudioCore.StatusStripUpdate = StatusStripUpdate;

			StatusStripUpdate(Properties.Strings.Status_Ready);
		}

		[DllImport("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

		#region Process
		private async void BuildAssetStructures() {
			if (m_studio.assetsManager.assetsFileList.Count == 0) {
				//StatusStripUpdate("No Unity file can be loaded.");
				StatusStripUpdate(Properties.Strings.Load_NoThing);
				return;
			}

			(var productName, var treeNodeCollection) = await Task.Run(() => m_studio.BuildAssetData());
			var typeMap = await Task.Run(() => m_studio.BuildClassStructure());

			Text = m_TextBase +
				$" - {(string.IsNullOrEmpty(productName) ? Properties.Strings.Load_NoProductName : productName)}" +
				$" - {m_studio.assetsManager.assetsFileList[0].unityVersion}" +
				$" - {m_studio.assetsManager.assetsFileList[0].m_TargetPlatform}";

			ui_tabLeft_page1_listView.VirtualListSize = m_studio.visibleAssets.Count;

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

			var types = m_studio.exportableAssets.Select(x => x.Type).Distinct().OrderBy(x => x.ToString()).ToArray();
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
				m_studio.assetsManager.assetsFileList.Count, ui_tabLeft_page1_listView.Items.Count);
			var m_ObjectsCount = m_studio.assetsManager.assetsFileList.Sum(x => x.m_Objects.Count);
			var objectsCount = m_studio.assetsManager.assetsFileList.Sum(x => x.Objects.Count);
			if (m_ObjectsCount != objectsCount) {
				//log += $" and {m_ObjectsCount - objectsCount} assets failed to read";
				log += String.Format(Properties.Strings.Load_AndFailRead, m_ObjectsCount - objectsCount);
			}
			StatusStripUpdate(log);
		}

		private void ResetForm() {
			Text = this.m_TextBase;

			// Studio things
			m_studio.assetsManager.Clear();
			m_studio.assemblyLoader.Clear();
			m_studio.exportableAssets.Clear();
			m_studio.visibleAssets.Clear();

			ui_tabLeft_page0_treeView.Nodes.Clear();
			ui_tabLeft_page1_listView.VirtualListSize = 0;
			ui_tabLeft_page1_listView.Items.Clear();
			ui_tabLeft_page2_classesListView.Items.Clear();
			ui_tabLeft_page2_classesListView.Groups.Clear();
			m_imageTexture?.Dispose();
			m_imageTexture = null;

			ResetPreview();

			lastSelectedItem = null;
			sortColumn = -1;
			reverseSort = false;
			enableFiltering = false;
			//ui_tabLeft_page1_listSearch.Text = " Filter ";
			ui_tabLeft_page1_listSearch.Text = m_page1_filter_default;
			ui_tabLeft_page1_listSearch.ForeColor = SystemColors.GrayText;

			ui_tabLeft_page0_treeSearch.Text = m_page0_search_default;
			ui_tabLeft_page0_treeSearch.ForeColor = SystemColors.GrayText;

			var count = ui_menuFilter.DropDownItems.Count;
			for (var i = 1; i < count; i++) {
				ui_menuFilter.DropDownItems.RemoveAt(1);
			}

			StatusStripUpdate(Properties.Strings.Status_Ready);
		}

		private void SetProgressBarValue(int value) {
			if (InvokeRequired) {
				BeginInvoke(new Action(() => { ui_progressBar0_down.Value = value; }));
			}
			else {
				ui_progressBar0_down.Value = value;
			}
		}

		private void StatusStripUpdate(string statusText) {
			if (InvokeRequired) {
				var r = BeginInvoke(new Action(() => { ui_statusLabel0_down.Text = statusText; }));
				EndInvoke(r);
			}
			else {
				ui_statusLabel0_down.Text = statusText;
			}
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
				m_studio.visibleAssets = m_studio.exportableAssets.FindAll(x => show.Contains(x.Type));
			}
			else {
				m_studio.visibleAssets = m_studio.exportableAssets;
			}
			if (ui_tabLeft_page1_listSearch.Text != m_page1_filter_default) {
				m_studio.visibleAssets = m_studio.visibleAssets.FindAll(
					x => x.Text.IndexOf(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
					x.SubItems[1].Text.IndexOf(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
					x.SubItems[3].Text.IndexOf(ui_tabLeft_page1_listSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}
			ui_tabLeft_page1_listView.VirtualListSize = m_studio.visibleAssets.Count;
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
		}

		private void ExportMergeObjects(bool animation) {
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
		}

		private void ExportAssets(ExportFilter type, ExportType exportType) {
			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = m_studio.visibleAssets;
						break;
					}
					StudioCore.ExportAssets(saveFolderDialog.Folder, toExportAssets, exportType);
				}
			}
			else {
				//StatusStripUpdate("No exportable assets loaded");
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsList(ExportFilter type, ExportListType filetype) {
			// XXX: Only exporting as XML for now, but would JSON(/CSV/other) be useful too?

			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = saveDirectoryBackup;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
					saveDirectoryBackup = saveFolderDialog.Folder;
					List<AssetItem> toExportAssets = null;
					switch (type) {
					case ExportFilter.All:
						toExportAssets = m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = m_studio.visibleAssets;
						break;
					}
					StudioCore.ExportAssetsList(saveFolderDialog.Folder, toExportAssets, filetype);
				}
			}
			else {
				//StatusStripUpdate("No exportable assets loaded");
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAnimatorAndSelectedAnimationClips() {
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
		}
		#endregion // Export

		#region AssetStudioGUIForm
		private void AssetStudioGUIForm_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Move;
			}
		}

		private async void AssetStudioGUIForm_DragDrop(object sender, DragEventArgs e) {
			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (paths.Length > 0) {
				ResetForm();
				m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;

				await Task.Run(() => m_studio.assetsManager.LoadDropIn(paths));
				BuildAssetStructures();
			}
		}

		private void AssetStudioForm_KeyDown(object sender, KeyEventArgs e) {
			if (ui_tabRight_page0_glPreview.Visible) {
				if (e.Control) {
					switch (e.KeyCode) {
					case Keys.W:
						//Toggle WireFrame
						wireFrameMode = (wireFrameMode + 1) % 3;
						ui_tabRight_page0_glPreview.Invalidate();
						break;
					case Keys.S:
						//Toggle Shade
						shadeMode = (shadeMode + 1) % 2;
						ui_tabRight_page0_glPreview.Invalidate();
						break;
					case Keys.N:
						//Normal mode
						normalMode = (normalMode + 1) % 2;
						GL_CreateVAO();
						ui_tabRight_page0_glPreview.Invalidate();
						break;
					case Keys.R:
						wireFrameMode = 2;
						shadeMode = 0;
						normalMode = 0;
						GL_InitMatrices();
						ui_tabRight_page0_glPreview.Invalidate();
						break;
					}
				}
			}
			else if (ui_tabRight_page0_previewPanel.Visible) {
				if (e.Control) {
					var need = false;
					switch (e.KeyCode) {
					case Keys.B:
						m_textureChannels[0] = !m_textureChannels[0];
						need = true;
						break;
					case Keys.G:
						m_textureChannels[1] = !m_textureChannels[1];
						need = true;
						break;
					case Keys.R:
						m_textureChannels[2] = !m_textureChannels[2];
						need = true;
						break;
					case Keys.A:
						m_textureChannels[3] = !m_textureChannels[3];
						need = true;
						break;
					}
					if (need) {
						if (lastSelectedItem != null) {
							PreviewAsset(lastSelectedItem);
							ui_tabRight_page0_assetInfoLabel.Text = lastSelectedItem.InfoText;
						}
					}
				}
			}
		}
		#endregion // AssetStudioGUIForm

		#region ContextMenu
		private void ui_conMenu_copyText_Click(object sender, EventArgs e) {
			Clipboard.SetDataObject(tempClipboard);
		}

		private void ui_conMenu_exportSelectedAssets_Click(object sender, EventArgs e) {
			ExportAssets(ExportFilter.Selected, ExportType.Convert);
		}

		private void ui_conMenu_exportAnimatorAndSelectedAnimationClips_Click(object sender, EventArgs e) {
			ExportAnimatorAndSelectedAnimationClips();
		}

		private void ui_conMenu_goToSceneHierarchy_Click(object sender, EventArgs e) {
			var selectasset = (AssetItem)ui_tabLeft_page1_listView.Items[ui_tabLeft_page1_listView.SelectedIndices[0]];
			if (selectasset.TreeNode != null) {
				ui_tabLeft_page0_treeView.SelectedNode = selectasset.TreeNode;
				ui_tabLeft_tab.SelectedTab = ui_tabLeft_page0;
			}
		}

		private void ui_conMenu_showOriginalFile_Click(object sender, EventArgs e) {
			var selectasset = (AssetItem)ui_tabLeft_page1_listView.Items[ui_tabLeft_page1_listView.SelectedIndices[0]];
			var args = $"/select, \"{selectasset.SourceFile.originalPath ?? selectasset.SourceFile.fullName}\"";
			var pfi = new ProcessStartInfo("explorer.exe", args);
			Process.Start(pfi);
		}

		private void ui_conMenu_OHMS_ExportSelected_Click(object sender, EventArgs e) {
			ExportAssetsOHMS(ExportFilter.Selected);
		}

		private void ui_conMenu_OHMS_ExportSelected_Struct_XML_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.XML);
		}

		private void ui_conMenu_OHMS_ExportSelected_Struct_JSON_Click(object sender, EventArgs e) {
			ExportAssetsStructured(ExportFilter.Selected, ExportListType.JSON);
		}
		#endregion ContextMenu

		#region Menu_File
		private async void ui_menuFile_loadFile_Click(object sender, EventArgs e) {
			ui_openFileDialog0.InitialDirectory = openDirectoryBackup;
			if (ui_openFileDialog0.ShowDialog(this) == DialogResult.OK) {
				ResetForm();
				openDirectoryBackup = Path.GetDirectoryName(ui_openFileDialog0.FileNames[0]);
				m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;
				await Task.Run(() => m_studio.assetsManager.LoadFiles(ui_openFileDialog0.FileNames));
				BuildAssetStructures();
			}
		}

		private async void ui_menuFile_loadFolder_Click(object sender, EventArgs e) {
			var openFolderDialog = new OpenFolderDialog();
			openFolderDialog.InitialFolder = openDirectoryBackup;
			if (openFolderDialog.ShowDialog(this) == DialogResult.OK) {
				ResetForm();
				openDirectoryBackup = openFolderDialog.Folder;
				m_studio.assetsManager.SpecifyUnityVersion = ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Text;
				await Task.Run(() => m_studio.assetsManager.LoadFolder(openFolderDialog.Folder));
				BuildAssetStructures();
			}
		}

		private async void ui_menuFile_extractFile_Click(object sender, EventArgs e) {
			if (ui_openFileDialog0.ShowDialog(this) == DialogResult.OK) {
				var saveFolderDialog = new OpenFolderDialog();
				//saveFolderDialog.Title = "Select the save folder";
				saveFolderDialog.Title = Properties.Strings.Export_SaveFolderDialog_Title;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					var fileNames = ui_openFileDialog0.FileNames;
					var savePath = saveFolderDialog.Folder;
					var extractedCount = await Task.Run(() => StudioCore.ExtractFile(fileNames, savePath));
					//StatusStripUpdate($"Finished extracting {extractedCount} files.");
					StatusStripUpdate(String.Format(Properties.Strings.Export_FinishExtracting, extractedCount));
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
					var extractedCount = await Task.Run(() => StudioCore.ExtractFolder(path, savePath));
					//StatusStripUpdate($"Finished extracting {extractedCount} files.");
					StatusStripUpdate(String.Format(Properties.Strings.Export_FinishExtracting, extractedCount));
				}
			}
		}

		private void ui_menuFile_clear_Click(object sender, EventArgs e) {
			ResetForm();
		}
		#endregion // Menu_File

		#region Menu_Options
		private void ui_menuOptions_language_Click(object sender, EventArgs e) {
			var langOpt = new LanguageOptions();
			langOpt.ShowDialog(this);
		}

		private void ui_menuOptions_exportOptions_Click(object sender, EventArgs e) {
			var exportOpt = new ExportOptions();
			exportOpt.ShowDialog(this);
		}

		private void ui_menuOptions_displayAllAssets_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.displayAll = ui_menuOptions_displayAllAssets.Checked;
			Properties.Settings.Default.Save();
		}

		private void ui_menuOptions_enablePreview_Check(object sender, EventArgs e) {
			if (ui_tabRight_tab.SelectedIndex == 0) {
				if (lastSelectedItem != null && ui_menuOptions_enablePreview.Checked) {
					PreviewAsset(lastSelectedItem);
				}
				else {
					ResetPreview();
				}
			}

			Properties.Settings.Default.enablePreview = ui_menuOptions_enablePreview.Checked;
			Properties.Settings.Default.Save();
		}

		private void ui_menuOptions_displayAssetInfo_Check(object sender, EventArgs e) {
			if (ui_menuOptions_displayInfo.Checked && ui_tabRight_page0_assetInfoLabel.Text != null) {
				ui_tabRight_page0_assetInfoLabel.Visible = true;
			}
			else {
				ui_tabRight_page0_assetInfoLabel.Visible = false;
			}

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
			logger.ShowErrorMessage = ui_menuDebug_showErrorMessage.Checked;
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

		private string m_page0_search_default;
		private void ui_tabLeft_page0_treeSearch_Enter(object sender, EventArgs e) {
			if (ui_tabLeft_page0_treeSearch.Text == m_page0_search_default) {
				ui_tabLeft_page0_treeSearch.Text = "";
				ui_tabLeft_page0_treeSearch.ForeColor = SystemColors.WindowText;
			}
		}

		private void ui_tabLeft_page0_treeSearch_Leave(object sender, EventArgs e) {
			if (ui_tabLeft_page0_treeSearch.Text == "") {
				ui_tabLeft_page0_treeSearch.Text = m_page0_search_default;
				ui_tabLeft_page0_treeSearch.ForeColor = SystemColors.GrayText;
			}
		}

		private void ui_tabLeft_page0_treeSearch_TextChanged(object sender, EventArgs e) {
			treeSrcResults.Clear();
			nextGObject = 0;
		}

		private void ui_tabLeft_page0_treeSearch_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == System.Windows.Forms.Keys.Enter) {
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
		}

		private void ui_tabLeft_page0_treeView_AfterCheck(object sender, TreeViewEventArgs e) {
			foreach (TreeNode childNode in e.Node.Nodes) {
				childNode.Checked = e.Node.Checked;
			}
		}

		private string m_page1_filter_default;
		private void ui_tabLeft_page1_listSearch_Enter(object sender, EventArgs e) {
			if (ui_tabLeft_page1_listSearch.Text == m_page1_filter_default) {
				ui_tabLeft_page1_listSearch.Text = "";
				ui_tabLeft_page1_listSearch.ForeColor = SystemColors.WindowText;
				enableFiltering = true;
			}
		}

		private void ui_tabLeft_page1_listSearch_Leave(object sender, EventArgs e) {
			if (ui_tabLeft_page1_listSearch.Text == "") {
				enableFiltering = false;
				ui_tabLeft_page1_listSearch.Text = m_page1_filter_default;
				ui_tabLeft_page1_listSearch.ForeColor = SystemColors.GrayText;
			}
		}

		private void ui_tabLeft_page1_ListSearchTextChanged(object sender, EventArgs e) {
			if (enableFiltering) {
				if (m_delayTimer.Enabled) {
					m_delayTimer.Stop();
					m_delayTimer.Start();
				}
				else {
					m_delayTimer.Start();
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
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
				m_studio.visibleAssets.Sort((a, b) => {
					var asf = a.FullSize;
					var bsf = b.FullSize;
					return reverseSort ? bsf.CompareTo(asf) : asf.CompareTo(bsf);
				});
			}
			else if (sortColumn == 3) // PathID
			{
				m_studio.visibleAssets.Sort((x, y) => {
					long pathID_X = x.m_PathID;
					long pathID_Y = y.m_PathID;
					return reverseSort ? pathID_Y.CompareTo(pathID_X) : pathID_X.CompareTo(pathID_Y);
				});
			}
			else if (sortColumn == 2) { // Type
				m_studio.visibleAssets.Sort((a, b) => {
					var at = a.SubItems[sortColumn].Text;
					var bt = b.SubItems[sortColumn].Text;
					return reverseSort ? bt.CompareTo(at) : at.CompareTo(bt);
				});
			}
			else {
				m_studio.visibleAssets.Sort((a, b) => {
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

		private void ui_tabLeft_page1_listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
			e.Item = m_studio.visibleAssets[e.ItemIndex];
		}

		private void ui_tabLeft_page1_listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			if (e.IsSelected) {
				lastSelectedItem = (AssetItem)e.Item;
				m_previewLoaded = 0;
				switch (ui_tabRight_tab.SelectedIndex) {
				case 0:
					if (ui_menuOptions_enablePreview.Checked) {
						PreviewAsset(lastSelectedItem);
					}
					else {
						ResetPreview();
					}
					break;
				case 1:
					PreviewDump(lastSelectedItem);
					break;
				case 2:
					PreviewDumpJSON(lastSelectedItem);
					break;
				}
			}
			else {
				lastSelectedItem = null;
				ResetPreview();
			}
		}

		private void ui_tabLeft_page2_classesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			StatusStripUpdate("");
			if (e.IsSelected) {
				ui_tabRight_page0_classTextPreview.Text = ((TypeTreeItem)ui_tabLeft_page2_classesListView.SelectedItems[0]).ToString();
				m_previewLoaded = 1 << 3;
				SwitchPreviewPage(PreviewType.ClassText);
			}
		}

		private void m_delayTimer_Elapsed(object sender, ElapsedEventArgs e) {
			m_delayTimer.Stop();
			Invoke(new Action(FilterAssetList));
		}
		#endregion LeftTabPage

		#region RightTabPage
		private void ui_tabRight_tab_SelectedIndexChanged(object sender, EventArgs e) {
			if ((m_previewLoaded & (1 << 3)) != 0) {
				return;
			}
			if (lastSelectedItem == null) {
				ResetPreview();
				return;
			}
			switch (ui_tabRight_tab.SelectedIndex) {
			case 0:
				if (Properties.Settings.Default.enablePreview)
					PreviewAsset(lastSelectedItem);
				break;
			case 1:
				PreviewDump(lastSelectedItem);
				break;
			case 2:
				PreviewDumpJSON(lastSelectedItem);
				break;
			}
		}
		#endregion RightTabPage

		#region Preview
		private int m_previewLoaded;

		private void ui_tabRight_page0_previewPanel_Resize(object sender, EventArgs e) {
			if (glControlLoaded && ui_tabRight_page0_glPreview.Visible) {
				GL_ChangeSize(ui_tabRight_page0_glPreview.Size);
				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

		enum PreviewType {
			None = 0,
			FMOD,
			Font,
			GL,
			Text,
			ClassText,
			Dump,
			DumpJson,
			COUNT
		}

		private void ResetPreview() {
			ui_tabRight_page0_previewPanel.BackgroundImage = Properties.Resources.preview;
			ui_tabRight_page0_previewPanel.BackgroundImageLayout = ImageLayout.Center;

			SwitchPreviewPage(PreviewType.None);

			ui_tabRight_page0_assetInfoLabel.Visible = false;
			ui_tabRight_page0_assetInfoLabel.Text = null;

			ui_tabRight_page1_dumpTextBox.Text = null;
			ui_tabRight_page2_dumpJsonTextBox.Text = null;

			m_previewLoaded = 0;

			//StatusStripUpdate("");
		}

		private void SwitchPreviewPage(PreviewType type) {
			switch (type) {
			case PreviewType.None:
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_fontPreviewBox.Visible = false;
				ui_tabRight_page0_glPreview.Visible = false;
				ui_tabRight_page0_textPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_classTextPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Text = null;
				ui_tabRight_page1_dumpTextBox.Text = null;
				ui_tabRight_page2_dumpJsonTextBox.Text = null;
				break;
			case PreviewType.FMOD:
				ui_tabRight_page0_FMODpanel.Visible = true;
				ui_tabRight_page0_fontPreviewBox.Visible = false;
				ui_tabRight_page0_glPreview.Visible = false;
				ui_tabRight_page0_textPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_classTextPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Text = null;
				break;
			case PreviewType.Font:
				ui_tabRight_page0_fontPreviewBox.Visible = true;
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_glPreview.Visible = false;
				ui_tabRight_page0_textPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_classTextPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Text = null;
				break;
			case PreviewType.GL:
				ui_tabRight_page0_glPreview.Visible = true;
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_fontPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_classTextPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Text = null;
				break;
			case PreviewType.Text:
				ui_tabRight_page0_textPreviewBox.Visible = true;
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_fontPreviewBox.Visible = false;
				ui_tabRight_page0_glPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Visible = false;
				ui_tabRight_page0_classTextPreview.Text = null;
				break;
			case PreviewType.ClassText:
				ui_tabRight_page0_classTextPreview.Visible = true;
				ui_tabRight_page0_FMODpanel.Visible = false;
				FMODreset();
				ui_tabRight_page0_fontPreviewBox.Visible = false;
				ui_tabRight_page0_glPreview.Visible = false;
				ui_tabRight_page0_textPreviewBox.Visible = false;
				ui_tabRight_page0_textPreviewBox.Text = null;
				ui_tabRight_page0_assetInfoLabel.Visible = false;
				ui_tabRight_page0_assetInfoLabel.Text = null;
				ui_tabRight_page1_dumpTextBox.Text = null;
				;
				ui_tabRight_page2_dumpJsonTextBox.Text = null;
				break;
			}
		}

		private void PreviewAsset(AssetItem assetItem) {
			if (assetItem == null) {
				ResetPreview();
				return;
			}
			if ((m_previewLoaded & (1 << 0)) != 0) {
				return;
			}
			try {
				switch (assetItem.Asset) {
				case Texture2D m_Texture2D:
					PreviewTexture2D(assetItem, m_Texture2D);
					break;
				case AudioClip m_AudioClip:
					PreviewAudioClip(assetItem, m_AudioClip);
					break;
				case Shader m_Shader:
					PreviewShader(m_Shader);
					break;
				case TextAsset m_TextAsset:
					PreviewTextAsset(m_TextAsset);
					break;
				case MonoBehaviour m_MonoBehaviour:
					PreviewMonoBehaviour(m_MonoBehaviour);
					break;
				case Font m_Font:
					PreviewFont(m_Font);
					break;
				case Mesh m_Mesh:
					PreviewMesh(m_Mesh);
					break;
				case VideoClip _:
				case MovieTexture _:
					//StatusStripUpdate("Only supported export.");
					StatusStripUpdate(Properties.Strings.Preview_OnlyExport);
					break;
				case Sprite m_Sprite:
					PreviewSprite(assetItem, m_Sprite);
					break;
				case Animator _:
					//StatusStripUpdate("Can be exported to FBX file.");
					StatusStripUpdate(Properties.Strings.Preview_OnlyExport_FBX);
					break;
				case AnimationClip _:
					//StatusStripUpdate("Can be exported with Animator or Objects");
					StatusStripUpdate(Properties.Strings.Preview_OnlyExport_Animator);
					break;
				default:
					PreviewText(m_studio.DumpAsset(assetItem.Asset));
					break;
				}
				if (ui_menuOptions_displayInfo.Checked && lastSelectedItem.InfoText != null) {
					ui_tabRight_page0_assetInfoLabel.Text = lastSelectedItem.InfoText;
					ui_tabRight_page0_assetInfoLabel.Visible = true;
				}
				else {
					ui_tabRight_page0_assetInfoLabel.Text = null;
					ui_tabRight_page0_assetInfoLabel.Visible = false;
				}
				m_previewLoaded |= 1 << 0;
			}
			catch (Exception e) {
				//MessageBox.Show($"Preview {assetItem.Type}:{assetItem.Text} error\r\n{e.Message}\r\n{e.StackTrace}");
				MessageBox.Show(
					String.Format(Properties.Strings.Preview_Exception, assetItem.Type, assetItem.Text)
					+ "\n" + e.Message + "\n" + e.StackTrace);
			}
		}

		private static char[] m_textureChannelNames = new[] { 'B', 'G', 'R', 'A' };
		private bool[] m_textureChannels = new[] { true, true, true, true };
		private void PreviewTexture2D(AssetItem assetItem, Texture2D m_Texture2D) {
			var image = m_Texture2D.ConvertToImage(true);
			if (image != null) {
				var bitmap = new DirectBitmap(image.ConvertToBytes(), m_Texture2D.m_Width, m_Texture2D.m_Height);
				image.Dispose();
				//assetItem.InfoText = $"Width: {m_Texture2D.m_Width}\nHeight: {m_Texture2D.m_Height}\nFormat: {m_Texture2D.m_TextureFormat}";
				assetItem.InfoText = String.Format(Properties.Strings.Preview_Tex2D_info,
					m_Texture2D.m_Width, m_Texture2D.m_Height, m_Texture2D.m_TextureFormat);
				switch (m_Texture2D.m_TextureSettings.m_FilterMode) {
				case 0:
					//assetItem.InfoText += "\nFilter Mode: Point ";
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Point);
					break;
				case 1:
					//assetItem.InfoText += "\nFilter Mode: Bilinear ";
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Bilinear);
					break;
				case 2:
					//assetItem.InfoText += "\nFilter Mode: Trilinear ";
					assetItem.InfoText += "\n" +
						String.Format(Properties.Strings.Preview_Tex2D_info_Filter_Mode,
						Properties.Strings.Preview_Tex2D_info_Filter_Mode_Trilinear);
					break;
				}
				//assetItem.InfoText += $"\nAnisotropic level: {m_Texture2D.m_TextureSettings.m_Aniso}\nMip map bias: {m_Texture2D.m_TextureSettings.m_MipBias}";
				assetItem.InfoText += "\n" +
					String.Format(Properties.Strings.Preview_Tex2D_info_mipmap,
					m_Texture2D.m_TextureSettings.m_Aniso, m_Texture2D.m_TextureSettings.m_MipBias);
				switch (m_Texture2D.m_TextureSettings.m_WrapMode) {
				case 0:
					assetItem.InfoText += "\n" +
						//	"Wrap mode: Repeat";
						Properties.Strings.Preview_Tex2D_info_wrap + Properties.Strings.Preview_Tex2D_info_wrap_repeat;
					break;
				case 1:
					assetItem.InfoText += "\n" +
						//	"Wrap mode: Clamp";
						Properties.Strings.Preview_Tex2D_info_wrap + Properties.Strings.Preview_Tex2D_info_wrap_clamp;
					break;
				}
				//assetItem.InfoText += "\n" + "Channels: ";
				assetItem.InfoText += "\n" + Properties.Strings.Preview_Tex2D_info_channels;
				int validChannel = 0;
				for (int i = 0; i < 4; i++) {
					if (m_textureChannels[i]) {
						assetItem.InfoText += m_textureChannelNames[i];
						validChannel++;
					}
				}
				if (validChannel == 0)
					//assetItem.InfoText += "None";
					assetItem.InfoText += Properties.Strings.Preview_Tex2D_info_channels_none;
				if (validChannel != 4) {
					var bytes = bitmap.Bits;
					for (int i = 0; i < bitmap.Height; i++) {
						int offset = Math.Abs(bitmap.Stride) * i;
						for (int j = 0; j < bitmap.Width; j++) {
							bytes[offset] = m_textureChannels[0] ? bytes[offset] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 1] = m_textureChannels[1] ? bytes[offset + 1] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 2] = m_textureChannels[2] ? bytes[offset + 2] : validChannel == 1 && m_textureChannels[3] ? byte.MaxValue : byte.MinValue;
							bytes[offset + 3] = m_textureChannels[3] ? bytes[offset + 3] : byte.MaxValue;
							offset += 4;
						}
					}
				}
				PreviewTexture(bitmap);

				//StatusStripUpdate("'Ctrl' + 'R'/'G'/'B'/'A' " + "for Channel Toggle");
				StatusStripUpdate("'Ctrl' + 'R'/'G'/'B'/'A' " + Properties.Strings.Preview_Tex2D_Channel_Toggle);
			}
			else {
				//StatusStripUpdate("Unsupported image for preview");
				StatusStripUpdate(Properties.Strings.Preview_Tex2D_unsupported);
			}
		}

		private void PreviewAudioClip(AssetItem assetItem, AudioClip m_AudioClip) {
			//Info
			assetItem.InfoText = Properties.Strings.Preview_Audio_formatHead;
			if (m_AudioClip.version[0] < 5) {
				switch (m_AudioClip.m_Type) {
				case FMODSoundType.ACC:
					assetItem.InfoText += "ACC";
					break;
				case FMODSoundType.AIFF:
					assetItem.InfoText += "AIFF";
					break;
				case FMODSoundType.IT:
					assetItem.InfoText += "Impulse tracker";
					break;
				case FMODSoundType.MOD:
					assetItem.InfoText += "Protracker / Fasttracker MOD";
					break;
				case FMODSoundType.MPEG:
					assetItem.InfoText += "MP2/MP3 MPEG";
					break;
				case FMODSoundType.OGGVORBIS:
					assetItem.InfoText += "Ogg vorbis";
					break;
				case FMODSoundType.S3M:
					assetItem.InfoText += "ScreamTracker 3";
					break;
				case FMODSoundType.WAV:
					assetItem.InfoText += "Microsoft WAV";
					break;
				case FMODSoundType.XM:
					assetItem.InfoText += "FastTracker 2 XM";
					break;
				case FMODSoundType.XMA:
					assetItem.InfoText += "Xbox360 XMA";
					break;
				case FMODSoundType.VAG:
					assetItem.InfoText += "PlayStation Portable ADPCM";
					break;
				case FMODSoundType.AUDIOQUEUE:
					assetItem.InfoText += "iPhone";
					break;
				default:
					assetItem.InfoText += "Unknown";
					break;
				}
			}
			else {
				switch (m_AudioClip.m_CompressionFormat) {
				case AudioCompressionFormat.PCM:
					assetItem.InfoText += "PCM";
					break;
				case AudioCompressionFormat.Vorbis:
					assetItem.InfoText += "Vorbis";
					break;
				case AudioCompressionFormat.ADPCM:
					assetItem.InfoText += "ADPCM";
					break;
				case AudioCompressionFormat.MP3:
					assetItem.InfoText += "MP3";
					break;
				case AudioCompressionFormat.PSMVAG:
					assetItem.InfoText += "PlayStation Portable ADPCM";
					break;
				case AudioCompressionFormat.HEVAG:
					assetItem.InfoText += "PSVita ADPCM";
					break;
				case AudioCompressionFormat.XMA:
					assetItem.InfoText += "Xbox360 XMA";
					break;
				case AudioCompressionFormat.AAC:
					assetItem.InfoText += "AAC";
					break;
				case AudioCompressionFormat.GCADPCM:
					assetItem.InfoText += "Nintendo 3DS/Wii DSP";
					break;
				case AudioCompressionFormat.ATRAC9:
					assetItem.InfoText += "PSVita ATRAC9";
					break;
				default:
					assetItem.InfoText += "Unknown";
					break;
				}
			}
			FMODreset();

			var m_AudioData = m_AudioClip.m_AudioData.GetData();
			if (m_AudioData == null || m_AudioData.Length == 0)
				return;
			var exinfo = new FMOD.CREATESOUNDEXINFO();

			exinfo.cbsize = Marshal.SizeOf(exinfo);
			exinfo.length = (uint)m_AudioClip.m_Size;

			var result = system.createSound(m_AudioData, FMOD.MODE.OPENMEMORY | loopMode, ref exinfo, out sound);
			if (FMOD_CHECK(result))
				return;

			sound.getNumSubSounds(out var numsubsounds);

			if (numsubsounds > 0) {
				result = sound.getSubSound(0, out var subsound);
				if (result == FMOD.RESULT.OK) {
					sound = subsound;
				}
			}

			result = sound.getLength(out FMODlenms, FMOD.TIMEUNIT.MS);
			if (FMOD_CHECK(result))
				return;

			result = system.playSound(sound, null, true, out channel);
			if (FMOD_CHECK(result))
				return;

			SwitchPreviewPage(PreviewType.FMOD);

			result = channel.getFrequency(out var frequency);
			if (FMOD_CHECK(result))
				return;

			ui_tabRight_page0_FMODinfoLabel.Text = frequency + " Hz";
			ui_tabRight_page0_FMODtimerLabel.Text = "00:00.00";

			ui_tabRight_page0_FMODdurationLabel.Text = String.Format("{0:D2}:{1:D2}.{2:D2}", FMODlenms / 1000 / 60, FMODlenms / 1000 % 60, FMODlenms / 10 % 100);
		}

		private void PreviewShader(Shader m_Shader) {
			var str = ShaderConverter.Convert(m_Shader);
			//PreviewText(str == null ? "Serialized Shader can't be read" : str.Replace("\n", "\r\n"));
			PreviewText(str == null ? Properties.Strings.Preview_Shader_Serialized : str.Replace("\n", "\r\n"));
		}

		private void PreviewTextAsset(TextAsset m_TextAsset) {
			var text = Encoding.UTF8.GetString(m_TextAsset.m_Script);
			text = text.Replace("\n", "\r\n").Replace("\0", "");
			PreviewText(text);
		}

		private void PreviewMonoBehaviour(MonoBehaviour m_MonoBehaviour) {
			var obj = m_MonoBehaviour.ToType();
			if (obj == null) {
				var type = m_studio.MonoBehaviourToTypeTree(m_MonoBehaviour);
				obj = m_MonoBehaviour.ToType(type);
			}
			var str = JsonConvert.SerializeObject(obj, Formatting.Indented);
			PreviewText(str);
		}

		private void PreviewFont(Font m_Font) {
			if (m_Font.m_FontData != null) {
				var data = Marshal.AllocCoTaskMem(m_Font.m_FontData.Length);
				Marshal.Copy(m_Font.m_FontData, 0, data, m_Font.m_FontData.Length);

				uint cFonts = 0;
				var re = AddFontMemResourceEx(data, (uint)m_Font.m_FontData.Length, IntPtr.Zero, ref cFonts);
				if (re != IntPtr.Zero) {
					using (var pfc = new PrivateFontCollection()) {
						pfc.AddMemoryFont(data, m_Font.m_FontData.Length);
						Marshal.FreeCoTaskMem(data);
						if (pfc.Families.Length > 0) {
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 0;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 80;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 16, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 81;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 12, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 138;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 18, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 195;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 24, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 252;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 36, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 309;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 48, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 366;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 56;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 60, FontStyle.Regular);
							ui_tabRight_page0_fontPreviewBox.SelectionStart = 423;
							ui_tabRight_page0_fontPreviewBox.SelectionLength = 55;
							ui_tabRight_page0_fontPreviewBox.SelectionFont = new System.Drawing.Font(pfc.Families[0], 72, FontStyle.Regular);
							SwitchPreviewPage(PreviewType.Font);
						}
					}
					return;
				}
			}
			//StatusStripUpdate("Unsupported font for preview. Try to export.");
			StatusStripUpdate(Properties.Strings.Preview_Font_Unsupported);
		}

		private void PreviewMesh(Mesh m_Mesh) {
			if (m_Mesh.m_VertexCount > 0) {
				#region Vertices
				if (m_Mesh.m_Vertices == null || m_Mesh.m_Vertices.Length == 0) {
					//StatusStripUpdate("Mesh can't be previewed.");
					StatusStripUpdate(Properties.Strings.Preview_GL_unable1);
					return;
				}
				int count = 3;
				if (m_Mesh.m_Vertices.Length == m_Mesh.m_VertexCount * 4) {
					count = 4;
				}
				vertexData = new Vector3[m_Mesh.m_VertexCount];
				// Calculate Bounding
				float[] min = new float[3];
				float[] max = new float[3];
				for (int i = 0; i < 3; i++) {
					min[i] = m_Mesh.m_Vertices[i];
					max[i] = m_Mesh.m_Vertices[i];
				}
				for (int v = 0; v < m_Mesh.m_VertexCount; v++) {
					for (int i = 0; i < 3; i++) {
						min[i] = Math.Min(min[i], m_Mesh.m_Vertices[v * count + i]);
						max[i] = Math.Max(max[i], m_Mesh.m_Vertices[v * count + i]);
					}
					vertexData[v] = new Vector3(
						m_Mesh.m_Vertices[v * count],
						m_Mesh.m_Vertices[v * count + 1],
						-m_Mesh.m_Vertices[v * count + 2]);
				}

				// Calculate modelMatrix
				Vector3 dist = Vector3.One;
				for (int i = 0; i < 3; i++) {
					dist[i] = max[i] - min[i];
				}
				float d = Math.Max(1e-5f, dist.Length);
				//if (d > 64.0f)
				//	d = 64.0f;
				m_cameraPos.Z = m_defaultCameraDis = d;
				GL_updateViewMatrix();

				#endregion
				#region Indicies
				indiceData = new int[m_Mesh.m_Indices.Count];
				for (int i = 0; i < m_Mesh.m_Indices.Count; i = i + 3) {
					indiceData[i] = (int)m_Mesh.m_Indices[i + 2];
					indiceData[i + 1] = (int)m_Mesh.m_Indices[i + 1];
					indiceData[i + 2] = (int)m_Mesh.m_Indices[i];
				}
				#endregion
				#region Normals
				if (m_Mesh.m_Normals != null && m_Mesh.m_Normals.Length > 0) {
					if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 3)
						count = 3;
					else if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 4)
						count = 4;
					normalData = new Vector3[m_Mesh.m_VertexCount];
					for (int n = 0; n < m_Mesh.m_VertexCount; n++) {
						normalData[n] = new Vector3(
							m_Mesh.m_Normals[n * count],
							m_Mesh.m_Normals[n * count + 1],
							-m_Mesh.m_Normals[n * count + 2]);
					}
				}
				else
					normalData = null;
				// calculate normal by ourself
				normal2Data = new Vector3[m_Mesh.m_VertexCount];
				int[] normalCalculatedCount = new int[m_Mesh.m_VertexCount];
				for (int i = 0; i < m_Mesh.m_VertexCount; i++) {
					normal2Data[i] = Vector3.Zero;
					normalCalculatedCount[i] = 0;
				}
				for (int i = 0; i < m_Mesh.m_Indices.Count; i = i + 3) {
					Vector3 dir1 = vertexData[indiceData[i + 1]] - vertexData[indiceData[i]];
					Vector3 dir2 = vertexData[indiceData[i + 2]] - vertexData[indiceData[i]];
					Vector3 normal = Vector3.Cross(dir1, dir2);
					normal.Normalize();
					for (int j = 0; j < 3; j++) {
						normal2Data[indiceData[i + j]] += normal;
						normalCalculatedCount[indiceData[i + j]]++;
					}
				}
				for (int i = 0; i < m_Mesh.m_VertexCount; i++) {
					if (normalCalculatedCount[i] == 0)
						normal2Data[i] = new Vector3(0, 1, 0);
					else
						normal2Data[i] /= normalCalculatedCount[i];
				}
				#endregion
				#region Colors
				if (m_Mesh.m_Colors != null && m_Mesh.m_Colors.Length == m_Mesh.m_VertexCount * 3) {
					colorData = new Vector4[m_Mesh.m_VertexCount];
					for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
						colorData[c] = new Vector4(
							m_Mesh.m_Colors[c * 3],
							m_Mesh.m_Colors[c * 3 + 1],
							m_Mesh.m_Colors[c * 3 + 2],
							1.0f);
					}
				}
				else if (m_Mesh.m_Colors != null && m_Mesh.m_Colors.Length == m_Mesh.m_VertexCount * 4) {
					colorData = new Vector4[m_Mesh.m_VertexCount];
					for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
						colorData[c] = new Vector4(
						m_Mesh.m_Colors[c * 4],
						m_Mesh.m_Colors[c * 4 + 1],
						m_Mesh.m_Colors[c * 4 + 2],
						m_Mesh.m_Colors[c * 4 + 3]);
					}
				}
				else {
					colorData = new Vector4[m_Mesh.m_VertexCount];
					for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
						colorData[c] = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
					}
				}
				#endregion
				SwitchPreviewPage(PreviewType.GL);
				GL_CreateVAO();
				StatusStripUpdate(String.Format(Properties.Strings.Preview_GL_info, GL.GetString(StringName.Version)));

				GL_InitMatrices();
				ui_tabRight_page0_glPreview.Invalidate();
			}
			else {
				StatusStripUpdate(Properties.Strings.Preview_GL_unable);
			}
		}

		private void PreviewSprite(AssetItem assetItem, Sprite m_Sprite) {
			var image = m_Sprite.GetImage();
			if (image != null) {
				var bitmap = new DirectBitmap(image.ConvertToBytes(), image.Width, image.Height);
				image.Dispose();
				//assetItem.InfoText = $"Width: {bitmap.Width}\nHeight: {bitmap.Height}\n";
				assetItem.InfoText =
					String.Format(Properties.Strings.Preview_Sprite_info + "\n", bitmap.Width, bitmap.Height);
				PreviewTexture(bitmap);
			}
			else {
				//StatusStripUpdate("Unsupported sprite for preview.");
				StatusStripUpdate(Properties.Strings.Preview_Sprite_unsupported);
			}
		}

		private void PreviewTexture(DirectBitmap bitmap) {
			m_imageTexture?.Dispose();
			m_imageTexture = bitmap;
			ui_tabRight_page0_previewPanel.BackgroundImage = m_imageTexture.Bitmap;
			if (m_imageTexture.Width > ui_tabRight_page0_previewPanel.Width || m_imageTexture.Height > ui_tabRight_page0_previewPanel.Height)
				ui_tabRight_page0_previewPanel.BackgroundImageLayout = ImageLayout.Zoom;
			else
				ui_tabRight_page0_previewPanel.BackgroundImageLayout = ImageLayout.Center;
			SwitchPreviewPage(PreviewType.None);
		}

		private void PreviewText(string text) {
			ui_tabRight_page0_textPreviewBox.Text = text;
			SwitchPreviewPage(PreviewType.Text);
		}

		private void PreviewDump(AssetItem assetItem) {
			if ((m_previewLoaded & (1 << 1)) != 0)
				return;
			ui_tabRight_page1_dumpTextBox.Text = m_studio.DumpAsset(assetItem.Asset);
			m_previewLoaded |= 1 << 1;
			SwitchPreviewPage(PreviewType.Dump);
		}

		private void PreviewDumpJSON(AssetItem assetItem) {
			if ((m_previewLoaded & (1 << 2)) != 0)
				return;
			ui_tabRight_page2_dumpJsonTextBox.Text = m_studio.DumpAssetJson(assetItem.Asset);
			m_previewLoaded |= 1 << 2;
			SwitchPreviewPage(PreviewType.DumpJson);
		}
		#endregion Preview

		#region FMOD_Controller
		private FMOD.System system;
		private FMOD.Sound sound;
		private FMOD.Channel channel;
		private FMOD.SoundGroup masterSoundGroup;
		private FMOD.MODE loopMode = FMOD.MODE.LOOP_OFF;
		private uint FMODlenms;
		private float FMODVolume = 0.8f;

		private void FMODinit() {
			FMODreset();

			var result = FMOD.Factory.System_Create(out system);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = system.getVersion(out var version);
			FMOD_CHECK(result);
			if (version < FMOD.VERSION.number) {
				MessageBox.Show($"Error!  You are using an old version of FMOD {version:X}.  This program requires {FMOD.VERSION.number:X}.");
				Application.Exit();
			}

			result = system.init(2, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = system.getMasterSoundGroup(out masterSoundGroup);
			if (FMOD_CHECK(result)) {
				return;
			}

			result = masterSoundGroup.setVolume(FMODVolume);
			if (FMOD_CHECK(result)) {
				return;
			}
		}

		private void FMODreset() {
			ui_tabRight_page0_FMODtimer.Stop();
			ui_tabRight_page0_FMODprogressBar.Value = 0;
			ui_tabRight_page0_FMODtimerLabel.Text = "00:00.00";
			ui_tabRight_page0_FMODdurationLabel.Text = "00:00.00";

			//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Stopped";
			ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
			ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
			ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

			ui_tabRight_page0_FMODinfoLabel.Text = "";

			if (sound != null && sound.isValid()) {
				var result = sound.release();
				FMOD_CHECK(result);
				sound = null;
			}
		}

		private bool FMOD_CHECK(FMOD.RESULT result) {
			if (result != FMOD.RESULT.OK) {
				FMODreset();
				StatusStripUpdate($"FMOD error! {result} - {FMOD.Error.String(result)}");
				return true;
			}
			return false;
		}

		private void ui_tabRight_page0_FMODplayButton_Click(object sender, EventArgs e) {
			if (sound != null && channel != null) {
				ui_tabRight_page0_FMODtimer.Start();
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.stop();
					if (FMOD_CHECK(result)) {
						return;
					}

					result = system.playSound(sound, null, false, out channel);
					if (FMOD_CHECK(result)) {
						return;
					}

					//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
					ui_tabRight_page0_FMODpauseButton.Visible = true;
					ui_tabRight_page0_FMODresumeButton.Visible = false;
				}
				else {
					result = system.playSound(sound, null, false, out channel);
					if (FMOD_CHECK(result)) {
						return;
					}
					//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Playing";
					ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
					ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
					ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

					if (ui_tabRight_page0_FMODprogressBar.Value > 0) {
						uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;

						result = channel.setPosition(newms, FMOD.TIMEUNIT.MS);
						if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
							if (FMOD_CHECK(result)) {
								return;
							}
						}

					}
				}
			}
		}

		private void ui_tabRight_page0_FMODpauseButton_Click(object sender, EventArgs e) {
			if (sound != null && channel != null) {
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.getPaused(out var paused);
					if (FMOD_CHECK(result)) {
						return;
					}
					result = channel.setPaused(!paused);
					if (FMOD_CHECK(result)) {
						return;
					}

					if (paused) {
						//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Playing";
						//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
						ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
						ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
						ui_tabRight_page0_FMODpauseButton.Visible = true;
						ui_tabRight_page0_FMODresumeButton.Visible = false;

						ui_tabRight_page0_FMODtimer.Start();
					}
					else {
						//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Paused";
						//ui_tabRight_page0_FMODpauseButton.Text = "Resume";
						ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
						ui_tabRight_page0_FMODstatusLabel_Paused.Visible = true;
						ui_tabRight_page0_FMODpauseButton.Visible = false;
						ui_tabRight_page0_FMODresumeButton.Visible = true;

						ui_tabRight_page0_FMODtimer.Stop();
					}
				}
			}
		}

		private void ui_tabRight_page0_FMODstopButton_Click(object sender, EventArgs e) {
			if (channel != null) {
				var result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					result = channel.stop();
					if (FMOD_CHECK(result)) {
						return;
					}
					//channel = null;
					//don't FMODreset, it will nullify the sound
					ui_tabRight_page0_FMODtimer.Stop();
					ui_tabRight_page0_FMODprogressBar.Value = 0;
					ui_tabRight_page0_FMODtimerLabel.Text = "00:00.00";
					//FMODdurationLabel.Text = "0:0.0";

					//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = "Stopped";
					ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
					ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
					ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;

					//ui_tabRight_page0_FMODpauseButton.Text = "Pause";
					ui_tabRight_page0_FMODpauseButton.Visible = true;
					ui_tabRight_page0_FMODresumeButton.Visible = false;
				}
			}
		}

		private void ui_tabRight_page0_FMODloopButton_CheckedChanged(object sender, EventArgs e) {
			FMOD.RESULT result;

			loopMode = ui_tabRight_page0_FMODloopButton.Checked ? FMOD.MODE.LOOP_NORMAL : FMOD.MODE.LOOP_OFF;

			if (sound != null) {
				result = sound.setMode(loopMode);
				if (FMOD_CHECK(result)) {
					return;
				}
			}

			if (channel != null) {
				result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				result = channel.getPaused(out var paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing || paused) {
					result = channel.setMode(loopMode);
					if (FMOD_CHECK(result)) {
						return;
					}
				}
			}
		}

		private void ui_tabRight_page0_FMODvolumeBar_ValueChanged(object sender, EventArgs e) {
			FMODVolume = Convert.ToSingle(ui_tabRight_page0_FMODvolumeBar.Value) / 10;

			var result = masterSoundGroup.setVolume(FMODVolume);
			if (FMOD_CHECK(result)) {
				return;
			}
		}

		private void ui_tabRight_page0_FMODprogressBar_Scroll(object sender, EventArgs e) {
			if (channel != null) {
				uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;
				//ui_tabRight_page0_FMODtimerLabel.Text = $"{newms / 1000 / 60}:{newms / 1000 % 60}.{newms / 10 % 100}";
				//FMODdurationLabel.Text = $"{FMODlenms / 1000 / 60}:{FMODlenms / 1000 % 60}.{FMODlenms / 10 % 100}";
				ui_tabRight_page0_FMODtimerLabel.Text = String.Format("{0:D2}:{1:D2}.{2:D2}", newms / 1000 / 60, newms / 1000 % 60, newms / 10 % 100);
			}
		}

		private void ui_tabRight_page0_FMODprogressBar_MouseDown(object sender, MouseEventArgs e) {
			ui_tabRight_page0_FMODtimer.Stop();
		}

		private void ui_tabRight_page0_FMODprogressBar_MouseUp(object sender, MouseEventArgs e) {
			if (channel != null) {
				uint newms = FMODlenms / 1000 * (uint)ui_tabRight_page0_FMODprogressBar.Value;

				var result = channel.setPosition(newms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}


				result = channel.isPlaying(out var playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					if (FMOD_CHECK(result)) {
						return;
					}
				}

				if (playing) {
					ui_tabRight_page0_FMODtimer.Start();
				}
			}
		}

		private void ui_tabRight_page0_FMODtimer_Tick(object sender, EventArgs e) {
			uint ms = 0;
			bool playing = false;
			bool paused = false;

			if (channel != null) {
				var result = channel.getPosition(out ms, FMOD.TIMEUNIT.MS);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}

				result = channel.isPlaying(out playing);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}

				result = channel.getPaused(out paused);
				if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE)) {
					FMOD_CHECK(result);
				}
			}

			//ui_tabRight_page0_FMODtimerLabel.Text = $"{ms / 1000 / 60}:{ms / 1000 % 60}.{ms / 10 % 100}";
			//FMODdurationLabel.Text = $"{FMODlenms / 1000 / 60}:{FMODlenms / 1000 % 60}.{FMODlenms / 10 % 100}";
			ui_tabRight_page0_FMODtimerLabel.Text = String.Format("{0:D2}:{1:D2}.{2:D2}", ms / 1000 / 60, ms / 1000 % 60, ms / 10 % 100);
			ui_tabRight_page0_FMODprogressBar.Value = (int)(ms * 1000 / FMODlenms);

			//ui_tabRight_page0_FMODstatusLabel_Stopped.Text = paused ? "Paused " : playing ? "Playing" : "Stopped";
			if (paused) {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = true;
			}
			else if (playing) {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = true;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
			}
			else {
				ui_tabRight_page0_FMODstatusLabel_Stopped.Visible = true;
				ui_tabRight_page0_FMODstatusLabel_Playing.Visible = false;
				ui_tabRight_page0_FMODstatusLabel_Paused.Visible = false;
			}

			if (system != null && channel != null) {
				system.update();
			}
		}
		#endregion FMOD_Controller

		#region GLControl
		private bool glControlLoaded;
		private int mdx, mdy;
		private bool lmdown, rmdown;
		private int pgmID, pgmColorID, pgmBlackID;
		private int attributeVertexPosition;
		private int attributeNormalDirection;
		private int attributeVertexColor;
		private int uniformModelMatrix;
		private int uniformViewMatrix;
		private int uniformProjMatrix;
		private int vao;
		private Vector3[] vertexData;
		private Vector3[] normalData;
		private Vector3[] normal2Data;
		private Vector4[] colorData;
		private Matrix4 modelMatrixData;
		private Matrix4 viewMatrixData;
		private Matrix4 projMatrixData;
		private Matrix4 viewRotMatrix;

		private float m_modelScale;
		private Vector3 m_modelRot;
		private Vector3 m_cameraPos;
		private Vector3 m_cameraRot;
		private float m_cameraFOV;
		private float m_defaultCameraDis;

		private int[] indiceData;
		private int wireFrameMode = 2;
		private int shadeMode;
		private int normalMode;

		private void GLInit() {
			m_modelScale = 1.0f;
			m_modelRot = Vector3.Zero;
			m_cameraPos = Vector3.Zero;
			m_cameraRot = Vector3.Zero;
			m_cameraFOV = (float)Math.PI / 4.0f;
		}

		private void OpenTK_Init() {
			GL_ChangeSize(ui_tabRight_page0_glPreview.Size);
			GL.ClearColor(System.Drawing.Color.CadetBlue);
			pgmID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmID, out int vsID);
			GL_LoadShader("shader_fs", ShaderType.FragmentShader, pgmID, out int fsID);
			GL.LinkProgram(pgmID);

			pgmColorID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmColorID, out vsID);
			GL_LoadShader("shader_fsColor", ShaderType.FragmentShader, pgmColorID, out fsID);
			GL.LinkProgram(pgmColorID);

			pgmBlackID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmBlackID, out vsID);
			GL_LoadShader("shader_fsBlack", ShaderType.FragmentShader, pgmBlackID, out fsID);
			GL.LinkProgram(pgmBlackID);

			attributeVertexPosition = GL.GetAttribLocation(pgmID, "vertexPosition");
			attributeNormalDirection = GL.GetAttribLocation(pgmID, "normalDirection");
			attributeVertexColor = GL.GetAttribLocation(pgmColorID, "vertexColor");
			uniformModelMatrix = GL.GetUniformLocation(pgmID, "modelMatrix");
			uniformViewMatrix = GL.GetUniformLocation(pgmID, "viewMatrix");
			uniformProjMatrix = GL.GetUniformLocation(pgmID, "projMatrix");
		}

		private static void GL_LoadShader(string filename, ShaderType type, int program, out int address) {
			address = GL.CreateShader(type);
			var str = (string)Properties.Resources.ResourceManager.GetObject(filename);
			GL.ShaderSource(address, str);
			GL.CompileShader(address);
			GL.AttachShader(program, address);
			GL.DeleteShader(address);
		}

		private static void GL_CreateVBO(out int vboAddress, Vector3[] data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboAddress);
			GL.BufferData(BufferTarget.ArrayBuffer,
									(IntPtr)(data.Length * Vector3.SizeInBytes),
									data,
									BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(address, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(address);
		}

		private static void GL_CreateVBO(out int vboAddress, Vector4[] data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboAddress);
			GL.BufferData(BufferTarget.ArrayBuffer,
									(IntPtr)(data.Length * Vector4.SizeInBytes),
									data,
									BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(address, 4, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(address);
		}

		private static void GL_CreateVBO(out int vboAddress, Matrix4 data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.UniformMatrix4(address, false, ref data);
		}

		private static void GL_CreateEBO(out int address, int[] data) {
			GL.GenBuffers(1, out address);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, address);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
							(IntPtr)(data.Length * sizeof(int)),
							data,
							BufferUsageHint.StaticDraw);
		}

		private void GL_CreateVAO() {
			GL.DeleteVertexArray(vao);
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			GL_CreateVBO(out var vboPositions, vertexData, attributeVertexPosition);
			if (normalMode == 0) {
				GL_CreateVBO(out var vboNormals, normal2Data, attributeNormalDirection);
			}
			else {
				if (normalData != null)
					GL_CreateVBO(out var vboNormals, normalData, attributeNormalDirection);
			}
			GL_CreateVBO(out var vboColors, colorData, attributeVertexColor);
			GL_CreateVBO(out var vboModelMatrix, modelMatrixData, uniformModelMatrix);
			GL_CreateVBO(out var vboViewMatrix, viewMatrixData, uniformViewMatrix);
			GL_CreateVBO(out var vboProjMatrix, projMatrixData, uniformProjMatrix);
			GL_CreateEBO(out var eboElements, indiceData);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		private void GL_ChangeSize(Size size) {
			GL.Viewport(0, 0, size.Width, size.Height);
			GL_updateProjMatrix(size);
		}

		private void GL_InitMatrices() {
			m_cameraPos = new Vector3(0.0f, 0.0f, 64.0f);
			m_cameraRot = Vector3.Zero;
			m_modelRot = Vector3.Zero;
			m_modelScale = 1.0f;
			m_cameraFOV = (float)Math.PI / 4.0f;
			GL_updateModelMatrix();
			GL_updateViewMatrix();
		}

		private void GL_updateModelMatrix() {
			var modelRotMatrix =
				Matrix4.CreateRotationZ(m_modelRot.Z) *
				Matrix4.CreateRotationX(m_modelRot.X) *
				Matrix4.CreateRotationY(m_modelRot.Y);
			modelMatrixData = Matrix4.CreateScale(m_modelScale * 64.0f / m_defaultCameraDis) * modelRotMatrix;
		}

		private void GL_updateViewMatrix() {
			viewRotMatrix =
				Matrix4.CreateRotationY(m_cameraRot.Y) *
				Matrix4.CreateRotationX(m_cameraRot.X) *
				Matrix4.CreateRotationZ(m_cameraRot.Z);
			viewMatrixData = Matrix4.CreateTranslation(-m_cameraPos) * viewRotMatrix;
		}

		private void GL_updateProjMatrix(Size size) {
			float k = 1.0f * size.Width / size.Height;
			if (m_cameraFOV > 175.0f) {
				m_cameraFOV = 175.0f;
			}
			else if (m_cameraFOV < 1.0f) {
				m_cameraFOV = 1.0f;
			}
			Matrix4.CreatePerspectiveFieldOfView(m_cameraFOV, k, 0.25f, 256.0f, out projMatrixData);
		}

		private void ui_tabRight_page0_glPreview_Load(object sender, EventArgs e) {
			OpenTK_Init();
			glControlLoaded = true;
			//MessageBox.Show("Load");
		}

		private void ui_tabRight_page0_glPreview_Paint(object sender, PaintEventArgs e) {
			ui_tabRight_page0_glPreview.MakeCurrent();
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			//GL.Enable(EnableCap.LineSmooth);
			//GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			//GL.Enable(EnableCap.PolygonSmooth);
			//GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

			GL.BindVertexArray(vao);
			switch (wireFrameMode) {
			case 0:
				GL.UseProgram(shadeMode == 0 ? pgmID : pgmColorID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			case 1:
				GL.UseProgram(pgmBlackID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			case 2:
				GL.Enable(EnableCap.PolygonOffsetFill);
				GL.PolygonOffset(1, 1);
				GL.UseProgram(shadeMode == 0 ? pgmID : pgmColorID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				GL.Disable(EnableCap.PolygonOffsetFill);
				GL.UseProgram(pgmBlackID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			}

			GL.BindVertexArray(0);
			GL.Flush();
			ui_tabRight_page0_glPreview.SwapBuffers();
		}

		private void ui_tabRight_page0_glPreview_MouseWheel(object sender, MouseEventArgs e) {
			if (ui_tabRight_page0_glPreview.Visible) {
				Vector4 front = new Vector4(0, 0, -1, 0);
				front = front * viewRotMatrix.Inverted();

				if ((ModifierKeys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control)
					m_cameraPos += front.Xyz * e.Delta / 50.0f;
				else
					m_cameraPos += front.Xyz * e.Delta / 200.0f;

				GL_updateViewMatrix();

				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

		private void ui_tabRight_page0_glPreview_MouseDown(object sender, MouseEventArgs e) {
			mdx = e.X;
			mdy = e.Y;
			if (e.Button == MouseButtons.Left) {
				lmdown = true;
			}
			if (e.Button == MouseButtons.Right) {
				rmdown = true;
			}
		}

		private void ui_tabRight_page0_glPreview_MouseMove(object sender, MouseEventArgs e) {
			if (lmdown || rmdown) {
				float dx = mdx - e.X;
				float dy = mdy - e.Y;
				mdx = e.X;
				mdy = e.Y;
				if (lmdown) {
					m_modelRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_updateModelMatrix();
				}
				if (rmdown) {
					m_cameraRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_updateViewMatrix();
				}
				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

		private void ui_tabRight_page0_glPreview_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				lmdown = false;
			}
			if (e.Button == MouseButtons.Right) {
				rmdown = false;
			}
		}
		#endregion GLControl

		#region OHMS
		private bool GetANewFolder(string iDir, out string oDir) {
			if (m_studio.assetsManager.m_lastOpenPaths.Length == 1) {
				oDir = Path.Combine(iDir, Path.GetFileName(m_studio.assetsManager.m_lastOpenPaths[0]));
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
			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
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
						toExportAssets = m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = m_studio.visibleAssets;
						break;
					}
					m_studio.ExportAssetsStructured(outdir, toExportAssets, listType);
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsArknights(ExportArknightsFilter type, ExportListType listType) {
			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
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
						StudioCore.ExportAssetsArknightsScene(outdir, m_studio.exportableAssets, listType);
						break;
					case ExportArknightsFilter.CharArt:
						StudioCore.ExportAssetsArknightsCharart(outdir, m_studio.exportableAssets);
						break;
					}
				}
			}
			else {
				StatusStripUpdate(Properties.Strings.Export_Nothing);
			}
		}

		private void ExportAssetsSprites(ExportFilter type) {
			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
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
						toExportAssets = m_studio.exportableAssets.Where(x => x.Type == ClassIDType.Sprite).ToList();
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets().Where(x => x.Type == ClassIDType.Sprite).ToList();
						break;
					case ExportFilter.Filtered:
						toExportAssets = m_studio.visibleAssets.Where(x => x.Type == ClassIDType.Sprite).ToList();
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
			if (m_studio.exportableAssets.Count > 0) {
				var saveFolderDialog = new OpenFolderDialog();
				saveFolderDialog.InitialFolder = Properties.SettingsOHMS.Default.ohmsLastFolder;
				if (saveFolderDialog.ShowDialog(this) == DialogResult.OK) {
					ui_tabRight_page0_FMODtimer.Stop();
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
						toExportAssets = m_studio.exportableAssets;
						break;
					case ExportFilter.Selected:
						toExportAssets = GetSelectedAssets();
						break;
					case ExportFilter.Filtered:
						toExportAssets = m_studio.visibleAssets;
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

		private void tESTToolStripMenuItem_Click(object sender, EventArgs e) {
			var test = new Studio_Special_Arknights();
			test.ShowDialog(this);
		}
	}
}
