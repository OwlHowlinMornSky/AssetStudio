using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.WinForms;
using OpenTK.Graphics.OpenGL;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;
using Matrix4 = OpenTK.Mathematics.Matrix4;
using AssetStudio;

namespace AssetStudioGUI.Controls {

	public partial class PreviewMeshControl : UserControl, IPreviewControl {

		public PreviewMeshControl() {
			InitializeComponent();
		}

		public void ResetPreview() {
			m_vertexData = null;
			m_indiceData = null;
			m_normalData = null;
			m_normal2Data = null;
			m_colorData = null;

			GL_Reset();
		}

		public void Preview(Mesh mesh) {
			OpenTK_Init();
			GL_Init();

			#region Vertices
			if (mesh.m_Vertices == null || mesh.m_Vertices.Length == 0) {
				//StatusStripUpdate("Mesh can't be previewed.");
				//////////////////////label1.Text = Properties.Strings.Preview_GL_unable1;
				return;
			}
			int count = 3;
			if (mesh.m_Vertices.Length == mesh.m_VertexCount * 4) {
				count = 4;
			}
			m_vertexData = new Vector3[mesh.m_VertexCount];

			// Calculate Bounding
			float[] min = new float[3];
			float[] max = new float[3];
			for (int i = 0; i < 3; i++) {
				min[i] = mesh.m_Vertices[i];
				max[i] = mesh.m_Vertices[i];
			}
			for (int v = 0; v < mesh.m_VertexCount; v++) {
				for (int i = 0; i < 3; i++) {
					min[i] = Math.Min(min[i], mesh.m_Vertices[v * count + i]);
					max[i] = Math.Max(max[i], mesh.m_Vertices[v * count + i]);
				}
				m_vertexData[v] = new Vector3(
					mesh.m_Vertices[v * count],
					mesh.m_Vertices[v * count + 1],
					-mesh.m_Vertices[v * count + 2]);
			}

			// Calculate modelMatrix
			Vector3 dist = Vector3.One;
			for (int i = 0; i < 3; i++) {
				dist[i] = max[i] - min[i];
			}
			float d = Math.Max(1e-5f, dist.Length);
			//if (d > 64.0f)
			//	d = 64.0f;
			m_cameraPos.Z = m_defaultCameraDis = d;
			GL_UpdateViewMatrix();

			#endregion

			#region Indicies
			m_indiceData = new int[mesh.m_Indices.Count];
			for (int i = 0; i < mesh.m_Indices.Count; i = i + 3) {
				m_indiceData[i] = (int)mesh.m_Indices[i + 2];
				m_indiceData[i + 1] = (int)mesh.m_Indices[i + 1];
				m_indiceData[i + 2] = (int)mesh.m_Indices[i];
			}
			#endregion
			#region Normals
			if (mesh.m_Normals != null && mesh.m_Normals.Length > 0) {
				if (mesh.m_Normals.Length == mesh.m_VertexCount * 3)
					count = 3;
				else if (mesh.m_Normals.Length == mesh.m_VertexCount * 4)
					count = 4;
				m_normalData = new Vector3[mesh.m_VertexCount];
				for (int n = 0; n < mesh.m_VertexCount; n++) {
					m_normalData[n] = new Vector3(
						mesh.m_Normals[n * count],
						mesh.m_Normals[n * count + 1],
						-mesh.m_Normals[n * count + 2]);
				}
			}
			else
				m_normalData = null;
			// calculate normal by ourself
			m_normal2Data = new Vector3[mesh.m_VertexCount];
			int[] normalCalculatedCount = new int[mesh.m_VertexCount];
			for (int i = 0; i < mesh.m_VertexCount; i++) {
				m_normal2Data[i] = Vector3.Zero;
				normalCalculatedCount[i] = 0;
			}
			for (int i = 0; i < mesh.m_Indices.Count; i = i + 3) {
				Vector3 dir1 = m_vertexData[m_indiceData[i + 1]] - m_vertexData[m_indiceData[i]];
				Vector3 dir2 = m_vertexData[m_indiceData[i + 2]] - m_vertexData[m_indiceData[i]];
				Vector3 normal = Vector3.Cross(dir1, dir2);
				normal.Normalize();
				for (int j = 0; j < 3; j++) {
					m_normal2Data[m_indiceData[i + j]] += normal;
					normalCalculatedCount[m_indiceData[i + j]]++;
				}
			}
			for (int i = 0; i < mesh.m_VertexCount; i++) {
				if (normalCalculatedCount[i] == 0)
					m_normal2Data[i] = new Vector3(0, 1, 0);
				else
					m_normal2Data[i] /= normalCalculatedCount[i];
			}
			#endregion
			#region Colors
			if (mesh.m_Colors != null && mesh.m_Colors.Length == mesh.m_VertexCount * 3) {
				m_colorData = new Vector4[mesh.m_VertexCount];
				for (int c = 0; c < mesh.m_VertexCount; c++) {
					m_colorData[c] = new Vector4(
						mesh.m_Colors[c * 3],
						mesh.m_Colors[c * 3 + 1],
						mesh.m_Colors[c * 3 + 2],
						1.0f);
				}
			}
			else if (mesh.m_Colors != null && mesh.m_Colors.Length == mesh.m_VertexCount * 4) {
				m_colorData = new Vector4[mesh.m_VertexCount];
				for (int c = 0; c < mesh.m_VertexCount; c++) {
					m_colorData[c] = new Vector4(
					mesh.m_Colors[c * 4],
					mesh.m_Colors[c * 4 + 1],
					mesh.m_Colors[c * 4 + 2],
					mesh.m_Colors[c * 4 + 3]);
				}
			}
			else {
				m_colorData = new Vector4[mesh.m_VertexCount];
				for (int c = 0; c < mesh.m_VertexCount; c++) {
					m_colorData[c] = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
				}
			}
			#endregion
			GL_CreateVAO();
			//StatusStripUpdate(String.Format(Properties.Strings.Preview_GL_info, GL.GetString(StringName.Version)));

			GL_InitMatrices();
		}

