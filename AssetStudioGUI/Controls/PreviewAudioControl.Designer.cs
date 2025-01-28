namespace AssetStudioGUI.Controls {
	partial class PreviewAudioControl {
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewAudioControl));
			label_playing = new System.Windows.Forms.Label();
			label_paused = new System.Windows.Forms.Label();
			button_resume = new System.Windows.Forms.Button();
			label_duration = new System.Windows.Forms.Label();
			label_copyright = new System.Windows.Forms.Label();
			label_sampleRate = new System.Windows.Forms.Label();
			label_timer = new System.Windows.Forms.Label();
			label_stopped = new System.Windows.Forms.Label();
			progressBar1 = new System.Windows.Forms.TrackBar();
			volumeBar = new System.Windows.Forms.TrackBar();
			button_loop = new System.Windows.Forms.CheckBox();
			button_stop = new System.Windows.Forms.Button();
			button_pause = new System.Windows.Forms.Button();
			button_play = new System.Windows.Forms.Button();
			timer1 = new System.Windows.Forms.Timer(components);
			((System.ComponentModel.ISupportInitialize)progressBar1).BeginInit();
			((System.ComponentModel.ISupportInitialize)volumeBar).BeginInit();
			SuspendLayout();
			// 
			// label_playing
			// 
			resources.ApplyResources(label_playing, "label_playing");
			label_playing.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_playing.Name = "label_playing";
			// 
			// label_paused
			// 
			resources.ApplyResources(label_paused, "label_paused");
			label_paused.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_paused.Name = "label_paused";
			// 
			// button_resume
			// 
			resources.ApplyResources(button_resume, "button_resume");
			button_resume.Name = "button_resume";
			button_resume.UseVisualStyleBackColor = true;
			button_resume.Click += Button_Pause_Click;
			// 
			// label_duration
			// 
			resources.ApplyResources(label_duration, "label_duration");
			label_duration.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_duration.Name = "label_duration";
			// 
			// label_copyright
			// 
			resources.ApplyResources(label_copyright, "label_copyright");
			label_copyright.ForeColor = System.Drawing.SystemColors.ControlLight;
			label_copyright.Name = "label_copyright";
			// 
			// label_sampleRate
			// 
			resources.ApplyResources(label_sampleRate, "label_sampleRate");
			label_sampleRate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_sampleRate.Name = "label_sampleRate";
			// 
			// label_timer
			// 
			resources.ApplyResources(label_timer, "label_timer");
			label_timer.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_timer.Name = "label_timer";
			// 
			// label_stopped
			// 
			resources.ApplyResources(label_stopped, "label_stopped");
			label_stopped.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			label_stopped.Name = "label_stopped";
			// 
			// progressBar1
			// 
			resources.ApplyResources(progressBar1, "progressBar1");
			progressBar1.Maximum = 1000;
			progressBar1.Name = "progressBar1";
			progressBar1.TickStyle = System.Windows.Forms.TickStyle.None;
			progressBar1.Scroll += ProgressBar_Scroll;
			progressBar1.MouseDown += ProgressBar_MouseDown;
			progressBar1.MouseUp += ProgressBar_MouseUp;
			// 
			// volumeBar
			// 
			resources.ApplyResources(volumeBar, "volumeBar");
			volumeBar.LargeChange = 2;
			volumeBar.Name = "volumeBar";
			volumeBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			volumeBar.Value = 8;
			volumeBar.ValueChanged += VolumeBar_ValueChanged;
			// 
			// button_loop
			// 
			resources.ApplyResources(button_loop, "button_loop");
			button_loop.Name = "button_loop";
			button_loop.UseVisualStyleBackColor = true;
			button_loop.CheckedChanged += Button_Loop_CheckedChanged;
			// 
			// button_stop
			// 
			resources.ApplyResources(button_stop, "button_stop");
			button_stop.Name = "button_stop";
			button_stop.UseVisualStyleBackColor = true;
			button_stop.Click += Button_Stop_Click;
			// 
			// button_pause
			// 
			resources.ApplyResources(button_pause, "button_pause");
			button_pause.Name = "button_pause";
			button_pause.UseVisualStyleBackColor = true;
			button_pause.Click += Button_Pause_Click;
			// 
			// button_play
			// 
			resources.ApplyResources(button_play, "button_play");
			button_play.Name = "button_play";
			button_play.UseVisualStyleBackColor = true;
			button_play.Click += Button_Play_Click;
			// 
			// timer1
			// 
			timer1.Tick += Timer_Tick;
			// 
			// PreviewAudioControl
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.ControlDark;
			Controls.Add(label_playing);
			Controls.Add(label_paused);
			Controls.Add(button_resume);
			Controls.Add(label_duration);
			Controls.Add(label_copyright);
			Controls.Add(label_sampleRate);
			Controls.Add(label_timer);
			Controls.Add(label_stopped);
			Controls.Add(progressBar1);
			Controls.Add(volumeBar);
			Controls.Add(button_loop);
			Controls.Add(button_stop);
			Controls.Add(button_pause);
			Controls.Add(button_play);
			Name = "PreviewAudioControl";
			Load += PreviewFMOD_Load;
			ClientSizeChanged += PreviewFMOD_ClientSizeChanged;
			((System.ComponentModel.ISupportInitialize)progressBar1).EndInit();
			((System.ComponentModel.ISupportInitialize)volumeBar).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label_playing;
		private System.Windows.Forms.Label label_paused;
		private System.Windows.Forms.Button button_resume;
		private System.Windows.Forms.Label label_duration;
		private System.Windows.Forms.Label label_copyright;
		private System.Windows.Forms.Label label_sampleRate;
		private System.Windows.Forms.Label label_timer;
		private System.Windows.Forms.Label label_stopped;
		private System.Windows.Forms.TrackBar progressBar1;
		private System.Windows.Forms.TrackBar volumeBar;
		private System.Windows.Forms.CheckBox button_loop;
		private System.Windows.Forms.Button button_stop;
		private System.Windows.Forms.Button button_pause;
		private System.Windows.Forms.Button button_play;
		private System.Windows.Forms.Timer timer1;
	}
}
