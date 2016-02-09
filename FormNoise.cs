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
    public partial class FormNoise : Form
    {
        public double Noise = 0;        

        public FormNoise()
        {
            InitializeComponent();            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                Noise = Convert.ToDouble(this.textBoxNoise.Text);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Noise Input Error: " + exp.Message, "Noise", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       
    }
}
