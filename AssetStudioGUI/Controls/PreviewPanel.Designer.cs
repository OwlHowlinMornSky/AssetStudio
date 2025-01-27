namespace AssetStudioGUI.Controls {
	partial class PreviewPanel {
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewPanel));
			ui_tabRight_tab = new System.Windows.Forms.TabControl();
			ui_tabRight_page0 = new System.Windows.Forms.TabPage();
			previewImage1 = new PreviewImage();
			ui_tabRight_page0_classTextPreview = new System.Windows.Forms.TextBox();
			ui_tabRight_page0_textPreviewBox = new System.Windows.Forms.TextBox();
			ui_tabRight_page0_fontPreviewBox = new System.Windows.Forms.RichTextBox();
			ui_tabRight_page1 = new System.Windows.Forms.TabPage();
			ui_tabRight_page1_dumpTextBox = new System.Windows.Forms.TextBox();
			ui_tabRight_page3 = new System.Windows.Forms.TabPage();
			ui_tabRight_page2_dumpJsonTextBox = new System.Windows.Forms.TextBox();
			ui_tabRight_page0_assetInfoLabel = new System.Windows.Forms.Label();
			tabPage1 = new System.Windows.Forms.TabPage();
			previewgl1 = new PreviewGL();
			tabPage2 = new System.Windows.Forms.TabPage();
			previewfmod1 = new PreviewFMOD();
			ui_tabRight_tab.SuspendLayout();
			ui_tabRight_page0.SuspendLayout();
			ui_tabRight_page1.SuspendLayout();
			ui_tabRight_page3.SuspendLayout();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			SuspendLayout();
			// 
			// ui_tabRight_tab
			// 
			ui_tabRight_tab.Controls.Add(ui_tabRight_page0);
			ui_tabRight_tab.Controls.Add(ui_tabRight_page1);
			ui_tabRight_tab.Controls.Add(ui_tabRight_page3);
			ui_tabRight_tab.Controls.Add(tabPage1);
			ui_tabRight_tab.Controls.Add(tabPage2);
			resources.ApplyResources(ui_tabRight_tab, "ui_tabRight_tab");
			ui_tabRight_tab.Name = "ui_tabRight_tab";
			ui_tabRight_tab.SelectedIndex = 0;
			ui_tabRight_tab.SelectedIndexChanged += Ui_tabRight_tab_SelectedIndexChanged;
			// 
			// ui_tabRight_page0
			// 
			ui_tabRight_page0.Controls.Add(previewImage1);
			ui_tabRight_page0.Controls.Add(ui_tabRight_page0_classTextPreview);
			ui_tabRight_page0.Controls.Add(ui_tabRight_page0_textPreviewBox);
			ui_tabRight_page0.Controls.Add(ui_tabRight_page0_fontPreviewBox);
			resources.ApplyResources(ui_tabRight_page0, "ui_tabRight_page0");
			ui_tabRight_page0.Name = "ui_tabRight_page0";
			ui_tabRight_page0.UseVisualStyleBackColor = true;
			// 
			// previewImage1
			// 
			resources.ApplyResources(previewImage1, "previewImage1");
			previewImage1.Name = "previewImage1";
			// 
			// ui_tabRight_page0_classTextPreview
			// 
			resources.ApplyResources(ui_tabRight_page0_classTextPreview, "ui_tabRight_page0_classTextPreview");
			ui_tabRight_page0_classTextPreview.Name = "ui_tabRight_page0_classTextPreview";
			ui_tabRight_page0_classTextPreview.ReadOnly = true;
			// 
			// ui_tabRight_page0_textPreviewBox
			// 
			resources.ApplyResources(ui_tabRight_page0_textPreviewBox, "ui_tabRight_page0_textPreviewBox");
			ui_tabRight_page0_textPreviewBox.Name = "ui_tabRight_page0_textPreviewBox";
			ui_tabRight_page0_textPreviewBox.ReadOnly = true;
			// 
			// ui_tabRight_page0_fontPreviewBox
			// 
			ui_tabRight_page0_fontPreviewBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(ui_tabRight_page0_fontPreviewBox, "ui_tabRight_page0_fontPreviewBox");
			ui_tabRight_page0_fontPreviewBox.Name = "ui_tabRight_page0_fontPreviewBox";
			ui_tabRight_page0_fontPreviewBox.ReadOnly = true;
			// 
			// ui_tabRight_page1
			// 
			ui_tabRight_page1.Controls.Add(ui_tabRight_page1_dumpTextBox);
			resources.ApplyResources(ui_tabRight_page1, "ui_tabRight_page1");
			ui_tabRight_page1.Name = "ui_tabRight_page1";
			ui_tabRight_page1.UseVisualStyleBackColor = true;
			// 
			// ui_tabRight_page1_dumpTextBox
			// 
			resources.ApplyResources(ui_tabRight_page1_dumpTextBox, "ui_tabRight_page1_dumpTextBox");
			ui_tabRight_page1_dumpTextBox.Name = "ui_tabRight_page1_dumpTextBox";
			// 
			// ui_tabRight_page3
			// 
			ui_tabRight_page3.Controls.Add(ui_tabRight_page2_dumpJsonTextBox);
			ui_tabRight_page3.Controls.Add(ui_tabRight_page0_assetInfoLabel);
			resources.ApplyResources(ui_tabRight_page3, "ui_tabRight_page3");
			ui_tabRight_page3.Name = "ui_tabRight_page3";
			ui_tabRight_page3.UseVisualStyleBackColor = true;
			// 
			// ui_tabRight_page2_dumpJsonTextBox
			// 
			resources.ApplyResources(ui_tabRight_page2_dumpJsonTextBox, "ui_tabRight_page2_dumpJsonTextBox");
			ui_tabRight_page2_dumpJsonTextBox.Name = "ui_tabRight_page2_dumpJsonTextBox";
			// 
			// ui_tabRight_page0_assetInfoLabel
			// 
			resources.ApplyResources(ui_tabRight_page0_assetInfoLabel, "ui_tabRight_page0_assetInfoLabel");
			ui_tabRight_page0_assetInfoLabel.BackColor = System.Drawing.Color.Transparent;
			ui_tabRight_page0_assetInfoLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			ui_tabRight_page0_assetInfoLabel.Name = "ui_tabRight_page0_assetInfoLabel";
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(previewgl1);
			resources.ApplyResources(tabPage1, "tabPage1");
			tabPage1.Name = "tabPage1";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// previewgl1
			// 
			resources.ApplyResources(previewgl1, "previewgl1");
			previewgl1.Name = "previewgl1";
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(previewfmod1);
			resources.ApplyResources(tabPage2, "tabPage2");
			tabPage2.Name = "tabPage2";
			tabPage2.UseVisualStyleBackColor = true;
			// 
			// previewfmod1
			// 
			previewfmod1.BackColor = System.Drawing.SystemColors.ControlDark;
			resources.ApplyResources(previewfmod1, "previewfmod1");
			previewfmod1.Name = "previewfmod1";
			// 
			// PreviewPanel
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(ui_tabRight_tab);
			Name = "PreviewPanel";
			ui_tabRight_tab.ResumeLayout(false);
			ui_tabRight_page0.ResumeLayout(false);
			ui_tabRight_page0.PerformLayout();
			ui_tabRight_page1.ResumeLayout(false);
			ui_tabRight_page1.PerformLayout();
			ui_tabRight_page3.ResumeLayout(false);
			ui_tabRight_page3.PerformLayout();
			tabPage1.ResumeLayout(false);
			tabPage2.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TabControl ui_tabRight_tab;
		private System.Windows.Forms.TabPage ui_tabRight_page0;
		private System.Windows.Forms.TabPage ui_tabRight_page1;
		private System.Windows.Forms.TabPage ui_tabRight_page3;
		private System.Windows.Forms.Label ui_tabRight_page0_assetInfoLabel;
		private System.Windows.Forms.TextBox ui_tabRight_page1_dumpTextBox;
		private System.Windows.Forms.TextBox ui_tabRight_page0_textPreviewBox;
		private System.Windows.Forms.RichTextBox ui_tabRight_page0_fontPreviewBox;
		private System.Windows.Forms.TextBox ui_tabRight_page2_dumpJsonTextBox;
		private System.Windows.Forms.TextBox ui_tabRight_page0_classTextPreview;
		private PreviewImage previewImage1;
		private System.Windows.Forms.TabPage tabPage1;
		private PreviewGL previewgl1;
		private System.Windows.Forms.TabPage tabPage2;
		private PreviewFMOD previewfmod1;
	}
}
