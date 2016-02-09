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
    public partial class FormSharpening : Form
    {              
        private Image OldPic;
        public Image Picture;

        public FormSharpening()
        {
            InitializeComponent();                       
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = OldPic;
            numericUpDownStep.Value = 2;
            numericUpDownThresholdHigh.Value = 175;
            numericUpDownThresholdLow.Value = 100;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Picture = ImageProcessing.Sharpen((Bitmap)Picture, (int)numericUpDownThresholdHigh.Value,
                (int)numericUpDownThresholdLow.Value, (int)numericUpDownStep.Value);
            this.Cursor = Cursors.Arrow;

            OldPic.Dispose();
            OldPic = null;
            this.Close();                       
        }

        private void buttonSharpen_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.pictureBox1.Image = ImageProcessing.Sharpen((Bitmap)pictureBox1.Image, (int)numericUpDownThresholdHigh.Value,
                (int)numericUpDownThresholdLow.Value, (int)numericUpDownStep.Value);
            this.pictureBox1.Refresh();
            this.Cursor = Cursors.Arrow;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {           
            OldPic.Dispose();
            OldPic = null;
            this.Close();            
        }

        private void FormSharpening_Load(object sender, EventArgs e)
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
        }
    }
}
