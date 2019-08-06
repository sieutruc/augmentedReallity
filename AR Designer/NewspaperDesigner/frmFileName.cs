using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewspaperDesigner
{
	public partial class frmFileName : Form
	{
		public static string fileName;

		public frmFileName()
		{
			InitializeComponent();
			fileName = "";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (tbFileName.Text != "")
			{
				fileName = tbFileName.Text;
				this.Close();
			}
		}
	}
}
