using System.IO;
using AssetStudioFBX;

namespace AssetStudio {
	public static class ModelExporter {
		public static void ExportFbx(string path, IImported imported, bool eulerFilter, float filterPrecision,
			bool allNodes, bool skins, bool animation, bool blendShape, bool castToBone, float boneSize, bool exportAllUvsAsDiffuseMaps, float scaleFactor, int versionIndex, bool isAscii) {
			var file = new FileInfo(path);
			var dir = file.Directory;

			if (!dir.Exists) {
				dir.Create();
			}

			var currentDir = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(dir.FullName);

			var name = Path.GetFileName(path);

			using (var exporter = new FbxExporter(name, imported, allNodes, skins, castToBone, boneSize, exportAllUvsAsDiffuseMaps, scaleFactor, versionIndex, isAscii)) {
				exporter.Initialize();
				exporter.ExportAll(blendShape, animation, eulerFilter, filterPrecision);
			}

			Directory.SetCurrentDirectory(currentDir);
		}
	}
}
