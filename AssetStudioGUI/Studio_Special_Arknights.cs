using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetStudioGUI {
	public partial class Studio_Special_Arknights : Form {
		public Studio_Special_Arknights() {
			InitializeComponent();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e) {
			MessageBox.Show(radioButton1.Checked.ToString(), "TEST");
		}
	}
}
