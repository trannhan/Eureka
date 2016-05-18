using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class Vector
    {
        public double[] Elements;         

        public Vector()
        {
                       
        }

        public Vector(int size)
        {            
            Elements = new double[size];
        }

        public Vector(double[] Arr)
        {
            Elements = (double[])Arr.Clone();
        }

        public Vector(Vector A)
        {
            Elements = (double[])A.Elements.Clone();
        }

        ~Vector()
        {
        }

        public Vector Clone()
        {
            return (new Vector(this));
        }

        public double this[int i]
        {
            get
            {
                //return (double)this.Elements.GetValue(i);
                return Elements[i];
            }
            set
            {
                //this.Elements.SetValue(value,i);
                Elements[i] = value;
            }
        }

        public static Vector RandomVector(int NumElements, int minvalue, int maxvalue)
        {
            Vector A = new Vector(NumElements);
            Random ranobj = new Random();
            for (int i = 0; i < NumElements; ++i)
                A[i] = ranobj.Next(minvalue, maxvalue);

            return A;
        }

        public static Vector operator +(Vector A, Vector B)
        {
            if (A.Elements.Length != B.Elements.Length)
                throw new MMatrixException("Two vectors are different size.");

            Vector C = new Vector(A.Elements.Length);
            for (int i = 0; i < A.Elements.Length; ++i)
            {
                C[i] = A[i] + B[i];
            }

            return C;
        }

        public static Vector operator -(Vector A, Vector B)
        {
            if (A.Elements.Length != B.Elements.Length)
                throw new MMatrixException("Two vectors are different size.");

            Vector C = new Vector(A.Elements.Length);
            for (int i = 0; i < A.Elements.Length; ++i)
            {
                C[i] = A[i] - B[i];
            }

            return C;
        }

        public static double operator *(Vector A, Vector B)
        {
            if (A.Elements.Length != B.Elements.Length)
                throw new MMatrixException("Two vectors are different size.");

            double c = 0;
            for (int i = 0; i < A.Elements.Length; ++i)
                c += A[i] * B[i];

            return c;
        }

        public static Vector operator *(Vector A, double Scalar)
        {
            Vector B = new Vector(A.Elements.Length);
            for (int i = 0; i < A.Elements.Length; ++i)
            {
                B[i] = A[i] * Scalar;
            }

            return B;
        }

        public static Vector operator *(double Scalar, Vector A)
        {           
            return A*Scalar;
        }

        public static Vector operator /(Vector A, double Scalar)
        {
            return A * (1 / Scalar);
        }

        public static double DotProduct(Vector A, Vector B)
        {
            return A*B;
        }

        public static double L2Norm(Vector A)
        {
            return Math.Sqrt(A*A);
        }

        public double L2Norm()
        {
            return L2Norm(this);
        }

        public static double LInfinitiveNorm(Vector A)
        {            
            Vector B = A.Abs();
            double Max = B[0];

            for (int j = 1; j < B.Elements.Length; ++j)
                if (Max < B[j])
                    Max = B[j];

            return Max;
        }

        public static Vector Abs(Vector A)
        {
            Vector B = new Vector(A.Elements.Length);
            for (int i = 0; i < A.Elements.Length; ++i)
                B[i] = Math.Abs(A[i]);

            return B;
        }

        public Vector Abs()
        {
            return Abs(this);
        }

        public static Vector Normalize(Vector A)
        {
            return A / L2Norm(A);
        }

        public Vector Normalize()
        {
            return Normalize(this);
        }

        public static Vector Orthonormalize(Vector A, Vector OrthonormalVector)
        {
            Vector B = new Vector();
            
            B = A - OrthonormalVector * A * OrthonormalVector;
            B = B.Normalize();

            return B;
        }

        public Vector Orthonormalize(Vector OrthonormalVector)
        {            
            return Orthonormalize(this,OrthonormalVector);
        }

        public static void Sort_Descending(Vector A)
        {
            int m = A.Elements.Length;
            double temp;
            
            for (int i = 0; i < m; ++i)
                for (int j = i + 1; j < m; ++j)
                {
                    if (A[i] < A[j])
                    {
                        temp = A[i];
                        A[i] = A[j];
                        A[j] = temp;
                    }
                }
        }

        public static void Sort_Ascending(Vector A)
        {
            int m = A.Elements.Length;
            double temp;

            for (int i = 0; i < m; ++i)
                for (int j = i + 1; j < m; ++j)
                {
                    if (A[i] > A[j])
                    {
                        temp = A[i];
                        A[i] = A[j];
                        A[j] = temp;
                    }
                }
        }

        public static void Sort(Vector A, bool bDescending)
        {
            Array.Sort(A.Elements);
            if (bDescending)
                //Sort_Descending(A);
                Array.Reverse(A.Elements);
            //else
                //Sort_Ascending(A);                
        }

        public void Sort(bool bDescending)
        {
            Sort(this, bDescending);
        }

        public string PrintToString()
        {
            StringBuilder sb = new StringBuilder("");

            for (int i = 0; i < this.Elements.Length; ++i)
            {
                sb.Append(Math.Round(this[i], GlobalMath.DIGITS));
                sb.Append("\t\t");
            }
            return sb.ToString();
        }

        public static double[] Swap(ref double a, ref double b)
        {
            double temp;
            temp = a;
            a = b;
            b = temp;

            return (new double[2] { a, b });
        }
    }
}
