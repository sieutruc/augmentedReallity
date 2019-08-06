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
    public partial class frmNewNewspaper : Form
    {
        public static CNewspaper news;
        public frmNewNewspaper()
        {
            InitializeComponent();
            nudPages.Value = 1;
        }

        private void New_Load(object sender, EventArgs e)
        {
            cbNewspaper.SelectedIndex = 0;
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
			news = new CNewspaper();
            news.KindOfNews = cbNewspaper.Text;
            news.Number = (int)nudNumber.Value;
            news.NumPages = (int)nudPages.Value;
            news.DefaultH = (int)nudH.Value;
            news.DefaultW = (int)nudW.Value;
            if (news.NumPages == 0)
            {
                MessageBox.Show("Please fill the number of pages!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            news.Datetime = dtpDate.Value.Day.ToString() + "/" +
                dtpDate.Value.Month.ToString() + "/" +
                dtpDate.Value.Year.ToString();
            this.Close();
        }
    }
}
