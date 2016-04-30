using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class MMatrix : MarshalByRefObject, IDisposable
    {
        public int row, col;
        public double[,] MArray;

        // Factor LDLt
        private double[] LDL_D1;
        public double[,] LDL_L;
        public double[,] LDL_D;

        //Factor LU
        public double[,] LU_P;
        public double[,] LU_L;
        public double[,] LU_U;

        //Factor SVD
        public double[,] SVD_U;
        public double[,] SVD_S;
        public double[,] SVD_Vt;

        #region Constructors_Deconstructor
        public MMatrix()
        {
        }        

        public MMatrix(int row, int column)
        {
            this.row = row;
            this.col = column;
            this.MArray = new double[row, column];
        }

        public MMatrix(MMatrix A, int row, int column)
        {
            if ((row > A.row) || (column > A.col))
                throw new MMatrixException("Cannot create matrix. Wrong indices");

            this.row = row;
            this.col = column;
            this.MArray = new double[row, column];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    this.MArray[i, j] = A.MArray[i, j];
        }
        //Trunkcate a matrix
        public MMatrix(MMatrix A, int FromRow, int ToRow, int FromColumn, int ToColumn)
        {
            if ((FromRow > A.row) || (FromColumn > A.col))
                throw new MMatrixException("Cannot create matrix. Wrong indices");
            
            if (ToRow > A.row)
                ToRow = A.row;
            if (ToColumn > A.col)
                ToColumn = A.col;

            if ((FromRow > ToRow) || (FromColumn > ToColumn))
                throw new MMatrixException("Cannot create matrix. Wrong indices");

            this.row = ToRow - FromRow;
            this.col = ToColumn - FromColumn;
            this.MArray = new double[this.row, this.col];
            for (int i = FromRow; i < ToRow; i++)
                for (int j = FromColumn; j < ToColumn; j++)
                    this.MArray[i - FromRow, j - FromColumn] = A.MArray[i, j];
        }

        public MMatrix(MMatrix A)
        {
            if (A != null)
            {
                this.row = A.row;
                this.col = A.col;
                if (A.MArray != null)
                    this.MArray = (double[,])A.MArray.Clone();
            }
        }

        public MMatrix(double[,] ValueArray, int row, int column)
        {
            if ((row > ValueArray.GetLength(0)) || (column > ValueArray.GetLength(1)))
                throw new MMatrixException("Cannot create matrix. Wrong indices");

            this.row = row;
            this.col = column;
            this.MArray = new double[this.row, this.col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    this.MArray[i, j] = ValueArray[i, j];
        }

        public MMatrix(double[,] ValueArray)
        {
            this.row = ValueArray.GetUpperBound(0)+1;
            this.col = ValueArray.GetUpperBound(1)+1;            
            this.MArray = (double[,])ValueArray.Clone();
        }

        ~MMatrix()
        {
                     
        }

        public void Dispose()
        {
                    
        }

        public override int GetHashCode()
        {
            return (Convert.ToInt32(this.MArray.GetHashCode() & 0xFFFFFFFF));
        }     

        public double this[int row, int col]
        {
            get
            {
                return GetElement(row, col);
                //return this.MArray[row, col];
            }
            set
            {
                SetElement(row, col, value);
                //this.MArray[row, col] = value;
            }
        }

        public MMatrix Clone()
        {
            return new MMatrix(this);
        }
        #endregion

        #region HandyOperations
        private double GetElement(int row, int col)
        {
            if (row < 0 || row > this.row - 1 || col < 0 || col > this.col - 1)
                throw new MMatrixException("Invalid index specified");
            return this.MArray[row, col];
        }

        private void SetElement(int row, int col, double value)
        {
            if (row < 0 || row > this.row - 1 || col < 0 || col > this.col - 1)
                throw new MMatrixException("Invalid index specified");
            this.MArray[row, col] = value;
        }        

        public static string PrintToString (MMatrix A)
        {
            StringBuilder sb = new StringBuilder("");                       

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.col; j++)
                {
                    sb.Append(Math.Round(A[i, j], GlobalMath.DIGITS));
                    sb.Append("\t\t");                    
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string PrintToString()
        {
            return PrintToString(this);
        }

        private static MMatrix Negate(MMatrix matrix)
        {
            return MMatrix.Multiply(matrix, -1);
        }        

        private static MMatrix Add(MMatrix matrix1, MMatrix matrix2)
        {
            if (matrix1.row != matrix2.row || matrix1.col != matrix2.col)
                throw new MMatrixException("Addition not impossible. Two matrices are different size");
            MMatrix result = new MMatrix(matrix1.row, matrix1.col);
            for (int i = 0; i < result.row; i++)
                for (int j = 0; j < result.col; j++)
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
            return result;
        }

        private static MMatrix Multiply(MMatrix matrix1, MMatrix matrix2)
        {
            if (matrix1.col != matrix2.row)
                throw new MMatrixException("Multiplication not impossible. Two matrices are different size");

            MMatrix result = new MMatrix(matrix1.row, matrix2.col);
            for (int i = 0; i < result.row; i++)
                for (int j = 0; j < result.col; j++)
                    for (int k = 0; k < matrix1.col; k++)              
                        result[i, j] += matrix1[i, k] * matrix2[k, j];                    
                    
            return result;
        }        

        private static MMatrix Multiply(MMatrix matrix, double dbl)
        {            
            if(dbl == 0)
                return new MMatrix(matrix.row, matrix.col);

            MMatrix result = new MMatrix(matrix.row, matrix.col);
            for (int i = 0; i < matrix.row; i++)
                for (int j = 0; j < matrix.col; j++)
                    result[i, j] = matrix[i, j] * dbl;
            return result;
        }

        public static MMatrix RandomMatrix(int row, int column, int minvalue, int maxvalue)
        {
            MMatrix A = new MMatrix(row, column);
            Random ranobj = new Random();
            for (int i = 0; i < A.row; i++)            
                for (int j = 0; j < A.col; j++)
                    A[i, j] = ranobj.Next(minvalue, maxvalue);            

            return A;
        }

        public static MMatrix RandomMatrix(int size, int minvalue, int maxvalue)
        {
            return RandomMatrix(size, size, minvalue, maxvalue);
        }

        public static MMatrix DiagonalMatrix(int size, int initvalue)
        {
            MMatrix A = new MMatrix(size, size);
            for (int i = 0; i < size; i++)           
                A[i, i] = initvalue;            

            return A;
        }

        public static MMatrix ScalarMatrix(int row, int column, int Scalar)
        {
            MMatrix A = new MMatrix(row, column);
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                A[i, j] = Scalar;

            return A;
        }

        public static MMatrix ScalarMatrix(int size, int Scalar)
        {
            return ScalarMatrix(size, size, Scalar);
        }

        public Vector Column(int j)
        {
            Vector v = new Vector(this.row);

            for (int i = 0; i < this.row; i++)
            {
                v[i] = this[i, j];
            }

            return v;
        }

        public Vector DiagVector()
        {
            if (!this.IsSquared())
                throw new MMatrixException("Cannot get diagonal of non-square matrix.");

            Vector v = new Vector(this.col);
            for (int i = 0; i < this.col; i++)
            {
                v[i] = this[i, i];
            }

            return v;
        }

        public static MMatrix Floor(MMatrix A)
        {
            MMatrix B = A.Clone();
            for (int i = 0; i < B.row; i++)
                for (int j = 0; j < B.col; j++)
                    B[i, j] = Math.Round(B[i,j], GlobalMath.DIGITS);
            
            return B;
        }

        public static MMatrix Concat_Horizontal(MMatrix A,MMatrix B)
        {
            if (A.row != B.row)
                throw new MMatrixException("Horizontal concat impossible. Two matrices are not the same rows.");

            MMatrix C = new MMatrix(A.row, A.col + B.col);

            for (int i = 0; i < C.row; i++)
            {
                for (int j = 0; j < A.col; j++)
                    C.MArray[i, j] = A.MArray[i, j];
                for (int j = A.col; j < C.col; j++)
                    C.MArray[i, j] = B.MArray[i, j - A.col];
            }
            return C;
        }

        public static MMatrix Concat_Vertical(MMatrix A, MMatrix B)
        {
            if (A.col != B.col)
                throw new MMatrixException("Vertical concat impossible. Two matrices are not the same columns.");

            MMatrix C = new MMatrix(A.row + B.row, A.col);

            for (int i = 0; i < A.row; i++)            
                for (int j = 0; j < C.col; j++)
                    C.MArray[i, j] = A.MArray[i, j];

            for (int i = A.row; i < C.row; i++)
                for (int j = 0; j < C.col; j++)
                    C.MArray[i, j] = B.MArray[i-A.row, j];
            return C;
        }

        #endregion

        #region MatrixOperators
        public static MMatrix operator -(MMatrix A)
        {
            return MMatrix.Negate(A);
        }

        public static MMatrix operator +(MMatrix matrix1, MMatrix matrix2)
        { 
            return MMatrix.Add(matrix1, matrix2); 
        }

        public static MMatrix operator -(MMatrix matrix1, MMatrix matrix2)
        { 
            return MMatrix.Add(matrix1, -matrix2); 
        }

        public static MMatrix operator *(MMatrix matrix1, MMatrix matrix2)
        { 
            return MMatrix.Multiply(matrix1, matrix2); 
        }

        public static MMatrix operator *(MMatrix matrix1, double dbl)
        { 
            return MMatrix.Multiply(matrix1, dbl); 
        }

        public static MMatrix operator *(double dbl, MMatrix matrix1)
        {
            return MMatrix.Multiply(matrix1, dbl); 
        }
  
        public static MMatrix operator /(MMatrix matrix1, double dbl)
        {
            return MMatrix.Multiply(matrix1, 1/dbl); 
        }
        /*
        public static bool operator ==(MMatrix matrix1, MMatrix matrix2)
        {            
            return IsEqual(matrix1, matrix2);
        }

        public static bool operator !=(MMatrix matrix1, MMatrix matrix2)
        {            
            return !IsEqual(matrix1, matrix2);
        }
        */
        #endregion

        #region MatrixBasicOperations
        private static MMatrix Abs(MMatrix A, int row, int column)
        {
            MMatrix B = new MMatrix(row, column);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    B[i, j] = Math.Abs(A[i, j]);

            return B;
        }

        public static MMatrix Abs(MMatrix A)
        {
            return Abs(A, A.row, A.col);
        }

        private static MMatrix PowEntry(MMatrix A, int row, int column, double pow)
        {
            MMatrix B = new MMatrix(row, column);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    B[i, j] = Math.Pow(A[i, j],pow);

            return B;
        }

        public static MMatrix PowEntry(MMatrix A, double pow)
        {
            return PowEntry(A, A.row, A.col, pow);
        }

        private static MMatrix Average(MMatrix A, int row, int column)
        {
            MMatrix B = new MMatrix(row, column);
            double sum = Sum(A);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    B[i, j] = A[i, j]/sum;

            return B;
        }

        public static MMatrix Average(MMatrix A)
        {
            return Average(A, A.row, A.col);
        }

        private static MMatrix Transpose(MMatrix A, int row, int column)
        {
            MMatrix B = new MMatrix(column, row);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    B[j, i] = A[i, j];

            return B;
        }

        public static MMatrix Transpose(MMatrix A)
        {
            return Transpose(A, A.row, A.col);
        }

        public MMatrix Transpose()
        {
            return Transpose(this);
        }

        public static MMatrix Minor(MMatrix A, int row, int col)
        {
            MMatrix minor = new MMatrix(A.row - 1, A.col - 1);
            int m = 0, n = 0;

            for (int i = 0; i < A.row; i++)
            {
                if (i == row)
                    continue;
                n = 0;
                for (int j = 0; j < A.col; j++)
                {
                    if (j == col)
                        continue;
                    minor[m, n] = A[i, j];
                    n++;
                }
                m++;
            }
            return minor;
        }

        public static double Determinant(MMatrix A)
        {
            double det = 0;
            if (A.row != A.col)
                throw new MMatrixException("Determinant of a non-square A doesn't exist");
            if (A.row == 1)
                return A[0, 0];
            for (int j = 0; j < A.col; j++)
            {
                if (A[0, j] == 0)
                    continue;
                det += (A[0, j] * Determinant(MMatrix.Minor(A, 0, j)) * System.Math.Pow(-1, 0 + j));
            }
            return det;
        }

        public static double Determinant_NonRecursive(MMatrix A)
        {
            double det = 1;
            if (!A.IsSquared())
                throw new MMatrixException("Determinant of a non-square matrix doesn't exist");

            A.FactorLU_withP();
            for (int i = 0; i < A.row; i++)
                det *= A.LU_L[i, i] * A.LU_U[i, i];
            det *= Determinant(new MMatrix(A.LU_P)); 

            return det;
        }  

        public double Determinant()
        {
            return Determinant_NonRecursive(this);
        }        

        public static MMatrix Inverse(MMatrix A)
        {
            if (!IsInvertible(A))
                throw new MMatrixException("Inverse of a singular A is not possible");
            return (Adjoint(A) / Determinant(A));
        }
        
        public MMatrix Inverse()
        {
            return Inverse(this);
        }
       
        public static MMatrix Adjoint(MMatrix A)
        {
            if (A.row != A.col)
                throw new MMatrixException("Adjoint of a non-square A does not exists");

            MMatrix AdjointMatrix = new MMatrix(A.row, A.col);
            for (int i = 0; i < A.row; i++)
                for (int j = 0; j < A.col; j++)
                    AdjointMatrix[i, j] = Math.Pow(-1, i + j) * Determinant(Minor(A, i, j));

            return Transpose(AdjointMatrix); 
        }

        public void InterchangeRow(int iRow1, int iRow2)
        {
            double temp;
            for (int j = 0; j < this.col; j++)
            {
                temp = this[iRow1, j];
                this[iRow1, j] = this[iRow2, j];
                this[iRow2, j] = temp;
            }
        }

        public void InterchangeColumn(int iCol1, int iCol2)
        {
            double temp;
            for (int j = 0; j < this.row; j++)
            {
                temp = this[j, iCol1];
                this[j, iCol1] = this[j, iCol2];
                this[j, iCol2] = temp; 
            }
        }

        public void MultiplyRow(int iRow, double dbl)
        {
            for (int j = 0; j < this.col; j++)           
                this[iRow, j] *= dbl;                          
        }

        public void AddRow(int iTargetRow, int iSecondRow, double iMultiple)
        {
            for (int j = 0; j < this.col; j++)
                this[iTargetRow, j] += (this[iSecondRow, j] * iMultiple);
        }

        public static MMatrix EchelonForm(MMatrix matrix)
        {
            try
            {
                MMatrix EchelonMatrix = new MMatrix(matrix);
                for (int i = 0; i < matrix.row; i++)
                {
                    /*Overcome round-off error to approximate values
                    for (int j = 0; j < EchelonMatrix.row; j++)
                        for (int k = 0 ; k < EchelonMatrix.col; k++)
                        if (Math.Abs(EchelonMatrix[j, k]) <= ZERO)
                            EchelonMatrix[j, k] = 0;
                    */
                    if (i < EchelonMatrix.col)
                    {
                        if (EchelonMatrix[i, i] == 0)	// if diagonal entry is zero, 
                            for (int j = i + 1; j < EchelonMatrix.row; j++)
                                if (EchelonMatrix[j, i] != 0)	 //check if some below entry is non-zero
                                    EchelonMatrix.InterchangeRow(i, j);	// then interchange the two rows
                        if (EchelonMatrix[i, i] == 0)	// if not found any non-zero diagonal entry
                            continue;	// increment i;

                        if (EchelonMatrix[i, i] != 1)	// if diagonal entry is not 1 , 	
                            for (int j = i + 1; j < EchelonMatrix.row; j++)
                                if (EchelonMatrix[j, i] == 1)	 //check if some below entry is 1
                                    EchelonMatrix.InterchangeRow(i, j);	// then interchange the two rows

                        EchelonMatrix.MultiplyRow(i, 1 / (EchelonMatrix[i, i]));

                        for (int j = i + 1; j < EchelonMatrix.row; j++)
                            EchelonMatrix.AddRow(j, i, -EchelonMatrix[j, i]);
                    }
                }
                return EchelonMatrix;
            }
            catch (Exception)
            {
                throw new MMatrixException("MMatrix can not be reduced to Echelon form");
            }
        }

        public MMatrix EchelonForm()
        {
            return EchelonForm(this);
        }

        public static MMatrix ReducedEchelonForm(MMatrix matrix)
        {
            try
            {
                MMatrix ReducedEchelonMatrix = new MMatrix(matrix);
                for (int i = 0; i < matrix.row; i++)
                {
                    if (i < ReducedEchelonMatrix.col)
                    {
                        if (ReducedEchelonMatrix[i, i] == 0)	// if diagonal entry is zero, 
                            for (int j = i + 1; j < ReducedEchelonMatrix.row; j++)
                                if (ReducedEchelonMatrix[j, i] != 0)	 //check if some below entry is non-zero
                                    ReducedEchelonMatrix.InterchangeRow(i, j);	// then interchange the two rows
                        if (ReducedEchelonMatrix[i, i] == 0)	// if not found any non-zero diagonal entry
                            continue;	// increment i;
                        if (ReducedEchelonMatrix[i, i] != 1)	// if diagonal entry is not 1 , 	
                            for (int j = i + 1; j < ReducedEchelonMatrix.row; j++)
                                if (ReducedEchelonMatrix[j, i] == 1)	 //check if some below entry is 1
                                    ReducedEchelonMatrix.InterchangeRow(i, j);	// then interchange the two rows
                        ReducedEchelonMatrix.MultiplyRow(i, 1 / (ReducedEchelonMatrix[i, i]));

                        for (int j = i + 1; j < ReducedEchelonMatrix.row; j++)
                            ReducedEchelonMatrix.AddRow(j, i, -ReducedEchelonMatrix[j, i]);
                        for (int j = i - 1; j >= 0; j--)
                            ReducedEchelonMatrix.AddRow(j, i, -ReducedEchelonMatrix[j, i]);
                    }
                }
                return ReducedEchelonMatrix;
            }
            catch (Exception)
            {
                throw new MMatrixException("MMatrix can not be reduced to Echelon form");
            }
        }

        public MMatrix ReducedEchelonForm()
        {
            return ReducedEchelonForm(this);
        }

        //Orthonormalize all eigenvectors based on the first one
        public static MMatrix Orthonormalize(Queue<Vector> Eigenvectors)
        {
            MMatrix A = new MMatrix(Eigenvectors.ElementAt(0).Elements.Length, Eigenvectors.Count);
            Vector v1, v;
            int column = 0;

            v1 = Eigenvectors.ElementAt(0);
            v1 = v1.Normalize();
            for (int i = 0; i < v1.Elements.Length; i++)
            {
                A[i, 0] = v1[i];
            }
            for (int j = 1; j < Eigenvectors.Count; j++)
            {
                v = Eigenvectors.ElementAt(j);
                v = v.Orthonormalize(v1);
                column++;
                for (int i = 0; i < v.Elements.Length; i++)
                {
                    A[i, column] = v[i];
                }
            }
            return A;
        }

        //Orthonormalize all eigenvectors based on the first one
        public static MMatrix Orthonormalize(MMatrix Eigenvectors)
        {
            MMatrix A = new MMatrix(Eigenvectors);
            Vector v1 = new Vector(Eigenvectors.row);
            Vector v = new Vector(Eigenvectors.row);                        
            
            for (int i = 0; i < v1.Elements.Length; i++)            
                v1[i] = A[i, 0];
            v1 = v1.Normalize();
            for (int i = 0; i < v1.Elements.Length; i++)
                A[i, 0] = v1[i];

            for (int i = 1; i < Eigenvectors.col; i++)
            {
                for (int j = 0; j < Eigenvectors.row; j++)
                    v[j] = Eigenvectors[j,i];
                v = v.Orthonormalize(v1);
                
                for (int j = 0; j < v.Elements.Length; j++)                
                    A[j, i] = v[j];                
            }
            return A;
        }

        //Sort Eigenvalues, Eigenvectors
        public static void SortEigen(ref Vector eigenvalues, ref MMatrix eigenvectors)
        {
            if (eigenvalues.Elements.Length != eigenvectors.col)
                throw new MMatrixException("Eigenvectors do not match eigenvalues.");
            
            double temp;
            for (int i = 0; i < eigenvalues.Elements.Length; i++)            
                for (int j = i + 1; j < eigenvalues.Elements.Length; j++)
                    if (eigenvalues[i] < eigenvalues[j])
                    {
                        temp = eigenvalues[i];
                        eigenvalues[i] = eigenvalues[j];
                        eigenvalues[j] = temp;

                        eigenvectors.InterchangeColumn(i, j);
                    }            
        }

        #endregion

        #region MatrixChecking

        public static bool IsInvertible(MMatrix A)
        {
            return (Determinant(A) != 0);
        }

        public bool IsInvertible()
        {
            return IsInvertible(this);
        }

        public static bool IsSquared(MMatrix A)
        {
            return (A.row == A.col);
        }

        public bool IsSquared()
        {
            return IsSquared(this);
        }

        public static bool IsSymetric(MMatrix A)
        {                        
            if(!IsSquared(A))
                return false;

            for (int i = 0; i < A.row; i++)                            
                for (int j = i+1; j < A.col; j++)
                    if(A[i,j]!=A[j,i])                    
                        return false;

            return true;
        }

        public bool IsSymetric()
        {            
            return IsSymetric(this);
        }

        public static bool IsEqual(MMatrix matrix1, MMatrix matrix2)
        {
            if ((matrix1 == null) || (matrix2 == null))
                return false;
            
            if ((matrix1.row != matrix2.row) || (matrix1.col != matrix2.col))
                return false;

            for (int i = 0; i < matrix1.row; i++)
                for (int j = 0; j < matrix1.col; j++)
                    if (Math.Round(matrix1[i, j], GlobalMath.DIGITS) != Math.Round(matrix2[i, j], GlobalMath.DIGITS))
                        return false;

            return true;
        }

        public static bool IsOrthogonal(MMatrix matrix)
        {
            MMatrix I = MMatrix.DiagonalMatrix(matrix.row, 1);

            return (matrix * matrix.Transpose() == I);
        }

        public bool IsOrthogonal()
        {
            return IsOrthogonal(this);
        }        
        #endregion

        #region MatrixFactor
        private static void FactorLDLt_Book(MMatrix A, int size)
        {
            double[] V = new double[size];
            double sum1 = 0, sum2 = 0;

            //Init value for matrix L and D
            A.LDL_D1 = new double[size];
            A.LDL_L = new double[size, size];
            A.LDL_D = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                A.LDL_L[i, i] = 1;
                //A.LDL_D[i] = A[i, i];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < i - 1; j++)
                    V[j] = A.LDL_L[i, j] * A.LDL_D1[j];

                //sum1 = 0;
                for (int j = 0; j < i - 1; j++)
                    sum1 += A.LDL_L[i, j] * V[j];
                A.LDL_D1[i] = A[i, i] - sum1;

                for (int j = i + 1; j < size; j++)
                {
                    //sum2 = 0;
                    for (int k = 0; k < i - 1; k++)
                        sum2 += A.LDL_L[j, k] * V[k];
                    A.LDL_L[j, i] = (A[j, i] - sum2) / A.LDL_D1[i];
                }
            }
            // Create Matrix DD from array D
            for (int i = 0; i < size; i++)
                A.LDL_D[i, i] = A.LDL_D1[i];
        }

        //A = L*D*Lt
        private static void FactorLDLt(MMatrix A, int size)
        {
            A.FactorLU();
            A.LDL_L = (double[,])A.LU_L.Clone();

            MMatrix MU = new MMatrix(A.LU_U);
            MU = MU.Transpose();
            MU.FactorLU();

            A.LDL_D = (double[,])MU.LU_U.Clone();
        }

        public static void FactorLDLt(MMatrix squareMatrix)
        {
            if (!squareMatrix.IsSymetric())
                throw new MMatrixException("Cannot factor LDLt a non-symetric matrix");
            FactorLDLt(squareMatrix, squareMatrix.row);
        }

        public void FactorLDLt()
        {
            FactorLDLt(this);
        }

        private static void FactorLU_Book(MMatrix A, int size)
        {
            double sum =0, sum1 =0, sum2 =0;
            
            //Init LU_L matrix 
            A.LU_L = new double[size, size];
            A.LU_U = new double[size, size];            
            for (int i = 0; i < size; i++)
            {
                A.LU_L[i, i] = 1;
                A.LU_U[0, i] = A[0, i];
            }

            if (A.LU_L[0,0]*A.LU_U[0, 0] == 0)
                throw new MMatrixException("LU factorization impossible.");

            for (int j = 1; j < size; j++)
            {
                A.LU_U[0,j] = A[0,j]/A.LU_L[0,0];
                A.LU_L[j, 0] = A[j, 0] / A.LU_U[0, 0];
            }

            for (int i = 1; i < size - 1; i++)
            {
                sum = 0;
                for (int k = 0; k < i - 1; k++)
                    sum += A.LU_L[i, k] * A.LU_U[k, i];
                A.LU_U[i, i] = A[i, i] - sum;

                if (A.LU_U[i, i] == 0)
                    throw new MMatrixException("LU factorization impossible.");

                for (int j = i + 1; j < size; j++)
                {
                    sum1 = 0;
                    for (int k = 0; k < i - 1; k++)
                        sum1 += A.LU_L[i, k] * A.LU_U[k, j];
                    A.LU_U[i, j] = (1 / A.LU_L[i, i]) * (A[i, j] - sum1);

                    sum2 = 0;
                    for (int k = 0; k < i - 1; k++)
                        sum2 += A.LU_L[j, k] * A.LU_U[k, i];
                    A.LU_L[j, i] = (1 / A.LU_U[i, i]) * (A[j, i] - sum2);
                }
            }
            sum = 0;
            for (int k = 0; k < size - 1; k++)
                sum += A.LU_L[size - 1, k] * A.LU_U[k, size - 1];
            A.LU_U[size - 1, size - 1] = A[size - 1, size - 1] - sum;
        }

        //A = L*U
        private static void FactorLU(MMatrix A, int size)
        {
            A.LU_L = new double[size, size];
            //A.LU_U = new double[size, size];
            A.LU_U = (double[,])A.MArray.Clone();

            for (int i = 0; i < size; i++)
                A.LU_L[i, i] = 1;

            for (int i = 0; i < size; i++)
                for (int j = i + 1; j < size; j++)
                {
                    A.LU_L[j, i] = A.LU_U[j, i] / A.LU_U[i, i];
                    for (int k = i; k < size; k++)
                        A.LU_U[j, k] = A.LU_U[j, k] - A.LU_U[i, k] * A.LU_L[j, i];
                }
        }        

        public static void FactorLU(MMatrix squaredMatrix)
        {
            if (!squaredMatrix.IsSquared())
                throw new MMatrixException("Matrix is non-square. LU factorization impossible.");
            FactorLU(squaredMatrix, squaredMatrix.row);
        }

        public void FactorLU()
        {
            FactorLU(this);
        }

        //A = P^1*L*U = Pt*L*U
        private static void FactorLU_withP(MMatrix matrix, int size)
        {
            MMatrix A = matrix.Clone();
            MMatrix P = MMatrix.DiagonalMatrix(size, 1);
            A.LU_L = new double[size, size];

            for (int i = 0; i < size; i++)
                A.LU_L[i, i] = 1;

            for (int i = 0; i < size; i++)
            {
                //Interchange row if the entry [i,i] is 0
                if (A[i, i] == 0)	// if diagonal entry is zero, 
                    for (int k = i + 1; k < A.row; k++)
                        if (A[k, i] != 0)	 //check if some below entry is non-zero
                        {
                            A.InterchangeRow(i, k);	// then interchange the two rows
                            P.InterchangeRow(i, k);
                        }
                if (A[i, i] == 0)	// if not found any non-zero diagonal entry
                    throw new MMatrixException("Cannot factor LU for this matrix");

                for (int j = i + 1; j < size; j++)
                {
                    A.LU_L[j, i] = A[j, i] / A[i, i];
                    A.AddRow(j, i, -A.LU_L[j, i]);
                }
            }
            matrix.LU_P = P.MArray;
            matrix.LU_L = A.LU_L;
            matrix.LU_U = A.MArray;
        }

        public void FactorLU_withP()
        {
            if (!this.IsSquared())
                throw new MMatrixException("Matrix is non-square. LU factorization impossible.");
            FactorLU_withP(this, this.row);
        }

        //A = U*S*Vt
        //A*At = U*S^2*Ut
        //At*A = V*S^2*Vt
        //A*At = (At*A)t
        public static void FactorSVD_LDLt(MMatrix A)
        {            
            MMatrix AA = new MMatrix();
            MMatrix Eigenvectors = new MMatrix();
            Vector Eigenvalues = new Vector();
            int index = A.row;
            if (A.row > A.col)
                index = A.col;

            AA = A * A.Transpose();
            AA.FactorLDLt();
            Eigenvectors = new MMatrix(AA.LDL_L);
            Eigenvalues = new Vector(AA.row);
            for (int i = 0; i < index; i++)
                Eigenvalues[i] = AA.LDL_D[i, i];
            SortEigen(ref Eigenvalues, ref Eigenvectors);
            A.SVD_U = Eigenvectors.MArray;
            
            
            A.SVD_S = new double[A.row, A.col];
            for (int i = 0; i < index; i++)
            {
                A.SVD_S[i, i] = Math.Sqrt(Eigenvalues[i]);
            }
                       
            AA = A.Transpose() * A;
            AA.FactorLDLt();
            Eigenvectors = new MMatrix(AA.LDL_L);
            Eigenvalues = new Vector(AA.row);
            for (int i = 0; i < index; i++)
                Eigenvalues[i] = AA.LDL_D[i, i];
            SortEigen(ref Eigenvalues, ref Eigenvectors);
            A.SVD_Vt = Eigenvectors.Transpose().MArray;            
        }        
        
        //A = U*S*Vt
        public static void FactorSVD(MMatrix A)
        {            
            MMatrix B = new MMatrix();
            MMatrix C = new MMatrix(); 
            Vector EigenvaluesU = new Vector();
            Vector EigenvaluesV = new Vector();
            MMatrix EigenvectorsU = new MMatrix();
            MMatrix EigenvectorsV = new MMatrix();   
            int index;
            
            //U
            B = A * A.Transpose();
            B.Jacobi_Cyclic_Method(ref EigenvaluesU,ref EigenvectorsU);
            SortEigen(ref EigenvaluesU,ref EigenvectorsU);
            A.SVD_U = EigenvectorsU.MArray;

            if (A.row < A.col)
                index = A.row;
            else
                index = A.col;
            //S
            A.SVD_S = new double[A.row, A.col];            
            for (int i = 0; i < index; i++)
            {
                A.SVD_S[i, i] = Math.Sqrt(EigenvaluesU[i]);
            }  
                                  
            //V
            C = A.Transpose() * A;
            C.Jacobi_Cyclic_Method(ref EigenvaluesV,ref EigenvectorsV);
            SortEigen(ref EigenvaluesV,ref EigenvectorsV);
            A.SVD_Vt = EigenvectorsV.Transpose().MArray;

            B = C = null;
            EigenvectorsU = EigenvectorsV = null;
        }

        public void FactorSVD()
        {
            FactorSVD(this);            
        }
        
        #endregion     

        #region Eigenvalues_Eigenvectors

        //Transform a matrix to trigdiagonal form
        public static MMatrix Householder(MMatrix A)
        {
            int n = A.row;
            double q, alpha, RSQ,sum,PROD;
            double[] V = new double[n];
            double[] U = new double[n];
            double[] Z = new double[n];

            for (int k = 0; k < n-2; k++)
            {
                q = 0;
                for (int j = k + 1; j < n; j++)
                    q += A[j, k]* A[j, k];

                if (A[k + 1, k] == 0)
                    alpha = -Math.Sqrt(q);
                else
                    alpha = -Math.Sqrt(q) * A[k + 1, k] / Math.Abs(A[k + 1, k]);

                RSQ = alpha * alpha - alpha * A[k + 1, k];
                
                V[k] = 0;
                V[k + 1] = A[k + 1, k] - alpha;
                for (int j = k + 2; j < n; j++)                
                    V[j] = A[j, k];

                for (int j = k; j < n; j++)
                {
                    sum = 0;
                    for (int i = k + 1; i < n; i++)
                        sum += A[j, i] * V[i];
                        U[j] = (1 / RSQ) * sum;
                }
                sum = 0;
                for (int i = k + 1; i < n; i++)
                    sum += V[i]*U[i];
                PROD = sum;

                for (int j = k; j < n; j++)                
                    Z[j] = U[j] - (PROD / (2 * RSQ)) * V[j];

                for (int l = k + 1; l < n - 1; l++)
                {
                    for (int j = l + 1; j < n; j++)
                    {
                        A[j, l] = A[j, l] - (V[l] * Z[j]) - (V[j] * Z[l]);
                        A[l, j] = A[j, l];
                    }
                    A[l, l] = A[l, l] - 2 * V[l] * Z[l];
                }

                A[n - 1, n - 1] = A[n - 1, n - 1] - 2 * V[n - 1] * Z[n - 1];

                for (int j = k + 2; j < n; j++)
                {
                    A[k, j] = 0;
                    A[j, k] = 0;
                }

                A[k + 1, k] = A[k + 1, k] - V[k + 1] * Z[k];
                A[k, k + 1] = A[k + 1, k];
            }
            return A;
        }

        public MMatrix Householder()
        {
            if (!this.IsSquared())
                throw new MMatrixException("Matrix must be squared.");
            return Householder(this);
        }
        
        //Matrix must be trigdiagonal (using Householder)
        //This routine cannot find exactly eigenvalues
        public static Queue<double> QR(MMatrix matrix, double TOL, int MaxIterations)
        {
            int n = matrix.row;
            int M = MaxIterations;
            int k;
            double SHIFT, lamda,b,c,d,m1,m2,lamda1,lamda2,min,sigma;
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];
            double[] D = new double[n];
            double[] Q = new double[n];
            double[] R = new double[n];
            double[] S = new double[n];
            double[] X = new double[n];
            double[] Y = new double[n];
            double[] Z = new double[n];
            double[] Sig = new double[n];
            Queue<double> QLamda=new Queue<double>();                        

            for (int i = 0; i < n; i++)            
                A[i] = matrix[i, i];
            for (int i = 1; i < n; i++)
                B[i] = matrix[i,i-1];

            k = 1;
            SHIFT = 0;

            while (k <= M)
            {
                if (Math.Abs(B[n - 1]) <= TOL)
                {
                    lamda = A[n - 1] + SHIFT;
                    QLamda.Enqueue(lamda);
                    n = n - 1;
                }

                if (B[1] <= TOL)
                {
                    lamda = A[0] + SHIFT;
                    QLamda.Enqueue(lamda);
                    n = n - 1;
                    A[0] = A[1];
                    for (int j = 1; j < n; j++)
                    {
                        A[j] = A[j + 1];
                        B[j] = B[j + 1];
                    }
                }

                if (n == 0)
                    return QLamda;

                if (n == 1)
                {
                    lamda = A[0] + SHIFT;
                    QLamda.Enqueue(lamda);
                    return QLamda;
                }
                /*
                for (int j = 2; j < n - 1; j++)
                {
                    if (Math.Abs(B[j]) <= TOL)
                    {
                        throw new MMatrixException("Split into A[0]..A[j-1], B[1]..B[j-1] and A[j]..A[n-1], B[j+1]..B[n-1]"+SHIFT.ToString());                        
                    }
                }
                */
                b = -(A[n - 2] + A[n - 1]);
                c = A[n - 1] * A[n - 2] - B[n - 1]* B[n - 1];
                d = Math.Sqrt(b * b - 4 * c);

                if(b > 0)
                {
                    m1 = -2*c/(b+d);
                    m2 = -(b+d)/2;
                }
                else
                {
                    m1 = (d-b)/2;
                    m2 = 2*c/(d-b);
                }

                if (n == 2)
                {
                    lamda1 = m1 + SHIFT;
                    lamda2 = m2 + SHIFT;
                    QLamda.Enqueue(lamda1);
                    QLamda.Enqueue(lamda2);
                    return QLamda;
                }

                min = Math.Min(Math.Abs(m1 - A[n - 1]), Math.Abs(m2 - A[n - 1]));
                sigma = min + A[n - 1];
                SHIFT += sigma;

                for (int j = 0; j < n; j++)
                    D[j] = A[j] - sigma;

                X[0] = D[0];
                Y[0] = B[1];

                for (int j = 1; j < n; j++)
                {
                    Z[j - 1] = Math.Sqrt(X[j-1]*X[j-1]+B[j]*B[j]);
                    C[j] = X[j-1] / Z[j-1];
                    Sig[j] = B[j] / Z[j - 1];
                    Q[j - 1] = C[j] * Y[j - 1] + S[j] * D[j];
                    X[j] = -Sig[j] * Y[j - 1] + C[j] * D[j];
                    if (j != n - 1)
                    {
                        R[j - 1] = Sig[j] * B[j + 1];
                        Y[j] = C[j] * B[j + 1];
                    }
                }

                Z[n - 1] = X[n - 1];
                A[0] = Sig[1] * Q[0] + C[1] * Z[0];
                B[1] = Sig[1] * Z[1];

                for (int j = 1; j < n - 1; j++)
                {
                    A[j] = Sig[j + 1] * Q[j] + C[j] * C[j + 1] * Z[j];
                    B[j + 1] = Sig[j + 1] * Z[j + 1];
                }

                A[n - 1] = C[n - 1] * Z[n - 1];

                k++;
            }
            throw new MMatrixException("Maximum number of iterations exceeded.");
        }

        public Queue<double> QR(double TOL, int MaxIterations)
        {
            return QR(this, TOL, MaxIterations);
        }

        public MMatrix QRIterationBasic(int max_iterations)
        {           
            MMatrix T = this.Clone();
            MMatrix[] QR = new MMatrix[2];

            for (int i = 0; i < max_iterations; i++)
            {
                QR = T.QRGramSchmidt();
                T = QR[1] * QR[0];
            }

            return T;
        }

        /// <summary>
        /// Gram-Schmidtian orthogonalization of an m by n matrix A, such that
        /// {Q, R} is returned, where A = QR, Q is m by n and orthogonal, R is
        /// n by n and upper triangular matrix.
        /// </summary>
        /// <returns></returns>
        public MMatrix[] QRGramSchmidt()
        {
            int m = row;
            int n = col;            

            MMatrix Q = new MMatrix(m, n);
            MMatrix R = new MMatrix(n, n);

            // the first column of Q equals the first column of this matrix
            for (int i = 0; i < m; i++)
                Q[i, 0] = this[i, 0];

            R[0, 0] = 1;

            for (int k = 0; k < n; k++)
            {
                R[k, k] = Vector.L2Norm(this.Column(k));

                for (int i = 0; i < m; i++)
                    Q[i, k] = this[i, k] / R[k, k];

                for (int j = k + 1; j < n; j++)
                {
                    R[k, j] = Vector.DotProduct(Q.Column(k), this.Column(j));

                    for (int i = 0; i < m; i++)
                        this[i, j] = this[i, j] - Q[i, k] * R[k, j];
                }
            }

            return new MMatrix[] { Q, R };
        }        

        /// <summary>
        /// Computes approximates of the eigenvalues of this matrix. WARNING: Computation
        /// uses basic QR iteration with Gram-Schmidtian orthogonalization. This implies that
        /// (1) only real matrices can be examined; (2) if the matrix has a multiple eigenvalue
        /// or complex eigenvalues, partial junk is returned. This is due to the eigenvalues having
        /// to be like |L1| > |L2| > ... > |Ln| for QR iteration to work properly.
        /// </summary>
        /// <returns></returns>
        public Vector Eigenvalues()
        {
            if (!this.IsSquared())
                throw new MMatrixException("Matrix must be squared.");
            return this.QRIterationBasic(10).DiagVector();
        }
        //This method is not correct enough
        public Vector Eigenvalues_Householder()
        {
            this.Householder();
            return new Vector(this.QR(1E-20, 1000).ToArray());
        }

        //Jacobi Cyclic Method is not correct enough
        public Vector Eigenvalues_JacobiCyclic()
        {
            Vector Eigenvalues = new Vector();
            MMatrix Eigenvectors = new MMatrix();
            MMatrix temp = this.Clone();

            if (!this.IsSquared())
                throw new MMatrixException("Matrix must be squared.");            

            temp.Jacobi_Cyclic_Method(ref Eigenvalues, ref Eigenvectors);
            
            return Eigenvalues;
        }

        //Find eigenvalues, eigenvectors (each column is a vector)
        private static void Jacobi_Cyclic_Method(ref Vector eigenvalues, ref MMatrix eigenvectors, MMatrix A, int n)
        {
            int i, j, k, m;
            MMatrix pAk, pAm, p_r, p_e;
            double threshold_norm;
            double threshold;
            double tan_phi, sin_phi, cos_phi, tan2_phi, sin2_phi, cos2_phi;
            double sin_2phi, cos_2phi, cot_2phi;
            double dum1;
            double dum2;
            double dum3;
            double max;

            eigenvalues = new Vector(n);
            eigenvectors = new MMatrix(n, n);
            pAk = new MMatrix();
            pAm = new MMatrix();
            p_r = new MMatrix();
            p_e = new MMatrix();

            // Take care of trivial cases
            if (n < 1)
                return;
            if (n == 1)
            {
                eigenvalues[0] = A[0, 0];
                eigenvectors[0, 0] = 1.0;
                return;
            }

            // Initialize the eigenvalues to the identity matrix.
            for (p_e = eigenvectors, i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                    if (i == j)
                        p_e[i, j] = 1.0;
                    else
                        p_e[i, j] = 0.0;

            // Calculate the threshold and threshold_norm.

            for (threshold = 0.0, pAk = A, i = 0; i < (n - 1); i++)
                for (j = i + 1; j < n; j++)
                    threshold += pAk[i, j] * pAk[i, j];

            threshold = Math.Sqrt(threshold + threshold);
            threshold_norm = threshold * GlobalMath.EPSILON;
            max = threshold + 1.0;

            while (threshold > threshold_norm)
            {
                threshold /= 10.0;
                if (max < threshold)
                    continue;
                max = 0.0;
                pAk = A;
                pAm = pAk;
                for (k = 0; k < (n - 1); k++)
                {
                    for (m = k + 1; m < n; m++)
                    {
                        if (Math.Abs(pAk[k, m]) < threshold)
                            continue;

                        // Calculate the sin and cos of the rotation angle which
                        // annihilates A[k][m].

                        cot_2phi = 0.5 * (pAk[k, k] - pAm[m, m]) / pAk[k, m];
                        dum1 = Math.Sqrt(cot_2phi * cot_2phi + 1.0);
                        if (cot_2phi < 0.0)
                            dum1 = -dum1;
                        tan_phi = -cot_2phi + dum1;
                        tan2_phi = tan_phi * tan_phi;
                        sin2_phi = tan2_phi / (1.0 + tan2_phi);
                        cos2_phi = 1.0 - sin2_phi;
                        sin_phi = Math.Sqrt(sin2_phi);
                        if (tan_phi < 0.0)
                            sin_phi = -sin_phi;
                        cos_phi = Math.Sqrt(cos2_phi);
                        sin_2phi = 2.0 * sin_phi * cos_phi;
                        cos_2phi = cos2_phi - sin2_phi;

                        // Rotate columns k and m for both the matrix A 
                        //     and the matrix of eigenvectors.

                        p_r = A;
                        dum1 = pAk[k, k];
                        dum2 = pAm[m, m];
                        dum3 = pAk[k, m];
                        pAk[k, k] = dum1 * cos2_phi + dum2 * sin2_phi + dum3 * sin_2phi;
                        pAm[m, m] = dum1 * sin2_phi + dum2 * cos2_phi - dum3 * sin_2phi;
                        pAk[k, m] = 0.0;
                        pAm[m, k] = 0.0;
                        for (i = 0; i < n; i++)
                        {
                            if ((i == k) || (i == m))
                                continue;
                            if (i < k)
                                dum1 = p_r[i, k];
                            else
                                dum1 = pAk[k, i];
                            if (i < m)
                                dum2 = p_r[i, m];
                            else
                                dum2 = pAm[m, i];
                            dum3 = dum1 * cos_phi + dum2 * sin_phi;
                            if (i < k)
                                p_r[i, k] = dum3;
                            else
                                pAk[k, i] = dum3;
                            dum3 = -dum1 * sin_phi + dum2 * cos_phi;
                            if (i < m)
                                p_r[i, m] = dum3;
                            else
                                pAm[m, i] = dum3;
                        }
                        for (p_e = eigenvectors, i = 0; i < n; i++)
                        {
                            dum1 = p_e[i, k];
                            dum2 = p_e[i, m];
                            p_e[i, k] = dum1 * cos_phi + dum2 * sin_phi;
                            p_e[i, m] = -dum1 * sin_phi + dum2 * cos_phi;
                        }
                    }
                    for (i = 0; i < n; i++)
                        if (i == k)
                            continue;
                        else if (max < Math.Abs(pAk[k, i]))
                            max = Math.Abs(pAk[k, i]);
                }
            }
            for (pAk = A, k = 0; k < n; k++)
                eigenvalues[k] = pAk[k, k];
        }

        public void Jacobi_Cyclic_Method(ref Vector eigenvalues, ref MMatrix eigenvectors)
        {
            if (!this.IsSquared())
                throw new MMatrixException("Matrix must be squared.");
            Jacobi_Cyclic_Method(ref eigenvalues, ref eigenvectors, this, this.row);
        }

        public static double SpectralRadius_Jacobi(MMatrix A)
        {
            Vector Eigenvalues = new Vector();            
            MMatrix Eigenvectors = new MMatrix();
            MMatrix temp = A.Clone();
            
            temp.Jacobi_Cyclic_Method(ref Eigenvalues, ref Eigenvectors);
            Eigenvalues.Sort(true);

            return Math.Abs(Eigenvalues[0]);
        }

        public static double SpectralRadius(MMatrix A)
        {
            Vector Eigenvalues = new Vector();

            if (!A.IsSquared())
                throw new MMatrixException("Matrix must be squared.");
            Eigenvalues = A.QRIterationBasic(10).DiagVector();
            Eigenvalues.Sort(true);
            
            return Math.Abs(Eigenvalues[0]);
        }

        public double SpectralRadius()
        {
            return SpectralRadius(this);
        }

        #endregion

        #region Matrix Norm
        public static double L2Norm(MMatrix A)
        {
            double SpectralRadius = MMatrix.SpectralRadius(A.Transpose() * A);

            return Math.Sqrt(SpectralRadius);
        }

        public double L2Norm()
        {
            return L2Norm(this);
        }

        public static double LInfinitiveNorm(MMatrix A)
        {
            double MaxRow = 0;
            double sum = 0;

            for (int i = 0; i < A.row; i++)
            {
                sum = 0;
                for (int j = 0; j < A.col; j++)
                    sum += Math.Abs(A[i, j]);
                if (MaxRow < sum)
                    MaxRow = sum;
            }

            return MaxRow;
        }

        public double LInfinitiveNorm()
        {
            return LInfinitiveNorm(this);
        }

        public static double ConditionNumber(MMatrix A)
        {
            return MMatrix.LInfinitiveNorm(A) * MMatrix.LInfinitiveNorm(A.Inverse());
        }

        public double ConditionNumber()
        {
            return ConditionNumber(this);
        }

        public static double RootMeanSquareError(MMatrix MActual,MMatrix MApproximate)
        {
            double err = 0;
            //err = MMatrix.LInfinitiveNorm(MActual - MApproximate) / MMatrix.LInfinitiveNorm(MActual);            
                        
            double sum = 0;
            double diff;
            for (int i = 0; i < MActual.row; i++)
                for (int j = 0; j < MActual.col; j++)
                {
                    diff = MActual[i, j] - MApproximate[i, j];
                    sum += diff * diff;
                }
            err = Math.Sqrt(sum / (MActual.row * MActual.col));
            
            return err;
        }

        public static int Rank(MMatrix A)
        {            
            MMatrix temp = A.EchelonForm();
            int index = Math.Min(temp.row, temp.col);
            int rank = 0;

            for (int i = 0; i < index; i++)
            {
                if (temp[i, i] != 0)
                    rank++;
            }
            
            return rank;
        }

        public int Rank()
        {
            return Rank(this);
        }

        public static double Sum(MMatrix A)
        {            
            double sum = 0;

            for (int i = 0; i < A.row; i++)
            {                
                for (int j = 0; j < A.col; j++)
                    sum += A[i, j];              
            }

            return sum;
        }

        #endregion        

        #region Gaussian Convolution
        /* This function returns a 2D Gaussian filter with size rows*cols; theta is 
        the angle that the filter rotated counter clockwise; and sigma1 and sigma2
        are the standard deviation of the gaussian functions. Gaussian kernel:
        */
        public static MMatrix D2Gauss(int rows, double stdDeviation1, int cols, double stdDeviation2, double theta)
        {
            MMatrix r = new MMatrix(2, 2);
            MMatrix u = new MMatrix(2, 1);
            MMatrix h = new MMatrix();
            MMatrix g = new MMatrix(cols, rows);

            r[0, 0] = Math.Cos(theta);
            r[0, 1] = -Math.Sin(theta);
            r[1, 0] = Math.Sin(theta);
            r[1, 1] = Math.Cos(theta);

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                {
                    u[0, 0] = j - (rows / 2);
                    u[1, 0] = i - (cols / 2);
                    h = r * u;
                    g[i, j] = Gauss(h[0, 0], stdDeviation1) * Gauss(h[1, 0], stdDeviation2);
                }
            g = g / Math.Sqrt(MMatrix.Sum(MMatrix.PowEntry(g,2)));

            return g;
        }
        //Create Sobel kernel for Canny
        public static MMatrix D2Gauss_Canny(int rows, double stdDeviation1, int cols, double stdDeviation2, double theta)
        {
            MMatrix r = new MMatrix(2, 2);
            MMatrix u = new MMatrix(2, 1);
            MMatrix h = new MMatrix();
            MMatrix g = new MMatrix(cols, rows);

            r[0, 0] = Math.Cos(theta);
            r[0, 1] = -Math.Sin(theta);
            r[1, 0] = Math.Sin(theta);
            r[1, 1] = Math.Cos(theta);

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                {
                    u[0, 0] = j - (rows / 2);
                    u[1, 0] = i - (cols / 2);
                    h = r * u;
                    g[i, j] = Gauss(h[0, 0], stdDeviation1) * DGauss(h[1, 0], stdDeviation2);
                }
            MMatrix ag = Abs(g);
            g = g / Math.Sqrt(MMatrix.Sum(MMatrix.PowEntry(ag,2)));

            return g;
        }

        public static double Gauss(double x, double stdDeviation)
        {
            return Math.Exp(-x * x / (2 * stdDeviation * stdDeviation)) / (stdDeviation * Math.Sqrt(2 * Math.PI));
        }

        public static double DGauss(double x, double stdDeviation)
        {
            return (-x) * Gauss(x, stdDeviation) / (stdDeviation * stdDeviation);
        }        

        public static MMatrix Gauss_Convolution(MMatrix Matrix, bool bCanny, int rows, double sigma1, int cols, double sigma2, double theta)
        {            
            int halfKernelRows = rows / 2;
            int halfKernelCols = cols / 2;
            int xMax = Matrix.row + halfKernelRows;
            int yMax = Matrix.col + halfKernelCols;
            double tmpBit;
            MMatrix Filter;
            MMatrix conMatrix = new MMatrix(Matrix.row + rows, Matrix.col + cols);

            //Create Gaussian matrix
            if(bCanny)
                Filter = D2Gauss_Canny(rows, sigma1, cols, sigma2, theta);
            else
                Filter = D2Gauss(rows, sigma1, cols, sigma2, theta);

            //Create a larger matrix to do convolution
            for (int i = halfKernelRows; i < xMax; i++)
            {
                for (int j = halfKernelCols; j < yMax; j++)
                {
                    conMatrix[i, j] = Matrix[i - halfKernelRows, j - halfKernelCols];                   
                }
            }
            //Do convolution on the bigger matrix
            for (int i = halfKernelRows; i < xMax; i++)
            {
                for (int j = halfKernelCols; j < yMax; j++)
                {
                    tmpBit = 0;
                    for (int k = 0; k < rows; k++)
                        for (int l = 0; l < cols; l++)
                        {
                            tmpBit += conMatrix[i - halfKernelRows + k, j - halfKernelCols + l] * Filter[k, l];                            
                        }
                    conMatrix[i, j] = tmpBit;
                }
            }
            //Copy back to the original-size matrix
            MMatrix newMatrix = new MMatrix(Matrix.row, Matrix.col);

            for (int i = halfKernelRows; i < xMax; i++)
            {
                for (int j = halfKernelCols; j < yMax; j++)
                {
                    newMatrix[i - halfKernelRows, j - halfKernelCols] = conMatrix[i, j];
                }
            }

            return newMatrix;
        }
        #endregion 

    }
}
