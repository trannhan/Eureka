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
    public partial class FormMatrixInput : Form
    {
        public MMatrix MainMatrix;
        private int min = -9999999;
        private int max = 9999999;
        const int MaxRow = 500;
        const int MaxCol = 500;
        const int ColWidth = 50;

        public FormMatrixInput()
        {
            InitializeComponent();            
        }        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (MainMatrix != null)
            {
                try
                {
                    for (int i = 0; i < MainMatrix.row; i++)
                        for (int j = 0; j < MainMatrix.col; j++)
                            MainMatrix[i, j] = Convert.ToDouble(dataGridViewMatrix[j, i].Value);
                }
                catch(Exception exp)
                {
                    MessageBox.Show("Input error: " + exp.Message, "Input Matrix", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
            this.Hide();
        }

        private void buttonGenerateMatrix_Click(object sender, EventArgs e)
        {                                  
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
            else if (radioButtonGaussian.Checked)
            {
                double std = 1.4;
                double theta = 0;
                MainMatrix = MMatrix.D2Gauss(rows, std, rows, std, theta);                
                //MainMatrix = MMatrix.D2Gauss_Canny(rows, std, rows, std, theta);  //Sobel kernel              
                cols = rows;
            }

            this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewMatrix.SuspendLayout();            
            dataGridViewMatrix.Columns.Clear();
            /*
            int ColCount = dataGridViewMatrix.Columns.Count;            
            for (int i = 0; i < ColCount; i++)
                dataGridViewMatrix.Columns.RemoveAt(0);           
            */
            DataGridViewColumn[] DCS = new DataGridViewColumn[cols];                                  
            for (int i = 0; i < cols; i++)
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = string.Concat("A", (i + 1).ToString());
                DCS[i] = column;                
            }
            this.dataGridViewMatrix.Columns.AddRange(DCS);
            dataGridViewMatrix.Rows.Add(rows);

            //for DataGrid: [column, row]
            for (int j = 0; j < MainMatrix.col; j++)
                for (int i = 0; i < MainMatrix.row; i++)                            
                    dataGridViewMatrix[j,i].Value = MainMatrix[i, j];

            //this.dataGridViewMatrix.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewMatrix.ResumeLayout();
            numericUpDownCol.Value = cols;            
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
        }
    }
}
