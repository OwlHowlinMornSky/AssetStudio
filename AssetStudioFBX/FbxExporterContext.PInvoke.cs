using AssetStudio;
using FbxInterop;
using Material = FbxInterop.Material;
using Node = FbxInterop.Node;

namespace AssetStudioFBX {

	internal class ContextS : Context {

		internal Node ExportSingleFrame(Node parentNode, string framePath, string frameName, in Vector3 localPosition, in Vector3 localRotation, in Vector3 localScale) {
			return ExportSingleFrame(parentNode, framePath, frameName, localPosition.X, localPosition.Y, localPosition.Z, localRotation.X, localRotation.Y, localRotation.Z, localScale.X, localScale.Y, localScale.Z);
		}

		internal Material CreateMaterial(string matName, in Color diffuse, in Color ambient, in Color emissive, in Color specular, in Color reflection, float shininess, float transparency) {
			return CreateMaterial(matName, diffuse.R, diffuse.G, diffuse.B, ambient.R, ambient.G, ambient.B, emissive.R, emissive.G, emissive.B, specular.R, specular.G, specular.B, reflection.R, reflection.G, reflection.B, shininess, transparency);
		}

	}

}
