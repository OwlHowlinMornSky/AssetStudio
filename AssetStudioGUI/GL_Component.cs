using OpenTK;
using OpenTK.WinForms;

namespace AssetStudioGUI {
	public class GL_Component : GLControl {
		public GL_Component() : base(new GLControlSettings { NumberOfSamples = 4 }) { }
	}
}
