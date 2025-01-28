namespace AssetStudioGUI.Controls {
	partial class PreviewTextControl {
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
			textBox1 = new System.Windows.Forms.TextBox();
			SuspendLayout();
			// 
			// textBox1
			// 
			textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			textBox1.Location = new System.Drawing.Point(0, 0);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(546, 308);
			textBox1.TabIndex = 0;
			textBox1.Text = "[No text to show]";
			// 
			// PreviewText
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(textBox1);
			Name = "PreviewText";
			Size = new System.Drawing.Size(546, 308);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
	}
}
