using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class SystemOfEquation
    {
        public MMatrix CoefficientMatrix;
        public MMatrix ArgumentMatrix;
        public Vector b;
        public Vector x;
        public bool InfiniteSolution;

        public SystemOfEquation()
        {
        }

        public SystemOfEquation(MMatrix CoefficientMatrix, Vector b)
        {
            this.CoefficientMatrix = CoefficientMatrix.Clone();
            this.b = b.Clone();
        }

        public static MMatrix CalculateArgumentMatrix(MMatrix CoefficientMatrix, Vector b)
        {
            //Form Argument matrix from CoefficientMatrix and b
            MMatrix ArgumentMatrix = new MMatrix(CoefficientMatrix.row, CoefficientMatrix.col + 1);
            for (int i = 0; i < CoefficientMatrix.row; i++)
                for (int j = 0; j < CoefficientMatrix.col; j++)
                    ArgumentMatrix[i, j] = CoefficientMatrix[i, j];
            for (int i = 0; i < CoefficientMatrix.row; i++)
                ArgumentMatrix[i, ArgumentMatrix.col - 1] = b[i];

            return ArgumentMatrix;
        }

        public void CalculateArgumentMatrix()
        {
            ArgumentMatrix = CalculateArgumentMatrix(this.CoefficientMatrix, this.b);
        }

        #region System of Equations

        //Homogenious system: Solve A*x = 0
        public static Vector SolveSystemOfEquations(MMatrix A)
        {
            MMatrix Ref = new MMatrix();
            Vector x = new Vector(A.col);
            double sum;

            Ref = A.EchelonForm();
            for (int i = A.row - 1; i >= 0; i--)
            {
                //The system has more than one solutions. Get one.
                if (Ref[i, i] == 0)
                    x[i] = 1;
                else
                {
                    sum = 0;
                    for (int j = i + 1; j < Ref.col; j++)
                    {
                        sum += Ref[i, j] * x[j];
                    }
                    x[i] = -sum / Ref[i, i];
                }
            }

            return x;
        }

        //Solve A*x = b
        //After solving the system, Argument Matrix will contain the reduced system
        public static Vector SolveSystemOfEquations(MMatrix CoefficientMatrix, Vector b,ref MMatrix ArgumentMatrix, ref bool InfiniteSolution)
        {
            if (CoefficientMatrix.row != b.Elements.Length)
                throw new MMatrixException("The coefficient matrix A and vector b are different size.");

            Vector x = new Vector(CoefficientMatrix.col);
            Vector xSetFlag = new Vector(CoefficientMatrix.col);
            double sum;
            InfiniteSolution = false;

            ArgumentMatrix = CalculateArgumentMatrix(CoefficientMatrix,b);
            
            try
            {
                //Gaussian elimination
                for (int i = 0; i < CoefficientMatrix.row; i++)
                {
                    if (i < ArgumentMatrix.col - 1)
                    {
                        if (ArgumentMatrix[i, i] == 0)	// if diagonal entry is zero, 
                            for (int j = i + 1; j < ArgumentMatrix.row; j++)
                                if (ArgumentMatrix[j, i] != 0)	 //check if some below entry is non-zero
                                    ArgumentMatrix.InterchangeRow(i, j);	// then interchange the two rows
                        if (ArgumentMatrix[i, i] == 0)	// if not found any non-zero diagonal entry
                            continue;	// increment i;

                        if (ArgumentMatrix[i, i] != 1)	// if diagonal entry is not 1 , 	
                            for (int j = i + 1; j < ArgumentMatrix.row; j++)
                                if (ArgumentMatrix[j, i] == 1)	 //check if some below entry is 1
                                    ArgumentMatrix.InterchangeRow(i, j);	// then interchange the two rows

                        ArgumentMatrix.MultiplyRow(i, 1 / (ArgumentMatrix[i, i]));

                        for (int j = i + 1; j < ArgumentMatrix.row; j++)
                            ArgumentMatrix.AddRow(j, i, -ArgumentMatrix[j, i]);
                    }
                }

                //Finding solution based on the reduced system == Argument matrix

                //More equations than unknown varriables x[i]
                if (CoefficientMatrix.row > CoefficientMatrix.col)
                    for (int i = CoefficientMatrix.col; i < CoefficientMatrix.row; i++)
                    {
                        if (ArgumentMatrix[i, CoefficientMatrix.col - 1] == 0)
                            if (ArgumentMatrix[i, ArgumentMatrix.col - 1] != 0)  //b[i] == 0
                                return null;
                    }                
                
                for (int i = Math.Min(CoefficientMatrix.row, CoefficientMatrix.col) - 1; i >= 0; i--)
                {
                    //The system has more than one solutions. Get one.
                    if (ArgumentMatrix[i, i] == 0)
                    {
                        //More unknowns than equations
                        int j;
                        for (j = i+1; j < CoefficientMatrix.col; j++)
                        {
                            if (ArgumentMatrix[i, j] != 0)
                            {
                                sum = 0;
                                for (int k = j + 1; k < CoefficientMatrix.col; k++)
                                {
                                    sum += ArgumentMatrix[i, k] * x[k];
                                }
                                if (xSetFlag[j] == 0)
                                {
                                    //this is the 1st time setting the solution
                                    x[j] = (ArgumentMatrix[i, ArgumentMatrix.col - 1] - sum) / ArgumentMatrix[i, j];
                                    xSetFlag[j] = 1;
                                }
                                //this is not the 1st time setting the solution
                                else
                                    if (x[j] != (ArgumentMatrix[i, ArgumentMatrix.col - 1] - sum) / CoefficientMatrix[i, j])
                                    {                                        
                                        return null;
                                    }
                                    else
                                        InfiniteSolution = true;
                                break;
                            }
                        }
                        //All coefficient == 0 & b[i] == 0 -> The system has infinitely many solutions
                        if ((j == CoefficientMatrix.col) && (ArgumentMatrix[i, ArgumentMatrix.col - 1] == 0))  
                        {
                            x[i] = 0;
                            //xSetFlag[i] = 1;-> since we may use other equations to update this x[i] later, so don't set flag for case 0 = 0.
                            InfiniteSolution = true;
                        }
                        else if ((j == CoefficientMatrix.col) && (ArgumentMatrix[i, ArgumentMatrix.col - 1] != 0)) 
                            return null; //All coefficient == 0 & b[i] != 0 -> The system has no solutions
                    }
                    else
                    {
                        sum = 0;
                        for (int j = i + 1; j < CoefficientMatrix.col; j++)
                        {
                            sum += ArgumentMatrix[i, j] * x[j];
                        }
                        x[i] = (ArgumentMatrix[i, ArgumentMatrix.col - 1] - sum) / ArgumentMatrix[i, i];
                        xSetFlag[i] = 1;
                    }
                }                

                return x;
            }
            catch (Exception exp)
            {
                throw new MMatrixException("The system of equation cannot be solved:" + exp.Message);
            }
        }

        public void SolveSystemOfEquations()
        {
            this.x = SolveSystemOfEquations(this.CoefficientMatrix, this.b, ref this.ArgumentMatrix, ref this.InfiniteSolution);
        }

        public static string PrintSystemOfEquations(MMatrix CoefficientMatrix, Vector b)
        {
            string s = "";
            string sign;

            for (int i = 0; i < CoefficientMatrix.row; i++)
            {
                for (int j = 0; j < CoefficientMatrix.col; j++)
                {
                    if (CoefficientMatrix[i, j] >= 0)
                        sign = "+";
                    else
                        sign = "";
                    s += sign + CoefficientMatrix[i, j].ToString() + " X" + (j + 1).ToString() + "\t";
                }
                s += " = " + b[i].ToString();
                s += "\r\n";
            }
            
            return s;
        }

        public string PrintSystemOfEquations()
        {
            return PrintSystemOfEquations(this.CoefficientMatrix, this.b);
        }

        //After solving the system, Argument Matrix will contain the reduced system
        public static string PrintSystemOfEquations(MMatrix AugmentedMatrix)
        {
            string s = "";
            string sign;

            for (int i = 0; i < AugmentedMatrix.row; i++)
            {
                for (int j = 0; j < AugmentedMatrix.col - 1; j++)
                {
                    if (AugmentedMatrix[i, j] >= 0)
                        sign = "+";
                    else
                        sign = "";
                    s += sign + Math.Round(AugmentedMatrix[i, j], MMatrix.DIGITS).ToString() + " X" + (j + 1).ToString() + "\t";
                }
                s += " = " + Math.Round(AugmentedMatrix[i, AugmentedMatrix.col - 1], MMatrix.DIGITS).ToString();
                s += "\r\n";
            }
            
            return s;
        }

        public string PrintSolutions(Vector Solutions, bool InfiniteSolution)
        {
            string s;

            if (Solutions == null)
            {
                s = "The system has no solution.";
                return s;
            }
            else
            {
                if ((InfiniteSolution) || (this.CoefficientMatrix.row < this.CoefficientMatrix.col))
                {
                    s = "The system has infinitely many solutions:\r\n";
                }
                else
                {
                    s = "The system has a unique solution:\r\n";
                }
            }
            for (int i = 0; i < Solutions.Elements.Length; i++)
            {
                s += "X" + (i + 1).ToString() + " = " + Math.Round(Solutions[i],MMatrix.DIGITS).ToString() + "\r\n";
            }

            return s;
        }

        public string PrintSolutions()
        {
            return PrintSolutions(this.x, this.InfiniteSolution);
        }
        
        #endregion
    }
}
