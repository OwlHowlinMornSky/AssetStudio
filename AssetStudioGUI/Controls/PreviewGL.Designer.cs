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
			myGlControl1 = new MyGLControl();
			SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// myGlControl1
			// 
			myGlControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
			myGlControl1.APIVersion = new System.Version(3, 3, 0, 0);
			resources.ApplyResources(myGlControl1, "myGlControl1");
			myGlControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
			myGlControl1.IsEventDriven = true;
			myGlControl1.Name = "myGlControl1";
			myGlControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
			myGlControl1.VSync = false;
			myGlControl1.Load += MyGlControl_Load;
			myGlControl1.Paint += MyGlControl_Paint;
			myGlControl1.MouseWheel += PreviewGL_MouseWheel;
			myGlControl1.Resize += MyGlControl_Resize;
			// 
			// PreviewGL
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(myGlControl1);
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
		protected MyGLControl myGlControl1;
	}
}
