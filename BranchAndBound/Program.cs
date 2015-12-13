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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SimplexTable st = MatrixReader.ReadSimplexTable("MatrixA.txt","VectorB.txt","VectorC.txt","Signs.txt");
            //CanonicalTransformation.TransformForSimplex(st);
            //SimplexAlgorithm sa = new SimplexAlgorithm(st, true);
            //sa.GetResultForSimplex();
            BranchAndBoundAlgorithm bb = new BranchAndBoundAlgorithm();
            SimplexAlgorithm sa = bb.GetResult(st, true);
            Console.Write("\nResult\n{\t");
            for (int i = 0; i < sa.Result.Length; i++)
            {
                Console.Write(sa.Result[i] + "\t");
            }
            Console.WriteLine("}");
            Console.Write("Function value {0} ", sa.FunctionValue);
            sw.Stop();
            Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
