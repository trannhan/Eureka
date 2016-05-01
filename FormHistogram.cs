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
    public partial class FormHistogram : Form
    {
        public Bitmap Picture;
        private Bitmap OldPic;
        const int step = 4;
        int maxValue = 255 - step;

        public FormHistogram()
        {
            InitializeComponent();
        }

        private void FormHistogram_Load(object sender, EventArgs e)
        {
            if (Picture != null)
            {
                if ((Picture.Width > pictureBox1.Width) && (Picture.Height > pictureBox1.Height))
                {
                    //ImageProcessing ImgP = new ImageProcessing();
                    OldPic = (Bitmap)ImageProcessing.ZoomOut(Picture, pictureBox1.Width, pictureBox1.Height);
                    //ImgP.Dispose();
                }
                else
                    OldPic = (Bitmap)Picture.Clone();

                this.pictureBox1.Image = OldPic;
                this.trackBarR.Value = 255;
                this.trackBarG.Value = 255;
                this.trackBarB.Value = 255;
            }
        }

        private Bitmap UpdateRGB(Bitmap Old)
        {
            int R = 0, G = 0, B = 0;
            Color P;      
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);

            for (int i = 0; i < Old.Width; i++)
            {
                for (int j = 0; j < Old.Height; j++)
                {
                    P = Old.GetPixel(i, j);
                    R = P.R + (int)this.numericUpDownR.Value;
                    if (R > 255)
                        R = 255;
                    else
                        if (R < 0)
                            R = 0;

                    G = P.G + (int)this.numericUpDownG.Value;
                    if (G > 255)
                        G = 255;
                    else
                        if (G < 0)
                            G = 0;

                    B = P.B + (int)this.numericUpDownB.Value;
                    if (B > 255)
                        B = 255;
                    else
                        if (B < 0)
                            B = 0;

                    NewImage.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
            return NewImage;  
        }
     
        private void trackBarR_MouseUp(object sender, MouseEventArgs e)
        {
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void trackBarG_MouseUp(object sender, MouseEventArgs e)
        {
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void trackBarB_MouseUp(object sender, MouseEventArgs e)
        {
            this.pictureBox1.Image = UpdateRGB(OldPic); 
        }        

        private void buttonCancel_Click(object sender, EventArgs e)
        {            
            OldPic.Dispose();
            OldPic = null;
            this.Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Picture = UpdateRGB(Picture);
            this.Cursor = Cursors.Arrow;
            
            OldPic.Dispose();
            OldPic = null;
            this.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = OldPic;
            this.trackBarR.Value = 255;
            this.trackBarG.Value = 255;
            this.trackBarB.Value = 255;
            this.numericUpDownR.Value = 0;
            this.numericUpDownG.Value = 0;
            this.numericUpDownB.Value = 0;
        }

        private void trackBarR_Scroll(object sender, EventArgs e)
        {
            this.numericUpDownR.Value = this.trackBarR.Value - 255;
        }

        private void trackBarG_Scroll(object sender, EventArgs e)
        {
            this.numericUpDownG.Value = this.trackBarG.Value - 255;
        }

        private void trackBarB_Scroll(object sender, EventArgs e)
        {
            this.numericUpDownB.Value = this.trackBarB.Value - 255;
        }        

        private void numericUpDownR_KeyUp(object sender, KeyEventArgs e)
        {
            this.trackBarR.Value = (int)this.numericUpDownR.Value + 255;
            this.trackBarR.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void numericUpDownG_KeyUp(object sender, KeyEventArgs e)
        {
            this.trackBarG.Value = (int)this.numericUpDownG.Value + 255;
            this.trackBarG.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void numericUpDownB_KeyUp(object sender, KeyEventArgs e)
        {
            this.trackBarB.Value = (int)this.numericUpDownB.Value + 255;
            this.trackBarB.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void numericUpDownR_MouseUp(object sender, MouseEventArgs e)
        {
            this.trackBarR.Value = (int)this.numericUpDownR.Value + 255;
            this.trackBarR.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void numericUpDownG_MouseUp(object sender, MouseEventArgs e)
        {
            this.trackBarG.Value = (int)this.numericUpDownG.Value + 255;
            this.trackBarG.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void numericUpDownB_MouseUp(object sender, MouseEventArgs e)
        {
            this.trackBarB.Value = (int)this.numericUpDownB.Value + 255;
            this.trackBarB.Refresh();
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void buttonBrighter_Click(object sender, EventArgs e)
        {
            
            if ((numericUpDownR.Value > maxValue) || (numericUpDownG.Value > maxValue) || (numericUpDownB.Value > maxValue))
                return;
            numericUpDownR.Value += step;
            numericUpDownG.Value += step;
            numericUpDownB.Value += step;
            this.trackBarR.Value = (int)this.numericUpDownR.Value + 255;
            this.trackBarG.Value = (int)this.numericUpDownG.Value + 255;
            this.trackBarB.Value = (int)this.numericUpDownB.Value + 255;
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }

        private void buttonDarker_Click(object sender, EventArgs e)
        {
            if ((numericUpDownR.Value < -maxValue) || (numericUpDownG.Value < -maxValue) || (numericUpDownB.Value < -maxValue))
                return;
            numericUpDownR.Value -= step;
            numericUpDownG.Value -= step;
            numericUpDownB.Value -= step;
            this.trackBarR.Value = (int)this.numericUpDownR.Value + 255;
            this.trackBarG.Value = (int)this.numericUpDownG.Value + 255;
            this.trackBarB.Value = (int)this.numericUpDownB.Value + 255;
            this.pictureBox1.Image = UpdateRGB(OldPic);
        }        
    }
}
