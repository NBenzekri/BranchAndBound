using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    class SimplexTable: ICloneable
    {
        public List<List<Fraction>> A { get; set; }
        public List<Fraction> B { get; set; } //nRows
        public List<Fraction> C { get; set; } //nColumns
        public List<string> Sign { get; set; } //nRows
        public List<byte> TypeOfVariable { get; set; } //nColumns 1-onw, 2-balance, 3-artificial  
        public int nColumns;
        public int nRows;
        public SimplexTable(Fraction[,] matrixA, Fraction[] arrayB, Fraction[] targetFunction, string[] sign, byte[] typeOfVariable = null)
        {
            nRows = matrixA.GetLength(0);
            nColumns = matrixA.GetLength(1);
            A = new List<List<Fraction>>();
            B = new List<Fraction>(arrayB);
            C = new List<Fraction>(targetFunction);
            Sign = new List<string>();
            for (int i = 0; i < nRows; i++)
            {
                A.Add(new List<Fraction>());
                Sign.Add((string)sign[i].Clone());
                for (int j = 0; j < nColumns; j++)
                {
                    A[i].Add(matrixA[i, j]);
                }
            }          
            if (typeOfVariable != null)
                TypeOfVariable = new List<byte>(typeOfVariable);
            else
            {
                TypeOfVariable = new List<byte>();
                for (int j = 0; j < nColumns; j++)
                    TypeOfVariable.Add(1);
            }
        }
        public SimplexTable(List<List<Fraction>> matrixA, List<Fraction> arrayB, List<Fraction> targetFunction, List<string> sign, List<byte> typeOfVariable = null)
        {
            nRows = matrixA.Count;
            nColumns = matrixA[0].Count;
            A = new List<List<Fraction>>();
            B = new List<Fraction>(arrayB);
            C = new List<Fraction>(targetFunction);
            Sign = new List<string>();
            for(int i=0; i<nRows; i++)
            {
                A.Add(new List<Fraction>(matrixA[i]));
                Sign.Add((string)sign[i].Clone());
            }
            if (typeOfVariable != null)
                TypeOfVariable = new List<byte>(typeOfVariable);
            else
            {
                TypeOfVariable = new List<byte>();
                for (int j = 0; j < nColumns; j++)
                    TypeOfVariable.Add(1);
            }
        }
        public object Clone()
        {
            return new SimplexTable(A, B, C, Sign, TypeOfVariable);
        }
        public void ChangTargetFunction()
        {
            for (int j = 0; j < nColumns; j++)
            {
                C[j] *= -1;
            }
        }
        public override string ToString()
        {
            string str="";
            for (int i=0; i<nRows; i++)
            {
                for (int j=0; j<nColumns; j++)
                {
                    str += (A[i][j].ToString()+"\t");
                }
                str += Sign[i]+B[i].ToString()+"\n";
            }
            return str;
        }
    }
    class SimplexAlgorithm
    {
        public SimplexTable simplexTable;       
        public Fraction[] Result { get; set; }
        public Fraction FunctionValue { get; set; }
        public bool TaskForMax { get; set; }
        private Fraction[] d; //nColumns
        private int[] basis; //nRows
        private int guideColumn;
        private int guideRow;

        public SimplexAlgorithm(SimplexTable simplexTable, bool taskForMax)
        {
            this.simplexTable = (SimplexTable)simplexTable.Clone();           
            d = new Fraction[simplexTable.nColumns];
            basis = new int[simplexTable.nRows];
            TaskForMax = taskForMax;
        }
        private void RecalculateTable()
        {
            Fraction temp;
            for (int i=0; i< simplexTable.nRows; i++ )
            {
                if (i != guideRow)
                {
                    temp = simplexTable.A[i][guideColumn].Reduce();
                    simplexTable.B[i] = (simplexTable.B[i] - simplexTable.B[guideRow] * temp / simplexTable.A[guideRow][ guideColumn]).Reduce();
                    for (int j = 0; j < simplexTable.nColumns; j++)
                        simplexTable.A[i][j] = (simplexTable.A[i][ j] - simplexTable.A[guideRow][ j] * temp / simplexTable.A[guideRow][ guideColumn]).Reduce();
                    simplexTable.A[i][guideColumn] = 0;
                }
                else
                {
                    temp = simplexTable.A[guideRow][guideColumn].Reduce();
                    simplexTable.B[guideRow] = (simplexTable.B[guideRow] / temp).Reduce();
                    for (int j = 0; j < simplexTable.nColumns; j++)
                        simplexTable.A[guideRow][j] = (simplexTable.A[guideRow][j] / temp).Reduce();
                }
            }

        }
        private void CalculateD()
        {
            for (int j=0; j < simplexTable.nColumns; j++)
            {
                d[j]=0;
                for (int i=0; i< simplexTable.nRows; i++)
                {
                    d[j] += simplexTable.C[basis[i]] * simplexTable.A[i][j]; 
                }
                d[j] -= simplexTable.C[j];
            }
        }
        private bool FindGuideColumnForSimplex()
        {
            Fraction min = 0;
            bool b = false;
            for (int j=0; j<simplexTable.nColumns; j++)
            {
                if (d[j]<min)
                {
                    min = d[j];
                    guideColumn = j;
                    b = true;
                }
            }
            return b;
        }
        private void FindGuideRowForSimplex()
        {
            Fraction min=-1;
            int k = 0;
            Fraction q;
            for (int i=0; (i<simplexTable.nRows)&&(min<0); i++)
            {
                if (simplexTable.A[i][guideColumn] != 0)
                {
                    min = simplexTable.B[i] / simplexTable.A[i][guideColumn];
                    k = i;
                }
            }
            if (min >= 0)
            {
                guideRow = k;
                for (int i = k; i < simplexTable.nRows; i++)
                {

                    if (simplexTable.A[i][guideColumn] != 0)
                    {
                        q = simplexTable.B[i] / simplexTable.A[i][guideColumn];
                        if ((q >= 0) && (q < min))
                        {
                            min = q;
                            guideRow = i;
                        }
                    }
                }
                
            }
            else
            {
                throw new Exception("no root");
            }
        }
        private void SelectBasis()
        {
            int onePos;
            bool b;
            for (int j = 0; j < simplexTable.nColumns; j++)
            {
                b = true;
                onePos = -1;
                for (int i = 0; (i < simplexTable.nRows) && b; i++)
                {
                    if ((simplexTable.A[i][j] == 1) && (onePos == -1))
                    {
                        onePos = i;
                    }
                    else
                    {
                        if (simplexTable.A[i][j] != 0)
                            b = false;
                    }
                }
                if (b && (onePos >= 0))
                    basis[onePos] = j;
            }
        }
        private void GetResult()
        {

            Result = new Fraction[simplexTable.nColumns];
            FunctionValue = 0;
            for (int j = 0; j < simplexTable.nColumns; j++)
            {
                Result[j] = 0;
            }
            for (int i = 0; i < simplexTable.nRows; i++)
            {
                Result[basis[i]] = simplexTable.B[i];
                if (simplexTable.TypeOfVariable[basis[i]] == 1)
                    FunctionValue += Result[basis[i]] * simplexTable.C[basis[i]];
            }
            FunctionValue = FunctionValue.Reduce();
        }
        public void GetResultForSimplex()
        {
            if (!TaskForMax)
                simplexTable.ChangTargetFunction();
            SelectBasis();
            CalculateD();
            while (FindGuideColumnForSimplex())
            {
                FindGuideRowForSimplex();          
                RecalculateTable();
                basis[guideRow] = guideColumn;
                CalculateD();
            }
            if (!TaskForMax)
                simplexTable.ChangTargetFunction();
            GetResult();
        }
        /*private bool FindGuideRowForDualSimplex()
        {
            Fraction min = 0;
            bool b = false;
            for (int i=0; i< simplexTable.nRows; i++)
            {
                if (simplexTable.B[i] < min)
                {
                    min = simplexTable.B[i];
                    guideRow = i;
                    b = true;
                }
            }
            return b;
        }*/
        /*private void FindGuideColumnForDualSimplex()
        {
            Fraction min=-1;
            int p=0;
            Fraction q;
            for (int j=0; j<simplexTable.nColumns&&(min<0); j++)
            {
                if (simplexTable.A[guideRow][j] < 0)
                {
                    min = (simplexTable.A[guideRow][j] / d[j]).Abs();
                    p = j;
                }
            }
            if (min>=0)
            {
                guideColumn = p;
                for (int j = p; j<simplexTable.nColumns; j++ )
                {
                    if ((simplexTable.A[guideRow][j] < 0)&&((q= (simplexTable.A[guideRow][j]/d[j]).Abs())<min))
                    {
                        min = q;
                        guideColumn = j;
                    }
                }
            }
            else
            {
                throw new Exception("no root");
            }
        }*/
        /*public void GetResultForDualSimplex()
        {
            if (TaskForMax)
                simplexTable.ChangTargetFunction();
            SelectBasis();
            CalculateD();
            while (FindGuideRowForDualSimplex())
            {
                FindGuideColumnForDualSimplex();
                RecalculateTable();
                basis[guideRow] = guideColumn;
                CalculateD();
                //{
                //    Console.WriteLine("\nSimplex table:");
                //    for (int i = 0; i < simplexTable.nRows; i++)
                //    {
                //        for (int j = 0; j < simplexTable.nColumns; j++)
                //            Console.Write("{0}\t", simplexTable.A[i, j]);
                //        Console.WriteLine("b[{0}]={1}",i,simplexTable.B[i]);
                //    }
                //    Console.WriteLine("d:");
                //    for (int j = 0; j < simplexTable.nColumns; j++)
                //        Console.Write(d[j] + "\t");
                //}
            }
            if (TaskForMax)
                simplexTable.ChangTargetFunction();
            GetResult();
        }*/
    }
}
