using System.Windows.Forms;

namespace AssetStudioGUI {
	partial class AssetStudioGUIForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetStudioGUIForm));
			ui_statusStrip0_down = new StatusStrip();
			ui_statusLabel0_down = new ToolStripStatusLabel();
			ui_menuStrip0_up = new MenuStrip();
			ui_menuFile = new ToolStripMenuItem();
			ui_menuFile_loadFile = new ToolStripMenuItem();
			ui_menuFile_loadFolder = new ToolStripMenuItem();
			ui_menuFile_separator0 = new ToolStripSeparator();
			ui_menuFile_extractFile = new ToolStripMenuItem();
			ui_menuFile_extractFolder = new ToolStripMenuItem();
			ui_menuFile_separator1 = new ToolStripSeparator();
			ui_menuFile_clear = new ToolStripMenuItem();
			ui_menuOptions = new ToolStripMenuItem();
			ui_menuOptions_language = new ToolStripMenuItem();
			ui_menuOptions_exportOptions = new ToolStripMenuItem();
			ui_menuOptions_separator0 = new ToolStripSeparator();
			ui_menuOptions_displayAllAssets = new ToolStripMenuItem();
			ui_menuOptions_enablePreview = new ToolStripMenuItem();
			ui_menuOptions_displayInfo = new ToolStripMenuItem();
			ui_menuOptions_separator1 = new ToolStripSeparator();
			ui_menuOptions_specifyUnityVersion = new ToolStripMenuItem();
			ui_menuOptions_specifyUnityVersion_specifyUnityVersion = new ToolStripTextBox();
			ui_menuFilter = new ToolStripMenuItem();
			ui_menuFilter_0_all = new ToolStripMenuItem();
			ui_menuModel = new ToolStripMenuItem();
			ui_menuModel_exportAllObjectsSplit = new ToolStripMenuItem();
			ui_menuModel_exportSelectedObjectsSplit = new ToolStripMenuItem();
			ui_menuModel_exportSelectedObjectsWithAnimationClip = new ToolStripMenuItem();
			ui_menuModel_separator0 = new ToolStripSeparator();
			ui_menuModel_exportSelectedObjectsMerge = new ToolStripMenuItem();
			ui_menuModel_exportSelectedObjectsMergeWithAnimationClips = new ToolStripMenuItem();
			ui_menuExport = new ToolStripMenuItem();
			ui_menuExport_allAssets = new ToolStripMenuItem();
			ui_menuExport_selectedAssets = new ToolStripMenuItem();
			ui_menuExport_filteredAssets = new ToolStripMenuItem();
			ui_menuExport_separator0 = new ToolStripSeparator();
			ui_menuExport_exportAnimatorAndSelectedAnimationClips = new ToolStripMenuItem();
			ui_menuExport_separator1 = new ToolStripSeparator();
			ui_menuExport_Raw = new ToolStripMenuItem();
			ui_menuExport_Raw_allAssets = new ToolStripMenuItem();
			ui_menuExport_Raw_slectedAssets = new ToolStripMenuItem();
			ui_menuExport_Raw_filteredAssets = new ToolStripMenuItem();
			ui_menuExport_Dump = new ToolStripMenuItem();
			ui_menuExport_Dump_allAssets = new ToolStripMenuItem();
			ui_menuExport_Dump_selectedAssets = new ToolStripMenuItem();
			ui_menuExport_Dump_filteredAssets = new ToolStripMenuItem();
			ui_menuExport_dumpJson = new ToolStripMenuItem();
			ui_menuExport_dumpJson_allAssets = new ToolStripMenuItem();
			ui_menuExport_dumpJson_selectedAssets = new ToolStripMenuItem();
			ui_menuExport_dumpJson_filteredAssets = new ToolStripMenuItem();
			ui_menuExport_separator2 = new ToolStripSeparator();
			ui_menuExport_assetsListToXml = new ToolStripMenuItem();
			ui_menuExport_assetsListToXml_allAssets = new ToolStripMenuItem();
			ui_menuExport_assetsListToXml_selectedAssets = new ToolStripMenuItem();
			ui_menuExport_assetsListToXml_filteredAssets = new ToolStripMenuItem();
			ui_menuExport_assetListToJson = new ToolStripMenuItem();
			ui_menuExport_assetListToJson_allAssets = new ToolStripMenuItem();
			ui_menuExport_assetListToJson_selectedAssets = new ToolStripMenuItem();
			ui_menuExport_assetListToJson_filteredAssets = new ToolStripMenuItem();
			ui_menuDebug = new ToolStripMenuItem();
			ui_menuDebug_showErrorMessage = new ToolStripMenuItem();
			ui_menuDebug_exportClassStructures = new ToolStripMenuItem();
			ui_TEST_ToolStripMenuItem = new ToolStripMenuItem();
			ui_menuOhmsExport = new ToolStripMenuItem();
			ui_menuOhmsExport_createANewFolder = new ToolStripMenuItem();
			ui_menuOhmsExport_separator0 = new ToolStripSeparator();
			ui_menuOhmsExport_allAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_selectedAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_displayedAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_separator1 = new ToolStripSeparator();
			ui_menuOhmsExport_structuredJsonList = new ToolStripMenuItem();
			ui_menuOhmsExport_structuredJsonList_allAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_structuredJsonList_selectedAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_structuredJsonList_displayedAssets = new ToolStripMenuItem();
			ui_menuOhmsExport_structuredXmlList = new ToolStripMenuItem();
			ui_menuOhmsExport_StructXml_All = new ToolStripMenuItem();
			ui_menuOhmsExport_StructXml_selected = new ToolStripMenuItem();
			ui_menuOhmsExport_StructXml_filtered = new ToolStripMenuItem();
			ui_menuOhmsExport_separator2 = new ToolStripSeparator();
			ui_menuOhmsExport_arknights = new ToolStripMenuItem();
			tESTToolStripMenuItem = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			ui_menuOhmsExport_arknights_sceneBundle = new ToolStripMenuItem();
			ui_menuOhmsExport_arknights_charartBundle = new ToolStripMenuItem();
			previewsToolStripMenuItem = new ToolStripMenuItem();
			ui_openFileDialog0 = new OpenFileDialog();
			ui_contextMenuStrip0 = new ContextMenuStrip(components);
			ui_conMenu_copyText = new ToolStripMenuItem();
			ui_conMenu_exportSelectedAssets = new ToolStripMenuItem();
			ui_conMenu_exportAnimatorAndSelectedAnimationClips = new ToolStripMenuItem();
			ui_conMenu_goToSceneHierarchy = new ToolStripMenuItem();
			ui_conMenu_showOriginalFile = new ToolStripMenuItem();
			ui_conMenu_separator0 = new ToolStripSeparator();
			ui_conMenu_OHMS_ExportSelected = new ToolStripMenuItem();
			ui_conMenu_OHMS_ExportSelected_Struct = new ToolStripMenuItem();
			ui_conMenu_OHMS_ExportSelected_Struct_JSON = new ToolStripMenuItem();
			ui_conMenu_OHMS_ExportSelected_Struct_XML = new ToolStripMenuItem();
			panel1 = new Panel();
			ui_tabLeft_tab = new TabControl();
			ui_tabLeft_page0 = new TabPage();
			ui_tabLeft_page0_treeView = new GOHierarchy();
			ui_tabLeft_page0_treeSearch = new TextBox();
			ui_tabLeft_page1 = new TabPage();
			ui_tabLeft_page1_listView = new ListView();
			ui_tabLeft_page1_listView_columnHeader_Name = new ColumnHeader();
			ui_tabLeft_page1_listView_columnHeader_Container = new ColumnHeader();
			ui_tabLeft_page1_listView_columnHeader_Type = new ColumnHeader();
			ui_tabLeft_page1_listView_columnHeader_PathID = new ColumnHeader();
			ui_tabLeft_page1_listView_columnHeader_Size = new ColumnHeader();
			ui_tabLeft_page1_listSearch = new TextBox();
			ui_tabLeft_page2 = new TabPage();
			ui_tabLeft_page2_classesListView = new ListView();
			ui_tabLeft_page2_listView_columnHeader_Name = new ColumnHeader();
			ui_tabLeft_page2_listView_columnHeader_ID = new ColumnHeader();
			ui_progressBar0_down = new ProgressBar();
			splitter1 = new Splitter();
			ui_statusStrip0_down.SuspendLayout();
			ui_menuStrip0_up.SuspendLayout();
			ui_contextMenuStrip0.SuspendLayout();
			panel1.SuspendLayout();
			ui_tabLeft_tab.SuspendLayout();
			ui_tabLeft_page0.SuspendLayout();
			ui_tabLeft_page1.SuspendLayout();
			ui_tabLeft_page2.SuspendLayout();
			SuspendLayout();
			// 
			// ui_statusStrip0_down
			// 
			resources.ApplyResources(ui_statusStrip0_down, "ui_statusStrip0_down");
			ui_statusStrip0_down.Items.AddRange(new ToolStripItem[] { ui_statusLabel0_down });
			ui_statusStrip0_down.Name = "ui_statusStrip0_down";
			// 
			// ui_statusLabel0_down
			// 
			resources.ApplyResources(ui_statusLabel0_down, "ui_statusLabel0_down");
			ui_statusLabel0_down.DisplayStyle = ToolStripItemDisplayStyle.Text;
			ui_statusLabel0_down.Name = "ui_statusLabel0_down";
			ui_statusLabel0_down.Spring = true;
			// 
			// ui_menuStrip0_up
			// 
			resources.ApplyResources(ui_menuStrip0_up, "ui_menuStrip0_up");
			ui_menuStrip0_up.Items.AddRange(new ToolStripItem[] { ui_menuFile, ui_menuOptions, ui_menuFilter, ui_menuModel, ui_menuExport, ui_menuDebug, ui_menuOhmsExport, previewsToolStripMenuItem });
			ui_menuStrip0_up.MdiWindowListItem = previewsToolStripMenuItem;
			ui_menuStrip0_up.Name = "ui_menuStrip0_up";
			// 
			// ui_menuFile
			// 
			resources.ApplyResources(ui_menuFile, "ui_menuFile");
			ui_menuFile.DropDownItems.AddRange(new ToolStripItem[] { ui_menuFile_loadFile, ui_menuFile_loadFolder, ui_menuFile_separator0, ui_menuFile_extractFile, ui_menuFile_extractFolder, ui_menuFile_separator1, ui_menuFile_clear });
			ui_menuFile.Name = "ui_menuFile";
			// 
			// ui_menuFile_loadFile
			// 
			resources.ApplyResources(ui_menuFile_loadFile, "ui_menuFile_loadFile");
			ui_menuFile_loadFile.Name = "ui_menuFile_loadFile";
			ui_menuFile_loadFile.Click += ui_menuFile_loadFile_Click;
			// 
			// ui_menuFile_loadFolder
			// 
			resources.ApplyResources(ui_menuFile_loadFolder, "ui_menuFile_loadFolder");
			ui_menuFile_loadFolder.Name = "ui_menuFile_loadFolder";
			ui_menuFile_loadFolder.Click += ui_menuFile_loadFolder_Click;
			// 
			// ui_menuFile_separator0
			// 
			resources.ApplyResources(ui_menuFile_separator0, "ui_menuFile_separator0");
			ui_menuFile_separator0.Name = "ui_menuFile_separator0";
			// 
			// ui_menuFile_extractFile
			// 
			resources.ApplyResources(ui_menuFile_extractFile, "ui_menuFile_extractFile");
			ui_menuFile_extractFile.Name = "ui_menuFile_extractFile";
			ui_menuFile_extractFile.Click += ui_menuFile_extractFile_Click;
			// 
			// ui_menuFile_extractFolder
			// 
			resources.ApplyResources(ui_menuFile_extractFolder, "ui_menuFile_extractFolder");
			ui_menuFile_extractFolder.Name = "ui_menuFile_extractFolder";
			ui_menuFile_extractFolder.Click += ui_menuFile_extractFolder_Click;
			// 
			// ui_menuFile_separator1
			// 
			resources.ApplyResources(ui_menuFile_separator1, "ui_menuFile_separator1");
			ui_menuFile_separator1.Name = "ui_menuFile_separator1";
			// 
			// ui_menuFile_clear
			// 
			resources.ApplyResources(ui_menuFile_clear, "ui_menuFile_clear");
			ui_menuFile_clear.Name = "ui_menuFile_clear";
			ui_menuFile_clear.Click += ui_menuFile_clear_Click;
			// 
			// ui_menuOptions
			// 
			resources.ApplyResources(ui_menuOptions, "ui_menuOptions");
			ui_menuOptions.DropDownItems.AddRange(new ToolStripItem[] { ui_menuOptions_language, ui_menuOptions_exportOptions, ui_menuOptions_separator0, ui_menuOptions_displayAllAssets, ui_menuOptions_enablePreview, ui_menuOptions_displayInfo, ui_menuOptions_separator1, ui_menuOptions_specifyUnityVersion });
			ui_menuOptions.Name = "ui_menuOptions";
			// 
			// ui_menuOptions_language
			// 
			resources.ApplyResources(ui_menuOptions_language, "ui_menuOptions_language");
			ui_menuOptions_language.Name = "ui_menuOptions_language";
			ui_menuOptions_language.Click += ui_menuOptions_language_Click;
			// 
			// ui_menuOptions_exportOptions
			// 
			resources.ApplyResources(ui_menuOptions_exportOptions, "ui_menuOptions_exportOptions");
			ui_menuOptions_exportOptions.Name = "ui_menuOptions_exportOptions";
			ui_menuOptions_exportOptions.Click += ui_menuOptions_exportOptions_Click;
			// 
			// ui_menuOptions_separator0
			// 
			resources.ApplyResources(ui_menuOptions_separator0, "ui_menuOptions_separator0");
			ui_menuOptions_separator0.Name = "ui_menuOptions_separator0";
			// 
			// ui_menuOptions_displayAllAssets
			// 
			resources.ApplyResources(ui_menuOptions_displayAllAssets, "ui_menuOptions_displayAllAssets");
			ui_menuOptions_displayAllAssets.CheckOnClick = true;
			ui_menuOptions_displayAllAssets.Name = "ui_menuOptions_displayAllAssets";
			ui_menuOptions_displayAllAssets.CheckedChanged += ui_menuOptions_displayAllAssets_CheckedChanged;
			// 
			// ui_menuOptions_enablePreview
			// 
			resources.ApplyResources(ui_menuOptions_enablePreview, "ui_menuOptions_enablePreview");
			ui_menuOptions_enablePreview.Checked = true;
			ui_menuOptions_enablePreview.CheckOnClick = true;
			ui_menuOptions_enablePreview.CheckState = CheckState.Checked;
			ui_menuOptions_enablePreview.Name = "ui_menuOptions_enablePreview";
			ui_menuOptions_enablePreview.CheckedChanged += ui_menuOptions_enablePreview_Check;
			// 
			// ui_menuOptions_displayInfo
			// 
			resources.ApplyResources(ui_menuOptions_displayInfo, "ui_menuOptions_displayInfo");
			ui_menuOptions_displayInfo.Checked = true;
			ui_menuOptions_displayInfo.CheckOnClick = true;
			ui_menuOptions_displayInfo.CheckState = CheckState.Checked;
			ui_menuOptions_displayInfo.Name = "ui_menuOptions_displayInfo";
			ui_menuOptions_displayInfo.CheckedChanged += ui_menuOptions_displayAssetInfo_Check;
			// 
			// ui_menuOptions_separator1
			// 
			resources.ApplyResources(ui_menuOptions_separator1, "ui_menuOptions_separator1");
			ui_menuOptions_separator1.Name = "ui_menuOptions_separator1";
			// 
			// ui_menuOptions_specifyUnityVersion
			// 
			resources.ApplyResources(ui_menuOptions_specifyUnityVersion, "ui_menuOptions_specifyUnityVersion");
			ui_menuOptions_specifyUnityVersion.DropDownItems.AddRange(new ToolStripItem[] { ui_menuOptions_specifyUnityVersion_specifyUnityVersion });
			ui_menuOptions_specifyUnityVersion.Name = "ui_menuOptions_specifyUnityVersion";
			// 
			// ui_menuOptions_specifyUnityVersion_specifyUnityVersion
			// 
			resources.ApplyResources(ui_menuOptions_specifyUnityVersion_specifyUnityVersion, "ui_menuOptions_specifyUnityVersion_specifyUnityVersion");
			ui_menuOptions_specifyUnityVersion_specifyUnityVersion.Name = "ui_menuOptions_specifyUnityVersion_specifyUnityVersion";
			// 
			// ui_menuFilter
			// 
			resources.ApplyResources(ui_menuFilter, "ui_menuFilter");
			ui_menuFilter.DropDownItems.AddRange(new ToolStripItem[] { ui_menuFilter_0_all });
			ui_menuFilter.Name = "ui_menuFilter";
			// 
			// ui_menuFilter_0_all
			// 
			resources.ApplyResources(ui_menuFilter_0_all, "ui_menuFilter_0_all");
			ui_menuFilter_0_all.Checked = true;
			ui_menuFilter_0_all.CheckOnClick = true;
			ui_menuFilter_0_all.CheckState = CheckState.Checked;
			ui_menuFilter_0_all.Name = "ui_menuFilter_0_all";
			ui_menuFilter_0_all.Click += ui_menuFilter_0_all_Click;
			// 
			// ui_menuModel
			// 
			resources.ApplyResources(ui_menuModel, "ui_menuModel");
			ui_menuModel.DropDownItems.AddRange(new ToolStripItem[] { ui_menuModel_exportAllObjectsSplit, ui_menuModel_exportSelectedObjectsSplit, ui_menuModel_exportSelectedObjectsWithAnimationClip, ui_menuModel_separator0, ui_menuModel_exportSelectedObjectsMerge, ui_menuModel_exportSelectedObjectsMergeWithAnimationClips });
			ui_menuModel.Name = "ui_menuModel";
			// 
			// ui_menuModel_exportAllObjectsSplit
			// 
			resources.ApplyResources(ui_menuModel_exportAllObjectsSplit, "ui_menuModel_exportAllObjectsSplit");
			ui_menuModel_exportAllObjectsSplit.Name = "ui_menuModel_exportAllObjectsSplit";
			ui_menuModel_exportAllObjectsSplit.Click += ui_menuModel_exportAllObjectsSplit_Click;
			// 
			// ui_menuModel_exportSelectedObjectsSplit
			// 
			resources.ApplyResources(ui_menuModel_exportSelectedObjectsSplit, "ui_menuModel_exportSelectedObjectsSplit");
			ui_menuModel_exportSelectedObjectsSplit.Name = "ui_menuModel_exportSelectedObjectsSplit";
			ui_menuModel_exportSelectedObjectsSplit.Click += ui_menuModel_exportSelectedObjectsSplit_Click;
			// 
			// ui_menuModel_exportSelectedObjectsWithAnimationClip
			// 
			resources.ApplyResources(ui_menuModel_exportSelectedObjectsWithAnimationClip, "ui_menuModel_exportSelectedObjectsWithAnimationClip");
			ui_menuModel_exportSelectedObjectsWithAnimationClip.Name = "ui_menuModel_exportSelectedObjectsWithAnimationClip";
			ui_menuModel_exportSelectedObjectsWithAnimationClip.Click += ui_menuModel_exportSelectedObjectsWithAnimationClip_Click;
			// 
			// ui_menuModel_separator0
			// 
			resources.ApplyResources(ui_menuModel_separator0, "ui_menuModel_separator0");
			ui_menuModel_separator0.Name = "ui_menuModel_separator0";
			// 
			// ui_menuModel_exportSelectedObjectsMerge
			// 
			resources.ApplyResources(ui_menuModel_exportSelectedObjectsMerge, "ui_menuModel_exportSelectedObjectsMerge");
			ui_menuModel_exportSelectedObjectsMerge.Name = "ui_menuModel_exportSelectedObjectsMerge";
			ui_menuModel_exportSelectedObjectsMerge.Click += ui_menuModel_exportSelectedObjectsMerge_Click;
			// 
			// ui_menuModel_exportSelectedObjectsMergeWithAnimationClips
			// 
			resources.ApplyResources(ui_menuModel_exportSelectedObjectsMergeWithAnimationClips, "ui_menuModel_exportSelectedObjectsMergeWithAnimationClips");
			ui_menuModel_exportSelectedObjectsMergeWithAnimationClips.Name = "ui_menuModel_exportSelectedObjectsMergeWithAnimationClips";
			ui_menuModel_exportSelectedObjectsMergeWithAnimationClips.Click += ui_menuModel_exportSelectedObjectsMergeWithAnimationClips_Click;
			// 
			// ui_menuExport
			// 
			resources.ApplyResources(ui_menuExport, "ui_menuExport");
			ui_menuExport.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_allAssets, ui_menuExport_selectedAssets, ui_menuExport_filteredAssets, ui_menuExport_separator0, ui_menuExport_exportAnimatorAndSelectedAnimationClips, ui_menuExport_separator1, ui_menuExport_Raw, ui_menuExport_Dump, ui_menuExport_dumpJson, ui_menuExport_separator2, ui_menuExport_assetsListToXml, ui_menuExport_assetListToJson });
			ui_menuExport.Name = "ui_menuExport";
			// 
			// ui_menuExport_allAssets
			// 
			resources.ApplyResources(ui_menuExport_allAssets, "ui_menuExport_allAssets");
			ui_menuExport_allAssets.Name = "ui_menuExport_allAssets";
			ui_menuExport_allAssets.Click += ui_menuExport_allAssets_Click;
			// 
			// ui_menuExport_selectedAssets
			// 
			resources.ApplyResources(ui_menuExport_selectedAssets, "ui_menuExport_selectedAssets");
			ui_menuExport_selectedAssets.Name = "ui_menuExport_selectedAssets";
			ui_menuExport_selectedAssets.Click += ui_menuExport_selectedAssets_Click;
			// 
			// ui_menuExport_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_filteredAssets, "ui_menuExport_filteredAssets");
			ui_menuExport_filteredAssets.Name = "ui_menuExport_filteredAssets";
			ui_menuExport_filteredAssets.Click += ui_menuExport_filteredAssets_Click;
			// 
			// ui_menuExport_separator0
			// 
			resources.ApplyResources(ui_menuExport_separator0, "ui_menuExport_separator0");
			ui_menuExport_separator0.Name = "ui_menuExport_separator0";
			// 
			// ui_menuExport_exportAnimatorAndSelectedAnimationClips
			// 
			resources.ApplyResources(ui_menuExport_exportAnimatorAndSelectedAnimationClips, "ui_menuExport_exportAnimatorAndSelectedAnimationClips");
			ui_menuExport_exportAnimatorAndSelectedAnimationClips.Name = "ui_menuExport_exportAnimatorAndSelectedAnimationClips";
			ui_menuExport_exportAnimatorAndSelectedAnimationClips.Click += ui_menuExport_exportAnimatorAndSelectedAnimationClips_Click;
			// 
			// ui_menuExport_separator1
			// 
			resources.ApplyResources(ui_menuExport_separator1, "ui_menuExport_separator1");
			ui_menuExport_separator1.Name = "ui_menuExport_separator1";
			// 
			// ui_menuExport_Raw
			// 
			resources.ApplyResources(ui_menuExport_Raw, "ui_menuExport_Raw");
			ui_menuExport_Raw.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_Raw_allAssets, ui_menuExport_Raw_slectedAssets, ui_menuExport_Raw_filteredAssets });
			ui_menuExport_Raw.Name = "ui_menuExport_Raw";
			// 
			// ui_menuExport_Raw_allAssets
			// 
			resources.ApplyResources(ui_menuExport_Raw_allAssets, "ui_menuExport_Raw_allAssets");
			ui_menuExport_Raw_allAssets.Name = "ui_menuExport_Raw_allAssets";
			ui_menuExport_Raw_allAssets.Click += ui_menuExport_Raw_allAssets_Click;
			// 
			// ui_menuExport_Raw_slectedAssets
			// 
			resources.ApplyResources(ui_menuExport_Raw_slectedAssets, "ui_menuExport_Raw_slectedAssets");
			ui_menuExport_Raw_slectedAssets.Name = "ui_menuExport_Raw_slectedAssets";
			ui_menuExport_Raw_slectedAssets.Click += ui_menuExport_Raw_selectedAssets_Click;
			// 
			// ui_menuExport_Raw_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_Raw_filteredAssets, "ui_menuExport_Raw_filteredAssets");
			ui_menuExport_Raw_filteredAssets.Name = "ui_menuExport_Raw_filteredAssets";
			ui_menuExport_Raw_filteredAssets.Click += ui_menuExport_Raw_filteredAssets_Click;
			// 
			// ui_menuExport_Dump
			// 
			resources.ApplyResources(ui_menuExport_Dump, "ui_menuExport_Dump");
			ui_menuExport_Dump.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_Dump_allAssets, ui_menuExport_Dump_selectedAssets, ui_menuExport_Dump_filteredAssets });
			ui_menuExport_Dump.Name = "ui_menuExport_Dump";
			// 
			// ui_menuExport_Dump_allAssets
			// 
			resources.ApplyResources(ui_menuExport_Dump_allAssets, "ui_menuExport_Dump_allAssets");
			ui_menuExport_Dump_allAssets.Name = "ui_menuExport_Dump_allAssets";
			ui_menuExport_Dump_allAssets.Click += ui_menuExport_Dump_allAssets_Click;
			// 
			// ui_menuExport_Dump_selectedAssets
			// 
			resources.ApplyResources(ui_menuExport_Dump_selectedAssets, "ui_menuExport_Dump_selectedAssets");
			ui_menuExport_Dump_selectedAssets.Name = "ui_menuExport_Dump_selectedAssets";
			ui_menuExport_Dump_selectedAssets.Click += ui_menuExport_Dump_selectedAssets_Click;
			// 
			// ui_menuExport_Dump_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_Dump_filteredAssets, "ui_menuExport_Dump_filteredAssets");
			ui_menuExport_Dump_filteredAssets.Name = "ui_menuExport_Dump_filteredAssets";
			ui_menuExport_Dump_filteredAssets.Click += ui_menuExport_Dump_filteredAssets_Click;
			// 
			// ui_menuExport_dumpJson
			// 
			resources.ApplyResources(ui_menuExport_dumpJson, "ui_menuExport_dumpJson");
			ui_menuExport_dumpJson.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_dumpJson_allAssets, ui_menuExport_dumpJson_selectedAssets, ui_menuExport_dumpJson_filteredAssets });
			ui_menuExport_dumpJson.Name = "ui_menuExport_dumpJson";
			// 
			// ui_menuExport_dumpJson_allAssets
			// 
			resources.ApplyResources(ui_menuExport_dumpJson_allAssets, "ui_menuExport_dumpJson_allAssets");
			ui_menuExport_dumpJson_allAssets.Name = "ui_menuExport_dumpJson_allAssets";
			ui_menuExport_dumpJson_allAssets.Click += ui_menuExport_dumpJson_allAssets_Click;
			// 
			// ui_menuExport_dumpJson_selectedAssets
			// 
			resources.ApplyResources(ui_menuExport_dumpJson_selectedAssets, "ui_menuExport_dumpJson_selectedAssets");
			ui_menuExport_dumpJson_selectedAssets.Name = "ui_menuExport_dumpJson_selectedAssets";
			ui_menuExport_dumpJson_selectedAssets.Click += ui_menuExport_dumpJson_selectedAssets_Click;
			// 
			// ui_menuExport_dumpJson_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_dumpJson_filteredAssets, "ui_menuExport_dumpJson_filteredAssets");
			ui_menuExport_dumpJson_filteredAssets.Name = "ui_menuExport_dumpJson_filteredAssets";
			ui_menuExport_dumpJson_filteredAssets.Click += ui_menuExport_dumpJson_filteredAssets_Click;
			// 
			// ui_menuExport_separator2
			// 
			resources.ApplyResources(ui_menuExport_separator2, "ui_menuExport_separator2");
			ui_menuExport_separator2.Name = "ui_menuExport_separator2";
			// 
			// ui_menuExport_assetsListToXml
			// 
			resources.ApplyResources(ui_menuExport_assetsListToXml, "ui_menuExport_assetsListToXml");
			ui_menuExport_assetsListToXml.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_assetsListToXml_allAssets, ui_menuExport_assetsListToXml_selectedAssets, ui_menuExport_assetsListToXml_filteredAssets });
			ui_menuExport_assetsListToXml.Name = "ui_menuExport_assetsListToXml";
			// 
			// ui_menuExport_assetsListToXml_allAssets
			// 
			resources.ApplyResources(ui_menuExport_assetsListToXml_allAssets, "ui_menuExport_assetsListToXml_allAssets");
			ui_menuExport_assetsListToXml_allAssets.Name = "ui_menuExport_assetsListToXml_allAssets";
			ui_menuExport_assetsListToXml_allAssets.Click += ui_menuExport_assetsListToXml_allAssets_Click;
			// 
			// ui_menuExport_assetsListToXml_selectedAssets
			// 
			resources.ApplyResources(ui_menuExport_assetsListToXml_selectedAssets, "ui_menuExport_assetsListToXml_selectedAssets");
			ui_menuExport_assetsListToXml_selectedAssets.Name = "ui_menuExport_assetsListToXml_selectedAssets";
			ui_menuExport_assetsListToXml_selectedAssets.Click += ui_menuExport_assetsListToXml_selectedAssets_Click;
			// 
			// ui_menuExport_assetsListToXml_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_assetsListToXml_filteredAssets, "ui_menuExport_assetsListToXml_filteredAssets");
			ui_menuExport_assetsListToXml_filteredAssets.Name = "ui_menuExport_assetsListToXml_filteredAssets";
			ui_menuExport_assetsListToXml_filteredAssets.Click += ui_menuExport_assetsListToXml_filteredAssets_Click;
			// 
			// ui_menuExport_assetListToJson
			// 
			resources.ApplyResources(ui_menuExport_assetListToJson, "ui_menuExport_assetListToJson");
			ui_menuExport_assetListToJson.DropDownItems.AddRange(new ToolStripItem[] { ui_menuExport_assetListToJson_allAssets, ui_menuExport_assetListToJson_selectedAssets, ui_menuExport_assetListToJson_filteredAssets });
			ui_menuExport_assetListToJson.Name = "ui_menuExport_assetListToJson";
			// 
			// ui_menuExport_assetListToJson_allAssets
			// 
			resources.ApplyResources(ui_menuExport_assetListToJson_allAssets, "ui_menuExport_assetListToJson_allAssets");
			ui_menuExport_assetListToJson_allAssets.Name = "ui_menuExport_assetListToJson_allAssets";
			ui_menuExport_assetListToJson_allAssets.Click += ui_menuExport_assetListToJson_allAssets_Click;
			// 
			// ui_menuExport_assetListToJson_selectedAssets
			// 
			resources.ApplyResources(ui_menuExport_assetListToJson_selectedAssets, "ui_menuExport_assetListToJson_selectedAssets");
			ui_menuExport_assetListToJson_selectedAssets.Name = "ui_menuExport_assetListToJson_selectedAssets";
			ui_menuExport_assetListToJson_selectedAssets.Click += ui_menuExport_assetListToJson_selectedAssets_Click;
			// 
			// ui_menuExport_assetListToJson_filteredAssets
			// 
			resources.ApplyResources(ui_menuExport_assetListToJson_filteredAssets, "ui_menuExport_assetListToJson_filteredAssets");
			ui_menuExport_assetListToJson_filteredAssets.Name = "ui_menuExport_assetListToJson_filteredAssets";
			ui_menuExport_assetListToJson_filteredAssets.Click += ui_menuExport_assetListToJson_filteredAssets_Click;
			// 
			// ui_menuDebug
			// 
			resources.ApplyResources(ui_menuDebug, "ui_menuDebug");
			ui_menuDebug.DropDownItems.AddRange(new ToolStripItem[] { ui_menuDebug_showErrorMessage, ui_menuDebug_exportClassStructures, ui_TEST_ToolStripMenuItem });
			ui_menuDebug.Name = "ui_menuDebug";
			// 
			// ui_menuDebug_showErrorMessage
			// 
			resources.ApplyResources(ui_menuDebug_showErrorMessage, "ui_menuDebug_showErrorMessage");
			ui_menuDebug_showErrorMessage.Checked = true;
			ui_menuDebug_showErrorMessage.CheckOnClick = true;
			ui_menuDebug_showErrorMessage.CheckState = CheckState.Checked;
			ui_menuDebug_showErrorMessage.Name = "ui_menuDebug_showErrorMessage";
			ui_menuDebug_showErrorMessage.Click += ui_menuDebug_showErrorMessage_Click;
			// 
			// ui_menuDebug_exportClassStructures
			// 
			resources.ApplyResources(ui_menuDebug_exportClassStructures, "ui_menuDebug_exportClassStructures");
			ui_menuDebug_exportClassStructures.Name = "ui_menuDebug_exportClassStructures";
			ui_menuDebug_exportClassStructures.Click += ui_menuDebug_exportClassStructures_Click;
			// 
			// ui_TEST_ToolStripMenuItem
			// 
			resources.ApplyResources(ui_TEST_ToolStripMenuItem, "ui_TEST_ToolStripMenuItem");
			ui_TEST_ToolStripMenuItem.Name = "ui_TEST_ToolStripMenuItem";
			ui_TEST_ToolStripMenuItem.Click += ui_TEST_ToolStripMenuItem_Click;
			// 
			// ui_menuOhmsExport
			// 
			resources.ApplyResources(ui_menuOhmsExport, "ui_menuOhmsExport");
			ui_menuOhmsExport.DropDownItems.AddRange(new ToolStripItem[] { ui_menuOhmsExport_createANewFolder, ui_menuOhmsExport_separator0, ui_menuOhmsExport_allAssets, ui_menuOhmsExport_selectedAssets, ui_menuOhmsExport_displayedAssets, ui_menuOhmsExport_separator1, ui_menuOhmsExport_structuredJsonList, ui_menuOhmsExport_structuredXmlList, ui_menuOhmsExport_separator2, ui_menuOhmsExport_arknights });
			ui_menuOhmsExport.Name = "ui_menuOhmsExport";
			// 
			// ui_menuOhmsExport_createANewFolder
			// 
			resources.ApplyResources(ui_menuOhmsExport_createANewFolder, "ui_menuOhmsExport_createANewFolder");
			ui_menuOhmsExport_createANewFolder.Checked = true;
			ui_menuOhmsExport_createANewFolder.CheckOnClick = true;
			ui_menuOhmsExport_createANewFolder.CheckState = CheckState.Checked;
			ui_menuOhmsExport_createANewFolder.ForeColor = System.Drawing.SystemColors.ControlText;
			ui_menuOhmsExport_createANewFolder.Name = "ui_menuOhmsExport_createANewFolder";
			ui_menuOhmsExport_createANewFolder.CheckedChanged += ui_menuOhmsExport_createANewFolder_CheckedChanged;
			// 
			// ui_menuOhmsExport_separator0
			// 
			resources.ApplyResources(ui_menuOhmsExport_separator0, "ui_menuOhmsExport_separator0");
			ui_menuOhmsExport_separator0.Name = "ui_menuOhmsExport_separator0";
			// 
			// ui_menuOhmsExport_allAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_allAssets, "ui_menuOhmsExport_allAssets");
			ui_menuOhmsExport_allAssets.Name = "ui_menuOhmsExport_allAssets";
			ui_menuOhmsExport_allAssets.Click += ui_menuOhmsExport_allAssets_Click;
			// 
			// ui_menuOhmsExport_selectedAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_selectedAssets, "ui_menuOhmsExport_selectedAssets");
			ui_menuOhmsExport_selectedAssets.Name = "ui_menuOhmsExport_selectedAssets";
			ui_menuOhmsExport_selectedAssets.Click += ui_menuOhmsExport_selectedAssets_Click;
			// 
			// ui_menuOhmsExport_displayedAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_displayedAssets, "ui_menuOhmsExport_displayedAssets");
			ui_menuOhmsExport_displayedAssets.Name = "ui_menuOhmsExport_displayedAssets";
			ui_menuOhmsExport_displayedAssets.Click += ui_menuOhmsExport_displayedAssets_Click;
			// 
			// ui_menuOhmsExport_separator1
			// 
			resources.ApplyResources(ui_menuOhmsExport_separator1, "ui_menuOhmsExport_separator1");
			ui_menuOhmsExport_separator1.Name = "ui_menuOhmsExport_separator1";
			// 
			// ui_menuOhmsExport_structuredJsonList
			// 
			resources.ApplyResources(ui_menuOhmsExport_structuredJsonList, "ui_menuOhmsExport_structuredJsonList");
			ui_menuOhmsExport_structuredJsonList.DropDownItems.AddRange(new ToolStripItem[] { ui_menuOhmsExport_structuredJsonList_allAssets, ui_menuOhmsExport_structuredJsonList_selectedAssets, ui_menuOhmsExport_structuredJsonList_displayedAssets });
			ui_menuOhmsExport_structuredJsonList.Name = "ui_menuOhmsExport_structuredJsonList";
			// 
			// ui_menuOhmsExport_structuredJsonList_allAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_structuredJsonList_allAssets, "ui_menuOhmsExport_structuredJsonList_allAssets");
			ui_menuOhmsExport_structuredJsonList_allAssets.Name = "ui_menuOhmsExport_structuredJsonList_allAssets";
			ui_menuOhmsExport_structuredJsonList_allAssets.Click += ui_menuOhmsExport_structuredJsonList_allAssets_Click;
			// 
			// ui_menuOhmsExport_structuredJsonList_selectedAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_structuredJsonList_selectedAssets, "ui_menuOhmsExport_structuredJsonList_selectedAssets");
			ui_menuOhmsExport_structuredJsonList_selectedAssets.Name = "ui_menuOhmsExport_structuredJsonList_selectedAssets";
			ui_menuOhmsExport_structuredJsonList_selectedAssets.Click += ui_menuOhmsExport_structuredJsonList_selectedAssets_Click;
			// 
			// ui_menuOhmsExport_structuredJsonList_displayedAssets
			// 
			resources.ApplyResources(ui_menuOhmsExport_structuredJsonList_displayedAssets, "ui_menuOhmsExport_structuredJsonList_displayedAssets");
			ui_menuOhmsExport_structuredJsonList_displayedAssets.Name = "ui_menuOhmsExport_structuredJsonList_displayedAssets";
			ui_menuOhmsExport_structuredJsonList_displayedAssets.Click += ui_menuOhmsExport_structuredJsonList_displayedAssets_Click;
			// 
			// ui_menuOhmsExport_structuredXmlList
			// 
			resources.ApplyResources(ui_menuOhmsExport_structuredXmlList, "ui_menuOhmsExport_structuredXmlList");
			ui_menuOhmsExport_structuredXmlList.DropDownItems.AddRange(new ToolStripItem[] { ui_menuOhmsExport_StructXml_All, ui_menuOhmsExport_StructXml_selected, ui_menuOhmsExport_StructXml_filtered });
			ui_menuOhmsExport_structuredXmlList.Name = "ui_menuOhmsExport_structuredXmlList";
			// 
			// ui_menuOhmsExport_StructXml_All
			// 
			resources.ApplyResources(ui_menuOhmsExport_StructXml_All, "ui_menuOhmsExport_StructXml_All");
			ui_menuOhmsExport_StructXml_All.Name = "ui_menuOhmsExport_StructXml_All";
			ui_menuOhmsExport_StructXml_All.Click += ui_menuOhmsExport_StructXml_All_Click;
			// 
			// ui_menuOhmsExport_StructXml_selected
			// 
			resources.ApplyResources(ui_menuOhmsExport_StructXml_selected, "ui_menuOhmsExport_StructXml_selected");
			ui_menuOhmsExport_StructXml_selected.Name = "ui_menuOhmsExport_StructXml_selected";
			ui_menuOhmsExport_StructXml_selected.Click += ui_menuOhmsExport_StructXml_selected_Click;
			// 
			// ui_menuOhmsExport_StructXml_filtered
			// 
			resources.ApplyResources(ui_menuOhmsExport_StructXml_filtered, "ui_menuOhmsExport_StructXml_filtered");
			ui_menuOhmsExport_StructXml_filtered.Name = "ui_menuOhmsExport_StructXml_filtered";
			ui_menuOhmsExport_StructXml_filtered.Click += ui_menuOhmsExport_StructXml_displayed_Click;
			// 
			// ui_menuOhmsExport_separator2
			// 
			resources.ApplyResources(ui_menuOhmsExport_separator2, "ui_menuOhmsExport_separator2");
			ui_menuOhmsExport_separator2.Name = "ui_menuOhmsExport_separator2";
			// 
			// ui_menuOhmsExport_arknights
			// 
			resources.ApplyResources(ui_menuOhmsExport_arknights, "ui_menuOhmsExport_arknights");
			ui_menuOhmsExport_arknights.DropDownItems.AddRange(new ToolStripItem[] { tESTToolStripMenuItem, toolStripSeparator1, ui_menuOhmsExport_arknights_sceneBundle, ui_menuOhmsExport_arknights_charartBundle });
			ui_menuOhmsExport_arknights.Name = "ui_menuOhmsExport_arknights";
			// 
			// tESTToolStripMenuItem
			// 
			resources.ApplyResources(tESTToolStripMenuItem, "tESTToolStripMenuItem");
			tESTToolStripMenuItem.Name = "tESTToolStripMenuItem";
			tESTToolStripMenuItem.Click += tESTToolStripMenuItem_Click;
			// 
			// toolStripSeparator1
			// 
			resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
			toolStripSeparator1.Name = "toolStripSeparator1";
			// 
			// ui_menuOhmsExport_arknights_sceneBundle
			// 
			resources.ApplyResources(ui_menuOhmsExport_arknights_sceneBundle, "ui_menuOhmsExport_arknights_sceneBundle");
			ui_menuOhmsExport_arknights_sceneBundle.Name = "ui_menuOhmsExport_arknights_sceneBundle";
			ui_menuOhmsExport_arknights_sceneBundle.Click += ui_menuOhmsExport_arknights_sceneBundle_Click;
			// 
			// ui_menuOhmsExport_arknights_charartBundle
			// 
			resources.ApplyResources(ui_menuOhmsExport_arknights_charartBundle, "ui_menuOhmsExport_arknights_charartBundle");
			ui_menuOhmsExport_arknights_charartBundle.Name = "ui_menuOhmsExport_arknights_charartBundle";
			ui_menuOhmsExport_arknights_charartBundle.Click += ui_menuOhmsExport_arknights_charartBundle_Click;
			// 
			// previewsToolStripMenuItem
			// 
			resources.ApplyResources(previewsToolStripMenuItem, "previewsToolStripMenuItem");
			previewsToolStripMenuItem.Name = "previewsToolStripMenuItem";
			// 
			// ui_openFileDialog0
			// 
			ui_openFileDialog0.AddExtension = false;
			resources.ApplyResources(ui_openFileDialog0, "ui_openFileDialog0");
			ui_openFileDialog0.Multiselect = true;
			ui_openFileDialog0.RestoreDirectory = true;
			// 
			// ui_contextMenuStrip0
			// 
			resources.ApplyResources(ui_contextMenuStrip0, "ui_contextMenuStrip0");
			ui_contextMenuStrip0.ImageScalingSize = new System.Drawing.Size(20, 20);
			ui_contextMenuStrip0.Items.AddRange(new ToolStripItem[] { ui_conMenu_copyText, ui_conMenu_exportSelectedAssets, ui_conMenu_exportAnimatorAndSelectedAnimationClips, ui_conMenu_goToSceneHierarchy, ui_conMenu_showOriginalFile, ui_conMenu_separator0, ui_conMenu_OHMS_ExportSelected, ui_conMenu_OHMS_ExportSelected_Struct });
			ui_contextMenuStrip0.Name = "contextMenuStrip1";
			// 
			// ui_conMenu_copyText
			// 
			resources.ApplyResources(ui_conMenu_copyText, "ui_conMenu_copyText");
			ui_conMenu_copyText.Name = "ui_conMenu_copyText";
			ui_conMenu_copyText.Click += Ui_conMenu_copyText_Click;
			// 
			// ui_conMenu_exportSelectedAssets
			// 
			resources.ApplyResources(ui_conMenu_exportSelectedAssets, "ui_conMenu_exportSelectedAssets");
			ui_conMenu_exportSelectedAssets.Name = "ui_conMenu_exportSelectedAssets";
			ui_conMenu_exportSelectedAssets.Click += Ui_conMenu_exportSelectedAssets_Click;
			// 
			// ui_conMenu_exportAnimatorAndSelectedAnimationClips
			// 
			resources.ApplyResources(ui_conMenu_exportAnimatorAndSelectedAnimationClips, "ui_conMenu_exportAnimatorAndSelectedAnimationClips");
			ui_conMenu_exportAnimatorAndSelectedAnimationClips.Name = "ui_conMenu_exportAnimatorAndSelectedAnimationClips";
			ui_conMenu_exportAnimatorAndSelectedAnimationClips.Click += Ui_conMenu_exportAnimatorAndSelectedAnimationClips_Click;
			// 
			// ui_conMenu_goToSceneHierarchy
			// 
			resources.ApplyResources(ui_conMenu_goToSceneHierarchy, "ui_conMenu_goToSceneHierarchy");
			ui_conMenu_goToSceneHierarchy.Name = "ui_conMenu_goToSceneHierarchy";
			ui_conMenu_goToSceneHierarchy.Click += Ui_conMenu_goToSceneHierarchy_Click;
			// 
			// ui_conMenu_showOriginalFile
			// 
			resources.ApplyResources(ui_conMenu_showOriginalFile, "ui_conMenu_showOriginalFile");
			ui_conMenu_showOriginalFile.Name = "ui_conMenu_showOriginalFile";
			ui_conMenu_showOriginalFile.Click += Ui_conMenu_showOriginalFile_Click;
			// 
			// ui_conMenu_separator0
			// 
			resources.ApplyResources(ui_conMenu_separator0, "ui_conMenu_separator0");
			ui_conMenu_separator0.Name = "ui_conMenu_separator0";
			// 
			// ui_conMenu_OHMS_ExportSelected
			// 
			resources.ApplyResources(ui_conMenu_OHMS_ExportSelected, "ui_conMenu_OHMS_ExportSelected");
			ui_conMenu_OHMS_ExportSelected.Name = "ui_conMenu_OHMS_ExportSelected";
			ui_conMenu_OHMS_ExportSelected.Click += Ui_conMenu_OHMS_ExportSelected_Click;
			// 
			// ui_conMenu_OHMS_ExportSelected_Struct
			// 
			resources.ApplyResources(ui_conMenu_OHMS_ExportSelected_Struct, "ui_conMenu_OHMS_ExportSelected_Struct");
			ui_conMenu_OHMS_ExportSelected_Struct.DropDownItems.AddRange(new ToolStripItem[] { ui_conMenu_OHMS_ExportSelected_Struct_JSON, ui_conMenu_OHMS_ExportSelected_Struct_XML });
			ui_conMenu_OHMS_ExportSelected_Struct.Name = "ui_conMenu_OHMS_ExportSelected_Struct";
			// 
			// ui_conMenu_OHMS_ExportSelected_Struct_JSON
			// 
			resources.ApplyResources(ui_conMenu_OHMS_ExportSelected_Struct_JSON, "ui_conMenu_OHMS_ExportSelected_Struct_JSON");
			ui_conMenu_OHMS_ExportSelected_Struct_JSON.Name = "ui_conMenu_OHMS_ExportSelected_Struct_JSON";
			ui_conMenu_OHMS_ExportSelected_Struct_JSON.Click += Ui_conMenu_OHMS_ExportSelected_Struct_JSON_Click;
			// 
			// ui_conMenu_OHMS_ExportSelected_Struct_XML
			// 
			resources.ApplyResources(ui_conMenu_OHMS_ExportSelected_Struct_XML, "ui_conMenu_OHMS_ExportSelected_Struct_XML");
			ui_conMenu_OHMS_ExportSelected_Struct_XML.Name = "ui_conMenu_OHMS_ExportSelected_Struct_XML";
			ui_conMenu_OHMS_ExportSelected_Struct_XML.Click += Ui_conMenu_OHMS_ExportSelected_Struct_XML_Click;
			// 
			// panel1
			// 
			resources.ApplyResources(panel1, "panel1");
			panel1.Controls.Add(ui_tabLeft_tab);
			panel1.Controls.Add(ui_progressBar0_down);
			panel1.Name = "panel1";
			// 
			// ui_tabLeft_tab
			// 
			resources.ApplyResources(ui_tabLeft_tab, "ui_tabLeft_tab");
			ui_tabLeft_tab.Controls.Add(ui_tabLeft_page0);
			ui_tabLeft_tab.Controls.Add(ui_tabLeft_page1);
			ui_tabLeft_tab.Controls.Add(ui_tabLeft_page2);
			ui_tabLeft_tab.Name = "ui_tabLeft_tab";
			ui_tabLeft_tab.SelectedIndex = 0;
			ui_tabLeft_tab.SizeMode = TabSizeMode.Fixed;
			ui_tabLeft_tab.Selected += ui_tabLeft_tab_Selected;
			// 
			// ui_tabLeft_page0
			// 
			resources.ApplyResources(ui_tabLeft_page0, "ui_tabLeft_page0");
			ui_tabLeft_page0.Controls.Add(ui_tabLeft_page0_treeView);
			ui_tabLeft_page0.Controls.Add(ui_tabLeft_page0_treeSearch);
			ui_tabLeft_page0.Name = "ui_tabLeft_page0";
			ui_tabLeft_page0.UseVisualStyleBackColor = true;
			// 
			// ui_tabLeft_page0_treeView
			// 
			resources.ApplyResources(ui_tabLeft_page0_treeView, "ui_tabLeft_page0_treeView");
			ui_tabLeft_page0_treeView.CheckBoxes = true;
			ui_tabLeft_page0_treeView.HideSelection = false;
			ui_tabLeft_page0_treeView.Name = "ui_tabLeft_page0_treeView";
			ui_tabLeft_page0_treeView.AfterCheck += ui_tabLeft_page0_treeView_AfterCheck;
			// 
			// ui_tabLeft_page0_treeSearch
			// 
			resources.ApplyResources(ui_tabLeft_page0_treeSearch, "ui_tabLeft_page0_treeSearch");
			ui_tabLeft_page0_treeSearch.ForeColor = System.Drawing.SystemColors.GrayText;
			ui_tabLeft_page0_treeSearch.Name = "ui_tabLeft_page0_treeSearch";
			ui_tabLeft_page0_treeSearch.TextChanged += ui_tabLeft_page0_treeSearch_TextChanged;
			ui_tabLeft_page0_treeSearch.KeyDown += ui_tabLeft_page0_treeSearch_KeyDown;
			// 
			// ui_tabLeft_page1
			// 
			resources.ApplyResources(ui_tabLeft_page1, "ui_tabLeft_page1");
			ui_tabLeft_page1.Controls.Add(ui_tabLeft_page1_listView);
			ui_tabLeft_page1.Controls.Add(ui_tabLeft_page1_listSearch);
			ui_tabLeft_page1.Name = "ui_tabLeft_page1";
			ui_tabLeft_page1.UseVisualStyleBackColor = true;
			// 
			// ui_tabLeft_page1_listView
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView, "ui_tabLeft_page1_listView");
			ui_tabLeft_page1_listView.Columns.AddRange(new ColumnHeader[] { ui_tabLeft_page1_listView_columnHeader_Name, ui_tabLeft_page1_listView_columnHeader_Container, ui_tabLeft_page1_listView_columnHeader_Type, ui_tabLeft_page1_listView_columnHeader_PathID, ui_tabLeft_page1_listView_columnHeader_Size });
			ui_tabLeft_page1_listView.FullRowSelect = true;
			ui_tabLeft_page1_listView.GridLines = true;
			ui_tabLeft_page1_listView.Name = "ui_tabLeft_page1_listView";
			ui_tabLeft_page1_listView.UseCompatibleStateImageBehavior = false;
			ui_tabLeft_page1_listView.View = View.Details;
			ui_tabLeft_page1_listView.VirtualMode = true;
			ui_tabLeft_page1_listView.ColumnClick += ui_tabLeft_page1_listView_ColumnClick;
			ui_tabLeft_page1_listView.ItemSelectionChanged += ui_tabLeft_page1_listView_ItemSelectionChanged;
			ui_tabLeft_page1_listView.RetrieveVirtualItem += ui_tabLeft_page1_listView_RetrieveVirtualItem;
			ui_tabLeft_page1_listView.MouseClick += ui_tabLeft_page1_listView_MouseClick;
			ui_tabLeft_page1_listView.MouseDoubleClick += Ui_tabLeft_page1_listView_MouseDoubleClick;
			// 
			// ui_tabLeft_page1_listView_columnHeader_Name
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView_columnHeader_Name, "ui_tabLeft_page1_listView_columnHeader_Name");
			// 
			// ui_tabLeft_page1_listView_columnHeader_Container
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView_columnHeader_Container, "ui_tabLeft_page1_listView_columnHeader_Container");
			// 
			// ui_tabLeft_page1_listView_columnHeader_Type
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView_columnHeader_Type, "ui_tabLeft_page1_listView_columnHeader_Type");
			// 
			// ui_tabLeft_page1_listView_columnHeader_PathID
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView_columnHeader_PathID, "ui_tabLeft_page1_listView_columnHeader_PathID");
			// 
			// ui_tabLeft_page1_listView_columnHeader_Size
			// 
			resources.ApplyResources(ui_tabLeft_page1_listView_columnHeader_Size, "ui_tabLeft_page1_listView_columnHeader_Size");
			// 
			// ui_tabLeft_page1_listSearch
			// 
			resources.ApplyResources(ui_tabLeft_page1_listSearch, "ui_tabLeft_page1_listSearch");
			ui_tabLeft_page1_listSearch.ForeColor = System.Drawing.SystemColors.GrayText;
			ui_tabLeft_page1_listSearch.Name = "ui_tabLeft_page1_listSearch";
			ui_tabLeft_page1_listSearch.TextChanged += ui_tabLeft_page1_ListSearchTextChanged;
			// 
			// ui_tabLeft_page2
			// 
			resources.ApplyResources(ui_tabLeft_page2, "ui_tabLeft_page2");
			ui_tabLeft_page2.Controls.Add(ui_tabLeft_page2_classesListView);
			ui_tabLeft_page2.Name = "ui_tabLeft_page2";
			ui_tabLeft_page2.UseVisualStyleBackColor = true;
			// 
			// ui_tabLeft_page2_classesListView
			// 
			resources.ApplyResources(ui_tabLeft_page2_classesListView, "ui_tabLeft_page2_classesListView");
			ui_tabLeft_page2_classesListView.Columns.AddRange(new ColumnHeader[] { ui_tabLeft_page2_listView_columnHeader_Name, ui_tabLeft_page2_listView_columnHeader_ID });
			ui_tabLeft_page2_classesListView.FullRowSelect = true;
			ui_tabLeft_page2_classesListView.MultiSelect = false;
			ui_tabLeft_page2_classesListView.Name = "ui_tabLeft_page2_classesListView";
			ui_tabLeft_page2_classesListView.UseCompatibleStateImageBehavior = false;
			ui_tabLeft_page2_classesListView.View = View.Details;
			ui_tabLeft_page2_classesListView.ItemSelectionChanged += ui_tabLeft_page2_classesListView_ItemSelectionChanged;
			// 
			// ui_tabLeft_page2_listView_columnHeader_Name
			// 
			resources.ApplyResources(ui_tabLeft_page2_listView_columnHeader_Name, "ui_tabLeft_page2_listView_columnHeader_Name");
			// 
			// ui_tabLeft_page2_listView_columnHeader_ID
			// 
			resources.ApplyResources(ui_tabLeft_page2_listView_columnHeader_ID, "ui_tabLeft_page2_listView_columnHeader_ID");
			// 
			// ui_progressBar0_down
			// 
			resources.ApplyResources(ui_progressBar0_down, "ui_progressBar0_down");
			ui_progressBar0_down.Name = "ui_progressBar0_down";
			ui_progressBar0_down.Step = 1;
			// 
			// splitter1
			// 
			resources.ApplyResources(splitter1, "splitter1");
			splitter1.Name = "splitter1";
			splitter1.TabStop = false;
			// 
			// AssetStudioGUIForm
			// 
			resources.ApplyResources(this, "$this");
			AllowDrop = true;
			Controls.Add(ui_statusStrip0_down);
			Controls.Add(splitter1);
			Controls.Add(panel1);
			Controls.Add(ui_menuStrip0_up);
			Icon = Properties.Resources._as;
			IsMdiContainer = true;
			KeyPreview = true;
			MainMenuStrip = ui_menuStrip0_up;
			Name = "AssetStudioGUIForm";
			DragDrop += AssetStudioGUIForm_DragDrop;
			DragEnter += AssetStudioGUIForm_DragEnter;
			ui_statusStrip0_down.ResumeLayout(false);
			ui_statusStrip0_down.PerformLayout();
			ui_menuStrip0_up.ResumeLayout(false);
			ui_menuStrip0_up.PerformLayout();
			ui_contextMenuStrip0.ResumeLayout(false);
			panel1.ResumeLayout(false);
			ui_tabLeft_tab.ResumeLayout(false);
			ui_tabLeft_page0.ResumeLayout(false);
			ui_tabLeft_page0.PerformLayout();
			ui_tabLeft_page1.ResumeLayout(false);
			ui_tabLeft_page1.PerformLayout();
			ui_tabLeft_page2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip ui_menuStrip0_up;
		private ToolStripMenuItem ui_menuFile;
		private ToolStripMenuItem ui_menuFile_loadFile;
		private ToolStripMenuItem ui_menuFile_loadFolder;
		private ToolStripMenuItem ui_menuExport;
		private ToolStripMenuItem ui_menuExport_allAssets;
		private ToolStripMenuItem ui_menuExport_selectedAssets;
		private StatusStrip ui_statusStrip0_down;
		private ToolStripStatusLabel ui_statusLabel0_down;
		private ToolStripMenuItem ui_menuExport_filteredAssets;
		private ToolStripMenuItem ui_menuModel;
		private ToolStripMenuItem ui_menuOptions;
		private ToolStripMenuItem ui_menuOptions_displayAllAssets;
		private ToolStripMenuItem ui_menuOptions_enablePreview;
		private ToolStripMenuItem ui_menuOptions_displayInfo;
		private ToolStripSeparator ui_menuFile_separator0;
		private ToolStripMenuItem ui_menuFile_extractFile;
		private ToolStripMenuItem ui_menuFile_extractFolder;
		private OpenFileDialog ui_openFileDialog0;
		private ToolStripMenuItem ui_menuOptions_exportOptions;
		private ToolStripMenuItem ui_menuDebug;
		private ToolStripMenuItem ui_menuDebug_exportClassStructures;
		private ContextMenuStrip ui_contextMenuStrip0;
		private ToolStripMenuItem ui_conMenu_showOriginalFile;
		private ToolStripMenuItem ui_conMenu_exportAnimatorAndSelectedAnimationClips;
		private ToolStripMenuItem ui_conMenu_exportSelectedAssets;
		private ToolStripMenuItem ui_menuFilter;
		private ToolStripMenuItem ui_menuFilter_0_all;
		private ToolStripMenuItem ui_menuModel_exportSelectedObjectsSplit;
		private ToolStripMenuItem ui_menuModel_exportSelectedObjectsWithAnimationClip;
		private ToolStripSeparator ui_menuExport_separator0;
		private ToolStripMenuItem ui_menuExport_exportAnimatorAndSelectedAnimationClips;
		private ToolStripMenuItem ui_menuModel_exportAllObjectsSplit;
		private ToolStripMenuItem ui_conMenu_goToSceneHierarchy;
		private ToolStripMenuItem ui_menuModel_exportSelectedObjectsMerge;
		private ToolStripMenuItem ui_menuModel_exportSelectedObjectsMergeWithAnimationClips;
		private ToolStripSeparator ui_menuModel_separator0;
		private ToolStripSeparator ui_menuExport_separator1;
		private ToolStripMenuItem ui_menuExport_Raw;
		private ToolStripMenuItem ui_menuExport_Raw_allAssets;
		private ToolStripMenuItem ui_menuExport_Raw_slectedAssets;
		private ToolStripMenuItem ui_menuExport_Raw_filteredAssets;
		private ToolStripMenuItem ui_menuExport_Dump;
		private ToolStripMenuItem ui_menuExport_Dump_allAssets;
		private ToolStripMenuItem ui_menuExport_Dump_selectedAssets;
		private ToolStripMenuItem ui_menuExport_Dump_filteredAssets;
		private ToolStripMenuItem ui_conMenu_copyText;
		private ToolStripSeparator ui_menuExport_separator2;
		private ToolStripMenuItem ui_menuExport_assetsListToXml;
		private ToolStripMenuItem ui_menuExport_assetsListToXml_allAssets;
		private ToolStripMenuItem ui_menuExport_assetsListToXml_selectedAssets;
		private ToolStripMenuItem ui_menuExport_assetsListToXml_filteredAssets;
		private ToolStripMenuItem ui_menuOptions_specifyUnityVersion;
		private ToolStripTextBox ui_menuOptions_specifyUnityVersion_specifyUnityVersion;
		private ToolStripMenuItem ui_menuDebug_showErrorMessage;
		private ToolStripMenuItem ui_menuOhmsExport;
		private ToolStripMenuItem ui_menuOhmsExport_structuredXmlList;
		private ToolStripMenuItem ui_menuOhmsExport_StructXml_All;
		private ToolStripMenuItem ui_menuOhmsExport_StructXml_selected;
		private ToolStripMenuItem ui_menuOhmsExport_StructXml_filtered;
		private ToolStripMenuItem ui_menuOhmsExport_createANewFolder;
		private ToolStripSeparator ui_menuFile_separator1;
		private ToolStripMenuItem ui_menuFile_clear;
		private ToolStripSeparator ui_menuOhmsExport_separator0;
		private ToolStripMenuItem ui_menuOhmsExport_allAssets;
		private ToolStripMenuItem ui_menuOhmsExport_selectedAssets;
		private ToolStripMenuItem ui_menuOhmsExport_displayedAssets;
		private ToolStripSeparator ui_menuOhmsExport_separator1;
		private ToolStripSeparator ui_conMenu_separator0;
		private ToolStripMenuItem ui_conMenu_OHMS_ExportSelected;
		private ToolStripMenuItem ui_conMenu_OHMS_ExportSelected_Struct;
		private ToolStripMenuItem ui_menuExport_dumpJson;
		private ToolStripMenuItem ui_menuExport_dumpJson_allAssets;
		private ToolStripMenuItem ui_menuExport_dumpJson_selectedAssets;
		private ToolStripMenuItem ui_menuExport_dumpJson_filteredAssets;
		private ToolStripMenuItem ui_menuExport_assetListToJson;
		private ToolStripMenuItem ui_menuExport_assetListToJson_allAssets;
		private ToolStripMenuItem ui_menuExport_assetListToJson_selectedAssets;
		private ToolStripMenuItem ui_menuExport_assetListToJson_filteredAssets;
		private ToolStripMenuItem ui_menuOhmsExport_structuredJsonList;
		private ToolStripMenuItem ui_menuOhmsExport_structuredJsonList_allAssets;
		private ToolStripMenuItem ui_menuOhmsExport_structuredJsonList_selectedAssets;
		private ToolStripMenuItem ui_menuOhmsExport_structuredJsonList_displayedAssets;
		private ToolStripMenuItem ui_conMenu_OHMS_ExportSelected_Struct_XML;
		private ToolStripMenuItem ui_conMenu_OHMS_ExportSelected_Struct_JSON;
		private ToolStripSeparator ui_menuOhmsExport_separator2;
		private ToolStripMenuItem ui_menuOhmsExport_arknights;
		private ToolStripMenuItem ui_menuOhmsExport_arknights_sceneBundle;
		private ToolStripMenuItem ui_menuOhmsExport_arknights_charartBundle;
		private ToolStripMenuItem ui_TEST_ToolStripMenuItem;
		private ToolStripMenuItem ui_menuOptions_language;
		private ToolStripSeparator ui_menuOptions_separator0;
		private ToolStripSeparator ui_menuOptions_separator1;
		private ToolStripMenuItem tESTToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private TabControl ui_tabLeft_tab;
		private TabPage ui_tabLeft_page0;
		private GOHierarchy ui_tabLeft_page0_treeView;
		private TextBox ui_tabLeft_page0_treeSearch;
		private TabPage ui_tabLeft_page1;
		private ListView ui_tabLeft_page1_listView;
		private ColumnHeader ui_tabLeft_page1_listView_columnHeader_Name;
		private ColumnHeader ui_tabLeft_page1_listView_columnHeader_Container;
		private ColumnHeader ui_tabLeft_page1_listView_columnHeader_Type;
		private ColumnHeader ui_tabLeft_page1_listView_columnHeader_PathID;
		private ColumnHeader ui_tabLeft_page1_listView_columnHeader_Size;
		private TextBox ui_tabLeft_page1_listSearch;
		private TabPage ui_tabLeft_page2;
		private ListView ui_tabLeft_page2_classesListView;
		private ColumnHeader ui_tabLeft_page2_listView_columnHeader_Name;
		private ColumnHeader ui_tabLeft_page2_listView_columnHeader_ID;
		private Panel panel1;
		private Splitter splitter1;
		private ProgressBar ui_progressBar0_down;
		private ToolStripMenuItem previewsToolStripMenuItem;
	}
}