		private void PreviewGL_MouseWheel(object sender, MouseEventArgs e) {
			if (myGlControl1.Visible) {
				Vector4 front = new(0, 0, -1, 0);
				front *= m_viewRotMatrix.Inverted();

				if ((ModifierKeys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control)
					m_cameraPos += front.Xyz * e.Delta / 50.0f;
				else
					m_cameraPos += front.Xyz * e.Delta / 200.0f;

				GL_UpdateViewMatrix();

				myGlControl1.Invalidate();
			}
		}

		private void PreviewGL_MouseDown(object sender, MouseEventArgs e) {
			m_mousePreviousLocation = e.Location;
			if (e.Button == MouseButtons.Left) {
				m_mouseLeftDown = true;
			}
			if (e.Button == MouseButtons.Right) {
				m_mouseRightDown = true;
			}
		}

		private void PreviewGL_MouseMove(object sender, MouseEventArgs e) {
			if (m_mouseLeftDown || m_mouseRightDown) {
				float dx = m_mousePreviousLocation.X - e.X;
				float dy = m_mousePreviousLocation.Y - e.Y;
				m_mousePreviousLocation = e.Location;

				if (m_mouseLeftDown) {
					m_modelRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_UpdateModelMatrix();
				}
				if (m_mouseRightDown) {
					m_cameraRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_UpdateViewMatrix();
				}
				myGlControl1.Invalidate();
			}
		}

		private void PreviewGL_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				m_mouseLeftDown = false;
			}
			if (e.Button == MouseButtons.Right) {
				m_mouseRightDown = false;
			}
		}

		private void PreviewGL_KeyDown(object sender, KeyEventArgs e) {
			if (e.Control) {
				switch (e.KeyCode) {
				case Keys.W:
					//Toggle WireFrame
					m_wireFrameMode = (m_wireFrameMode + 1) % 3;
					myGlControl1.Invalidate();
					break;
				case Keys.S:
					//Toggle Shade
					m_shadeMode = (m_shadeMode + 1) % 2;
					myGlControl1.Invalidate();
					break;
				case Keys.N:
					//Normal mode
					m_normalMode = (m_normalMode + 1) % 2;
					GL_CreateVAO();
					myGlControl1.Invalidate();
					break;
				case Keys.R:
					m_wireFrameMode = 2;
					m_shadeMode = 0;
					m_normalMode = 0;
					GL_InitMatrices();
					myGlControl1.Invalidate();
					break;
				}
			}
		}

