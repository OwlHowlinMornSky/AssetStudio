using AssetStudio.PInvoke;
using System;
using System.Runtime.InteropServices;

namespace AssetStudio.FbxInterop {
	partial class FbxExporterContext {

		private static IntPtr AsFbxExportSingleFrame(IntPtr context, IntPtr parentNode, string framePath, string frameName, in Vector3 localPosition, in Vector3 localRotation, in Vector3 localScale) {
			return FbxNative.FbxContent.ExportSingleFrame(context, parentNode, framePath, frameName, localPosition.X, localPosition.Y, localPosition.Z, localRotation.X, localRotation.Y, localRotation.Z, localScale.X, localScale.Y, localScale.Z);
		}


		private static IntPtr AsFbxCreateMaterial(IntPtr pContext, string matName, in Color diffuse, in Color ambient, in Color emissive, in Color specular, in Color reflection, float shininess, float transparency) {
			return FbxNative.FbxContent.CreateMaterial(pContext, matName, diffuse.R, diffuse.G, diffuse.B, ambient.R, ambient.G, ambient.B, emissive.R, emissive.G, emissive.B, specular.R, specular.G, specular.B, reflection.R, reflection.G, reflection.B, shininess, transparency);
		}

	}
}
