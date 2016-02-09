using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormNewRankInput : Form
    {
        public int NewRank;
        public int BestRank;

        public FormNewRankInput()
        {
            InitializeComponent();
        }        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            NewRank = (int)numericUpDownNewRank.Value;
            this.Close();
        }

        private void FormNewRankInput_Shown(object sender, EventArgs e)
        {
            textBoxBestRank.Text = BestRank.ToString();
            textBoxOldRank.Text = NewRank.ToString();
            numericUpDownNewRank.Value = BestRank;

        }
    }
}
