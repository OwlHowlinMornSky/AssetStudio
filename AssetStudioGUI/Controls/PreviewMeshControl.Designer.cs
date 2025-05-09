﻿using AssetStudioGUI.Properties;

namespace AssetStudioGUI.Controls {
	partial class PreviewMeshControl {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewMeshControl));
			myGlControl1 = new MyGLControl();
			SuspendLayout();
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
			myGlControl1.Paint += MyGlControl_Paint;
			myGlControl1.KeyDown += PreviewGL_KeyDown;
			myGlControl1.MouseDown += PreviewGL_MouseDown;
			myGlControl1.MouseMove += PreviewGL_MouseMove;
			myGlControl1.MouseUp += PreviewGL_MouseUp;
			myGlControl1.MouseWheel += PreviewGL_MouseWheel;
			myGlControl1.Resize += MyGlControl_Resize;
			// 
			// PreviewMeshControl
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(myGlControl1);
			Name = "PreviewMeshControl";
			ResumeLayout(false);
		}

		#endregion
		protected MyGLControl myGlControl1;
	}
}
