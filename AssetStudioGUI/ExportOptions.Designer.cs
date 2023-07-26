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
			OKbutton.Location = new System.Drawing.Point(371, 497);
			OKbutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			OKbutton.Name = "OKbutton";
			OKbutton.Size = new System.Drawing.Size(88, 30);
			OKbutton.TabIndex = 6;
			OKbutton.Text = "OK";
			OKbutton.UseVisualStyleBackColor = true;
			OKbutton.Click += OKbutton_Click;
			// 
			// Cancel
			// 
			Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Cancel.Location = new System.Drawing.Point(465, 497);
			Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			Cancel.Name = "Cancel";
			Cancel.Size = new System.Drawing.Size(88, 30);
			Cancel.TabIndex = 7;
			Cancel.Text = "Cancel";
			Cancel.UseVisualStyleBackColor = true;
			Cancel.Click += Cancel_Click;
			// 
			// groupBox1
			// 
			groupBox1.AutoSize = true;
			groupBox1.Controls.Add(jsonStructureCheckBox);
			groupBox1.Controls.Add(openAfterExport);
			groupBox1.Controls.Add(restoreExtensionName);
			groupBox1.Controls.Add(assetGroupOptions);
			groupBox1.Controls.Add(label6);
			groupBox1.Controls.Add(convertAudio);
			groupBox1.Controls.Add(panel1);
			groupBox1.Controls.Add(converttexture);
			groupBox1.Location = new System.Drawing.Point(14, 17);
			groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			groupBox1.Name = "groupBox1";
			groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			groupBox1.Size = new System.Drawing.Size(271, 473);
			groupBox1.TabIndex = 9;
			groupBox1.TabStop = false;
			groupBox1.Text = "Export";
			// 
			// jsonStructureCheckBox
			// 
			jsonStructureCheckBox.AutoSize = true;
			jsonStructureCheckBox.Checked = true;
			jsonStructureCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			jsonStructureCheckBox.Location = new System.Drawing.Point(7, 258);
			jsonStructureCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			jsonStructureCheckBox.Name = "jsonStructureCheckBox";
			jsonStructureCheckBox.Size = new System.Drawing.Size(115, 21);
			jsonStructureCheckBox.TabIndex = 11;
			jsonStructureCheckBox.Text = "Indented JSON";
			jsonStructureCheckBox.UseVisualStyleBackColor = true;
			// 
			// openAfterExport
			// 
			openAfterExport.AutoSize = true;
			openAfterExport.Checked = true;
			openAfterExport.CheckState = System.Windows.Forms.CheckState.Checked;
			openAfterExport.Location = new System.Drawing.Point(7, 227);
			openAfterExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			openAfterExport.Name = "openAfterExport";
			openAfterExport.Size = new System.Drawing.Size(171, 21);
			openAfterExport.TabIndex = 10;
			openAfterExport.Text = "Open folder after export";
			openAfterExport.UseVisualStyleBackColor = true;
			// 
			// restoreExtensionName
			// 
			restoreExtensionName.AutoSize = true;
			restoreExtensionName.Checked = true;
			restoreExtensionName.CheckState = System.Windows.Forms.CheckState.Checked;
			restoreExtensionName.Location = new System.Drawing.Point(7, 82);
			restoreExtensionName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			restoreExtensionName.Name = "restoreExtensionName";
			restoreExtensionName.Size = new System.Drawing.Size(226, 21);
			restoreExtensionName.TabIndex = 9;
			restoreExtensionName.Text = "Restore TextAsset extension name";
			restoreExtensionName.UseVisualStyleBackColor = true;
			// 
			// assetGroupOptions
			// 
			assetGroupOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			assetGroupOptions.FormattingEnabled = true;
			assetGroupOptions.Items.AddRange(new object[] { "type name", "container path", "source file name", "do not group" });
			assetGroupOptions.Location = new System.Drawing.Point(7, 45);
			assetGroupOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			assetGroupOptions.Name = "assetGroupOptions";
			assetGroupOptions.Size = new System.Drawing.Size(173, 25);
			assetGroupOptions.TabIndex = 8;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(7, 24);
			label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(160, 17);
			label6.TabIndex = 7;
			label6.Text = "Group exported assets by";
			// 
			// convertAudio
			// 
			convertAudio.AutoSize = true;
			convertAudio.Checked = true;
			convertAudio.CheckState = System.Windows.Forms.CheckState.Checked;
			convertAudio.Location = new System.Drawing.Point(7, 196);
			convertAudio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			convertAudio.Name = "convertAudio";
			convertAudio.Size = new System.Drawing.Size(215, 21);
			convertAudio.TabIndex = 6;
			convertAudio.Text = "Convert AudioClip to WAV(PCM)";
			convertAudio.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			panel1.Controls.Add(totga);
			panel1.Controls.Add(tojpg);
			panel1.Controls.Add(topng);
			panel1.Controls.Add(tobmp);
			panel1.Location = new System.Drawing.Point(23, 144);
			panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(236, 42);
			panel1.TabIndex = 5;
			// 
			// totga
			// 
			totga.AutoSize = true;
			totga.Location = new System.Drawing.Point(175, 8);
			totga.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			totga.Name = "totga";
			totga.Size = new System.Drawing.Size(48, 21);
			totga.TabIndex = 2;
			totga.Text = "Tga";
			totga.UseVisualStyleBackColor = true;
			// 
			// tojpg
			// 
			tojpg.AutoSize = true;
			tojpg.Location = new System.Drawing.Point(113, 8);
			tojpg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			tojpg.Name = "tojpg";
			tojpg.Size = new System.Drawing.Size(54, 21);
			tojpg.TabIndex = 4;
			tojpg.Text = "Jpeg";
			tojpg.UseVisualStyleBackColor = true;
			// 
			// topng
			// 
			topng.AutoSize = true;
			topng.Checked = true;
			topng.Location = new System.Drawing.Point(58, 8);
			topng.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			topng.Name = "topng";
			topng.Size = new System.Drawing.Size(48, 21);
			topng.TabIndex = 3;
			topng.TabStop = true;
			topng.Text = "Png";
			topng.UseVisualStyleBackColor = true;
			// 
			// tobmp
			// 
			tobmp.AutoSize = true;
			tobmp.Location = new System.Drawing.Point(4, 8);
			tobmp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			tobmp.Name = "tobmp";
			tobmp.Size = new System.Drawing.Size(53, 21);
			tobmp.TabIndex = 2;
			tobmp.Text = "Bmp";
			tobmp.UseVisualStyleBackColor = true;
			// 
			// converttexture
			// 
			converttexture.AutoSize = true;
			converttexture.Checked = true;
			converttexture.CheckState = System.Windows.Forms.CheckState.Checked;
			converttexture.Location = new System.Drawing.Point(7, 113);
			converttexture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			converttexture.Name = "converttexture";
			converttexture.Size = new System.Drawing.Size(135, 21);
			converttexture.TabIndex = 1;
			converttexture.Text = "Convert Texture2D";
			converttexture.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			groupBox2.AutoSize = true;
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
			groupBox2.Location = new System.Drawing.Point(292, 17);
			groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			groupBox2.Name = "groupBox2";
			groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			groupBox2.Size = new System.Drawing.Size(261, 473);
			groupBox2.TabIndex = 11;
			groupBox2.TabStop = false;
			groupBox2.Text = "Fbx";
			// 
			// exportAllUvsAsDiffuseMaps
			// 
			exportAllUvsAsDiffuseMaps.AccessibleDescription = "";
			exportAllUvsAsDiffuseMaps.AutoSize = true;
			exportAllUvsAsDiffuseMaps.Location = new System.Drawing.Point(7, 242);
			exportAllUvsAsDiffuseMaps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			exportAllUvsAsDiffuseMaps.Name = "exportAllUvsAsDiffuseMaps";
			exportAllUvsAsDiffuseMaps.Size = new System.Drawing.Size(205, 21);
			exportAllUvsAsDiffuseMaps.TabIndex = 23;
			exportAllUvsAsDiffuseMaps.Text = "Export all UVs as diffuse maps";
			exportUvsTooltip.SetToolTip(exportAllUvsAsDiffuseMaps, "Unchecked: UV1 exported as normal map. Check this if your export is missing a UV map.");
			exportAllUvsAsDiffuseMaps.UseVisualStyleBackColor = true;
			// 
			// exportBlendShape
			// 
			exportBlendShape.AutoSize = true;
			exportBlendShape.Checked = true;
			exportBlendShape.CheckState = System.Windows.Forms.CheckState.Checked;
			exportBlendShape.Location = new System.Drawing.Point(7, 180);
			exportBlendShape.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			exportBlendShape.Name = "exportBlendShape";
			exportBlendShape.Size = new System.Drawing.Size(137, 21);
			exportBlendShape.TabIndex = 22;
			exportBlendShape.Text = "Export blendshape";
			exportBlendShape.UseVisualStyleBackColor = true;
			// 
			// exportAnimations
			// 
			exportAnimations.AutoSize = true;
			exportAnimations.Checked = true;
			exportAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
			exportAnimations.Location = new System.Drawing.Point(7, 149);
			exportAnimations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			exportAnimations.Name = "exportAnimations";
			exportAnimations.Size = new System.Drawing.Size(132, 21);
			exportAnimations.TabIndex = 21;
			exportAnimations.Text = "Export animations";
			exportAnimations.UseVisualStyleBackColor = true;
			// 
			// scaleFactor
			// 
			scaleFactor.DecimalPlaces = 2;
			scaleFactor.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
			scaleFactor.Location = new System.Drawing.Point(97, 317);
			scaleFactor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			scaleFactor.Name = "scaleFactor";
			scaleFactor.Size = new System.Drawing.Size(70, 23);
			scaleFactor.TabIndex = 20;
			scaleFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			scaleFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(7, 320);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(74, 17);
			label5.TabIndex = 19;
			label5.Text = "ScaleFactor";
			// 
			// fbxFormat
			// 
			fbxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			fbxFormat.FormattingEnabled = true;
			fbxFormat.Items.AddRange(new object[] { "Binary", "Ascii" });
			fbxFormat.Location = new System.Drawing.Point(90, 360);
			fbxFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			fbxFormat.Name = "fbxFormat";
			fbxFormat.Size = new System.Drawing.Size(70, 25);
			fbxFormat.TabIndex = 18;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(7, 366);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(71, 17);
			label4.TabIndex = 17;
			label4.Text = "FBXFormat";
			// 
			// fbxVersion
			// 
			fbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			fbxVersion.FormattingEnabled = true;
			fbxVersion.Items.AddRange(new object[] { "6.1", "7.1", "7.2", "7.3", "7.4", "7.5" });
			fbxVersion.Location = new System.Drawing.Point(90, 402);
			fbxVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			fbxVersion.Name = "fbxVersion";
			fbxVersion.Size = new System.Drawing.Size(54, 25);
			fbxVersion.TabIndex = 16;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(7, 407);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(74, 17);
			label3.TabIndex = 15;
			label3.Text = "FBXVersion";
			// 
			// boneSize
			// 
			boneSize.Location = new System.Drawing.Point(76, 279);
			boneSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			boneSize.Name = "boneSize";
			boneSize.Size = new System.Drawing.Size(54, 23);
			boneSize.TabIndex = 11;
			boneSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(7, 282);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(61, 17);
			label2.TabIndex = 10;
			label2.Text = "BoneSize";
			// 
			// exportSkins
			// 
			exportSkins.AutoSize = true;
			exportSkins.Checked = true;
			exportSkins.CheckState = System.Windows.Forms.CheckState.Checked;
			exportSkins.Location = new System.Drawing.Point(7, 118);
			exportSkins.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			exportSkins.Name = "exportSkins";
			exportSkins.Size = new System.Drawing.Size(98, 21);
			exportSkins.TabIndex = 8;
			exportSkins.Text = "Export skins";
			exportSkins.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(30, 55);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(88, 17);
			label1.TabIndex = 7;
			label1.Text = "FilterPrecision";
			// 
			// filterPrecision
			// 
			filterPrecision.DecimalPlaces = 2;
			filterPrecision.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
			filterPrecision.Location = new System.Drawing.Point(148, 52);
			filterPrecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			filterPrecision.Name = "filterPrecision";
			filterPrecision.Size = new System.Drawing.Size(59, 23);
			filterPrecision.TabIndex = 6;
			filterPrecision.Value = new decimal(new int[] { 25, 0, 0, 131072 });
			// 
			// castToBone
			// 
			castToBone.AutoSize = true;
			castToBone.Location = new System.Drawing.Point(7, 211);
			castToBone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			castToBone.Name = "castToBone";
			castToBone.Size = new System.Drawing.Size(158, 21);
			castToBone.TabIndex = 5;
			castToBone.Text = "All nodes cast to bone";
			castToBone.UseVisualStyleBackColor = true;
			// 
			// exportAllNodes
			// 
			exportAllNodes.AutoSize = true;
			exportAllNodes.Checked = true;
			exportAllNodes.CheckState = System.Windows.Forms.CheckState.Checked;
			exportAllNodes.Location = new System.Drawing.Point(7, 86);
			exportAllNodes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			exportAllNodes.Name = "exportAllNodes";
			exportAllNodes.Size = new System.Drawing.Size(122, 21);
			exportAllNodes.TabIndex = 4;
			exportAllNodes.Text = "Export all nodes";
			exportAllNodes.UseVisualStyleBackColor = true;
			// 
			// eulerFilter
			// 
			eulerFilter.AutoSize = true;
			eulerFilter.Checked = true;
			eulerFilter.CheckState = System.Windows.Forms.CheckState.Checked;
			eulerFilter.Location = new System.Drawing.Point(7, 28);
			eulerFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			eulerFilter.Name = "eulerFilter";
			eulerFilter.Size = new System.Drawing.Size(84, 21);
			eulerFilter.TabIndex = 3;
			eulerFilter.Text = "EulerFilter";
			eulerFilter.UseVisualStyleBackColor = true;
			// 
			// ExportOptions
			// 
			AcceptButton = OKbutton;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = Cancel;
			ClientSize = new System.Drawing.Size(567, 544);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Controls.Add(Cancel);
			Controls.Add(OKbutton);
			Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ExportOptions";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Export options";
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