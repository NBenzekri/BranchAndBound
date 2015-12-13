using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    class CanonicalTransformation
    {
        private static Fraction FindM(SimplexTable st)
        {
            Fraction max = st.C[0].Abs();
            for (int j=0; j<st.nColumns; j++)
            {
                if ((st.C[j].Abs() > max)&&(st.TypeOfVariable[j]==1))
                    max = st.C[j].Abs();
            }
            return max * (-10);
        }
        private static void AddNewColumn(List<List<Fraction>> listF)
        {
            for (int i=0; i<listF.Count; i++)
            {
                listF[i].Add(0);
            }
        }
        public static void Transform(SimplexTable simplexTable, int startRow=0)
        {
            Fraction M = FindM(simplexTable);
            for (int i = startRow; i < simplexTable.nRows; i++)
            {
                simplexTable.nColumns++;
                AddNewColumn(simplexTable.A);
                simplexTable.A[i][simplexTable.A[i].Count - 1] = 1;
                if (simplexTable.Sign[i] == "<=") 
                {
                    simplexTable.C.Add(0);
                    simplexTable.TypeOfVariable.Add(2);
                }
                else
                {
                    if (simplexTable.Sign[i] == ">=")
                    {
                        simplexTable.C.Add(M);
                        simplexTable.TypeOfVariable.Add(3);
                        simplexTable.nColumns++;
                        AddNewColumn(simplexTable.A);
                        simplexTable.A[i][simplexTable.A[i].Count - 1] = -1;
                        simplexTable.C.Add(0);
                        simplexTable.TypeOfVariable.Add(2);
                    }
                    else
                    {
                        simplexTable.C.Add(M);
                        simplexTable.TypeOfVariable.Add(3);
                    }
                }
                simplexTable.Sign[i] = "=";
            }
        } 
        public static void TransformForSimplex(SimplexTable simplexTable, int startRow=0)
        {
            for (int i = startRow; i < simplexTable.nRows; i++)
            {
                if (simplexTable.B[i] < 0)
                {
                    for (int j = 0; j < simplexTable.nColumns; j++)
                    {
                        simplexTable.A[i][j] *= -1;
                    }
                    simplexTable.B[i] *= -1;
                    if (simplexTable.Sign[i] == "<=")
                        simplexTable.Sign[i] = ">=";
                    else
                    {
                        if (simplexTable.Sign[i] == ">=")
                            simplexTable.Sign[i] = "<=";
                    }
                }
            }
            Transform(simplexTable, startRow);
        }
    }
}
