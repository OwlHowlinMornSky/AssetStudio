using AssetStudio;
using System.Windows.Forms;

namespace AssetStudioGUI {
	internal class GameObjectTreeNode : TreeNode {
		public GameObject gameObject;

		public GameObjectTreeNode(GameObject gameObject) {
			this.gameObject = gameObject;
			Text = gameObject.m_Name;
		}
	}
}
