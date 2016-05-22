using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public class ImageProcessing
    {
        private byte[,] MRed;
        private byte[,] MGreen;
        private byte[,] MBlue;
        private bool bGrayScale;

        public const int BMP_FRAC_SIZE = 6;
        //Algorithm Types:
        public const int A_PIECEWISELINEAR = 0;
        public const int A_PIECEWISESMOOTH = 1;
        public const int A_SVD = 2;
        public const int A_BORDERTRACING = 3;
        public const int A_SKELETON = 4;
        public const int A_RG_EDGEDETECTION = 5;
        public const int A_BLACKWHITE = 6;
        public const int A_GRAYSCALE = 7;
        public const int A_NOISEGENERATOR = 8;
        public const int A_NEGATING = 9;
        public const int A_CANNY = 10;


        public ImageProcessing()
        {
        }
        /*
        ~ImageProcessing()
        {      
        }
        */

        #region Pixel Processing
        //Very slow
        public void MatrixFromBitmap1(ref Bitmap bmp)
        {
            Color PixelColor;
            bGrayScale = IsGrayScale(ref bmp);
            MRed = new byte[bmp.Width, bmp.Height];

            if (bGrayScale)
                for (int i = 0; i < bmp.Width; ++i)
                    for (int j = 0; j < bmp.Height; ++j)
                        MRed[i, j] = bmp.GetPixel(i, j).R;
            else
            {
                MGreen = new byte[bmp.Width, bmp.Height];
                MBlue = new byte[bmp.Width, bmp.Height];
                for (int i = 0; i < bmp.Width; ++i)
                    for (int j = 0; j < bmp.Height; ++j)
                    {
                        PixelColor = bmp.GetPixel(i, j);
                        MRed[i, j] = PixelColor.R;
                        MGreen[i, j] = PixelColor.G;
                        MBlue[i, j] = PixelColor.B;
                    }
            }
        }

        public void MatrixFromBitmap(ref Bitmap bmp)
        {
            int cs, r3;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            MRed = new byte[bmp.Width, bmp.Height];
            MGreen = new byte[bmp.Width, bmp.Height];
            MBlue = new byte[bmp.Width, bmp.Height];
            for (int column = 0; column < bmpData.Height; column++)
            {
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {
                    r3 = row * 3;
                    MBlue[row, column] = rgbValues[cs + r3];
                    MGreen[row, column] = rgbValues[cs + r3 + 1];
                    MRed[row, column] = rgbValues[cs + r3 + 2];
                }
            }
            // Copy the RGB values back to the bitmap
            //Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);
        }
        //slow
        public static Bitmap BitmapFromMatrix1(ref byte[,] MRed, ref byte[,] MGreen, ref byte[,] MBlue)
        {
            Bitmap tmpBmp = new Bitmap(MRed.GetLength(0), MRed.GetLength(1));

            for (int i = 0; i < MRed.GetLength(0); ++i)
                for (int j = 0; j < MRed.GetLength(1); ++j)
                    tmpBmp.SetPixel(i, j, Color.FromArgb(MRed[i, j], MGreen[i, j], MBlue[i, j]));

            return tmpBmp;
        }

        public Bitmap BitmapFromMatrix(ref byte[,] MRed, ref byte[,] MGreen, ref byte[,] MBlue)
        {
            int cs, r3;
            Bitmap bmp = new Bitmap(MRed.GetLength(0), MRed.GetLength(1));
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            //Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            for (int column = 0; column < bmpData.Height; column++)
            {
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {
                    r3 = row * 3;
                    rgbValues[cs + r3] = MBlue[row, column];
                    rgbValues[cs + r3 + 1] = MGreen[row, column];
                    rgbValues[cs + r3 + 2] = MRed[row, column];
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static Bitmap DrawTextToBitmap(ref Bitmap Bmp, string s, int x, int y)
        {
            Graphics g = Graphics.FromImage((Image)Bmp);

            g.DrawString(s, SystemFonts.DefaultFont, Brushes.Black, new Point(x, y));

            return Bmp;
        }

        private byte[,] FloorToBitmap(double[,] BitmapMatrix)
        {
            byte[,] BArray = new byte[BitmapMatrix.GetLength(0), BitmapMatrix.GetLength(1)];

            for (int i = 0; i < BitmapMatrix.GetLength(0); ++i)
                for (int j = 0; j < BitmapMatrix.GetLength(1); ++j)
                {
                    if (BitmapMatrix[i, j] < 0)
                        BArray[i, j] = 0;
                    else if (BitmapMatrix[i, j] > 255)
                        BArray[i, j] = 255;
                    else
                        BArray[i, j] = (byte)BitmapMatrix[i, j];
                }
            return BArray;
        }

        public static bool IsGrayScale(ref Bitmap bmp)
        {
            Color PixelColor;
            bool bGrayScale = true;

            PixelColor = bmp.GetPixel(0, 0);
            if ((PixelColor.R != PixelColor.G) || (PixelColor.R != PixelColor.B) || (PixelColor.G != PixelColor.B))
                bGrayScale = false;
            PixelColor = bmp.GetPixel(bmp.Width / 2, bmp.Height / 2);
            if ((PixelColor.R != PixelColor.G) || (PixelColor.R != PixelColor.B) || (PixelColor.G != PixelColor.B))
                bGrayScale = false;

            return bGrayScale;
        }
        #endregion

        #region Basic Image Processing
        //slow
        public Image GrayScale1(Bitmap Old)
        {
            Bitmap NewBmp = new Bitmap(Old.Width, Old.Height);
            int NewRGB;
            Color PixelColor;

            for (int i = 0; i < NewBmp.Width; ++i)
            {
                for (int j = 0; j < NewBmp.Height; ++j)
                {
                    PixelColor = Old.GetPixel(i, j);
                    NewRGB = (PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    NewBmp.SetPixel(i, j, Color.FromArgb(NewRGB, NewRGB, NewRGB));
                }
            }
            return NewBmp;
        }

        public Image GrayScale(Bitmap Old)
        {
            int cs, r3;
            byte GrayColor;
            Bitmap bmp = new Bitmap(Old);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            MRed = new byte[bmp.Width, bmp.Height];
            for (int column = 0; column < bmpData.Height; column++)
            {
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {
                    r3 = row * 3;
                    GrayColor = (byte)((rgbValues[cs + r3] + rgbValues[cs + r3 + 1] + rgbValues[cs + r3 + 2]) / 3);
                    rgbValues[cs + r3] = GrayColor; //blue
                    rgbValues[cs + r3 + 1] = GrayColor; //green
                    rgbValues[cs + r3 + 2] = GrayColor; //red
                    MRed[row, column] = GrayColor;
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        //slow
        public Image BlackWhite1(Bitmap Old)
        {
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            Color PixelColor;

            for (int i = 0; i < Old.Width; ++i)
            {
                for (int j = 0; j < Old.Height; ++j)
                {
                    PixelColor = Old.GetPixel(i, j);
                    if ((PixelColor.R + PixelColor.G + PixelColor.B) / 3 < 125)
                        NewImage.SetPixel(i, j, Color.Black);
                    else
                        NewImage.SetPixel(i, j, Color.White);
                }
            }
            return NewImage;
        }

        public Image BlackWhite(Bitmap Old)
        {
            int cs, r3;
            Bitmap bmp = new Bitmap(Old);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            MRed = new byte[bmp.Width, bmp.Height];
            for (int column = 0; column < bmpData.Height; column++)
            {
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {
                    r3 = row * 3;
                    if ((byte)((rgbValues[cs + r3] + rgbValues[cs + r3 + 1] + rgbValues[cs + r3 + 2]) / 3) < 125)
                    {
                        rgbValues[cs + r3] = 0; //blue
                        rgbValues[cs + r3 + 1] = 0; //green
                        rgbValues[cs + r3 + 2] = 0; //red
                        MRed[row, column] = 0;
                    }
                    else
                    {
                        rgbValues[cs + r3] = 255;
                        rgbValues[cs + r3 + 1] = 255;
                        rgbValues[cs + r3 + 2] = 255;
                        MRed[row, column] = 255;
                    }
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        //slow
        public Image Negate1(Bitmap Old)
        {
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            Color PixelColor;

            for (int i = 0; i < Old.Width; ++i)
            {
                for (int j = 0; j < Old.Height; ++j)
                {
                    PixelColor = Old.GetPixel(i, j);
                    NewImage.SetPixel(i, j, Color.FromArgb(255 - PixelColor.R, 255 - PixelColor.G, 255 - PixelColor.B));
                }
            }
            return NewImage;
        }

        public Image Negate(Bitmap Old)
        {
            int cs, r3;
            Bitmap bmp = new Bitmap(Old);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            for (int column = 0; column < bmpData.Height; column++)
            {
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {
                    r3 = row * 3;                  
                    rgbValues[cs + r3] = (byte)(255 - rgbValues[cs + r3]); //blue
                    rgbValues[cs + r3 + 1] = (byte)(255 - rgbValues[cs + r3 + 1]); //green
                    rgbValues[cs + r3 + 2] = (byte)(255 - rgbValues[cs + r3 + 2]); //red
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        //slow
        public Image ZoomOut1(Bitmap Old, int newWidth, int newHeight)
        {
            Bitmap New = new Bitmap(newWidth, newHeight);
            Color PixelColor;
            double WidthRate, HeightRate;
            int x, y;

            WidthRate = (double)Old.Width / newWidth;
            HeightRate = (double)Old.Height / newHeight;

            for (int i = 0; i < newWidth; ++i)
            {
                x = (int)(i * WidthRate);
                for (int j = 0; j < newHeight; ++j)
                {
                    y = (int)(j * HeightRate);
                    PixelColor = Old.GetPixel(x, y);
                    New.SetPixel(i, j, Color.FromArgb(PixelColor.R, PixelColor.G, PixelColor.B));
                }
            }
            return New;
        }

        public Image ZoomOut(Bitmap Old, int newWidth, int newHeight)
        {            
            int cs, r3;
            Bitmap bmp = new Bitmap(newWidth, newHeight);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            double WidthRate, HeightRate;
            int x, y;

            MatrixFromBitmap(ref Old);
            WidthRate = (double)Old.Width / newWidth;
            HeightRate = (double)Old.Height / newHeight;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array. bmpData.Scan0 returns the address of the first line.
            //Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            for (int column = 0; column < bmpData.Height; column++)
            {
                y = (int)(column * HeightRate);
                cs = column * bmpData.Stride;
                for (int row = 0; row < bmpData.Width; row++)
                {                    
                    x = (int)(row * WidthRate);
                    r3 = row * 3;
                    rgbValues[cs + r3] = MBlue[x, y]; 
                    rgbValues[cs + r3 + 1] = MGreen[x, y];
                    rgbValues[cs + r3 + 2] = MRed[x, y];
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static Image Rotate(ref Bitmap Old, double Angle)
        {
            Angle = Angle * Math.PI / 180;
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            double[,] TransformMatrix = new double[2, 2] { { Math.Cos(Angle), -Math.Sin(Angle) }, { Math.Sin(Angle), Math.Cos(Angle) } };
            int x, y;
            Color PixelColor;

            for (int i = 0; i < Old.Width; ++i)
            {
                for (int j = 0; j < Old.Height; ++j)
                {
                    x = (int)Math.Abs(i * TransformMatrix[0, 0] + j * TransformMatrix[1, 0]);
                    y = (int)Math.Abs(i * TransformMatrix[0, 1] + j * TransformMatrix[1, 1]);
                    if ((x >= Old.Width) || (y >= Old.Height))
                        continue;
                    PixelColor = Old.GetPixel(i, j);
                    NewImage.SetPixel(x, y, Color.FromArgb(PixelColor.R, PixelColor.G, PixelColor.B));
                }
            }
            return NewImage;
        }
        
        public void SVD(ref MMatrix mat, int newrank)
        {
            if (newrank >= Math.Min(mat.row, mat.col))
                return;

            mat.FactorSVD();
            //MMatrix.FactorSVD_LDLt(mat, mat.row); 

            MMatrix U = new MMatrix(mat.SVD_U, mat.row, newrank);
            MMatrix S = new MMatrix(mat.SVD_S, newrank, newrank);
            MMatrix Vt = new MMatrix(mat.SVD_Vt, newrank, mat.col);

            mat = U * S * Vt;
        }

        //Apply SVD on the whole bitmap. Very slow if the bitmap is large
        public Image SVD_nonFraction(ref Bitmap bmp, int newrank)
        {
            MMatrix tmpMat;

            if (newrank >= Math.Min(bmp.Height, bmp.Width))
                return bmp;

            MatrixFromBitmap(ref bmp);

            tmpMat = new MMatrix(MRed);
            SVD(ref tmpMat, newrank);
            MRed = FloorToBitmap(tmpMat.MArray);

            tmpMat = new MMatrix(MGreen);
            SVD(ref tmpMat, newrank);
            MGreen = FloorToBitmap(tmpMat.MArray);

            tmpMat = new MMatrix(MBlue);
            SVD(ref tmpMat, newrank);
            MBlue = FloorToBitmap(tmpMat.MArray);

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }
        //Apply SVD on each small piece of the bitmap
        public Image SVD_Fraction(Bitmap bmp, int newRank, ref double RootMeanSquareError)
        {
            int bestRank = (bmp.Height * bmp.Width) / (1 + bmp.Height + bmp.Width);
            if (newRank > bestRank)
                return bmp;

            int FactorRank = (int)Math.Round(BMP_FRAC_SIZE * ((double)newRank / bestRank));
            MatrixFromBitmap(ref bmp);

            int numx = bmp.Width / BMP_FRAC_SIZE;
            int numy = bmp.Height / BMP_FRAC_SIZE;

            MMatrix[] C = new MMatrix[numx];
            MMatrix tmpMat = new MMatrix(BMP_FRAC_SIZE, BMP_FRAC_SIZE);
            int FromRow, ToRow;
            MMatrix Red = new MMatrix(MRed);
            for (int k = 0; k < numx; ++k)
            {
                FromRow = k * BMP_FRAC_SIZE;
                ToRow = (k + 1) * BMP_FRAC_SIZE;
                tmpMat.Copy(Red, FromRow, ToRow, 0, BMP_FRAC_SIZE);
                SVD(ref tmpMat, FactorRank);
                C[k] = tmpMat;
                for (int m = 1; m < numy; ++m)
                {
                    tmpMat.Copy(Red, FromRow, ToRow, m * BMP_FRAC_SIZE, (m + 1) * BMP_FRAC_SIZE);
                    SVD(ref tmpMat, FactorRank);
                    C[k] = MMatrix.Concat_Horizontal(C[k], tmpMat);
                }
            }            
            for (int k = 1; k < numx; ++k)
                C[0] = MMatrix.Concat_Vertical(C[0], C[k]);
            MRed = FloorToBitmap(C[0].MArray);
            RootMeanSquareError = MMatrix.RootMeanSquareError(new MMatrix(MRed), C[0]);

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }
        
        private double f(int x, int y)
        {
            return MRed[x, y];
        }

        private double fj(int xIndex, int yIndex, int h)
        {
            int x1, x2, y1, y2;

            x1 = xIndex * h - h;
            y1 = yIndex * h - h;
            x2 = xIndex * h + h;
            y2 = yIndex * h + h;

            return ((MRed[x2, y2] - MRed[x1, y1]) / (2 * h));
        }

        //Finding discontinuities for piecewise-linear functions
        private Bitmap FindDis_PiecewiseLinear(Bitmap bmp, double delta)
        {
            int xMax = bmp.Width - 1;
            int yMax = bmp.Height - 1;
            Bitmap tmpBmp = new Bitmap(bmp.Width, bmp.Height);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

            const int h = 1;
            const int h2 = 2 * h;
            const int h22 = h2 * h;
            double epsilon = (2 * delta) / (h * h);
            //double max = round(1/h);
            //int found = 0;

            //(xj0,xj1), (yj0,yj1): interval containing the discontinuity point
            int xj0, xj, xj1, yj0, yj, yj1;
            double Gm;

            for (int i = 1; i < yMax; ++i)
            {
                yj0 = (i - 1);
                yj = i;
                yj1 = (i + 1);
                for (int m = 1; m < xMax; ++m)
                {
                    xj0 = (m - 1);
                    xj = m;
                    xj1 = (m + 1);
                    Gm = (MRed[xj1, yj1] - 2 * MRed[xj, yj] + MRed[xj0, yj0]) / (h22);

                    if ((Math.Min(xj1, xj) > h2) && (Math.Min(yj1, yj) > h2))
                    {
                        if (Math.Abs(Gm) > epsilon)
                        {
                            //Discontinuity point found
                            //found++;
                            //the jump
                            //pj = MRed[xj1, yj1] - MRed[xj0, yj0];

                            //draw an ellipse to keep track the point found
                            //g.DrawEllipse(p,Math.Abs(xj0+xj1)/2, Math.Abs(yj0+yj1)/2, PointWidth, PointHeight); 
                            tmpBmp.SetPixel((xj0 + xj1) / 2, (yj0 + yj1) / 2, Color.Black);
                        }
                    }
                }
            }
            return tmpBmp;
        }

        //Finding discontinuities for piecewise-smooth functions
        private Bitmap FindDis_PiecewiseSmooth(Bitmap bmp, double delta)
        {
            int xMax = bmp.Width - 2;
            int yMax = bmp.Height - 2;
            Bitmap tmpBmp = new Bitmap(bmp.Width, bmp.Height);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

            //(x1,x2), (y1,y2): interval containing the discontinuity point                            
            int x1, x2, y1, y2;

            const int h = 1;
            const int h3 = 3 * h;
            const int M2 = 255;
            double epsilon = Math.Sqrt(2 * M2 * delta);
            double fj0, fj1;
            //int found = 0;

            for (int i = 1; i < yMax; ++i)
            {
                y1 = i * h - h;
                y2 = y1 + h3;
                for (int j = 1; j < xMax; ++j)
                {
                    fj0 = fj(j, i, h);
                    fj1 = fj(j + 1, i + 1, h);

                    if (Math.Min(Math.Abs(fj0), Math.Abs(fj1)) > epsilon)
                    {
                        if (Math.Abs(fj1 - fj0) > 2 * epsilon)
                        {
                            //Discontinuity point found
                            //found++;
                            //the jump
                            //P = 2 * (fj1 - fj0) + 5 * epsilon;

                            //(x1,x2), (y1,y2): interval containing the discontinuity point                            
                            x1 = j * h - h;
                            x2 = x1 + h3;
                            //g.DrawLine(p, x1, y1, x2, y2);
                            tmpBmp.SetPixel(Convert.ToInt16((x1 + x2) * 0.5), Convert.ToInt16((y1 + y2) * 0.5), Color.Black);
                        }
                    }
                }
            }
            return tmpBmp;
        }

        public Image Bitmap_FindingDiscontinuities(Bitmap bmp, double Noise, int Method)
        {
            //GrayScale(bmp);
            MatrixFromBitmap(ref bmp);

            switch (Method)
            {
                case A_PIECEWISELINEAR:
                    return FindDis_PiecewiseLinear(bmp, Noise);
                case A_PIECEWISESMOOTH:
                    return FindDis_PiecewiseSmooth(bmp, Noise);
            }

            return bmp;
        }

        public Image Bitmap_InnerBorderTracing(Graphics g, Bitmap bmp)
        {
            Bitmap tmpBmp = (Bitmap)BlackWhite(bmp);

            int xMax = bmp.Width; //colume wrt to mat
            int yMax = bmp.Height; //row wrt to mat
            int Max = Math.Min(xMax, yMax);

            Pen p = new Pen(Color.Red);
            //detect the first point whose color is diferent from 
            //the color of the first top most and left most point of the bitmap
            int detectingColor = Math.Abs(MRed[0, 0] - 255);
            int PointWidth = 1;
            int PointHeight = 1;

            int Dir = 7; //direction for tracing
            int[] xDir = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 }; //colume direction
            int[] yDir = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 }; //row direction
            int DirMax = 8; //search in 8 directions

            //the first detected pixel
            int FirstX = 0; // colume
            int FirstY = 0; // row  
            //the second detected pixel
            int SecondX = 0;
            int SecondY = 0;
            //the current point
            int curX = 0; // current colume
            int curY = 0; // current row     
            //the previous point
            int preX = 0;
            int preY = 0;
            //the next point
            int nextX = 0;
            int nextY = 0;

            int count = 0;

            //search for the first white pixel
            for (int i = 0; i < Max; ++i)
            {
                if (MRed[i, i] >= detectingColor)
                {
                    FirstX = i;
                    FirstY = i;
                    //draw an ellipse to keep track the point found
                    g.DrawEllipse(p, FirstX, FirstY, PointWidth, PointHeight);
                    break;
                }
            }
            curX = FirstX;
            curY = FirstY;
            count++;

            //from the first white pixel, trace in 8 directions to find the next pixel having the same color
            while (true)
            {
                //search all directions               
                for (int i = 0; i < DirMax; ++i)
                {
                    nextX = curX + xDir[i];
                    nextY = curY + yDir[i];
                    if ((nextX < xMax) && (nextY < yMax) && (nextX >= 0) && (nextY >= 0))
                    {
                        if ((MRed[nextX, nextY] >= detectingColor) && (nextX != preX) && (nextY != preY))
                        {
                            preX = curX;
                            preY = curY;
                            curX = nextX;
                            curY = nextY;
                            if (count < 2)
                            {
                                SecondX = curX;
                                SecondY = curY;
                            }
                            count++;
                            //draw an ellipse to keep track the point found
                            g.DrawEllipse(p, curX, curY, PointWidth, PointHeight);
                            //update new direction
                            Dir = i;
                            break;
                        }
                    }
                }
                //update the direction
                if (Dir % 2 == 0)
                {
                    Dir = (Dir + 7) % 8;
                }
                else
                {
                    Dir = (Dir + 6) % 8;
                }
                //If the current boundary element Pn is equal to the second border element P2, 
                //and if the previous border element Pn-1 is equal to P1, stop. 
                if ((curX == SecondX) && (curY == SecondY) && (preX == FirstX) && (preY == FirstY) && (count > 2))
                {
                    break;
                }
            }
            return bmp;
        }
        //slow
        public Bitmap Bitmap_BorderTracing1(Bitmap bmp)
        {
            Bitmap tmpBmp = new Bitmap(bmp.Width, bmp.Height);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

            const int DirMax = 8; //search in 8 directions
            int[] xDir = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 }; //colume direction
            int[] yDir = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 }; //row direction             
            //the next point
            int nextX = 0;
            int nextY = 0;
            Bitmap BlackWhiteBmp = (Bitmap)BlackWhite(bmp);

            //trace in 8 directions to find the next pixel having the same color
            for (int i = 0; i < bmp.Width; ++i)
            {
                for (int j = 0; j < bmp.Height; ++j)
                {
                    for (int k = 0; k < DirMax; ++k)
                    {
                        nextX = i + xDir[k];
                        nextY = j + yDir[k];
                        if ((nextX < bmp.Width) && (nextY < bmp.Height) && (nextX >= 0) && (nextY >= 0))
                        {
                            //if the point around point (i,j) has different color, point (i,j) is the boundary point
                            if (MRed[nextX, nextY] != MRed[i, j])
                            {
                                //draw an ellipse to keep track the point found
                                //g.DrawEllipse(p, i, j, PointWidth, PointHeight); 
                                tmpBmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                                break;
                            }
                        }
                    }
                }
            }
            return tmpBmp;
        }

        public Bitmap Bitmap_BorderTracing(Bitmap bmp)
        {
            BlackWhite(bmp);
            return FindDis_PiecewiseLinear(bmp, 5);
        }

        public Image Bitmap_Skeleton(Bitmap bmp, Color color, int SkeletonDifference)
        {
            int xMax = bmp.Width; //colume wrt to mat
            int yMax = bmp.Height; //row wrt to mat
            Bitmap tmpBmp = new Bitmap(bmp);

            const int DirMax = 8; //search in 8 directions
            int[] xDir = { 1, 1, 0, -1, -1, -1, 0, 1 }; //colume direction
            int[] yDir = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 }; //row direction            

            //the next point
            int nextX = 0;
            int nextY = 0;

            MatrixFromBitmap(ref bmp);

            //trace in 8 directions to find the next pixel having the same color
            for (int i = 0; i < xMax; ++i)
            {
                for (int j = 0; j < yMax; ++j)
                {
                    for (int k = 0; k < DirMax; ++k)
                    {
                        nextX = i + xDir[k];
                        nextY = j + yDir[k];
                        if ((nextX < xMax) && (nextY < yMax) && (nextX >= 0) && (nextY >= 0))
                        {
                            //if the point around point (i,j) has different color, point (i,j) is the boundary point
                            if (Math.Abs(MRed[nextX, nextY] - MRed[i, j]) > SkeletonDifference)
                            {
                                tmpBmp.SetPixel(i, j, color);
                                break;
                            }
                        }
                    }
                }
            }
            return tmpBmp;
        }     

        public Bitmap CreateNoise(Bitmap Old, double Noise)
        {
            Bitmap bmp = new Bitmap(Old);

            MatrixFromBitmap(ref bmp);
            MMatrix RandMatrix = MMatrix.RandomMatrix(bmp.Width, bmp.Height, 0, (int)(255 * Noise));
            MRed = FloorToBitmap(((new MMatrix(MRed)) + RandMatrix).MArray);
            MGreen = FloorToBitmap(((new MMatrix(MGreen)) + RandMatrix).MArray);
            MBlue = FloorToBitmap(((new MMatrix(MBlue)) + RandMatrix).MArray);

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }      

        public Bitmap Sharpen(Bitmap bmp, int Threshold_High, int Threshold_Low, byte ChangeStep)
        {
            MatrixFromBitmap(ref bmp);

            for (int i = 0; i < bmp.Width; ++i)
                for (int j = 0; j < bmp.Height; ++j)
                {
                    if ((MRed[i, j] > Threshold_High) && (MRed[i, j] < 255 - ChangeStep))
                        MRed[i, j] += ChangeStep;
                    else if ((MRed[i, j] < Threshold_Low) && (MRed[i, j] > ChangeStep))
                        MRed[i, j] -= ChangeStep;

                    if ((MGreen[i, j] > Threshold_High) && (MGreen[i, j] < 255 - ChangeStep))
                        MGreen[i, j] += ChangeStep;
                    else if ((MGreen[i, j] < Threshold_Low) && (MGreen[i, j] > ChangeStep))
                        MGreen[i, j] -= ChangeStep;

                    if ((MBlue[i, j] > Threshold_High) && (MBlue[i, j] < 255 - ChangeStep))
                        MBlue[i, j] += ChangeStep;
                    else if ((MBlue[i, j] < Threshold_Low) && (MBlue[i, j] > ChangeStep))
                        MBlue[i, j] -= ChangeStep;
                }

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }       

        public Bitmap Gauss_Convolution(Bitmap bmp, bool bCanny, int rows, double sigma1, int cols, double sigma2, double theta)
        {
            int NewRGB;

            MatrixFromBitmap(ref bmp);
            MRed = FloorToBitmap(MMatrix.Gauss_Convolution(new MMatrix(MRed), bCanny, rows, sigma1, cols, sigma2, theta).MArray);
            MGreen = FloorToBitmap(MMatrix.Gauss_Convolution(new MMatrix(MGreen), bCanny, rows, sigma1, cols, sigma2, theta).MArray);
            MBlue = FloorToBitmap(MMatrix.Gauss_Convolution(new MMatrix(MBlue), bCanny, rows, sigma1, cols, sigma2, theta).MArray);

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }        

        public Bitmap Canny(Bitmap bmp)
        {
            //The algorithm parameters:
            //1. Parameters of edge detecting filters:
            //   X-axis direction filter:
            const int Nx1 = 3;
            const int Nx2 = 3;
            const double Sigmax1 = 1.4;
            const double Sigmax2 = 1.4;
            const double Theta1 = GlobalMath.HALFPI;
            //   Y-axis direction filter:           
            //const int Ny1 = 3;
            //const int Ny2 = 3;
            //const double Sigmay1 = 1.4;
            //const double Sigmay2 = 1.4;
            //const double Theta2 = 0;
            //2. The thresholding parameter alfa:
            //double alfa=0.1;                             

            //if (!IsGrayScale(Bmp))
            //    Bmp = (Bitmap)GrayScale(Bmp);            
            MatrixFromBitmap(ref bmp);

            MMatrix Mx = MMatrix.Gauss_Convolution(new MMatrix(MRed), true, Nx1, Sigmax1, Nx2, Sigmax2, Theta1);
            //MMatrix My = MMatrix.Gauss_Convolution(MRed, true, Ny1, Sigmay1, Ny2, Sigmay2, Theta2);
            //My = My / MMatrix.Sum(My);
            //My = My + MMatrix.ScalarMatrix(My.row, My.col, 125);
            //MMatrix MNorm = MMatrix.PowEntry(MMatrix.PowEntry(Mx, 2) + MMatrix.PowEntry(My, 2), 0.5);
            //Bmp = BitmapFromMatrix(MNorm, MNorm, MNorm);

            Mx = MMatrix.PowEntry(6 * MMatrix.PowEntry(Mx, 2), 0.5);
            byte[,] MNorm = FloorToBitmap(Mx.MArray);
            for (int i = 0; i < bmp.Width; ++i)
                for (int j = 0; j < bmp.Height; ++j)
                    if (MRed[i, j] < 50)
                    {
                        MRed[i, j] = MNorm[i, j];
                        MGreen[i, j] = MNorm[i, j];
                        MBlue[i, j] = MNorm[i, j];
                    }

            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }        

        public Bitmap BitmapBitwise(Bitmap Bmp1, Bitmap Bmp2, string Operation)
        {
            MatrixFromBitmap(ref Bmp1);
            ImageProcessing IP = new ImageProcessing();
            IP.MatrixFromBitmap(ref Bmp2);

            if (Operation == "&")
                for (int i = 0; i < Bmp1.Width; ++i)
                    for (int j = 0; j < Bmp1.Height; ++j)
                    {
                        MRed[i, j] &= IP.MRed[i, j];
                        MGreen[i, j] &= IP.MGreen[i, j];
                        MBlue[i, j] &= IP.MBlue[i, j];
                    }
            else if (Operation == "|")
                for (int i = 0; i < Bmp1.Width; ++i)
                    for (int j = 0; j < Bmp1.Height; ++j)
                    {
                        MRed[i, j] |= IP.MRed[i, j];
                        MGreen[i, j] |= IP.MGreen[i, j];
                        MBlue[i, j] |= IP.MBlue[i, j];
                    }
            else //if (Operation == "^")
                for (int i = 0; i < Bmp1.Width; ++i)
                    for (int j = 0; j < Bmp1.Height; ++j)
                    {
                        MRed[i, j] ^= IP.MRed[i, j];
                        MGreen[i, j] ^= IP.MGreen[i, j];
                        MBlue[i, j] ^= IP.MBlue[i, j];
                    }
            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }

        public Bitmap BitmapMix(Bitmap Bmp1, Bitmap Bmp2)
        {
            Bitmap tmpBmp = new Bitmap(Bmp1.Width, Bmp1.Height);

            MatrixFromBitmap(ref Bmp1);
            ImageProcessing IP = new ImageProcessing();
            IP.MatrixFromBitmap(ref Bmp2);

            for (int i = 0; i < Bmp1.Width; ++i)
                for (int j = 0; j < Bmp1.Height; ++j)
                {
                    MRed[i, j] ^= IP.MRed[i, j];
                    MGreen[i, j] |= IP.MGreen[i, j];
                    MBlue[i, j] &= IP.MBlue[i, j];
                }
            return BitmapFromMatrix(ref MRed, ref MGreen, ref MBlue);
        }
        #endregion
    }
}
