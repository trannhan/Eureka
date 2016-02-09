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
    public partial class FormScalarInput : Form
    {
        public double Scalar = 0;

        public FormScalarInput()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                Scalar = Convert.ToDouble(this.textBoxScalar.Text);
                this.Hide();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Scalar Input Error: " + exp.Message, "Scalar Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
