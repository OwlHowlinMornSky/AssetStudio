namespace AssetStudioGUI {
	partial class PreviewForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewForm));
			ui_tabRight_page3 = new System.Windows.Forms.TabPage();
			ui_tabRight_page2_dumpJsonTextBox = new System.Windows.Forms.TextBox();
			ui_tabRight_page0_assetInfoLabel = new System.Windows.Forms.Label();
			ui_tabRight_page1 = new System.Windows.Forms.TabPage();
			ui_tabRight_page1_dumpTextBox = new System.Windows.Forms.TextBox();
			tabPage_Preview = new System.Windows.Forms.TabPage();
			ui_tabRight_tab = new System.Windows.Forms.TabControl();
			tabPage_info = new System.Windows.Forms.TabPage();
			ui_tabRight_page3.SuspendLayout();
			ui_tabRight_page1.SuspendLayout();
			ui_tabRight_tab.SuspendLayout();
			SuspendLayout();
			// 
			// ui_tabRight_page3
			// 
			resources.ApplyResources(ui_tabRight_page3, "ui_tabRight_page3");
			ui_tabRight_page3.Controls.Add(ui_tabRight_page2_dumpJsonTextBox);
			ui_tabRight_page3.Controls.Add(ui_tabRight_page0_assetInfoLabel);
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
			// ui_tabRight_page1
			// 
			resources.ApplyResources(ui_tabRight_page1, "ui_tabRight_page1");
			ui_tabRight_page1.Controls.Add(ui_tabRight_page1_dumpTextBox);
			ui_tabRight_page1.Name = "ui_tabRight_page1";
			ui_tabRight_page1.UseVisualStyleBackColor = true;
			// 
			// ui_tabRight_page1_dumpTextBox
			// 
			resources.ApplyResources(ui_tabRight_page1_dumpTextBox, "ui_tabRight_page1_dumpTextBox");
			ui_tabRight_page1_dumpTextBox.Name = "ui_tabRight_page1_dumpTextBox";
			// 
			// tabPage_Preview
			// 
			resources.ApplyResources(tabPage_Preview, "tabPage_Preview");
			tabPage_Preview.Name = "tabPage_Preview";
			tabPage_Preview.UseVisualStyleBackColor = true;
			// 
			// ui_tabRight_tab
			// 
			resources.ApplyResources(ui_tabRight_tab, "ui_tabRight_tab");
			ui_tabRight_tab.Controls.Add(tabPage_Preview);
			ui_tabRight_tab.Controls.Add(ui_tabRight_page1);
			ui_tabRight_tab.Controls.Add(ui_tabRight_page3);
			ui_tabRight_tab.Controls.Add(tabPage_info);
			ui_tabRight_tab.Name = "ui_tabRight_tab";
			ui_tabRight_tab.SelectedIndex = 0;
			// 
			// tabPage_info
			// 
			resources.ApplyResources(tabPage_info, "tabPage_info");
			tabPage_info.Name = "tabPage_info";
			tabPage_info.UseVisualStyleBackColor = true;
			// 
			// PreviewForm
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			Controls.Add(ui_tabRight_tab);
			Name = "PreviewForm";
			ShowIcon = false;
			ShowInTaskbar = false;
			FormClosing += PreviewForm_FormClosing;
			ui_tabRight_page3.ResumeLayout(false);
			ui_tabRight_page3.PerformLayout();
			ui_tabRight_page1.ResumeLayout(false);
			ui_tabRight_page1.PerformLayout();
			ui_tabRight_tab.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TabPage ui_tabRight_page3;
		private System.Windows.Forms.TextBox ui_tabRight_page2_dumpJsonTextBox;
		private System.Windows.Forms.Label ui_tabRight_page0_assetInfoLabel;
		private System.Windows.Forms.TabPage ui_tabRight_page1;
		private System.Windows.Forms.TextBox ui_tabRight_page1_dumpTextBox;
		private System.Windows.Forms.TabPage tabPage_Preview;
		private System.Windows.Forms.TabControl ui_tabRight_tab;
		private System.Windows.Forms.TabPage tabPage_info;
	}
}