		private void MyGlControl_Resize(object sender, EventArgs e) {
			GL_ChangeSize(myGlControl1.ClientSize);
			myGlControl1.Invalidate();
		}

		private void MyGlControl_Paint(object sender, PaintEventArgs e) {
			if (myGlControl1.IsDesignMode)
				return;

			myGlControl1.MakeCurrent();

			GL.Viewport(0, 0, myGlControl1.ClientSize.Width, myGlControl1.ClientSize.Height);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			//GL.Enable(EnableCap.LineSmooth);
			//GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			//GL.Enable(EnableCap.PolygonSmooth);
			//GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

			if (m_vao != 0) {
				GL.BindVertexArray(m_vao);
				switch (m_wireFrameMode) {
				case 0: {
					ShaderProgram shader = m_shadeMode == 0 ? m_pgNormal : m_pgColor;
					GL.UseProgram(shader.Program);
					shader.UpdateMatrices(ref m_projMatrixData, ref m_viewMatrixData, ref m_modelMatrixData);
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
					GL.DrawElements(BeginMode.Triangles, m_indiceData.Length, DrawElementsType.UnsignedInt, 0);
					break;
				}
				case 1:
					GL.UseProgram(m_pgBlack.Program);
					m_pgBlack.UpdateMatrices(ref m_projMatrixData, ref m_viewMatrixData, ref m_modelMatrixData);
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
					GL.DrawElements(BeginMode.Triangles, m_indiceData.Length, DrawElementsType.UnsignedInt, 0);
					break;
				case 2: {
					GL.Enable(EnableCap.PolygonOffsetFill);
					GL.PolygonOffset(1, 1);
					ShaderProgram shader = m_shadeMode == 0 ? m_pgNormal : m_pgColor;
					GL.UseProgram(shader.Program);
					shader.UpdateMatrices(ref m_projMatrixData, ref m_viewMatrixData, ref m_modelMatrixData);
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
					GL.DrawElements(BeginMode.Triangles, m_indiceData.Length, DrawElementsType.UnsignedInt, 0);

					GL.Disable(EnableCap.PolygonOffsetFill);
					GL.UseProgram(m_pgBlack.Program);
					m_pgBlack.UpdateMatrices(ref m_projMatrixData, ref m_viewMatrixData, ref m_modelMatrixData);
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
					GL.DrawElements(BeginMode.Triangles, m_indiceData.Length, DrawElementsType.UnsignedInt, 0);
					break;
				}
				}

				GL.BindVertexArray(0);
			}

			GL.Flush();
			myGlControl1.SwapBuffers();
		}

		/// <summary>
		/// For moving and rotating.
		/// </summary>
		private Point m_mousePreviousLocation;
		private bool m_mouseLeftDown, m_mouseRightDown;

		/// <summary>
		/// For shader programs.
		/// </summary>
		private ShaderProgram m_pgNormal, m_pgColor, m_pgBlack;
		internal class ShaderProgram() : IDisposable {
			private int m_programID = -1;

			private int m_programAttributeVertexPosition = -1;
			private int m_programAttributeNormalDirection = -1;
			private int m_programAttributeVertexColor = -1;
			private int m_programUniformModelMatrix = -1;
			private int m_programUniformViewMatrix = -1;
			private int m_programUniformProjMatrix = -1;

			public int Program {
				get {
					return m_programID;
				}
			}

			public int AttribVtxPos {
				get {
					return m_programAttributeVertexPosition;
				}
			}
			public int AttribNmlDir {
				get {
					return m_programAttributeNormalDirection;
				}
			}
			public int AttribVtxClr {
				get {
					return m_programAttributeVertexColor;
				}
			}

			public int UniMatM {
				get {
					return m_programUniformModelMatrix;
				}
			}
			public int UniMatV {
				get {
					return m_programUniformViewMatrix;
				}
			}
			public int UniMatP {
				get {
					return m_programUniformProjMatrix;
				}
			}

