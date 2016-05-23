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
    public partial class FormGaussianSmooth : Form
    {
        private ImageProcessing ImgProcess = new ImageProcessing();
        private Image OldPic;
        public Image Picture;
        int rows = 0;
        int cols = 0;
        double sigma1 = 0;        
        double sigma2 = 0;
        double theta = 0;

        public FormGaussianSmooth()
        {
            InitializeComponent();            
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = OldPic;

            rows = (int)numericUpDownRow.Value;
            cols = (int)numericUpDownCol.Value;
            sigma1 = Convert.ToDouble(textBoxSigma1.Text);
            sigma2 = Convert.ToDouble(textBoxSigma2.Text);
            theta = Convert.ToDouble(textBoxTheta.Text);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            rows = (int)numericUpDownRow.Value;
            cols = (int)numericUpDownCol.Value;
            Picture = ImgProcess.GaussSmooth((Bitmap)Picture, false, rows, sigma1, cols, sigma2, theta);
            this.Cursor = Cursors.Arrow;

            this.Close();                       
        }

        private void buttonRunGaussian_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            sigma1 += Convert.ToDouble(textBoxSigma1.Text)/50;
            sigma2 += Convert.ToDouble(textBoxSigma2.Text)/50;
            theta += Convert.ToDouble(textBoxTheta.Text)/50;
            this.pictureBox1.Image = ImgProcess.GaussSmooth((Bitmap)pictureBox1.Image, false,
                (int)numericUpDownRow.Value, Convert.ToDouble(textBoxSigma1.Text),
                (int)numericUpDownCol.Value, Convert.ToDouble(textBoxSigma2.Text), 
                Convert.ToDouble(textBoxTheta.Text));
            this.pictureBox1.Refresh();
            this.Cursor = Cursors.Arrow;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {            
            this.Close();            
        }

        private void FormGaussianSmooth_Load(object sender, EventArgs e)
        {
            if (Picture != null)
            {
                if ((Picture.Width > pictureBox1.Width) && (Picture.Height > pictureBox1.Height))
                {
                    OldPic = ImgProcess.Zoom((Bitmap)Picture, pictureBox1.Width, pictureBox1.Height);
                }
                else
                    OldPic = (Image)Picture.Clone();

                this.pictureBox1.Image = OldPic;
            }

            rows = (int)numericUpDownRow.Value;
            cols = (int)numericUpDownCol.Value;
            sigma1 = Convert.ToDouble(textBoxSigma1.Text);
            sigma2 = Convert.ToDouble(textBoxSigma2.Text);
            theta = Convert.ToDouble(textBoxTheta.Text);
        }
    }
}
