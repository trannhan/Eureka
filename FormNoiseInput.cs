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
    public partial class FormNoiseInput : Form
    {
        public double Noise = 0;
        public int Method = ImageProcessing.A_PIECEWISELINEAR;

        public FormNoiseInput()
        {
            InitializeComponent();
            comboBoxMethod.SelectedIndex = 0;
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

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            Method = comboBoxMethod.SelectedIndex;
            if (Method == 0)
                textBoxNoise.Text = "5";
            else
                textBoxNoise.Text = "0.01";
        }
    }
}