			public void Load(string vsName, string fsName) {
				m_programID = GL.CreateProgram();
				LoadShader(vsName, ShaderType.VertexShader, m_programID, out int vsId);
				LoadShader(fsName, ShaderType.FragmentShader, m_programID, out int fsId);
				GL.LinkProgram(m_programID);

				GL.DetachShader(m_programID, vsId);
				GL.DetachShader(m_programID, fsId);

				m_programAttributeVertexPosition = GL.GetAttribLocation(m_programID, "vertexPosition");
				m_programAttributeNormalDirection = GL.GetAttribLocation(m_programID, "normalDirection");
				m_programAttributeVertexColor = GL.GetAttribLocation(m_programID, "vertexColor");

				m_programUniformModelMatrix = GL.GetUniformLocation(m_programID, "modelMatrix");
				m_programUniformViewMatrix = GL.GetUniformLocation(m_programID, "viewMatrix");
				m_programUniformProjMatrix = GL.GetUniformLocation(m_programID, "projMatrix");
			}

			public void UpdateMatrices(ref Matrix4 p, ref Matrix4 v, ref Matrix4 m) {
				GL.UniformMatrix4(m_programUniformModelMatrix, false, ref m);
				GL.UniformMatrix4(m_programUniformViewMatrix, false, ref v);
				GL.UniformMatrix4(m_programUniformProjMatrix, false, ref p);
			}

			private static void LoadShader(string filename, ShaderType type, int program, out int address) {
				address = GL.CreateShader(type);
				var str = (string)Properties.Resources.ResourceManager.GetObject(filename);
				GL.ShaderSource(address, str);
				GL.CompileShader(address);
				GL.AttachShader(program, address);
				GL.DeleteShader(address);
			}

			~ShaderProgram() {
				Dispose(false);
			}

			public void Dispose() {
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			private bool m_disposed = false;
			protected virtual void Dispose(bool disposing) {
				if (m_disposed)
					return;
				m_disposed = true;

				if (m_programID != -1)
					GL.DeleteProgram(m_programID);
				m_programID = -1;
			}
		}

		/// <summary>
		/// For model.
		/// </summary>
		private int m_vao = 0;
		private Vector3[] m_vertexData;
		private int[] m_indiceData;
		private Vector3[] m_normalData;
		private Vector3[] m_normal2Data;
		private Vector4[] m_colorData;
		private Matrix4 m_modelMatrixData;
		private Matrix4 m_viewMatrixData;
		private Matrix4 m_projMatrixData;
		private Matrix4 m_viewRotMatrix;

		private int vboPositions, vboNormals, vboColors, vboModelMatrix, vboViewMatrix, vboProjMatrix, eboElements;

		private float m_modelScale;
		private Vector3 m_modelRot;
		private Vector3 m_cameraPos;
		private Vector3 m_cameraRot;
		private float m_cameraFOV;
		private float m_defaultCameraDis;

		private int m_wireFrameMode = 2; // Face = 0, LineFrame, FaceAndLine;
		private int m_shadeMode; // Color = 0, Light;
		private int m_normalMode;

		private void OpenTK_Init() {
			if (myGlControl1.IsDesignMode)
				return;

			GL_ChangeSize(myGlControl1.Size);
			GL.ClearColor(System.Drawing.Color.CadetBlue);

			m_pgNormal ??= new ShaderProgram();
			m_pgNormal.Load("shader_vs", "shader_fs");
			m_pgColor ??= new ShaderProgram();
			m_pgColor.Load("shader_vs", "shader_fsColor");
			m_pgBlack ??= new ShaderProgram();
			m_pgBlack.Load("shader_vs", "shader_fsBlack");
		}

		private void GL_Init() {
			m_modelScale = 1.0f;
			m_modelRot = Vector3.Zero;
			m_cameraPos = Vector3.Zero;
			m_cameraRot = Vector3.Zero;
			m_cameraFOV = (float)Math.PI / 4.0f;
		}

		private void GL_Reset() {
			if (myGlControl1.IsDesignMode)
				return;

			m_pgNormal?.Dispose();
			m_pgNormal = null;
			m_pgColor?.Dispose();
			m_pgColor = null;
			m_pgBlack?.Dispose();
			m_pgBlack = null;

			GL.DeleteBuffers(
				7,
				[vboPositions, vboNormals, vboColors, vboModelMatrix, vboViewMatrix, vboProjMatrix, eboElements]
			);

			if (m_vao != 0)
				GL.DeleteVertexArray(m_vao);
		}

