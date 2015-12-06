using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BranchAndBound
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hELLO");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Fraction[,] a = { { 7, 4, 2}, 
                { 6, 8, 9},
                { 1, 4, 5} };
            Fraction[] b = { 9, 15, 6 };
            Fraction[] c = { 2, 15, 2};
            SimplexTable st = new SimplexTable(a, b, c, new string[] { "<=", "<=", "<=" }); 

            //double[,] a = { { -5, -6, 1, 0, 0 }, { -15, 0, 0, 1, 0 }, { -7, -12, 0, 0, 1} };
            //double[] b = { -1, -1, -1 };
            //double[] c = { 1, 1, 0, 0, 0 };

            //CanonicalTransformation.TransformForSimplex(st);
            //SimplexAlgorithm sa = new SimplexAlgorithm(st, true);
            //sa.GetResultForSimplex();
            BranchAndBoundAlgorithm bb = new BranchAndBoundAlgorithm();
            SimplexAlgorithm sa = bb.GetResult(st, true);

            Console.Write("\nResult\n{\t");
            for (int i = 0; i <sa.Result.Length; i++)
            {
                Console.Write(sa.Result[i]+ "\t");
            }
            Console.WriteLine("}");
            Console.Write("Function value {0} ",sa.FunctionValue);
            //Console.WriteLine("{0}",res[1].Reduce());
            sw.Stop();
            Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
