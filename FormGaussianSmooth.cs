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
        private ImageProcessing ImgProcess;        
        private Image OldPic;
        public Image Picture;

        public FormGaussianSmooth()
        {
            InitializeComponent();
            
            ImgProcess = new ImageProcessing();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = OldPic;
            numericUpDownRow.Value = 5;
            numericUpDownCol.Value = 5;
            textBoxSigma1.Text = "0.4";
            textBoxSigma2.Text = "0.4";
            textBoxTheta.Text = "0";      
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Picture = ImgProcess.Gauss_Convolution((Bitmap)Picture, false, (int)numericUpDownRow.Value,
                Convert.ToDouble(this.textBoxSigma1.Text), (int)numericUpDownCol.Value,
                Convert.ToDouble(this.textBoxSigma2.Text), Convert.ToDouble(this.textBoxTheta.Text));
            this.Cursor = Cursors.Arrow;

            OldPic.Dispose();
            OldPic = null;
            ImgProcess.Dispose(); 
            this.Close();                       
        }

        private void buttonRunGaussian_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.pictureBox1.Image = ImgProcess.Gauss_Convolution((Bitmap)pictureBox1.Image, false,
                (int)numericUpDownRow.Value, Convert.ToDouble(textBoxSigma1.Text),
                (int)numericUpDownCol.Value, Convert.ToDouble(textBoxSigma2.Text), 
                Convert.ToDouble(textBoxTheta.Text));
            this.pictureBox1.Refresh();
            this.Cursor = Cursors.Arrow;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {            
            OldPic.Dispose();
            OldPic = null;
            ImgProcess.Dispose();
            this.Close();            
        }

        private void FormGaussianSmooth_Load(object sender, EventArgs e)
        {
            if (Picture != null)
            {
                if ((Picture.Width > pictureBox1.Width) && (Picture.Height > pictureBox1.Height))
                {
                    OldPic = ImageProcessing.ZoomOut((Bitmap)Picture, pictureBox1.Width, pictureBox1.Height);
                }
                else
                    OldPic = (Image)Picture.Clone();

                this.pictureBox1.Image = OldPic;
            }
            numericUpDownRow.Value = 5;
            numericUpDownCol.Value = 5;
            textBoxSigma1.Text = "0.4";
            textBoxSigma2.Text = "0.4";
            textBoxTheta.Text = "0";             
        }
    }
}
