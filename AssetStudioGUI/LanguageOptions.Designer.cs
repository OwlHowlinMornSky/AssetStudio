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
			ui_lang_en_us = new System.Windows.Forms.RadioButton();
			ui_lang_zh_cn = new System.Windows.Forms.RadioButton();
			ui_button_ok = new System.Windows.Forms.Button();
			ui_button_cancle = new System.Windows.Forms.Button();
			ui_label_en_us = new System.Windows.Forms.Label();
			ui_label_zh_cn = new System.Windows.Forms.Label();
			ui_label_restart_note = new System.Windows.Forms.Label();
			ui_lang_auto = new System.Windows.Forms.RadioButton();
			ui_label_detected = new System.Windows.Forms.Label();
			SuspendLayout();
			// 
			// ui_lang_en_us
			// 
			resources.ApplyResources(ui_lang_en_us, "ui_lang_en_us");
			ui_lang_en_us.Name = "ui_lang_en_us";
			ui_lang_en_us.UseVisualStyleBackColor = true;
			ui_lang_en_us.CheckedChanged += ui_lang_en_us_CheckedChanged;
			// 
			// ui_lang_zh_cn
			// 
			resources.ApplyResources(ui_lang_zh_cn, "ui_lang_zh_cn");
			ui_lang_zh_cn.Name = "ui_lang_zh_cn";
			ui_lang_zh_cn.UseVisualStyleBackColor = true;
			ui_lang_zh_cn.CheckedChanged += ui_lang_zh_cn_CheckedChanged;
			// 
			// ui_button_ok
			// 
			resources.ApplyResources(ui_button_ok, "ui_button_ok");
			ui_button_ok.Name = "ui_button_ok";
			ui_button_ok.UseVisualStyleBackColor = true;
			ui_button_ok.Click += ui_button_ok_Click;
			// 
			// ui_button_cancle
			// 
			resources.ApplyResources(ui_button_cancle, "ui_button_cancle");
			ui_button_cancle.Name = "ui_button_cancle";
			ui_button_cancle.UseVisualStyleBackColor = true;
			ui_button_cancle.Click += ui_button_cancle_Click;
			// 
			// ui_label_en_us
			// 
			resources.ApplyResources(ui_label_en_us, "ui_label_en_us");
			ui_label_en_us.Name = "ui_label_en_us";
			// 
			// ui_label_zh_cn
			// 
			resources.ApplyResources(ui_label_zh_cn, "ui_label_zh_cn");
			ui_label_zh_cn.Name = "ui_label_zh_cn";
			// 
			// ui_label_restart_note
			// 
			resources.ApplyResources(ui_label_restart_note, "ui_label_restart_note");
			ui_label_restart_note.Name = "ui_label_restart_note";
			// 
			// ui_lang_auto
			// 
			resources.ApplyResources(ui_lang_auto, "ui_lang_auto");
			ui_lang_auto.Name = "ui_lang_auto";
			ui_lang_auto.TabStop = true;
			ui_lang_auto.UseVisualStyleBackColor = true;
			ui_lang_auto.CheckedChanged += ui_lang_auto_CheckedChanged;
			// 
			// ui_label_detected
			// 
			resources.ApplyResources(ui_label_detected, "ui_label_detected");
			ui_label_detected.Name = "ui_label_detected";
			// 
			// LanguageOptions
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(ui_label_detected);
			Controls.Add(ui_lang_auto);
			Controls.Add(ui_label_restart_note);
			Controls.Add(ui_label_zh_cn);
			Controls.Add(ui_label_en_us);
			Controls.Add(ui_button_cancle);
			Controls.Add(ui_button_ok);
			Controls.Add(ui_lang_zh_cn);
			Controls.Add(ui_lang_en_us);
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
		private System.Windows.Forms.RadioButton ui_lang_en_us;
		private System.Windows.Forms.RadioButton ui_lang_zh_cn;
		private System.Windows.Forms.Button ui_button_ok;
		private System.Windows.Forms.Button ui_button_cancle;
		private System.Windows.Forms.Label ui_label_en_us;
		private System.Windows.Forms.Label ui_label_zh_cn;
		private System.Windows.Forms.Label ui_label_restart_note;
		private System.Windows.Forms.RadioButton ui_lang_auto;
		private System.Windows.Forms.Label ui_label_detected;
	}
}