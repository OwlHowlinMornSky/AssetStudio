namespace AssetStudioGUI {
	partial class ExportOptions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportOptions));
			OKbutton = new System.Windows.Forms.Button();
			Cancel = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			jsonStructureCheckBox = new System.Windows.Forms.CheckBox();
			openAfterExport = new System.Windows.Forms.CheckBox();
			restoreExtensionName = new System.Windows.Forms.CheckBox();
			assetGroupOptions = new System.Windows.Forms.ComboBox();
			label6 = new System.Windows.Forms.Label();
			convertAudio = new System.Windows.Forms.CheckBox();
			panel1 = new System.Windows.Forms.Panel();
			totga = new System.Windows.Forms.RadioButton();
			tojpg = new System.Windows.Forms.RadioButton();
			topng = new System.Windows.Forms.RadioButton();
			tobmp = new System.Windows.Forms.RadioButton();
			converttexture = new System.Windows.Forms.CheckBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			exportAllUvsAsDiffuseMaps = new System.Windows.Forms.CheckBox();
			exportBlendShape = new System.Windows.Forms.CheckBox();
			exportAnimations = new System.Windows.Forms.CheckBox();
			scaleFactor = new System.Windows.Forms.NumericUpDown();
			label5 = new System.Windows.Forms.Label();
			fbxFormat = new System.Windows.Forms.ComboBox();
			label4 = new System.Windows.Forms.Label();
			fbxVersion = new System.Windows.Forms.ComboBox();
			label3 = new System.Windows.Forms.Label();
			boneSize = new System.Windows.Forms.NumericUpDown();
			label2 = new System.Windows.Forms.Label();
			exportSkins = new System.Windows.Forms.CheckBox();
			label1 = new System.Windows.Forms.Label();
			filterPrecision = new System.Windows.Forms.NumericUpDown();
			castToBone = new System.Windows.Forms.CheckBox();
			exportAllNodes = new System.Windows.Forms.CheckBox();
			eulerFilter = new System.Windows.Forms.CheckBox();
			exportUvsTooltip = new System.Windows.Forms.ToolTip(components);
			groupBox1.SuspendLayout();
			panel1.SuspendLayout();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)scaleFactor).BeginInit();
			((System.ComponentModel.ISupportInitialize)boneSize).BeginInit();
			((System.ComponentModel.ISupportInitialize)filterPrecision).BeginInit();
			SuspendLayout();
			// 
			// OKbutton
			// 
			resources.ApplyResources(OKbutton, "OKbutton");
			OKbutton.Name = "OKbutton";
			OKbutton.UseVisualStyleBackColor = true;
			OKbutton.Click += OKbutton_Click;
			// 
			// Cancel
			// 
			Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(Cancel, "Cancel");
			Cancel.Name = "Cancel";
			Cancel.UseVisualStyleBackColor = true;
			Cancel.Click += Cancel_Click;
			// 
			// groupBox1
			// 
			resources.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(jsonStructureCheckBox);
			groupBox1.Controls.Add(openAfterExport);
			groupBox1.Controls.Add(restoreExtensionName);
			groupBox1.Controls.Add(assetGroupOptions);
			groupBox1.Controls.Add(label6);
			groupBox1.Controls.Add(convertAudio);
			groupBox1.Controls.Add(panel1);
			groupBox1.Controls.Add(converttexture);
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			// 
			// jsonStructureCheckBox
			// 
			resources.ApplyResources(jsonStructureCheckBox, "jsonStructureCheckBox");
			jsonStructureCheckBox.Checked = true;
			jsonStructureCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			jsonStructureCheckBox.Name = "jsonStructureCheckBox";
			jsonStructureCheckBox.UseVisualStyleBackColor = true;
			// 
			// openAfterExport
			// 
			resources.ApplyResources(openAfterExport, "openAfterExport");
			openAfterExport.Checked = true;
			openAfterExport.CheckState = System.Windows.Forms.CheckState.Checked;
			openAfterExport.Name = "openAfterExport";
			openAfterExport.UseVisualStyleBackColor = true;
			// 
			// restoreExtensionName
			// 
			resources.ApplyResources(restoreExtensionName, "restoreExtensionName");
			restoreExtensionName.Checked = true;
			restoreExtensionName.CheckState = System.Windows.Forms.CheckState.Checked;
			restoreExtensionName.Name = "restoreExtensionName";
			restoreExtensionName.UseVisualStyleBackColor = true;
			// 
			// assetGroupOptions
			// 
			assetGroupOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			assetGroupOptions.FormattingEnabled = true;
			assetGroupOptions.Items.AddRange(new object[] { resources.GetString("assetGroupOptions.Items"), resources.GetString("assetGroupOptions.Items1"), resources.GetString("assetGroupOptions.Items2"), resources.GetString("assetGroupOptions.Items3") });
			resources.ApplyResources(assetGroupOptions, "assetGroupOptions");
			assetGroupOptions.Name = "assetGroupOptions";
			// 
			// label6
			// 
			resources.ApplyResources(label6, "label6");
			label6.Name = "label6";
			// 
			// convertAudio
			// 
			resources.ApplyResources(convertAudio, "convertAudio");
			convertAudio.Checked = true;
			convertAudio.CheckState = System.Windows.Forms.CheckState.Checked;
			convertAudio.Name = "convertAudio";
			convertAudio.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			panel1.Controls.Add(totga);
			panel1.Controls.Add(tojpg);
			panel1.Controls.Add(topng);
			panel1.Controls.Add(tobmp);
			resources.ApplyResources(panel1, "panel1");
			panel1.Name = "panel1";
			// 
			// totga
			// 
			resources.ApplyResources(totga, "totga");
			totga.Name = "totga";
			totga.UseVisualStyleBackColor = true;
			// 
			// tojpg
			// 
			resources.ApplyResources(tojpg, "tojpg");
			tojpg.Name = "tojpg";
			tojpg.UseVisualStyleBackColor = true;
			// 
			// topng
			// 
			resources.ApplyResources(topng, "topng");
			topng.Checked = true;
			topng.Name = "topng";
			topng.TabStop = true;
			topng.UseVisualStyleBackColor = true;
			// 
			// tobmp
			// 
			resources.ApplyResources(tobmp, "tobmp");
			tobmp.Name = "tobmp";
			tobmp.UseVisualStyleBackColor = true;
			// 
			// converttexture
			// 
			resources.ApplyResources(converttexture, "converttexture");
			converttexture.Checked = true;
			converttexture.CheckState = System.Windows.Forms.CheckState.Checked;
			converttexture.Name = "converttexture";
			converttexture.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Controls.Add(exportAllUvsAsDiffuseMaps);
			groupBox2.Controls.Add(exportBlendShape);
			groupBox2.Controls.Add(exportAnimations);
			groupBox2.Controls.Add(scaleFactor);
			groupBox2.Controls.Add(label5);
			groupBox2.Controls.Add(fbxFormat);
			groupBox2.Controls.Add(label4);
			groupBox2.Controls.Add(fbxVersion);
			groupBox2.Controls.Add(label3);
			groupBox2.Controls.Add(boneSize);
			groupBox2.Controls.Add(label2);
			groupBox2.Controls.Add(exportSkins);
			groupBox2.Controls.Add(label1);
			groupBox2.Controls.Add(filterPrecision);
			groupBox2.Controls.Add(castToBone);
			groupBox2.Controls.Add(exportAllNodes);
			groupBox2.Controls.Add(eulerFilter);
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			// 
			// exportAllUvsAsDiffuseMaps
			// 
			resources.ApplyResources(exportAllUvsAsDiffuseMaps, "exportAllUvsAsDiffuseMaps");
			exportAllUvsAsDiffuseMaps.Name = "exportAllUvsAsDiffuseMaps";
			exportUvsTooltip.SetToolTip(exportAllUvsAsDiffuseMaps, resources.GetString("exportAllUvsAsDiffuseMaps.ToolTip"));
			exportAllUvsAsDiffuseMaps.UseVisualStyleBackColor = true;
			// 
			// exportBlendShape
			// 
			resources.ApplyResources(exportBlendShape, "exportBlendShape");
			exportBlendShape.Checked = true;
			exportBlendShape.CheckState = System.Windows.Forms.CheckState.Checked;
			exportBlendShape.Name = "exportBlendShape";
			exportBlendShape.UseVisualStyleBackColor = true;
			// 
			// exportAnimations
			// 
			resources.ApplyResources(exportAnimations, "exportAnimations");
			exportAnimations.Checked = true;
			exportAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
			exportAnimations.Name = "exportAnimations";
			exportAnimations.UseVisualStyleBackColor = true;
			// 
			// scaleFactor
			// 
			scaleFactor.DecimalPlaces = 2;
			scaleFactor.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
			resources.ApplyResources(scaleFactor, "scaleFactor");
			scaleFactor.Name = "scaleFactor";
			scaleFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// label5
			// 
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			// 
			// fbxFormat
			// 
			fbxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			fbxFormat.FormattingEnabled = true;
			fbxFormat.Items.AddRange(new object[] { resources.GetString("fbxFormat.Items"), resources.GetString("fbxFormat.Items1") });
			resources.ApplyResources(fbxFormat, "fbxFormat");
			fbxFormat.Name = "fbxFormat";
			// 
			// label4
			// 
			resources.ApplyResources(label4, "label4");
			label4.Name = "label4";
			// 
			// fbxVersion
			// 
			fbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			fbxVersion.FormattingEnabled = true;
			fbxVersion.Items.AddRange(new object[] { resources.GetString("fbxVersion.Items"), resources.GetString("fbxVersion.Items1"), resources.GetString("fbxVersion.Items2"), resources.GetString("fbxVersion.Items3"), resources.GetString("fbxVersion.Items4"), resources.GetString("fbxVersion.Items5") });
			resources.ApplyResources(fbxVersion, "fbxVersion");
			fbxVersion.Name = "fbxVersion";
			// 
			// label3
			// 
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			// 
			// boneSize
			// 
			resources.ApplyResources(boneSize, "boneSize");
			boneSize.Name = "boneSize";
			boneSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			// 
			// exportSkins
			// 
			resources.ApplyResources(exportSkins, "exportSkins");
			exportSkins.Checked = true;
			exportSkins.CheckState = System.Windows.Forms.CheckState.Checked;
			exportSkins.Name = "exportSkins";
			exportSkins.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// filterPrecision
			// 
			filterPrecision.DecimalPlaces = 2;
			filterPrecision.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
			resources.ApplyResources(filterPrecision, "filterPrecision");
			filterPrecision.Name = "filterPrecision";
			filterPrecision.Value = new decimal(new int[] { 25, 0, 0, 131072 });
			// 
			// castToBone
			// 
			resources.ApplyResources(castToBone, "castToBone");
			castToBone.Name = "castToBone";
			castToBone.UseVisualStyleBackColor = true;
			// 
			// exportAllNodes
			// 
			resources.ApplyResources(exportAllNodes, "exportAllNodes");
			exportAllNodes.Checked = true;
			exportAllNodes.CheckState = System.Windows.Forms.CheckState.Checked;
			exportAllNodes.Name = "exportAllNodes";
			exportAllNodes.UseVisualStyleBackColor = true;
			// 
			// eulerFilter
			// 
			resources.ApplyResources(eulerFilter, "eulerFilter");
			eulerFilter.Checked = true;
			eulerFilter.CheckState = System.Windows.Forms.CheckState.Checked;
			eulerFilter.Name = "eulerFilter";
			eulerFilter.UseVisualStyleBackColor = true;
			// 
			// ExportOptions
			// 
			AcceptButton = OKbutton;
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = Cancel;
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Controls.Add(Cancel);
			Controls.Add(OKbutton);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ExportOptions";
			ShowIcon = false;
			ShowInTaskbar = false;
			TopMost = true;
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)scaleFactor).EndInit();
			((System.ComponentModel.ISupportInitialize)boneSize).EndInit();
			((System.ComponentModel.ISupportInitialize)filterPrecision).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox converttexture;
		private System.Windows.Forms.RadioButton tojpg;
		private System.Windows.Forms.RadioButton topng;
		private System.Windows.Forms.RadioButton tobmp;
		private System.Windows.Forms.RadioButton totga;
		private System.Windows.Forms.CheckBox convertAudio;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown boneSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox exportSkins;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown filterPrecision;
		private System.Windows.Forms.CheckBox castToBone;
		private System.Windows.Forms.CheckBox exportAllNodes;
		private System.Windows.Forms.CheckBox eulerFilter;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox fbxVersion;
		private System.Windows.Forms.ComboBox fbxFormat;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown scaleFactor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox exportBlendShape;
		private System.Windows.Forms.CheckBox exportAnimations;
		private System.Windows.Forms.ComboBox assetGroupOptions;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox restoreExtensionName;
		private System.Windows.Forms.CheckBox openAfterExport;
		private System.Windows.Forms.CheckBox exportAllUvsAsDiffuseMaps;
		private System.Windows.Forms.ToolTip exportUvsTooltip;
		private System.Windows.Forms.CheckBox jsonStructureCheckBox;
	}
}