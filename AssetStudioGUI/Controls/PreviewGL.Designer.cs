using AssetStudioGUI.Properties;

namespace AssetStudioGUI.Controls {
	partial class PreviewGL {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewGL));
			label1 = new System.Windows.Forms.Label();
			ui_tabRight_page0_glPreview = new MyGLControl();
			SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// ui_tabRight_page0_glPreview
			// 
			ui_tabRight_page0_glPreview.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
			ui_tabRight_page0_glPreview.APIVersion = new System.Version(3, 3, 0, 0);
			resources.ApplyResources(ui_tabRight_page0_glPreview, "ui_tabRight_page0_glPreview");
			ui_tabRight_page0_glPreview.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
			ui_tabRight_page0_glPreview.IsEventDriven = true;
			ui_tabRight_page0_glPreview.Name = "ui_tabRight_page0_glPreview";
			ui_tabRight_page0_glPreview.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
			ui_tabRight_page0_glPreview.VSync = false;
			ui_tabRight_page0_glPreview.Paint += Ui_tabRight_page0_glPreview_Paint;
			ui_tabRight_page0_glPreview.MouseWheel += Ui_tabRight_page0_glPreview_MouseWheel;
			// 
			// PreviewGL
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(ui_tabRight_page0_glPreview);
			Controls.Add(label1);
			Name = "PreviewGL";
			Load += PreviewGL_Load;
			KeyDown += PreviewGL_KeyDown;
			MouseDown += PreviewGL_MouseDown;
			MouseMove += PreviewGL_MouseMove;
			MouseUp += PreviewGL_MouseUp;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private MyGLControl ui_tabRight_page0_glPreview;
	}
}
