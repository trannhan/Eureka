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
    public partial class FormSystemOfEquationsInput : Form
    {
        public MMatrix MainMatrix;
        public Vector b;
        private int min = -9999999;
        private int max = 9999999;
        private int MaxRow = 500;
        private int MaxCol = 500;
        private int ColWidth = 50;

        public FormSystemOfEquationsInput()
        {
            InitializeComponent();            
        }        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ((MainMatrix != null) && (b != null))
            {
                try
                {
                    for (int i = 0; i < MainMatrix.row; i++)
                        for (int j = 0; j < MainMatrix.col; j++)
                            MainMatrix[i, j] = Convert.ToDouble(dataGridViewMatrix[j, i].Value);

                    for (int i = 0; i < b.Elements.Length; i++)
                        b[i] = Convert.ToDouble(dataGridViewVectorb[0, i].Value);
                }
                catch(Exception exp)
                {
                    MessageBox.Show("Input error: " + exp.Message, "Input System of Equation: Ax = b", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
            this.Hide();
        }

        private void buttonGenerateMatrix_Click(object sender, EventArgs e)
        {                       
            string sColHeader;            
            Random rand = new Random();
            int rows = (int)numericUpDownRow.Value;
            int cols = (int)numericUpDownCol.Value;

            min = (int)numericUpDownMin.Value;
            max = (int)numericUpDownMax.Value;

            if (radioButtonRandom.Checked)
                MainMatrix = MMatrix.RandomMatrix(rows, cols, min, max);
            else if (radioButtonDiagonal.Checked)
            {
                MainMatrix = MMatrix.DiagonalMatrix(rows, rand.Next(max));
                cols = rows;
            }
            else if (radioButtonIdentity.Checked)
            {
                MainMatrix = MMatrix.DiagonalMatrix(rows, 1);
                cols = rows;
            }
            else if (radioButtonSymetric.Checked)
            {
                MainMatrix = MMatrix.RandomMatrix(rows, cols, min, (int)Math.Sqrt(max));
                MainMatrix = MainMatrix * MainMatrix.Transpose();
                cols = rows;
            }

            //Matrix A
            dataGridViewMatrix.SuspendLayout();
            dataGridViewMatrix.Columns.Clear();            
            for (int i = 0; i < cols; i++)
            {
                sColHeader = "A" + (i + 1).ToString();
                this.dataGridViewMatrix.Columns.Add(sColHeader,sColHeader);
                dataGridViewMatrix.Columns[i].Width = ColWidth;
            }
            
            dataGridViewMatrix.Rows.Add(rows);

            //for DataGrid: [column, row]
            for (int i = 0; i < MainMatrix.row; i++)            
                for (int j = 0; j < MainMatrix.col; j++)
                    dataGridViewMatrix[j,i].Value = MainMatrix[i, j];             
            
            dataGridViewMatrix.ResumeLayout();            
            numericUpDownCol.Value = cols;

            //Vector b
            b = Vector.RandomVector(rows, min, max);
            dataGridViewVectorb.SuspendLayout();
            dataGridViewVectorb.Columns.Clear();
            dataGridViewVectorb.Columns.Add("b","b");
            dataGridViewVectorb.Columns[0].Width = ColWidth;
            dataGridViewVectorb.Rows.Add(rows);
            for (int i = 0; i < b.Elements.Length; i++)
                dataGridViewVectorb[0, i].Value = b[i];
            dataGridViewVectorb.ResumeLayout();  
        }   

        private void numericUpDownRowValueChange(object sender, EventArgs e)
        {
            if (numericUpDownRow.Value < 1)
                numericUpDownRow.Value = 1;
            else if ((numericUpDownRow.Value > MaxRow))
                numericUpDownRow.Value = MaxRow;
        }

        private void numericUpDownColValueChange(object sender, EventArgs e)
        {
            if (numericUpDownCol.Value < 1)
                numericUpDownCol.Value = 1;
            else if ((numericUpDownCol.Value > MaxCol))
                numericUpDownCol.Value = MaxCol;
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            MainMatrix = null;
            dataGridViewMatrix.Columns.Clear();

            b = null;
            dataGridViewVectorb.Columns.Clear();
        }
    }
}
