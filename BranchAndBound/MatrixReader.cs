using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BranchAndBound
{
    public class MatrixReader
    {
        public static List<List<Fraction>> ReadMatrix(string fileName, int nRows = 0, int nColumns = 0)
        {
            List<List<Fraction>> matrixA = new List<List<Fraction>>();
            string[] lines = File.ReadAllLines(fileName);
            if ((nRows == 0)||(nRows>lines.Length))
                nRows = lines.Length;
            for (int i=0; i < nRows; i++)
            {
                matrixA.Add(new List<Fraction>());
                string[] str = lines[i].Split(',');
                if ((nColumns == 0) || (nColumns > str.Length))
                    nColumns = str.Length;
                for (int j = 0; j < nColumns; j++)
                {
                    matrixA[i].Add(int.Parse(str[j]));
                }
            }
            return matrixA;
        }
        public static List<Fraction> ReadVector(string fileName, int n = 0)
        {
            List<Fraction> vector = new List<Fraction>();
            string[] lines = File.ReadAllLines(fileName);
            if ((n == 0)||(n>lines.Length))
                n = lines.Length;
            for (int i = 0; i < n; i++)
                vector.Add(int.Parse(lines[i]));
            return vector;
        }
        public static List<string> ReadStringVector(string fileName, int n=0)
        {
            List<string> vector = new List<string>();
            string[] lines = File.ReadAllLines(fileName);
            if ((n == 0) || (n > lines.Length))
                n = lines.Length;
            for (int i = 0; i < n; i++)
                vector.Add(lines[i]);
            return vector;
        }
        public static SimplexTable ReadSimplexTable(string matrixA, string vectorB, string vectorC, string sign)
        {
            SimplexTable st = new SimplexTable(ReadMatrix(matrixA), ReadVector(vectorB), ReadVector(vectorC), ReadStringVector(sign));
            return st;
        }
    }
}
