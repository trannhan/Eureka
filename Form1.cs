using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
//using DevComponents.DotNetBar;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string sProjectTile = "Eureka";
        bool bSaveDone;

        //Matrix Processing
        MMatrix OutputMatrix;
        MMatrix argumentMatrix;
        SystemOfEquation SystemEquations;                        
        FormMatrixInput InputMatrixForm;        
        FormSystemOfEquationsInput SystemOfEquationsForm;
        FormScalarInput ScalarForm;                
        double ScalarNumber = 0;        

        //Bitmap Processing
        Image InputImage;
        Image OutputImage;
        FormNewRankInput NewRankForm; //for SVD algorithm  
        FormNoiseInput NoiseForDiscontinuityForm; //for Discontinuity Finding algorithm
        FormNoise CreateNoiseForm;
        FormGaussianSmooth GaussForm;
        FormSharpening SharpenForm;
        FormSVDBitmapDisplay ImageForm;
        FormHistogram FormHis;
        ImageProcessing ImgProcess;
        ColorDialog colordlg;
        int SkeletonDifference = 15; //for Skeleton algorithm  
        Image tmpImage;

        public Form1()
        {            
            InitializeComponent();            

            //resultMatrix = new MMatrix();
            bSaveDone = true;
            InputMatrixForm = new FormMatrixInput();
            SystemOfEquationsForm = new FormSystemOfEquationsInput(); 
            ScalarForm = new FormScalarInput();
            NewRankForm = new FormNewRankInput();
            NoiseForDiscontinuityForm = new FormNoiseInput();
            CreateNoiseForm = new FormNoise();
            GaussForm = new FormGaussianSmooth();
            SharpenForm = new FormSharpening();
            ImageForm = new FormSVDBitmapDisplay();
            FormHis = new FormHistogram();
            colordlg = new ColorDialog();
            ImgProcess = new ImageProcessing();

            for (int i = 1; i < 256; ++i)
            {
                toolStripSkeletonDiff.Items.Add(i);
                toolStripComboBoxSkeletonDiff.Items.Add(i);
            }
            colordlg.FullOpen = true;
            colordlg.ShowHelp = true;
            colordlg.AnyColor = true;   
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region Administration
        private void toolStripBNew_Click(object sender, EventArgs e)
        {
            GC.Collect();            
            newToolStripMenuItem_Click(sender, e);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (this.pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
                hScrollBar1.Visible = false;
                vScrollBar1.Visible = false;
            }
            this.richTextBox1.Text = "";
            argumentMatrix = null;
            if (OutputImage != null)
            {
                OutputImage.Dispose();
                OutputImage = null;
            }
            if (InputImage != null)
            {
                InputImage.Dispose();
                InputImage = null;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 About = new AboutBox1();
            About.Show();
        }

        private void buttonOK_Click_1(object sender, EventArgs e)
        {
            toolStripBMatrix_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
            String sFilePath;
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPEG;*.GIF)|*.BMP;*.JPEG;*.JPG;*.GIF|All files (*.*)|*.*";

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sFilePath = this.openFileDialog1.FileName;                
                InitializeBitmap(sFilePath);
                
                this.richTextBox1.Visible = false;
                this.pictureBox1.Visible = true;
                Form1_Resize(sender, e);
            }
        }

        private void InitializeBitmap(String sFilePath)
        {
            try
            {
                if (InputImage != null)
                    InputImage.Dispose();
                InputImage = (Bitmap)Bitmap.FromFile(sFilePath);

                //pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                if (this.pictureBox1.Image != null)
                    this.pictureBox1.Image.Dispose();
                pictureBox1.Image = InputImage;
                OutputImage = InputImage;                   
            }
            catch (Exception)
            {
                InputImage = null;
                MessageBox.Show("There was an error opening the picture. Please check the path to the picture",
                    sProjectTile,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        
        private void OpenFile_Click(object sender, EventArgs e)
        {
            this.openToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();            
        }        

        private void toolStripBSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (bSaveDone)
                {
                    if (richTextBox1.Visible == true)
                        SaveMatrixText(OutputMatrix);
                    else
                        SaveBitmap();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Test

        public void SVDMatrixTest()
        {
            double[,] Arr1 = new double[3, 3] { { 4, -1, 1 }, { -1, 4.25, 2.75 }, { 1, 2.75, 3.5 } };
            double[,] Arr2 = new double[4, 4] { { 1, 1, 0, 3 }, { 2, 1, -1, 1 }, { 3, -1, -1, 2 }, { -1, 2, 3, -1 } };
            double[,] Arr3 = new double[2, 3] { { 3, 1, 1 }, { -1, 3, 1 } };
            double[,] Arr4 = new double[4, 4] { { 3, 1, 0, 1 }, { 1, 3, 1, 1 }, { 0, 1, 3, 1 }, { 1, 1, 1, 3 } };
            MMatrix A = new MMatrix(Arr2);
            MMatrix B = new MMatrix();
            string s = "";

            try
            {
                //A = MMatrix.RandomMatrix(size,0,255);                               
                A.FactorSVD();

                s = "Factor SVD:\n";
                s += "\nMatrix A=\n" + A.PrintToString();
                s += "\nU=\n" + (new MMatrix(A.SVD_U)).PrintToString();
                s += "\nS=\n" + (new MMatrix(A.SVD_S)).PrintToString();
                s += "\nVt=\n" + (new MMatrix(A.SVD_Vt)).PrintToString();

                B = (new MMatrix(A.SVD_U)) * (new MMatrix(A.SVD_S)) * (new MMatrix(A.SVD_Vt));
                s += "\nA=USV*\n" + B.PrintToString();

                this.RichText_Display(s);
                OutputMatrix = A;
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void EigenvectorTest()
        {
            double[,] Arr1 = new double[4, 4] { { 4, 1, -2, 2 }, { 1, 2, 0, 1 }, { -2, 0, 3, -2 }, { 2, 1, -2, -1 } };
            double[,] Arr2 = new double[2, 3] { { 3,1,1 }, { -1, 3,1 }};
            double[,] Arr3 = new double[5, 5] { { 2, 0, 8, 6, 0 }, { 1, 6, 0, 1, 7 }, { 5, 0, 7, 4, 0 }, { 7, 0, 8, 5, 0 }, { 0, 10, 0, 0, 7 } };
            double[,] Arr4 = new double[4, 4] { { 3, 1, 0, 1 }, { 1, 3, 1, 1 }, { 0, 1, 3, 1 }, { 1, 1, 1, 3 } };
            MMatrix A = new MMatrix(Arr2);
            MMatrix B = new MMatrix();
            string s = "";            
            Vector v = new Vector();            
            MMatrix Eigenvectors = new MMatrix();

            try
            {
                //A = MMatrix.RandomMatrix(3,0,255);                
                B = A*A.Transpose();                                
                s += "\nAA*=\n" + B.PrintToString();                

                B.Jacobi_Cyclic_Method(ref v, ref Eigenvectors);
                s += "\nBefore sort:";
                s += "\nEigenvalues of(A*At)=" + v.PrintToString();                                                                                    
                s += "\nEigenvectors of A*At:\n" + Eigenvectors.PrintToString();

                MMatrix.SortEigen(ref v, ref Eigenvectors);
                s += "\nAfter sort:";
                s += "\nEigenvalues of(A*At)=" + v.PrintToString();
                s += "\nEigenvectors of A*At:\n" + Eigenvectors.PrintToString();
                
                //This is not neccesary anymore since the eigenvectors are already orthonomalized
                s += "\nOrthonormalize eigenvectors(Ax = 0)\n" + MMatrix.Orthonormalize(Eigenvectors).PrintToString();

                this.RichText_Display(s);
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        public void QRTest()
        {
            double[,] Arr1 = new double[4, 4] { { 4, 1, -2, 2 }, { 1, 2, 0, 1 }, { -2, 0, 3, -2 }, { 2, 1, -2, -1 } };
            double[,] Arr2 = new double[4, 4] { { 3,1,0,1 }, { 1, 3, 1,1 }, {0,1,3,1 },{1,1,1,3} };            
            MMatrix A = new MMatrix(Arr2);
            MMatrix B = new MMatrix(Arr2); 
            string s = "";                     

            try
            {
                
                s += "\nA=\n" + A.PrintToString();                

                A.Householder();                
                s += "\nHouseholder (A): A(n-1)=\n" + A.PrintToString();                
                s += "\nEigenvalues approximation: " + A.Eigenvalues().PrintToString();                                                  
                                               
                s += "\nEigenvalues: " + B.Eigenvalues().PrintToString();
                
                this.RichText_Display(s);
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }        
        
        public void MatrixTest()
        {
            double[,] Arr1 = new double[4, 4] { { 1, 1, 0, 3 }, { 2, 1, -1, 1 }, { 3, -1, -1, 2 }, { -1, 2, 3, -1 } };
            double[,] Arr2 = new double[3, 3] { { 4, -1, 1 }, { -1, 4.25, 2.75 }, { 1, 2.75, 3.5 } };
            MMatrix A = new MMatrix(Arr1);
            MMatrix B = new MMatrix();
            MMatrix C = new MMatrix();
            string s = "";

            try
            {
                //A = MMatrix.RandomMatrix(size,0,10);                 
                
                s += "\nMatrix A=\n" + A.PrintToString();                
                s += "\nDet (A)=                    \n" + Convert.ToString(A.Determinant()) + "\n";                
                s += "\nA^-1=                    \n" + A.Inverse().PrintToString();                
                s += "\nA*A^-1=                     \n" + (A * A.Inverse()).PrintToString();                
                C = A * A.Transpose();                
                s += "\nC=A*A^T=\n" + C.PrintToString();                

                /////////////////Factor A = LDL*
                
                MMatrix.FactorLDLt(C);                                
                s += "\nFactor LDL*(C): L=\n" + (new MMatrix(C.LDL_L)).PrintToString();                
                s += "\nFactor LDL*(C): D=\n" + (new MMatrix(C.LDL_D)).PrintToString();                
                B = (new MMatrix(C.LDL_L)) * (new MMatrix(C.LDL_D)) * (new MMatrix(C.LDL_L)).Transpose();                
                s += "\nFactor LDL*: C=L*D*L^T\n" + B.PrintToString();                
                B = (new MMatrix(C.LDL_L)) * (new MMatrix(C.LDL_L)).Transpose();                
                s += "\nFactor LDL*: C=L*L^T\n" + B.PrintToString();                

                /////////////////Factor A = LU 

                C.FactorLU();                
                s += "\nFactor LU(C): L=\n" + (new MMatrix(C.LU_L)).PrintToString();                
                s += "\nFactor LU(C): U=\n" + (new MMatrix(C.LU_U)).PrintToString();                
                s += "\nFactor LU: C=L*U\n" + ((new MMatrix(C.LU_L)) * (new MMatrix(C.LU_U))).PrintToString();                

                ///////////////////Factor A = PLU

                A.FactorLU_withP();
                s += "\nFactor LU (A): P=\n" + (new MMatrix(A.LU_P)).PrintToString();
                s += "\nFactor LU (A): L=\n" + (new MMatrix(A.LU_L)).PrintToString();              
                s += "\nFactor LU (A): U=\n" + (new MMatrix(A.LU_U)).PrintToString();                                                
                s += "\nFactor LU: A=PLU\n" + ((new MMatrix(A.LU_P)).Transpose() * (new MMatrix(A.LU_L)) * (new MMatrix(A.LU_U))).PrintToString();

                this.RichText_Display(s);
                OutputMatrix = C;
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion       

        #region ScrollBar
        public void DisplayScrollBars()
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Normal)
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
                if (pictureBox1.Height > pictureBox1.Image.Height - this.hScrollBar1.Height)
                {
                    vScrollBar1.Visible = false;
                }
                else
                {
                    vScrollBar1.Visible = true;
                }
            }
        }

        private void HandleScroll(Object sender, ScrollEventArgs se)
        {
            Graphics g = pictureBox1.CreateGraphics();
            const int x = 0, y = 0;           
            int v = 0;            
            int h = 0;

            if (vScrollBar1.Visible == true)
                v = 1;
            if (hScrollBar1.Visible == true)
                h = 1;
            /*
            if (pictureBox1.SizeMode == PictureBoxSizeMode.CenterImage)
            {
                x = (pictureBox1.Width - pictureBox1.Image.Width) * 0.5;
                y = (pictureBox1.Height - pictureBox1.Image.Height) * 0.5;
            }
            */
            //g.Clear(Color.White);
            g.DrawImage(pictureBox1.Image,
              new Rectangle(x, y, pictureBox1.Right - v * vScrollBar1.Width+x,
              pictureBox1.Bottom - h * hScrollBar1.Height+y),
              new Rectangle(hScrollBar1.Value, vScrollBar1.Value,
              pictureBox1.Right - v * vScrollBar1.Width + x,
              pictureBox1.Bottom - h * hScrollBar1.Height + y),
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
            this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 2;
            this.hScrollBar1.SmallChange = this.hScrollBar1.Maximum / 5;

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
            this.vScrollBar1.LargeChange = this.vScrollBar1.Maximum / 2;
            this.vScrollBar1.SmallChange = this.vScrollBar1.Maximum / 5;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // If the PictureBox has an image, see if it needs 
            // scrollbars and refresh the image. 
            if ((pictureBox1.Image != null) && (pictureBox1.SizeMode == PictureBoxSizeMode.Normal))
            {
                this.DisplayScrollBars();
                this.SetScrollBarValues();
                this.Refresh();                
            }
            if (richTextBox1.Visible == true)
            {
                this.richTextBox1.Width = this.pictureBox1.Width;
                this.richTextBox1.Height = this.pictureBox1.Height;
                this.richTextBox1.Top = this.pictureBox1.Top;
                this.richTextBox1.Left = this.pictureBox1.Left;
            }
            pictureBox1.Left = this.ClientRectangle.Left;
            pictureBox1.Top = this.ClientRectangle.Top + 2*toolStrip1.Height;
            pictureBox1.Height = this.ClientRectangle.Height - 2*toolStrip1.Height;
            pictureBox1.Width = this.ClientRectangle.Width;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HandleScroll(sender, e);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            HandleScroll(sender, e);
        }
        #endregion

        #region ImageToolBox
        private void toolStripComboBoxZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newW, newH, Ratio;
            string sRatio;

            switch (toolStripComboBoxZoom.SelectedIndex)
            {
                case 9://100%
                    if (this.richTextBox1.Visible)
                        this.richTextBox1.ZoomFactor = 1;
                    else
                    {
                        if (OutputImage != null)
                            pictureBox1.Image = OutputImage;
                    }
                    break;
                default:
                    sRatio = toolStripComboBoxZoom.SelectedItem.ToString();
                    sRatio = sRatio.Trim('%');
                    Ratio = Convert.ToInt16(sRatio);

                    if (this.richTextBox1.Visible)
                        this.richTextBox1.ZoomFactor = (float)Ratio / 100;
                    else
                    {
                        if (OutputImage != null)
                        {
                            newW = OutputImage.Width * Ratio / 100;
                            newH = OutputImage.Height * Ratio / 100;
                            pictureBox1.Image = ImgProcess.ZoomOut((Bitmap)OutputImage, newW, newH);
                        }
                    }
                    break;
            }            
            if ((this.richTextBox1.Visible == false) && (OutputImage != null) && (pictureBox1.SizeMode == PictureBoxSizeMode.Normal))
            {
                this.DisplayScrollBars();
                this.SetScrollBarValues();
                this.Refresh();
            }
        }

        private void toolStripComboBoxPicMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            this.pictureBox1.Visible = true;

            //if (OutputImage != null)
            {
                switch (toolStripComboBoxPicMode.SelectedIndex)
                {
                    case 0:
                        pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                        Form1_Resize(sender, e);
                        break;
                    case 1:
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        vScrollBar1.Visible = false;
                        hScrollBar1.Visible = false;
                        break;
                    case 2:
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        vScrollBar1.Visible = false;
                        hScrollBar1.Visible = false;
                        break;
                    default:
                        break;
                }                
            }
        }
        #endregion

        #region Save
        public void SaveText(string FileName, string sData, bool bAppend)
        {
            if (File.Exists(FileName))
                File.Delete(FileName); 
                    
            StreamWriter SWriter = new StreamWriter(FileName, bAppend, System.Text.Encoding.UTF8);
            //FileStream Fs = new FileStream(FileName, FileMode.Create);                       

            if (SWriter != null)
            {
                try
                {                    
                    SWriter.Write(sData);
                    SWriter.Flush();
                    SWriter.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Save text error: \n" + e.Message,
                        sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        public void SaveMatrixText(MMatrix matrix)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Title = "Save Output Matrix";
            saveDlg.Filter = "Rich Text Format (*.rtf)|*.rtf|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string FileName = "";

            bSaveDone = false;
            //if(matrix != null)
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                FileName = saveDlg.FileName;
            }            
            if (FileName != "") 
            {                
                richTextBox1.SaveFile(FileName);
                /*
                string[] FileNamePart = new string[2];
                string FileExtension = ".txt";
                 
                FileNamePart = FileName.Split('.');
                if (FileNamePart.Length > 1)
                    FileExtension = "." + FileNamePart[1];                

                SaveText(FileName, matrix.PrintToString(), false);
                //SVD
                if (matrix.SVD_U != null)
                    SaveText(FileNamePart[0] + "_SVD_U" + FileExtension, (new MMatrix(matrix.SVD_U)).PrintToString(), false);
                if (matrix.SVD_S != null)
                    SaveText(FileNamePart[0] + "_SVD_S" + FileExtension, (new MMatrix(matrix.SVD_S)).PrintToString(), false);
                if (matrix.SVD_Vt != null)
                    SaveText(FileNamePart[0] + "_SVD_Vt" + FileExtension, (new MMatrix(matrix.SVD_Vt)).PrintToString(), false);
                //LU
                if (matrix.LU_P != null)
                    SaveText(FileNamePart[0] + "_LU_P" + FileExtension, (new MMatrix(matrix.LU_P)).PrintToString(), false);
                if (matrix.LU_L != null)
                    SaveText(FileNamePart[0] + "_LU_L" + FileExtension, (new MMatrix(matrix.LU_L)).PrintToString(), false);
                if (matrix.LU_U != null)
                    SaveText(FileNamePart[0] + "_LU_U" + FileExtension, (new MMatrix(matrix.LU_U)).PrintToString(), false);
                //LDL*
                if (matrix.LDL_L != null)
                    SaveText(FileNamePart[0] + "_LDL*_L" + FileExtension, (new MMatrix(matrix.LDL_L)).PrintToString(), false);
                if (matrix.LDL_D != null)
                    SaveText(FileNamePart[0] + "_LDL*_D" + FileExtension, (new MMatrix(matrix.LDL_D)).PrintToString(), false);
                if (matrix.LDL_L != null)
                    SaveText(FileNamePart[0] + "_LDL*_Lt" + FileExtension, (new MMatrix(matrix.LDL_L)).Transpose().PrintToString(), false);
                 * */
            }
            bSaveDone = true;
        }

        public void SaveBitmap()
        {
            bSaveDone = false;
            if (OutputImage != null)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Title = "Save Image";
                saveDlg.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG files (*.jpg)|*.jpg|" +
                    "GIF files (*.gif)|*.gif|Icon files (*.ico)|*.ico|PNG files (*.png)|*.png|" +
                    "TIFF files (*.tif)|*.tif|WMF files (*.wmf)|*.wmf|EMF files (*.emf)|*.emf|" +
                    "EXIF files (*.exif)|*.exif|Memory Bitmap files (*.psd)|*.psd|All files (*.*)|*.*";
                string FileName = "";
                int FileTypeIndex = 0;
                System.Drawing.Imaging.ImageFormat[] ImgFormat = new System.Drawing.Imaging.ImageFormat[10];

                ImgFormat[0] = System.Drawing.Imaging.ImageFormat.Bmp;
                ImgFormat[1] = System.Drawing.Imaging.ImageFormat.Jpeg;
                ImgFormat[2] = System.Drawing.Imaging.ImageFormat.Gif;
                ImgFormat[3] = System.Drawing.Imaging.ImageFormat.Icon;
                ImgFormat[4] = System.Drawing.Imaging.ImageFormat.Png;
                ImgFormat[5] = System.Drawing.Imaging.ImageFormat.Tiff;
                ImgFormat[6] = System.Drawing.Imaging.ImageFormat.Wmf;
                ImgFormat[7] = System.Drawing.Imaging.ImageFormat.Emf;
                ImgFormat[8] = System.Drawing.Imaging.ImageFormat.Exif;
                ImgFormat[9] = System.Drawing.Imaging.ImageFormat.MemoryBmp;                

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveDlg.FileName;
                    FileTypeIndex = saveDlg.FilterIndex - 1;
                }
                if (FileName != "")
                {
                    if (FileTypeIndex < ImgFormat.Length)
                        OutputImage.Save(FileName, ImgFormat[FileTypeIndex]);
                    else
                        OutputImage.Save(FileName);
                }
            }
            bSaveDone = true;
        }

        #endregion                       

        #region Matrix
        private void toolStripBMatrix_Click(object sender, EventArgs e)
        {
            //SVDMatrixTest();
            //MatrixTest();
            //QRTest();
            //EigenvectorTest();
            //SVDBitmapTest();             
        }

        private void RichText_Display(string s)
        {
            //this.richTextBox1.Text = s;
            richTextBox1.AppendText(s);
            this.pictureBox1.Visible = false;
            this.richTextBox1.Visible = true;
        }

        private void inputMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (InputMatrixForm.ShowDialog() == DialogResult.OK)
            {
                if (InputMatrixForm.MainMatrix != null)
                {
                    string s = string.Concat("\n\n\nMatrix A = \n", InputMatrixForm.MainMatrix.PrintToString());
                    RichText_Display(s);
                    OutputMatrix = InputMatrixForm.MainMatrix;
                }
            }
        }

        private void determinantToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Det (A) = " + Math.Round(OutputMatrix.Determinant(),GlobalMath.DIGITS).ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void inverseToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Inverse (A) = \n" + OutputMatrix.Inverse().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void transposeToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Transpose (A) = \n" + OutputMatrix.Transpose().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void housholderToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Housholder (A) = \n" + OutputMatrix.Clone().Householder().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void eigenvaluesToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Eigenvalues (A) = \n" + OutputMatrix.Eigenvalues().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void eigenvectorsToolStripMenuItem_Click(object sender, EventArgs e)
        {                        
            try
            {
                StringBuilder sb = new StringBuilder("");
                Vector eigenvalues = new Vector();
                MMatrix eigenvectors = new MMatrix();
                if (OutputMatrix != null)
                {
                    MMatrix temp = OutputMatrix.Clone();
                    temp.Jacobi_Cyclic_Method(ref eigenvalues, ref eigenvectors);
                    sb.Append("\n\n\nEigenvectors (A) = \n");
                    for (int i = 0; i < eigenvectors.col; ++i)
                    {
                        sb.Append("V");
                        sb.Append(i + 1);
                        sb.Append("\t\t");
                    }
                    sb.Append("\n");
                    sb.Append(eigenvectors.PrintToString());
                    RichText_Display(sb.ToString());
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void echelonFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Echelon Form (A) = \n" + OutputMatrix.EchelonForm().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void reduceEchelonToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Reduced Echelon Form (A) = \n" + OutputMatrix.ReducedEchelonForm().PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void orthonormalizToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Orthonormalize (A) = \n" + MMatrix.Orthonormalize(OutputMatrix).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void factorLUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {                
                if (OutputMatrix != null)
                {
                    OutputMatrix.FactorLU_withP();
                    MMatrix P = new MMatrix(OutputMatrix.LU_P);
                    MMatrix L = new MMatrix(OutputMatrix.LU_L);
                    MMatrix U = new MMatrix(OutputMatrix.LU_U);
                    string s = "\n\n\nFactor LU:\n";                    
                    s += "\nP=\n" + P.PrintToString();
                    s += "\nL=\n" + L.PrintToString();
                    s += "\nU=\n" + U.PrintToString();                    
                    s += "\nA=P*LU\n" + (P.Transpose()*L*U).PrintToString();                    
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        private void factorLDLtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {                
                if (OutputMatrix != null)
                {
                    OutputMatrix.FactorLDLt();
                    MMatrix L = new MMatrix(OutputMatrix.LDL_L);
                    MMatrix D = new MMatrix(OutputMatrix.LDL_D);
                    string s = "\n\n\nFactor LDL*:\n";                    
                    s += "\nL=\n" + L.PrintToString();
                    s += "\nD=\n" + D.PrintToString();                    
                    s += "\nA=LDL*\n" + (L*D*L.Transpose()).PrintToString();                                        
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }  
        }

        private void factorSVDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (OutputMatrix != null)
                {
                    OutputMatrix.FactorSVD();
                    MMatrix U = new MMatrix(OutputMatrix.SVD_U);
                    MMatrix S = new MMatrix(OutputMatrix.SVD_S);
                    MMatrix Vt = new MMatrix(OutputMatrix.SVD_Vt);
                    string s = "\n\n\n";                    
                    s += "Factor SVD:\n";                    
                    s += "\nU=\n" + U.PrintToString();
                    s += "\nS=\n" + S.PrintToString();
                    s += "\nV'=\n" + Vt.PrintToString();
                    //s += "\nA=USV*\n" + (U * S * Vt.Transpose()).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void spectralRadiusToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Spectral Radius (A) = " + Math.Round(OutputMatrix.SpectralRadius(),GlobalMath.DIGITS).ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void l2NormToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "L2 Norm (A) = " + Math.Round(OutputMatrix.L2Norm(),GlobalMath.DIGITS).ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lInfinitiveToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "L_Infinitive Norm (A) = " + Math.Round(OutputMatrix.LInfinitiveNorm(),GlobalMath.DIGITS).ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void conditionNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Condition Number (A) = " + Math.Round(OutputMatrix.ConditionNumber(),GlobalMath.DIGITS).ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            if (InputMatrixForm.ShowDialog() == DialogResult.OK)
            {
                if (InputMatrixForm.MainMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Matrix B = \n" + InputMatrixForm.MainMatrix.PrintToString();
                    RichText_Display(s);
                    argumentMatrix = InputMatrixForm.MainMatrix;
                }
            }
            try
            {
                if ((OutputMatrix != null) && (argumentMatrix!=null))
                {
                    string s = "\n\n\n";
                    s += "A + B = \n" + (OutputMatrix + argumentMatrix).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void substractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            if (InputMatrixForm.ShowDialog() == DialogResult.OK)
            {
                if (InputMatrixForm.MainMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Matrix B = \n" + InputMatrixForm.MainMatrix.PrintToString();
                    RichText_Display(s);
                    argumentMatrix = InputMatrixForm.MainMatrix;
                }
            }
            try
            {
                if ((OutputMatrix != null) && (argumentMatrix != null))
                {
                    string s = "\n\n\n";
                    s += "A - B = \n" + (OutputMatrix - argumentMatrix).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void multiplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            if (InputMatrixForm.ShowDialog() == DialogResult.OK)
            {
                if (InputMatrixForm.MainMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Matrix B = \n" + InputMatrixForm.MainMatrix.PrintToString();
                    RichText_Display(s);
                    argumentMatrix = InputMatrixForm.MainMatrix;
                }
            }
            try
            {
                if ((OutputMatrix != null) && (argumentMatrix != null))
                {
                    string s = "\n\n\n";
                    s += "A * B = \n" + (OutputMatrix * argumentMatrix).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void scalarMultiplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "\n\n\n";
            if (ScalarForm.ShowDialog() == DialogResult.OK)
            {
                ScalarNumber = ScalarForm.Scalar;                
                s += "Scalar number = " + ScalarNumber.ToString();                               
            }
            try
            {
                if (OutputMatrix != null)
                {                    
                    s += "\n\nA * Scalar number = \n" + (OutputMatrix * ScalarForm.Scalar).PrintToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rankToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            try
            {
                if (OutputMatrix != null)
                {
                    string s = "\n\n\n";
                    s += "Rank (A) = " + OutputMatrix.Rank().ToString();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void inputSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SystemOfEquationsForm.ShowDialog() == DialogResult.OK)
            {
                if ((SystemOfEquationsForm.MainMatrix != null) && (SystemOfEquationsForm.b != null))
                {
                    SystemEquations = new SystemOfEquation(SystemOfEquationsForm.MainMatrix, SystemOfEquationsForm.b);
                    string s = "\n\n\n";
                    s += "System of equations Ax = b: \n" + SystemEquations.PrintSystemOfEquations();
                    RichText_Display(s);                    
                }
            }
        }

        private void reduceSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            try
            {
                if (SystemEquations != null)
                {
                    SystemEquations.SolveSystemOfEquations();
                    string s = "\n\n\n";
                    s += "Reduced System of Ax = b:\n" + SystemOfEquation.PrintSystemOfEquations(SystemEquations.ArgumentMatrix);
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("System Of Equations Warning: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void solutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SystemEquations != null)
                {
                    SystemEquations.SolveSystemOfEquations();
                    string s = "\n\n\n";
                    s += "Solutions to Ax = b:\n" + SystemEquations.PrintSolutions();
                    RichText_Display(s);
                }
            }
            catch (MMatrixException exp)
            {
                MessageBox.Show("System Of Equations Warning: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Bitmap
        private void toolStripBitmap_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            this.pictureBox1.Visible = true;
        }

        private void SetNewImage(Image Img)
        {
            OutputImage = Img;
            this.pictureBox1.Image = OutputImage;            
        }

        private void PictureBox_PrintString(string s, int x, int y)
        {
            this.pictureBox1.CreateGraphics().DrawString(s, this.Font, Brushes.Black, new Point(x, y));
        }

        private void grayscale_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;            
            if (this.OutputImage != null)
            {
                this.Cursor = Cursors.WaitCursor;
                tmpImage = ImgProcess.GrayScale(new Bitmap(this.OutputImage));
                this.Cursor = Cursors.Arrow;
           
                if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_GRAYSCALE, "", "") == DialogResult.OK)
                    SetNewImage(tmpImage);                 
            }
        }

        private void toolStripBlackWhite_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;           
            if (this.OutputImage != null)
            {
                this.Cursor = Cursors.WaitCursor;
                tmpImage = ImgProcess.BlackWhite(new Bitmap(this.OutputImage));
                this.Cursor = Cursors.Arrow;
         
                if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_BLACKWHITE, "", "") == DialogResult.OK)
                    SetNewImage(tmpImage);   
            }
        }

        private void negatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;         

            if (this.OutputImage != null)
            {
                this.Cursor = Cursors.WaitCursor;
                tmpImage = ImgProcess.Negate((Bitmap)this.OutputImage);
                this.Cursor = Cursors.Arrow;
            
                if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_NEGATING, "", "") == DialogResult.OK)
                    SetNewImage(tmpImage);
            }
        }            

        private void findingDiscontToolStripMenuItem_Click(object sender, EventArgs e)
        {                        
            double Noise = 0;
            int Method = -1;

            this.richTextBox1.Visible = false;            
            try
            {                
                if (this.OutputImage != null)
                {                    
                    if (NoiseForDiscontinuityForm.ShowDialog() == DialogResult.OK)
                    {
                        Noise = NoiseForDiscontinuityForm.Noise;
                        Method = NoiseForDiscontinuityForm.Method;
                    }
                    if (Method != -1)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        tmpImage = ImgProcess.Bitmap_FindingDiscontinuities((Bitmap)(OutputImage), Noise, Method);                                              
                        tmpImage = ImgProcess.BitmapBitwise((Bitmap)tmpImage, (Bitmap)pictureBox1.Image, "^");
                        this.Cursor = Cursors.Arrow;
                        
                        if (ImageForm.ShowImage(tmpImage, Method, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);                        
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        public void SVDBitmap(int NewRank)
        {
            try
            {
                if (this.OutputImage != null)
                {
                    double Err = 0;
                    
                    tmpImage = ImgProcess.Bitmap_SVD((Bitmap)(this.OutputImage), NewRank, ref Err);
               
                    if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_SVD, NewRank.ToString(), Err.ToString()) == DialogResult.OK)
                        SetNewImage(tmpImage);                                        
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void singularValueDecompositionToolStripMenuItem1_Click(object sender, EventArgs e)
        {            
            this.richTextBox1.Visible = false;
            if (this.OutputImage != null)
            {
                /*                
                NewRankForm.NewRank = Math.Min(OutputImage.Width, OutputImage.Height);
                NewRankForm.BestRank = (OutputImage.Height * OutputImage.Width) /
                    (1 + OutputImage.Height + OutputImage.Width);
                if (NewRankForm.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;                    
                    SVDBitmap(NewRankForm.NewRank);                    
                    this.Cursor = Cursors.Arrow;
                }
                */
                this.Cursor = Cursors.WaitCursor;
                //These 2 lines are slow:
                //MMatrix RandMatrix = MMatrix.RandomMatrix(OutputImage.Width, OutputImage.Height, 0, 100);
                //tmpImage = ImageProcessing.BitmapBitwise((Bitmap)(OutputImage), ImageProcessing.BitmapFromMatrix(ref RandMatrix, ref RandMatrix, ref RandMatrix), "|");
                tmpImage = ImgProcess.CreateNoise((Bitmap)(OutputImage), 0.3);
                this.Cursor = Cursors.Arrow;
              
                if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_NOISEGENERATOR, "", "") == DialogResult.OK)
                    SetNewImage(tmpImage);
            }            
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            if (this.OutputImage != null)
            {                
                FormHis.Picture = (Bitmap)this.OutputImage;
                if (FormHis.ShowDialog() == DialogResult.OK)
                {                    
                    SetNewImage(FormHis.Picture);
                }                
            }
        }

        private void borderTracingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;            
            try
            {
                if (this.OutputImage != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                                       
                    tmpImage = ImgProcess.Bitmap_BorderTracing(pictureBox1.CreateGraphics(), (Bitmap)(this.OutputImage));                    
                    this.Cursor = Cursors.Arrow;
              
                    if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_BORDERTRACING, "", "") == DialogResult.OK)
                        SetNewImage(tmpImage);                    
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripSkeletonDiff_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.richTextBox1.Visible = false;            
            try
            {
                SkeletonDifference = toolStripSkeletonDiff.SelectedIndex + 1;
                toolStripSkeletonDiff.PerformClick();
                if (this.OutputImage != null)
                {                   
                    Color color = Color.Black;
                    if (colordlg.ShowDialog() == DialogResult.OK)
                    {
                        color = colordlg.Color;

                        this.Cursor = Cursors.WaitCursor;
                        tmpImage = ImgProcess.Bitmap_Skeleton(new Bitmap(this.OutputImage), color, SkeletonDifference);
                        this.Cursor = Cursors.Arrow;
                   
                        if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_SKELETON, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);                        
                    }              
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }                     
        }

        private void rGEdgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double Noise = 0;
            int Method = -1;
                        
            try
            {
                if (this.OutputImage != null)
                {
                    if (NoiseForDiscontinuityForm.ShowDialog() == DialogResult.OK)
                    {
                        Noise = NoiseForDiscontinuityForm.Noise;
                        Method = NoiseForDiscontinuityForm.Method;
                    }
                    if (Method != -1)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        //Apply Gaussian Smoothing first.....
                        tmpImage = ImgProcess.Bitmap_FindingDiscontinuities((Bitmap)(OutputImage), Noise, Method);
                        this.Cursor = Cursors.Arrow;
                        if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_RG_EDGEDETECTION, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);                         
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }  
        }

        private void noiseGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double Noise = 0;

            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    if (CreateNoiseForm.ShowDialog() == DialogResult.OK)
                    {
                        Noise = CreateNoiseForm.Noise;

                        this.Cursor = Cursors.WaitCursor;
                                                
                        tmpImage = ImgProcess.CreateNoise((Bitmap)(OutputImage), Noise);
                        this.Cursor = Cursors.Arrow;
                      
                        if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_NOISEGENERATOR, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gaussianSmoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    GaussForm.Picture = this.OutputImage;
                    if (GaussForm.ShowDialog() == DialogResult.OK)
                    {                        
                        SetNewImage(GaussForm.Picture);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void sharpeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    SharpenForm.Picture = this.OutputImage;
                    if (SharpenForm.ShowDialog() == DialogResult.OK)
                    {                        
                        SetNewImage(SharpenForm.Picture);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cannyEdgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    
                    tmpImage = ImgProcess.Canny(new Bitmap(this.OutputImage));                                   
                    this.Cursor = Cursors.Arrow;
               
                    if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_CANNY, "", "") == DialogResult.OK)
                        SetNewImage(tmpImage);                    
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.richTextBox1.Width = this.pictureBox1.Width;
            this.richTextBox1.Height = this.pictureBox1.Height;
            this.richTextBox1.Top = this.pictureBox1.Top;
            this.richTextBox1.Left = this.pictureBox1.Left;

            this.buttonOK.Visible = false;
            GlobalMath.DIGITS = 3;
            this.toolStripComboBoxDigits.SelectedIndex = GlobalMath.DIGITS;
            this.toolStripComboBoxZoom.SelectedIndex = toolStripComboBoxZoom.Items.Count - 1;
            this.toolStripComboBoxPicMode.SelectedIndex = 0;                            
        }

        private void toolStripMatrix_ButtonClick(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = true;
            this.pictureBox1.Visible = false;
        }

        private void toolStripSystemofEqua_ButtonClick(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = true;
            this.pictureBox1.Visible = false;
        }

        private void toolStripComboBoxDigits_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMath.DIGITS = Convert.ToInt16(toolStripComboBoxDigits.SelectedItem.ToString());
        }

        private void amazingEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double Noise = 0;
            int Method = -1;

            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    if (NoiseForDiscontinuityForm.ShowDialog() == DialogResult.OK)
                    {
                        Noise = NoiseForDiscontinuityForm.Noise;
                        Method = NoiseForDiscontinuityForm.Method;
                    }
                    if (Method != -1)
                    {
                        this.Cursor = Cursors.WaitCursor;                        
                        tmpImage = ImgProcess.Bitmap_FindingDiscontinuities((Bitmap)(OutputImage), Noise, Method);                                              
                        tmpImage = ImgProcess.BitmapBitwise((Bitmap)tmpImage, (Bitmap)pictureBox1.Image, "&");
                        this.Cursor = Cursors.Arrow;
                      
                        if (ImageForm.ShowImage(tmpImage, Method, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void colorPaintingEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double Noise = 0;
            int Method = -1;

            this.richTextBox1.Visible = false;
            try
            {
                if (this.OutputImage != null)
                {
                    if (NoiseForDiscontinuityForm.ShowDialog() == DialogResult.OK)
                    {
                        Noise = NoiseForDiscontinuityForm.Noise;
                        Method = NoiseForDiscontinuityForm.Method;
                    }
                    if (Method != -1)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        
                        tmpImage = ImgProcess.Bitmap_FindingDiscontinuities((Bitmap)(OutputImage), Noise, Method);                                              
                        tmpImage = ImgProcess.BitmapBitwise((Bitmap)tmpImage, (Bitmap)pictureBox1.Image, "|");
                        this.Cursor = Cursors.Arrow;
                     
                        if (ImageForm.ShowImage(tmpImage, Method, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void bitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Visible = false;
            this.pictureBox1.Visible = true;
        }

        private void toolStripComboBoxSkeletonDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SkeletonDifference = toolStripComboBoxSkeletonDiff.SelectedIndex + 1;
                toolStripComboBoxSkeletonDiff.PerformClick();
                if (this.OutputImage != null)
                {
                    Color color = Color.Black;                    
                    if (colordlg.ShowDialog() == DialogResult.OK)
                    {
                        color = colordlg.Color;

                        this.Cursor = Cursors.WaitCursor;
                        tmpImage = ImgProcess.Bitmap_Skeleton(new Bitmap(this.OutputImage), color, SkeletonDifference);
                        this.Cursor = Cursors.Arrow;

                        if (ImageForm.ShowImage(tmpImage, ImageProcessing.A_SKELETON, "", "") == DialogResult.OK)
                            SetNewImage(tmpImage);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("INFO: " + exp.Message, sProjectTile, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
