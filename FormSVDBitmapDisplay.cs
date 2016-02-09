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
    public partial class FormSVDBitmapDisplay : Form
    {
        public Image Picture;

        public FormSVDBitmapDisplay()
        {
            InitializeComponent();                                   
        }

        public Graphics GetGraphics()
        {
            return this.pictureBox1.CreateGraphics();
        }

        public DialogResult ShowImage(Image Img, int AlgType, string Report, string RootMeanSquareError)
        {
            string Title;

            this.Text = "Algorithm: ";
            this.pictureBox1.Image = Img;
            Picture = Img;

            switch (AlgType)
            {
                case ImageProcessing.A_SVD:
                    {
                        Title = "SINGULAR VALUE DECOMPOSITION";
                        this.Text += "Singular Value Decomposition - Rank " + Report;                        
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_PIECEWISELINEAR:
                    {                        
                        Title = "FINDING DISCONTINUITY PIECEWISE-LINEAR";
                        this.Text += "Finding Discontinuity Piecewise-Linear";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_PIECEWISESMOOTH:
                    {                        
                        Title = "FINDING DISCONTINUITY PIECEWISE-SMOOTH";
                        this.Text += "Finding Discontinuity Piecewise-Smooth";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_BORDERTRACING:
                    {                        
                        Title = "BORDER TRACING";
                        this.Text += "Border Tracing";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_SKELETON:
                    {                        
                        Title = "FINDING SKELETON";
                        this.Text += "Finding Skeleton";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_RG_EDGEDETECTION:
                    {                        
                        Title = "R-G EDGE DETECTION";
                        this.Text += "R-G Edge Detection";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_BLACKWHITE:
                    {                        
                        Title = "BLACK-WHITE";
                        this.Text += "Black-White";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_GRAYSCALE:
                    {                        
                        Title = "GRAY-SCALE";
                        this.Text += "Gray-scale";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_NOISEGENERATOR:
                    {                        
                        Title = "NOISE GENERATOR";
                        this.Text += "Noise Generator";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                case ImageProcessing.A_NEGATING:
                    {
                        Title = "NEGATIVE IMAGE";
                        this.Text += "Negative Image";
                        this.DisplayStatistic(Img, AlgType, Title, Report, RootMeanSquareError);
                        break;
                    }
                default:
                    break;
            }
            this.DisplayScrollBars();
            this.SetScrollBarValues();
            //this.Show(); this method will allow multiple dialogs

            return this.ShowDialog(); //this method will allow only 1 dialog
        }

        private void DisplayStatistic(Image Img, int AlgType, string Title, string NewRankValue, string RootMeanSquareError)
        {
            string s = Title + ":\n";            
            int OldSize = Img.Width*Img.Height;

            s += "\nImage Width\t\t : " + Img.Width.ToString();
            s += "\nImage Height\t\t : " + Img.Height.ToString();
            if (AlgType == ImageProcessing.A_SVD)
            {
                int rank = Convert.ToInt32(NewRankValue);
                int NewSize = rank + Img.Width * rank + Img.Height * rank;
                double Ratio = Math.Round((double)(100 * NewSize / OldSize));

                s += "\nNew Rank\t\t : " + NewRankValue;
                s += "\nOld Size\t\t\t : " + OldSize.ToString();
                s += "\nNew Size\t\t\t : " + NewSize.ToString();
                s += "\nRatio\t\t\t : " + Ratio.ToString() + "%";
                s += "\nSaving Percent\t\t : " + (100 - Ratio).ToString() + "%";                
            }
            s += "\nRoot Mean Square Error\t : " + RootMeanSquareError;
            this.richTextBox1.Text = s;
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        public void DisplayScrollBars()
        {
            // If the image is wider than the PictureBox, show the HScrollBar.
            if (pictureBox1.Width > pictureBox1.Image.Width - this.vScrollBar1.Width)
            {
                hScrollBar1.Visible = false;
            }
            else
            {
                hScrollBar1.Visible = true;
            }

            // If the image is taller than the PictureBox, show the VScrollBar.
            if (pictureBox1.Height >
                pictureBox1.Image.Height - this.hScrollBar1.Height)
            {
                vScrollBar1.Visible = false;
            }
            else
            {
                vScrollBar1.Visible = true;
            }
        }

        private void HandleScroll(Object sender, ScrollEventArgs se)
        {
            /* Create a graphics object and draw a portion 
               of the image in the PictureBox. */                        
            Graphics g = pictureBox1.CreateGraphics();             
            int v = 0, h = 0;            

            if (vScrollBar1.Visible == true)
                v = 1;
            if (hScrollBar1.Visible == true)
                h = 1;
            
            //g.Clear(Color.White);
            g.DrawImage(pictureBox1.Image,
              new Rectangle(0, 0, pictureBox1.Right - v*vScrollBar1.Width,
              pictureBox1.Bottom - h*hScrollBar1.Height),
              new Rectangle(hScrollBar1.Value, vScrollBar1.Value,
              pictureBox1.Right - v*vScrollBar1.Width,
              pictureBox1.Bottom - h*hScrollBar1.Height),
              GraphicsUnit.Pixel);            
        }

        public void SetScrollBarValues()
        {
            // Set the Maximum, Minimum, LargeChange and SmallChange properties.
            this.vScrollBar1.Minimum = 0;
            this.hScrollBar1.Minimum = 0;
            this.vScrollBar1.Value = 0;
            this.hScrollBar1.Value = 0;

            // If the offset does not make the Maximum less than zero, set its value. 
            if ((this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width) > 0)
            {
                this.hScrollBar1.Maximum =
                    this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width;
            }
            // If the VScrollBar is visible, adjust the Maximum of the 
            // HSCrollBar to account for the width of the VScrollBar.  
            if (this.vScrollBar1.Visible)
            {
                this.hScrollBar1.Maximum += this.vScrollBar1.Width;
            }
            this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 10;
            this.hScrollBar1.SmallChange = this.hScrollBar1.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.hScrollBar1.Maximum += this.hScrollBar1.LargeChange;

            // If the offset does not make the Maximum less than zero, set its value.    
            if ((this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height) > 0)
            {
                this.vScrollBar1.Maximum =
                    this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height;
            }

            // If the HScrollBar is visible, adjust the Maximum of the 
            // VSCrollBar to account for the width of the HScrollBar.
            if (this.hScrollBar1.Visible)
            {
                this.vScrollBar1.Maximum += this.hScrollBar1.Height;
            }
            this.vScrollBar1.LargeChange = this.vScrollBar1.Maximum / 10;
            this.vScrollBar1.SmallChange = this.vScrollBar1.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            // If the PictureBox has an image, see if it needs 
            // scrollbars and refresh the image. 
            if (pictureBox1.Image != null)
            {
                this.DisplayScrollBars();
                this.SetScrollBarValues();                
                this.Refresh();
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HandleScroll(sender, e);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HandleScroll(sender, e);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Form2_Resize(sender, e);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {                
                pictureBox1.Image = null;
            }
            GC.SuppressFinalize(this);
        }

        private void FormSVDBitmapDisplay_SizeChanged(object sender, EventArgs e)
        {
            Form2_Resize(sender, e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Picture.Dispose();
            Picture = null;
            this.Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Close();
        }      

    }
}