		private void GL_InitMatrices() {
			m_cameraPos = new Vector3(0.0f, 0.0f, 64.0f);
			m_cameraRot = Vector3.Zero;
			m_modelRot = Vector3.Zero;
			m_modelScale = 1.0f;
			m_cameraFOV = (float)Math.PI / 4.0f;
			GL_UpdateModelMatrix();
			GL_UpdateViewMatrix();
		}

		private void GL_CreateVAO() {
			if (myGlControl1.IsDesignMode)
				return;

			if (m_vao != 0)
				GL.DeleteVertexArray(m_vao);
			GL.GenVertexArrays(1, out m_vao);
			GL.BindVertexArray(m_vao);

			GL_CreateVBO(out vboPositions, m_vertexData, m_pgNormal.AttribVtxPos);
			if (m_normalMode == 0) {
				GL_CreateVBO(out vboNormals, m_normal2Data, m_pgNormal.AttribNmlDir);
			}
			else if (m_normalData != null) {
				GL_CreateVBO(out vboNormals, m_normalData, m_pgNormal.AttribNmlDir);
			}
			GL_CreateVBO(out vboColors, m_colorData, m_pgNormal.AttribVtxClr);

			GL_CreateVBO(out vboModelMatrix, m_modelMatrixData, m_pgNormal.UniMatM);
			GL_CreateVBO(out vboViewMatrix, m_viewMatrixData, m_pgNormal.UniMatV);
			GL_CreateVBO(out vboProjMatrix, m_projMatrixData, m_pgNormal.UniMatP);

			GL_CreateEBO(out eboElements, m_indiceData);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		private static void GL_CreateVBO(out int vboAddress, Vector3[] data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboAddress);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				(IntPtr)(data.Length * Vector3.SizeInBytes),
				data,
				BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(address, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(address);
		}

		private static void GL_CreateVBO(out int vboAddress, Vector4[] data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboAddress);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				(IntPtr)(data.Length * Vector4.SizeInBytes),
				data,
				BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(address, 4, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(address);
		}

		private static void GL_CreateVBO(out int vboAddress, Matrix4 data, int address) {
			GL.GenBuffers(1, out vboAddress);
			GL.UniformMatrix4(address, false, ref data);
		}

		private static void GL_CreateEBO(out int address, int[] data) {
			GL.GenBuffers(1, out address);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, address);
			GL.BufferData(
				BufferTarget.ElementArrayBuffer,
				(IntPtr)(data.Length * sizeof(int)),
				data,
				BufferUsageHint.StaticDraw
			);
		}

		private void GL_ChangeSize(Size size) {
			if (myGlControl1.IsDesignMode)
				return;

			GL_UpdateProjMatrix(size);
		}

		private void GL_UpdateModelMatrix() {
			var modelRotMatrix =
				Matrix4.CreateRotationZ(m_modelRot.Z) *
				Matrix4.CreateRotationX(m_modelRot.X) *
				Matrix4.CreateRotationY(m_modelRot.Y);
			m_modelMatrixData = Matrix4.CreateScale(m_modelScale * 64.0f / m_defaultCameraDis) * modelRotMatrix;
		}

		private void GL_UpdateViewMatrix() {
			m_viewRotMatrix =
				Matrix4.CreateRotationY(m_cameraRot.Y) *
				Matrix4.CreateRotationX(m_cameraRot.X) *
				Matrix4.CreateRotationZ(m_cameraRot.Z);
			m_viewMatrixData = Matrix4.CreateTranslation(-m_cameraPos) * m_viewRotMatrix;
		}

		private void GL_UpdateProjMatrix(Size size) {
			float k = 1.0f * size.Width / size.Height;
			if (m_cameraFOV > 175.0f) {
				m_cameraFOV = 175.0f;
			}
			else if (m_cameraFOV < 1.0f) {
				m_cameraFOV = 1.0f;
			}
			Matrix4.CreatePerspectiveFieldOfView(m_cameraFOV, k, 0.25f, 256.0f, out m_projMatrixData);
		}
	}

	public class MyGLControl : GLControl {
		public MyGLControl() : base(new GLControlSettings { NumberOfSamples = 4 }) {
		}
	}
}
