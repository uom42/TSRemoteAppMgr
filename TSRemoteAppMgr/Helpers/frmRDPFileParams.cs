using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSRemoteAppMgr.Helpers
{
	internal partial class frmRDPFileParams : Form
	{
		private frmRDPFileParams()
		{
			InitializeComponent();
		}

		public static bool EditObject(object objectToEdit, string title)
		{
			using (frmRDPFileParams rpd = new())
			{
				rpd.Text = title;
				rpd.pg1.SelectedObject = objectToEdit;
				return (rpd.ShowDialog() == DialogResult.OK);
			}
		}
	}
}
