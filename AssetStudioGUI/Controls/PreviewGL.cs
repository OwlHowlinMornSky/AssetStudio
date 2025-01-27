using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.WinForms;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace AssetStudioGUI.Controls {

	public partial class PreviewGL : UserControl {

		public PreviewGL() {
			InitializeComponent();

			GLInit();
		}

		public void PreviewMesh(AssetStudio.Mesh m_Mesh) {
			#region Vertices
			if (m_Mesh.m_Vertices == null || m_Mesh.m_Vertices.Length == 0) {
				//StatusStripUpdate("Mesh can't be previewed.");
				label1.Text = Properties.Strings.Preview_GL_unable1;
				return;
			}
			int count = 3;
			if (m_Mesh.m_Vertices.Length == m_Mesh.m_VertexCount * 4) {
				count = 4;
			}
			vertexData = new Vector3[m_Mesh.m_VertexCount];
			// Calculate Bounding
			float[] min = new float[3];
			float[] max = new float[3];
			for (int i = 0; i < 3; i++) {
				min[i] = m_Mesh.m_Vertices[i];
				max[i] = m_Mesh.m_Vertices[i];
			}
			for (int v = 0; v < m_Mesh.m_VertexCount; v++) {
				for (int i = 0; i < 3; i++) {
					min[i] = Math.Min(min[i], m_Mesh.m_Vertices[v * count + i]);
					max[i] = Math.Max(max[i], m_Mesh.m_Vertices[v * count + i]);
				}
				vertexData[v] = new Vector3(
					m_Mesh.m_Vertices[v * count],
					m_Mesh.m_Vertices[v * count + 1],
					-m_Mesh.m_Vertices[v * count + 2]);
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
			GL_updateViewMatrix();

			#endregion

			#region Indicies
			indiceData = new int[m_Mesh.m_Indices.Count];
			for (int i = 0; i < m_Mesh.m_Indices.Count; i = i + 3) {
				indiceData[i] = (int)m_Mesh.m_Indices[i + 2];
				indiceData[i + 1] = (int)m_Mesh.m_Indices[i + 1];
				indiceData[i + 2] = (int)m_Mesh.m_Indices[i];
			}
			#endregion
			#region Normals
			if (m_Mesh.m_Normals != null && m_Mesh.m_Normals.Length > 0) {
				if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 3)
					count = 3;
				else if (m_Mesh.m_Normals.Length == m_Mesh.m_VertexCount * 4)
					count = 4;
				normalData = new Vector3[m_Mesh.m_VertexCount];
				for (int n = 0; n < m_Mesh.m_VertexCount; n++) {
					normalData[n] = new Vector3(
						m_Mesh.m_Normals[n * count],
						m_Mesh.m_Normals[n * count + 1],
						-m_Mesh.m_Normals[n * count + 2]);
				}
			}
			else
				normalData = null;
			// calculate normal by ourself
			normal2Data = new Vector3[m_Mesh.m_VertexCount];
			int[] normalCalculatedCount = new int[m_Mesh.m_VertexCount];
			for (int i = 0; i < m_Mesh.m_VertexCount; i++) {
				normal2Data[i] = Vector3.Zero;
				normalCalculatedCount[i] = 0;
			}
			for (int i = 0; i < m_Mesh.m_Indices.Count; i = i + 3) {
				Vector3 dir1 = vertexData[indiceData[i + 1]] - vertexData[indiceData[i]];
				Vector3 dir2 = vertexData[indiceData[i + 2]] - vertexData[indiceData[i]];
				Vector3 normal = Vector3.Cross(dir1, dir2);
				normal.Normalize();
				for (int j = 0; j < 3; j++) {
					normal2Data[indiceData[i + j]] += normal;
					normalCalculatedCount[indiceData[i + j]]++;
				}
			}
			for (int i = 0; i < m_Mesh.m_VertexCount; i++) {
				if (normalCalculatedCount[i] == 0)
					normal2Data[i] = new Vector3(0, 1, 0);
				else
					normal2Data[i] /= normalCalculatedCount[i];
			}
			#endregion
			#region Colors
			if (m_Mesh.m_Colors != null && m_Mesh.m_Colors.Length == m_Mesh.m_VertexCount * 3) {
				colorData = new Vector4[m_Mesh.m_VertexCount];
				for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
					colorData[c] = new Vector4(
						m_Mesh.m_Colors[c * 3],
						m_Mesh.m_Colors[c * 3 + 1],
						m_Mesh.m_Colors[c * 3 + 2],
						1.0f);
				}
			}
			else if (m_Mesh.m_Colors != null && m_Mesh.m_Colors.Length == m_Mesh.m_VertexCount * 4) {
				colorData = new Vector4[m_Mesh.m_VertexCount];
				for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
					colorData[c] = new Vector4(
					m_Mesh.m_Colors[c * 4],
					m_Mesh.m_Colors[c * 4 + 1],
					m_Mesh.m_Colors[c * 4 + 2],
					m_Mesh.m_Colors[c * 4 + 3]);
				}
			}
			else {
				colorData = new Vector4[m_Mesh.m_VertexCount];
				for (int c = 0; c < m_Mesh.m_VertexCount; c++) {
					colorData[c] = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
				}
			}
			#endregion
			GL_CreateVAO();
			//StatusStripUpdate(String.Format(Properties.Strings.Preview_GL_info, GL.GetString(StringName.Version)));

			GL_InitMatrices();
		}

		private int mdx, mdy;
		private bool lmdown, rmdown;
		private int pgmID, pgmColorID, pgmBlackID;
		private int attributeVertexPosition;
		private int attributeNormalDirection;
		private int attributeVertexColor;
		private int uniformModelMatrix;
		private int uniformViewMatrix;
		private int uniformProjMatrix;
		private int vao;
		private Vector3[] vertexData;
		private Vector3[] normalData;
		private Vector3[] normal2Data;
		private Vector4[] colorData;
		private Matrix4 modelMatrixData;
		private Matrix4 viewMatrixData;
		private Matrix4 projMatrixData;
		private Matrix4 viewRotMatrix;

		private float m_modelScale;
		private Vector3 m_modelRot;
		private Vector3 m_cameraPos;
		private Vector3 m_cameraRot;
		private float m_cameraFOV;
		private float m_defaultCameraDis;

		private int[] indiceData;
		private int wireFrameMode = 2;
		private int shadeMode;
		private int normalMode;

		private void GLInit() {
			m_modelScale = 1.0f;
			m_modelRot = Vector3.Zero;
			m_cameraPos = Vector3.Zero;
			m_cameraRot = Vector3.Zero;
			m_cameraFOV = (float)Math.PI / 4.0f;
		}

		private void OpenTK_Init() {
			GL_ChangeSize(ui_tabRight_page0_glPreview.Size);
			GL.ClearColor(System.Drawing.Color.CadetBlue);
			pgmID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmID, out int vsID);
			GL_LoadShader("shader_fs", ShaderType.FragmentShader, pgmID, out int fsID);
			GL.LinkProgram(pgmID);

			pgmColorID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmColorID, out vsID);
			GL_LoadShader("shader_fsColor", ShaderType.FragmentShader, pgmColorID, out fsID);
			GL.LinkProgram(pgmColorID);

			pgmBlackID = GL.CreateProgram();
			GL_LoadShader("shader_vs", ShaderType.VertexShader, pgmBlackID, out vsID);
			GL_LoadShader("shader_fsBlack", ShaderType.FragmentShader, pgmBlackID, out fsID);
			GL.LinkProgram(pgmBlackID);

			attributeVertexPosition = GL.GetAttribLocation(pgmID, "vertexPosition");
			attributeNormalDirection = GL.GetAttribLocation(pgmID, "normalDirection");
			attributeVertexColor = GL.GetAttribLocation(pgmColorID, "vertexColor");
			uniformModelMatrix = GL.GetUniformLocation(pgmID, "modelMatrix");
			uniformViewMatrix = GL.GetUniformLocation(pgmID, "viewMatrix");
			uniformProjMatrix = GL.GetUniformLocation(pgmID, "projMatrix");
		}

		private static void GL_LoadShader(string filename, ShaderType type, int program, out int address) {
			address = GL.CreateShader(type);
			var str = (string)Properties.Resources.ResourceManager.GetObject(filename);
			GL.ShaderSource(address, str);
			GL.CompileShader(address);
			GL.AttachShader(program, address);
			GL.DeleteShader(address);
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

		private void GL_CreateVAO() {
			GL.DeleteVertexArray(vao);
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			GL_CreateVBO(out var vboPositions, vertexData, attributeVertexPosition);
			if (normalMode == 0) {
				GL_CreateVBO(out var vboNormals, normal2Data, attributeNormalDirection);
			}
			else {
				if (normalData != null)
					GL_CreateVBO(out var vboNormals, normalData, attributeNormalDirection);
			}
			GL_CreateVBO(out var vboColors, colorData, attributeVertexColor);
			GL_CreateVBO(out var vboModelMatrix, modelMatrixData, uniformModelMatrix);
			GL_CreateVBO(out var vboViewMatrix, viewMatrixData, uniformViewMatrix);
			GL_CreateVBO(out var vboProjMatrix, projMatrixData, uniformProjMatrix);
			GL_CreateEBO(out var eboElements, indiceData);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		private void GL_ChangeSize(Size size) {
			GL.Viewport(0, 0, size.Width, size.Height);
			GL_updateProjMatrix(size);
		}

		private void GL_InitMatrices() {
			m_cameraPos = new Vector3(0.0f, 0.0f, 64.0f);
			m_cameraRot = Vector3.Zero;
			m_modelRot = Vector3.Zero;
			m_modelScale = 1.0f;
			m_cameraFOV = (float)Math.PI / 4.0f;
			GL_updateModelMatrix();
			GL_updateViewMatrix();
		}

		private void GL_updateModelMatrix() {
			var modelRotMatrix =
				Matrix4.CreateRotationZ(m_modelRot.Z) *
				Matrix4.CreateRotationX(m_modelRot.X) *
				Matrix4.CreateRotationY(m_modelRot.Y);
			modelMatrixData = Matrix4.CreateScale(m_modelScale * 64.0f / m_defaultCameraDis) * modelRotMatrix;
		}

		private void GL_updateViewMatrix() {
			viewRotMatrix =
				Matrix4.CreateRotationY(m_cameraRot.Y) *
				Matrix4.CreateRotationX(m_cameraRot.X) *
				Matrix4.CreateRotationZ(m_cameraRot.Z);
			viewMatrixData = Matrix4.CreateTranslation(-m_cameraPos) * viewRotMatrix;
		}

		private void GL_updateProjMatrix(Size size) {
			float k = 1.0f * size.Width / size.Height;
			if (m_cameraFOV > 175.0f) {
				m_cameraFOV = 175.0f;
			}
			else if (m_cameraFOV < 1.0f) {
				m_cameraFOV = 1.0f;
			}
			Matrix4.CreatePerspectiveFieldOfView(m_cameraFOV, k, 0.25f, 256.0f, out projMatrixData);
		}


		private void PreviewGL_Load(object sender, EventArgs e) {
			OpenTK_Init();
		}

		private void Ui_tabRight_page0_glPreview_Paint(object sender, PaintEventArgs e) {
			ui_tabRight_page0_glPreview.MakeCurrent();
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			//GL.Enable(EnableCap.LineSmooth);
			//GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			//GL.Enable(EnableCap.PolygonSmooth);
			//GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

			GL.BindVertexArray(vao);
			switch (wireFrameMode) {
			case 0:
				GL.UseProgram(shadeMode == 0 ? pgmID : pgmColorID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			case 1:
				GL.UseProgram(pgmBlackID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			case 2:
				GL.Enable(EnableCap.PolygonOffsetFill);
				GL.PolygonOffset(1, 1);
				GL.UseProgram(shadeMode == 0 ? pgmID : pgmColorID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				GL.Disable(EnableCap.PolygonOffsetFill);
				GL.UseProgram(pgmBlackID);
				GL.UniformMatrix4(uniformModelMatrix, false, ref modelMatrixData);
				GL.UniformMatrix4(uniformViewMatrix, false, ref viewMatrixData);
				GL.UniformMatrix4(uniformProjMatrix, false, ref projMatrixData);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawElements(BeginMode.Triangles, indiceData.Length, DrawElementsType.UnsignedInt, 0);
				break;
			}

			GL.BindVertexArray(0);
			GL.Flush();
			ui_tabRight_page0_glPreview.SwapBuffers();
		}

		private void Ui_tabRight_page0_glPreview_MouseWheel(object sender, MouseEventArgs e) {
			if (ui_tabRight_page0_glPreview.Visible) {
				Vector4 front = new Vector4(0, 0, -1, 0);
				front = front * viewRotMatrix.Inverted();

				if ((ModifierKeys & System.Windows.Forms.Keys.Control) == System.Windows.Forms.Keys.Control)
					m_cameraPos += front.Xyz * e.Delta / 50.0f;
				else
					m_cameraPos += front.Xyz * e.Delta / 200.0f;

				GL_updateViewMatrix();

				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

		private void PreviewGL_MouseDown(object sender, MouseEventArgs e) {
			mdx = e.X;
			mdy = e.Y;
			if (e.Button == MouseButtons.Left) {
				lmdown = true;
			}
			if (e.Button == MouseButtons.Right) {
				rmdown = true;
			}
		}

		private void PreviewGL_MouseMove(object sender, MouseEventArgs e) {
			if (lmdown || rmdown) {
				float dx = mdx - e.X;
				float dy = mdy - e.Y;
				mdx = e.X;
				mdy = e.Y;
				if (lmdown) {
					m_modelRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_updateModelMatrix();
				}
				if (rmdown) {
					m_cameraRot -= new Vector3(dy * 0.005f, dx * 0.005f, 0.0f);
					GL_updateViewMatrix();
				}
				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

		private void PreviewGL_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				lmdown = false;
			}
			if (e.Button == MouseButtons.Right) {
				rmdown = false;
			}
		}

		private void PreviewGL_KeyDown(object sender, KeyEventArgs e) {
			if (e.Control) {
				switch (e.KeyCode) {
				case Keys.W:
					//Toggle WireFrame
					wireFrameMode = (wireFrameMode + 1) % 3;
					ui_tabRight_page0_glPreview.Invalidate();
					break;
				case Keys.S:
					//Toggle Shade
					shadeMode = (shadeMode + 1) % 2;
					ui_tabRight_page0_glPreview.Invalidate();
					break;
				case Keys.N:
					//Normal mode
					normalMode = (normalMode + 1) % 2;
					GL_CreateVAO();
					ui_tabRight_page0_glPreview.Invalidate();
					break;
				case Keys.R:
					wireFrameMode = 2;
					shadeMode = 0;
					normalMode = 0;
					GL_InitMatrices();
					ui_tabRight_page0_glPreview.Invalidate();
					break;
				}
			}
		}

		private void Ui_tabRight_page0_glPreview_Resize(object sender, EventArgs e) {
			if (glControlLoaded && ui_tabRight_page0_glPreview.Visible) {
				GL_ChangeSize(ui_tabRight_page0_glPreview.Size);
				ui_tabRight_page0_glPreview.Invalidate();
			}
		}

	}

	public class MyGLControl() :
		GLControl(new GLControlSettings { NumberOfSamples = 4 }) {
	}
}
