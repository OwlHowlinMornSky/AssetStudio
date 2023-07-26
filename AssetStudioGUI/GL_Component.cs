using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.WinForms;

namespace AssetStudioGUI {
	public class GL_Component : GLControl {
		public GL_Component() : base(new GLControlSettings { NumberOfSamples = 4 }) { }
	}
}
