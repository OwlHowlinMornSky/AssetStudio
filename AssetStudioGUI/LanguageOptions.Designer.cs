namespace AssetStudioGUI {
	partial class LanguageOptions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageOptions));
			lang_0 = new System.Windows.Forms.RadioButton();
			lang_en_us = new System.Windows.Forms.RadioButton();
			lang_zh_cn = new System.Windows.Forms.RadioButton();
			button_ok = new System.Windows.Forms.Button();
			button_cancle = new System.Windows.Forms.Button();
			label_en_US = new System.Windows.Forms.Label();
			label_zh_CN = new System.Windows.Forms.Label();
			label_restart_note = new System.Windows.Forms.Label();
			SuspendLayout();
			// 
			// lang_0
			// 
			resources.ApplyResources(lang_0, "lang_0");
			lang_0.Name = "lang_0";
			lang_0.UseVisualStyleBackColor = true;
			lang_0.CheckedChanged += lang_0_CheckedChanged;
			// 
			// lang_en_us
			// 
			resources.ApplyResources(lang_en_us, "lang_en_us");
			lang_en_us.Name = "lang_en_us";
			lang_en_us.UseVisualStyleBackColor = true;
			lang_en_us.CheckedChanged += lang_en_us_CheckedChanged;
			// 
			// lang_zh_cn
			// 
			resources.ApplyResources(lang_zh_cn, "lang_zh_cn");
			lang_zh_cn.Name = "lang_zh_cn";
			lang_zh_cn.UseVisualStyleBackColor = true;
			lang_zh_cn.CheckedChanged += lang_zh_cn_CheckedChanged;
			// 
			// button_ok
			// 
			resources.ApplyResources(button_ok, "button_ok");
			button_ok.Name = "button_ok";
			button_ok.UseVisualStyleBackColor = true;
			button_ok.Click += button_ok_Click;
			// 
			// button_cancle
			// 
			resources.ApplyResources(button_cancle, "button_cancle");
			button_cancle.Name = "button_cancle";
			button_cancle.UseVisualStyleBackColor = true;
			button_cancle.Click += button_cancle_Click;
			// 
			// label_en_US
			// 
			resources.ApplyResources(label_en_US, "label_en_US");
			label_en_US.Name = "label_en_US";
			// 
			// label_zh_CN
			// 
			resources.ApplyResources(label_zh_CN, "label_zh_CN");
			label_zh_CN.Name = "label_zh_CN";
			// 
			// label_restart_note
			// 
			resources.ApplyResources(label_restart_note, "label_restart_note");
			label_restart_note.Name = "label_restart_note";
			// 
			// LanguageOptions
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(label_restart_note);
			Controls.Add(label_zh_CN);
			Controls.Add(label_en_US);
			Controls.Add(button_cancle);
			Controls.Add(button_ok);
			Controls.Add(lang_zh_cn);
			Controls.Add(lang_en_us);
			Controls.Add(lang_0);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "LanguageOptions";
			ShowIcon = false;
			ShowInTaskbar = false;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.RadioButton lang_0;
		private System.Windows.Forms.RadioButton lang_en_us;
		private System.Windows.Forms.RadioButton lang_zh_cn;
		private System.Windows.Forms.Button button_ok;
		private System.Windows.Forms.Button button_cancle;
		private System.Windows.Forms.Label label_en_US;
		private System.Windows.Forms.Label label_zh_CN;
		private System.Windows.Forms.Label label_restart_note;
	}
}