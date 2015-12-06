using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    class BranchAndBoundAlgorithm
    {
        public SimplexAlgorithm GetResult(SimplexTable simplexTable, bool taskForMax)
        {
            if (!taskForMax)
                simplexTable.ChangTargetFunction();
            return GetResult(simplexTable,  0);
        }
        private SimplexAlgorithm GetResult(SimplexTable simplexTable, int startRowForTransform)
        {
            SimplexTable simplexT = (SimplexTable)simplexTable.Clone();
            CanonicalTransformation.TransformForSimplex(simplexT, startRowForTransform);
            SimplexAlgorithm smpAlg= new SimplexAlgorithm(simplexT, true);
            smpAlg.GetResultForSimplex();
            int nOfDouble = IsDouble(smpAlg.Result);
            if (nOfDouble==-1)
                return smpAlg;
            //Branch 1
            Console.WriteLine("start branch 1");
            SimplexTable simplexT1 = (SimplexTable) simplexT.Clone();
            AddLimitation(simplexT1, nOfDouble, smpAlg.Result[nOfDouble], "<=");
            SimplexAlgorithm smpAlg1;
            smpAlg1 = GetResult(simplexT1, simplexT1.nRows - 1);

            //Brackh 2
            Console.WriteLine("start branch 2");
            SimplexTable simplexT2  = (SimplexTable) simplexT.Clone();
            AddLimitation(simplexT2, nOfDouble, smpAlg.Result[nOfDouble], ">=");
            SimplexAlgorithm smpAlg2;
            smpAlg2 = GetResult(simplexT2, simplexT2.nRows - 1);

            if (smpAlg1.FunctionValue>smpAlg2.FunctionValue)
                return smpAlg1;
            else
                return smpAlg2;
        }
        private void AddLimitation(SimplexTable simplexTable, int nOfDouble, Fraction value, string sign)
        {
            simplexTable.nRows++;
            simplexTable.A.Add(new List<Fraction>());
            for (int j = 0; j < simplexTable.nColumns; j++)
            {
                if (j != nOfDouble)
                    simplexTable.A[simplexTable.A.Count - 1].Add(0);
                else
                    simplexTable.A[simplexTable.A.Count - 1].Add(1);
            }
            simplexTable.Sign.Add((string)sign.Clone());
            Fraction intPart;
            intPart = (int)value;
            if (value < 0)
                intPart = intPart - 1;
            if (sign==">=")           
                simplexTable.B.Add(intPart+1);
            else
                simplexTable.B.Add(intPart - 1);
        }
        private int IsDouble(Fraction[] f)
        {
            for (int j = 0; j < f.Length; j++)
                if (!f[j].IsInteger())
                    return j;
            return -1;
        }
    }
}
