using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class ImageProcessing : IDisposable
    {
        private MMatrix MRed;
        private MMatrix MGreen;
        private MMatrix MBlue;
        private bool bGrayScale;

        public const int BITMAP_FRACTION_SIZE = 6;
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

        ~ImageProcessing()
        {
            Dispose();                   
        }

        public void Dispose()
        {
            if (MRed != null)
                MRed.Dispose();
            if(MGreen != null)
                MGreen.Dispose();
            if(MBlue != null)
                MBlue.Dispose();
            GC.SuppressFinalize(this);
        }

        #region Pixel Processing
        public void MatrixFromBitmap(Bitmap bmp)
        {                        
            Color PixelColor;
            bGrayScale = IsGrayScale(bmp);
            //unsafe
            {
                if (bGrayScale)
                {
                    MRed = new MMatrix(bmp.Width, bmp.Height);
                    for (int i = 0; i < bmp.Width; i++)
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            PixelColor = bmp.GetPixel(i, j);
                            MRed[i, j] = PixelColor.R;
                        }
                }
                else
                {
                    MRed = new MMatrix(bmp.Width, bmp.Height);
                    MGreen = new MMatrix(bmp.Width, bmp.Height);
                    MBlue = new MMatrix(bmp.Width, bmp.Height);
                    for (int i = 0; i < bmp.Width; i++)
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            PixelColor = bmp.GetPixel(i, j);
                            MRed[i, j] = PixelColor.R;
                            MGreen[i, j] = PixelColor.G;
                            MBlue[i, j] = PixelColor.B;
                        }
                }
            }    
        }

        private static Bitmap BitmapFromMatrix(MMatrix MRed, MMatrix MGreen, MMatrix MBlue)
        {
            Bitmap tmpBmp = new Bitmap(MRed.row,MRed.col);

            for (int i = 0; i < MRed.row; i++)
            {
                for (int j = 0; j < MRed.col; j++)
                {
                    tmpBmp.SetPixel(i, j, Color.FromArgb((int)MRed[i, j], (int)MGreen[i, j], (int)MBlue[i, j]));
                }
            }

            return tmpBmp;
        }

        public static Bitmap DrawTextToBitmap(Bitmap Bmp, string s, int x, int y)
        {
            Graphics g = Graphics.FromImage((Image)Bmp);

            g.DrawString(s, SystemFonts.DefaultFont, Brushes.Black, new Point(x, y));

            return Bmp;
        }

        private void FloorToBitmap(ref MMatrix BitmapMatrix)
        {
            //BitmapMatrix = MMatrix.Abs(BitmapMatrix);

            for (int i = 0; i < BitmapMatrix.row; i++)
                for (int j = 0; j < BitmapMatrix.col; j++)
                {
                    if (BitmapMatrix[i, j] > 255)
                        BitmapMatrix[i, j] = 255;
                    else if (double.IsNaN(BitmapMatrix[i, j]))
                        BitmapMatrix[i, j] = MRed[i, j]; 
                    else if (BitmapMatrix[i, j] < 0)
                        BitmapMatrix[i, j] = 0;
                }
        }

        public static bool IsGrayScale(Bitmap bmp)
        {
            Color PixelColor;
            bool bGrayScale = true;

            PixelColor = bmp.GetPixel(0, 0);
            if ((PixelColor.R != PixelColor.G) || (PixelColor.R != PixelColor.B) || (PixelColor.G != PixelColor.B))
                bGrayScale = false;
            PixelColor = bmp.GetPixel(bmp.Width/2, bmp.Height/2);
            if ((PixelColor.R != PixelColor.G) || (PixelColor.R != PixelColor.B) || (PixelColor.G != PixelColor.B))
                bGrayScale = false;         

            return bGrayScale;
        }
        #endregion

        #region Basic Image Processing
        public static Image GrayScale(Bitmap Old)
        {
            Bitmap NewBmp = new Bitmap(Old.Width, Old.Height);
            int NewRGB;
            Color PixelColor;

            for (int i = 0; i < NewBmp.Width; i++)
                for (int j = 0; j < NewBmp.Height; j++)
                {
                    PixelColor = Old.GetPixel(i, j);
                    NewRGB = (PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    NewBmp.SetPixel(i, j, Color.FromArgb(NewRGB, NewRGB, NewRGB));
                }

            return NewBmp;
        }       

        public static Image BlackWhite(Bitmap Old)
        {
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            Color PixelColor;

            for (int i = 0; i < Old.Width; i++)
            {
                for (int j = 0; j < Old.Height; j++)
                {
                    PixelColor = Old.GetPixel(i, j);
                    if ((PixelColor.R + PixelColor.G + PixelColor.B) / 3 < 125)
                        NewImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    else
                        NewImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            }
            return NewImage;
        }

        public static Image Negate(Bitmap Old)
        {
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            Color PixelColor;

            for (int i = 0; i < Old.Width; i++)
            {
                for (int j = 0; j < Old.Height; j++)
                {
                    PixelColor = Old.GetPixel(i, j);
                    NewImage.SetPixel(i, j, Color.FromArgb(255 - PixelColor.R, 255 - PixelColor.G, 255 - PixelColor.B));
                }
            }
            return NewImage;
        }

        public static Image ZoomOut(Bitmap Old, int newWidth, int newHeight)
        {
            Bitmap New = new Bitmap(newWidth, newHeight);            
            Color PixelColor;
            double WidthRate, HeightRate;
            int x, y;

            WidthRate = (double)Old.Width / newWidth;
            HeightRate = (double)Old.Height / newHeight;

            for (int i = 0; i < newWidth; i++)
                for (int j = 0; j < newHeight; j++)
                {
                    x = (int)(i * WidthRate);
                    y = (int)(j * HeightRate);
                    PixelColor = Old.GetPixel(x, y);                    
                    New.SetPixel(i, j, Color.FromArgb(PixelColor.R, PixelColor.G, PixelColor.B));
                }

            return New;
        }

        public static Image Rotate(Bitmap Old, double Angle)
        {
            Angle = Angle * Math.PI / 180;
            Bitmap NewImage = new Bitmap(Old.Width, Old.Height);
            double[,] TransformMatrix = new double[2, 2] { { Math.Cos(Angle), -Math.Sin(Angle) }, { Math.Sin(Angle), Math.Cos(Angle) } };
            int x, y;
            Color PixelColor;

            for (int i = 0; i < Old.Width; i++)
            {
                for (int j = 0; j < Old.Height; j++)
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

        //Very slow if the bitmap is large
        public Image SVD_nonFraction(Bitmap bmp,int newrank)
        {
            MMatrix newMatrix = new MMatrix();
            MMatrix newUMatrix = new MMatrix();
            MMatrix newSMatrix = new MMatrix();
            MMatrix newVtMatrix = new MMatrix();
            MMatrix B = new MMatrix();

            if (newrank >= Math.Min(bmp.Height, bmp.Width))
                return bmp;

            MatrixFromBitmap(bmp);   
         
            if (bGrayScale)
            {
                newMatrix = MRed;                             
                newMatrix.FactorSVD();
                //MMatrix.FactorSVD_LDLt(newMatrix, newMatrix.row);
                
                newUMatrix = new MMatrix(newMatrix.SVD_U, newMatrix.row, newrank);
                newSMatrix = new MMatrix(newMatrix.SVD_S, newrank, newrank);
                newVtMatrix = new MMatrix(newMatrix.SVD_Vt, newrank, newMatrix.col);                

                B = newUMatrix * newSMatrix * newVtMatrix;
                FloorToBitmap(ref B);

                for (int i = 0; i < B.row; i++)
                    for (int j = 0; j < B.col; j++)
                        bmp.SetPixel(i, j, Color.FromArgb((int)B[i, j], (int)B[i, j], (int)B[i, j]));                                   
            }
            else
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)                    
                        bmp.SetPixel(i, j, Color.FromArgb(255 - (int)MRed[i, j], 255 - (int)MGreen[i, j], 255 - (int)MBlue[i, j]));                    

            return bmp;
        }

        public static void SVD(ref MMatrix matrix, int newrank)
        {            
            MMatrix newUMatrix = new MMatrix();
            MMatrix newSMatrix = new MMatrix();
            MMatrix newVtMatrix = new MMatrix();            

            if (newrank >= Math.Min(matrix.row,matrix.col))
                return;

            matrix.FactorSVD();            

            newUMatrix = new MMatrix(matrix.SVD_U, matrix.row, newrank);
            newSMatrix = new MMatrix(matrix.SVD_S, newrank, newrank);
            newVtMatrix = new MMatrix(matrix.SVD_Vt, newrank, matrix.col);

            matrix = newUMatrix * newSMatrix * newVtMatrix;

            newUMatrix.Dispose();
            newSMatrix.Dispose();
            newVtMatrix.Dispose();
            newUMatrix = null;
            newSMatrix = null;
            newVtMatrix = null;
        }

        public Image Bitmap_SVD(Bitmap bmp, int newRank, ref double RootMeanSquareError)
        {
            int numx,numy;            
            int FactorMatrixRank, bestRank;
            MMatrix newMatrix = new MMatrix();
            MMatrix B = new MMatrix();
            Bitmap TmpBmp = new Bitmap(bmp);

            bestRank = (bmp.Height * bmp.Width) / (1 + bmp.Height + bmp.Width);
            if (newRank > bestRank)
                return bmp;
            
            FactorMatrixRank = (int)Math.Round(BITMAP_FRACTION_SIZE * ((double)newRank / bestRank));

            MatrixFromBitmap(bmp);

            if (bGrayScale)
            {
                newMatrix = MRed;

                numx = (int)Math.Ceiling((double)MRed.row / BITMAP_FRACTION_SIZE);
                numy = (int)Math.Ceiling((double)MRed.col / BITMAP_FRACTION_SIZE);
                MMatrix[,] MatrixOfMatrices = new MMatrix[numx, numy];

                for (int k = 0; k < numx; k++)
                    for (int m = 0; m < numy; m++)
                    {
                        MatrixOfMatrices[k, m] = new MMatrix(newMatrix, k * BITMAP_FRACTION_SIZE, (k + 1) * BITMAP_FRACTION_SIZE, m * BITMAP_FRACTION_SIZE, (m + 1) * BITMAP_FRACTION_SIZE);
                        SVD(ref MatrixOfMatrices[k, m], FactorMatrixRank);
                    }

                MMatrix[] C = new MMatrix[numx];
                for (int k = 0; k < numx; k++)
                {
                    C[k] = MatrixOfMatrices[k, 0];                    
                    for (int m = 1; m < numy; m++)
                    {
                        C[k] = MMatrix.Concat_Horizontal(C[k], MatrixOfMatrices[k, m]);
                        MatrixOfMatrices[k, m].Dispose();
                        MatrixOfMatrices[k, m] = null;
                    }
                    MatrixOfMatrices[k, 0].Dispose();
                    MatrixOfMatrices[k, 0] = null;
                }
                B = C[0];
                for (int k = 1; k < numx; k++)
                {
                    B = MMatrix.Concat_Vertical(B, C[k]);
                    C[k].Dispose();
                    C[k] = null;
                }
                C[0].Dispose();
                C[0] = null;
                
                FloorToBitmap(ref B);
                RootMeanSquareError = MMatrix.RootMeanSquareError(newMatrix, B);

                for (int i = 0; i < B.row; i++)
                    for (int j = 0; j < B.col; j++)
                        TmpBmp.SetPixel(i, j, Color.FromArgb((int)B[i, j], (int)B[i, j], (int)B[i, j]));
                
                B.Dispose();
                newMatrix.Dispose();                
                C = null;
                B = null;                                
                newMatrix = null;                
                MatrixOfMatrices = null;
            }
            return TmpBmp;
        }

        private double f(int x, int y)
        {        
            return MRed[x, y];
        }

        private double fj(int xIndex, int yIndex, int h)
        {
            int x1, x2, y1, y2;
            double f1, f2;

            x1 = xIndex * h - h;
            y1 = yIndex * h - h;
            f1 = f(x1, y1);

            x2 = xIndex * h + h;
            y2 = yIndex * h + h;
            f2 = f(x2, y2);

            return ((f2 - f1) / (2 * h));
        }

        private Bitmap FindDis_PiecewiseLinear(Graphics g, Bitmap bmp, double delta)
        {
            //Finding discontinuities for piecewise-linear functions
            
            int xMax = bmp.Width;
            int yMax = bmp.Height;            
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, xMax, yMax);
            
            //Pen p = new Pen(Color.Red);
            //int PointWidth = 1;
            //int PointHeight = 1;   

            double h = 1;
            //double max = round(1/h);
            int found = 0;            

            //(xj0,xj1), (yj0,yj1): interval containing the discontinuity point
            int xj0,xj,xj1,yj0,yj,yj1;
            double Gm,pj;

            for(int i = 1; i < yMax-1; i++)
            {
                for(int m = 1; m < xMax-1; m++)
                {
                    xj0 = (m-1);
                    xj = m;
                    xj1 = (m+1);
                    
                    yj0 = (i-1);
                    yj = i;
                    yj1 = (i+1);
                    Gm = (f(xj1,yj1) - 2*f(xj,yj) + f(xj0,yj0))/(2*h*h);

                    if ((Math.Min(xj1, xj) > 2 * h) && (Math.Min(yj1, yj) > 2 * h))
                    {
                        if (Math.Abs(Gm) > (2 * delta) / (h*h))
                        {
                            //Discontinuity point found
                            found++;
                            //the jump
                            pj = f(xj1, yj1) - f(xj0, yj0);

                            //(xj0,xj1), (yj0,yj1): interval containing the discontinuity point                                                            
                            //g.DrawLine(p, xj0, yj0, xj1, yj1);
                            //draw an ellipse to keep track the point found
                            //g.DrawEllipse(p,Math.Abs(xj0+xj1)/2, Math.Abs(yj0+yj1)/2, PointWidth, PointHeight); 
                            tmpBmp.SetPixel(Math.Abs(xj0 + xj1) / 2, Math.Abs(yj0 + yj1) / 2, Color.FromArgb(0, 0, 0));
                        }
                    }
                }
            }
            return tmpBmp;

        }

        private Bitmap FindDis_PiecewiseSmooth(Graphics g, Bitmap bmp, double delta)
        {
            //Finding discontinuities for piecewise-smooth functions

            int xMax = bmp.Width;
            int yMax = bmp.Height;
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, xMax, yMax);

            //Pen p = new Pen(Color.Red);
            //(x_1,x_2), (y_1,y_2): interval containing the discontinuity point                            
            int x_1, x_2, y_1, y_2;            

            int h = 1;
            double M2 = 255;
            double epsilon = Math.Sqrt(2 * M2 * delta);
            double fj0, fj1, P;
            int found = 0;                       

            for (int i = 1; i < yMax - 2; i++)
            {
                for (int j = 1; j < xMax - 2; j++)
                {
                    fj0 = fj(j, i, h);
                    fj1 = fj(j + 1, i + 1, h);
                   
                    if (Math.Min(Math.Abs(fj0), Math.Abs(fj1)) > epsilon) 
                    {
                        if (Math.Abs(fj1 - fj0) > 2 * epsilon)
                        {
                            //Discontinuity point found
                            found++;
                            //the jump
                            P = 2 * (fj1 - fj0) + 5 * epsilon;

                            //(x_1,x_2), (y_1,y_2): interval containing the discontinuity point                            
                            x_1 = j * h - h;
                            x_2 = j * h + 2 * h;
                            y_1 = i * h - h;
                            y_2 = i * h + 2 * h;
                            //g.DrawLine(p, x_1, y_1, x_2, y_2);
                            tmpBmp.SetPixel(Math.Abs(x_1 + x_2) / 2, Math.Abs(y_1 + y_2) / 2, Color.FromArgb(0, 0, 0));
                        }
                    }
                }
            }
            return tmpBmp;
        }

        public Image Bitmap_FindingDiscontinuities(Graphics g, Bitmap bmp, double Noise,int Method)
        {
            if (!IsGrayScale(bmp))
                bmp = (Bitmap)GrayScale(bmp);
            MatrixFromBitmap(bmp);

            switch (Method)
            {
                case A_PIECEWISELINEAR:
                    return FindDis_PiecewiseLinear(g, bmp, Noise);                    
                case A_PIECEWISESMOOTH:
                    return FindDis_PiecewiseSmooth(g, bmp, Noise);                    
            }

            return bmp;           
        }

        public Image Bitmap_InnerBorderTracing(Graphics g, Bitmap bmp)
        {
            Bitmap tmpBmp = (Bitmap)BlackWhite(bmp);

            MatrixFromBitmap(tmpBmp);

            int xMax = bmp.Width; //colume wrt to matrix
            int yMax = bmp.Height; //row wrt to matrix
            int Max = Math.Min(xMax, yMax);

            Pen p = new Pen(Color.Red);
            //detect the first point whose color is diferent from 
            //the color of the first top most and left most point of the bitmap
            int detectingColor = (int)Math.Abs(f(0,0) - 255);  
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
            for (int i = 0; i < Max; i++)
            {                
                if (f(i, i) >= detectingColor)
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
            while(true)
            {
                //search all directions               
                for (int i = 0; i < DirMax; i++)
                {
                    nextX = curX + xDir[i];
                    nextY = curY + yDir[i];
                    if ((nextX < xMax) && (nextY < yMax) && (nextX >= 0) && (nextY >= 0))
                    {
                        if ((f(nextX, nextY) >= detectingColor) && (nextX != preX) && (nextY != preY))
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

        public Bitmap Bitmap_BorderTracing(Graphics g, Bitmap bmp)
        {            
            int xMax = bmp.Width; //colume wrt to matrix
            int yMax = bmp.Height; //row wrt to matrix
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, xMax, yMax);

            //Bitmap tmpBmp = new Bitmap(bmp);                        
            //Pen p = new Pen(Color.Red);
            //int PointWidth = 1;
            //int PointHeight = 1;            

            int DirMax = 8; //search in 8 directions
            int[] xDir = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 }; //colume direction
            int[] yDir = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 }; //row direction             
            //the next point
            int nextX = 0;
            int nextY = 0;
            Bitmap BlackWhiteBmp = (Bitmap)BlackWhite(bmp);
            MatrixFromBitmap(BlackWhiteBmp);

            //trace in 8 directions to find the next pixel having the same color
            for (int i = 0; i < xMax; i++)
            {
                for (int j = 0; j < yMax; j++)
                {
                    for (int k = 0; k < DirMax; k++)
                    {
                        nextX = i + xDir[k];
                        nextY = j + yDir[k];
                        if ((nextX < xMax) && (nextY < yMax) && (nextX >= 0) && (nextY >= 0))
                        {
                            //if the point around point (i,j) has different color, point (i,j) is the boundary point
                            if (f(nextX, nextY) != f(i, j))
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

        public Image Bitmap_Skeleton(Bitmap bmp, Color color, int SkeletonDifference)
        {                        
            int xMax = bmp.Width; //colume wrt to matrix
            int yMax = bmp.Height; //row wrt to matrix
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            Graphics g1 = Graphics.FromImage((Image)tmpBmp);
            g1.FillRectangle(Brushes.White, 0, 0, xMax, yMax);

            int DirMax = 8; //search in 8 directions
            int[] xDir = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 }; //colume direction
            int[] yDir = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 }; //row direction            

            //the next point
            int nextX = 0;
            int nextY = 0;

            if (!IsGrayScale(bmp))
                bmp = (Bitmap)GrayScale(bmp);
            MatrixFromBitmap(bmp);

            //trace in 8 directions to find the next pixel having the same color
            for (int i = 0; i < xMax; i++)
            {
                for (int j = 0; j < yMax; j++)
                {
                    for (int k = 0; k < DirMax; k++)
                    {
                        nextX = i + xDir[k];
                        nextY = j + yDir[k];
                        if ((nextX < xMax) && (nextY < yMax) && (nextX >= 0) && (nextY >= 0))
                        {
                            //if the point around point (i,j) has different color, point (i,j) is the boundary point
                            if (Math.Abs(f(nextX, nextY) - f(i, j)) > SkeletonDifference)
                            {
                                tmpBmp.SetPixel(i, j, Color.FromArgb(color.ToArgb()));
                                break;
                            }
                        }
                    }
                }
            }
            return tmpBmp;
        }

        public Bitmap CreateNoise(Bitmap bmp, double Noise)
        {
            int xMax = bmp.Width; //colume wrt to matrix
            int yMax = bmp.Height; //row wrt to matrix
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            MMatrix RandMatrix = MMatrix.RandomMatrix(xMax, yMax, 0, 255);
            int NewRGB;

            MatrixFromBitmap(bmp);

            RandMatrix = RandMatrix * Noise;

            MRed += RandMatrix;
            FloorToBitmap(ref MRed);

            if (this.bGrayScale)
            {
                for (int i = 0; i < xMax; i++)
                {
                    for (int j = 0; j < yMax; j++)
                    {
                        NewRGB = (int)MRed[i, j];
                        tmpBmp.SetPixel(i, j, Color.FromArgb(NewRGB, NewRGB, NewRGB));
                    }
                }
            }
            else
            {
                MGreen += RandMatrix;
                FloorToBitmap(ref MGreen);
                MBlue += RandMatrix;
                FloorToBitmap(ref MBlue);

                for (int i = 0; i < xMax; i++)
                {
                    for (int j = 0; j < yMax; j++)
                    {
                        tmpBmp.SetPixel(i, j, Color.FromArgb((int)MRed[i, j], (int)MGreen[i, j], (int)MBlue[i, j]));
                    }
                }
            }

            return tmpBmp;
        }        

        public static Bitmap Sharpen(Bitmap bmp, int Threshold_High, int Threshold_Low, int ChangeStep)
        {
            int xMax = bmp.Width; 
            int yMax = bmp.Height; 
            Bitmap NewBmp = new Bitmap(bmp);            
            int NewR, NewG, NewB;            
            Color PixelColor;
            bool bGrayScale = IsGrayScale(bmp);

            if (bGrayScale)
            {
                for (int i = 0; i < xMax; i++)
                    for (int j = 0; j < yMax; j++)
                    {
                        PixelColor = bmp.GetPixel(i, j);

                        NewR = PixelColor.R;
                        if ((PixelColor.R > Threshold_High) && (PixelColor.R < 255 - ChangeStep))
                        {
                            NewR += ChangeStep;
                        }
                        else
                            if ((PixelColor.R < Threshold_Low) && (PixelColor.R > ChangeStep))
                            {
                                NewR -= ChangeStep;
                            }
                        NewBmp.SetPixel(i, j, Color.FromArgb(NewR, NewR, NewR));
                    }
            }
            else
            {
                for (int i = 0; i < xMax; i++)
                    for (int j = 0; j < yMax; j++)
                    {
                        PixelColor = bmp.GetPixel(i, j);

                        NewR = PixelColor.R;
                        if ((PixelColor.R > Threshold_High) && (PixelColor.R < 255 - ChangeStep))
                        {
                            NewR += ChangeStep;
                        }
                        else
                            if ((PixelColor.R < Threshold_Low) && (PixelColor.R > ChangeStep))
                            {
                                NewR -= ChangeStep;
                            }

                        NewG = PixelColor.G;
                        if ((PixelColor.G > Threshold_High) && (PixelColor.G < 255 - ChangeStep))
                        {
                            NewG += ChangeStep;
                        }
                        else
                            if ((PixelColor.G < Threshold_Low) && (PixelColor.G > ChangeStep))
                            {
                                NewG -= ChangeStep;
                            }

                        NewB = PixelColor.B;
                        if ((PixelColor.B > Threshold_High) && (PixelColor.B < 255 - ChangeStep))
                        {
                            NewB += ChangeStep;
                        }
                        else
                            if ((PixelColor.B < Threshold_Low) && (PixelColor.B > ChangeStep))
                            {
                                NewB -= ChangeStep;
                            }
                        NewBmp.SetPixel(i, j, Color.FromArgb(NewR, NewG, NewB));
                    }
            }                   
                
            return NewBmp;            
        }

        //Blur Image
        public Bitmap Gauss_Convolution(Bitmap bmp, bool bCanny, int rows, double sigma1, int cols, double sigma2, double theta)
        {
            int xMax = bmp.Width;
            int yMax = bmp.Height;
            Bitmap tmpBmp = new Bitmap(xMax, yMax);
            int NewRGB;

            MatrixFromBitmap(bmp);

            if (this.bGrayScale)
            {
                MRed = MMatrix.Gauss_Convolution(MRed, bCanny, rows, sigma1, cols, sigma2, theta);
                FloorToBitmap(ref MRed);
                for (int i = 0; i < xMax; i++)
                {
                    for (int j = 0; j < yMax; j++)
                    {
                        NewRGB = (int)MRed[i, j];
                        tmpBmp.SetPixel(i, j, Color.FromArgb(NewRGB, NewRGB, NewRGB));
                    }
                }
            }
            else
            {
                MRed = MMatrix.Gauss_Convolution(MRed, bCanny, rows, sigma1, cols, sigma2, theta);
                FloorToBitmap(ref MRed);
                MGreen = MMatrix.Gauss_Convolution(MGreen, bCanny, rows, sigma1, cols, sigma2, theta);
                FloorToBitmap(ref MGreen);
                MBlue = MMatrix.Gauss_Convolution(MBlue, bCanny, rows, sigma1, cols, sigma2, theta);
                FloorToBitmap(ref MBlue);
                for (int i = 0; i < xMax; i++)
                {
                    for (int j = 0; j < yMax; j++)
                    {
                        tmpBmp.SetPixel(i, j, Color.FromArgb((int)MRed[i, j], (int)MGreen[i, j], (int)MBlue[i, j]));
                    }
                }
            }

            return tmpBmp;
        }

        public Bitmap Canny(Bitmap Bmp)
        {
            //The algorithm parameters:
            //1. Parameters of edge detecting filters:
            //   X-axis direction filter:
            int Nx1=3;
            int Nx2=3;
            double Sigmax1 = 1.4;
            double Sigmax2 = 1.4;
            double Theta1 = Math.PI / 2;
            //   Y-axis direction filter:           
            int Ny1 = 3;
            int Ny2 = 3;
            double Sigmay1 = 1.4;
            double Sigmay2 = 1.4;
            double Theta2 = 0;                
            //2. The thresholding parameter alfa:
            //double alfa=0.1;                                 

            if (!IsGrayScale(Bmp))
                Bmp = (Bitmap)GrayScale(Bmp);

            //Bitmap tmpBmpx = Gauss_Convolution(Bmp, true, Nx1, Sigmax1, Nx2, Sigmax2, Theta1);
            //Bitmap tmpBmpy = Gauss_Convolution(Bmp, true, Ny1, Sigmay1, Ny2, Sigmay2, Theta2);
            
            MatrixFromBitmap(Bmp);

            MMatrix Mx = MMatrix.Gauss_Convolution(MRed, true, Nx1, Sigmax1, Nx2, Sigmax2, Theta1);
            //FloorToBitmap(ref Mx);

            MMatrix My = MMatrix.Gauss_Convolution(MRed, true, Ny1, Sigmay1, Ny2, Sigmay2, Theta2);
            My = My / MMatrix.Sum(My);
            //My = My + MMatrix.ScalarMatrix(My.row, My.col, 125);
            //FloorToBitmap(ref My);

            MMatrix Norm = MMatrix.PowEntry(MMatrix.PowEntry(Mx,2) + MMatrix.PowEntry(My,2),0.5);            
            FloorToBitmap(ref Norm);

            //Bitmap tmpBmp = BitmapFromMatrix(Mx, Mx, Mx);
            //Bitmap tmpBmp = BitmapFromMatrix(My, My, My);
            Bitmap tmpBmp = BitmapFromMatrix(Norm, Norm, Norm);

            return tmpBmp;
        }

        #endregion                        
    }
}
