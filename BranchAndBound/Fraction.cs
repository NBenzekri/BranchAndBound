using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    struct Fraction : ICloneable, IComparable
    {
        int Numerator { get; set; }
        int Denominator { get; set; }
        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            if (Denominator == 0)
                Denominator = 1;
        }
        public Fraction(int integer) : this(integer, 1) { }
        //public Fraction() : this( 0, 1) { }
        public object Clone()
        {
            return new Fraction(Numerator, Denominator);
        }
        public int CompareTo(object obj)
        {
            Fraction temp = (Fraction)obj;
            if (Denominator == temp.Denominator)
                return Numerator.CompareTo(temp.Numerator);
            else
                return (Numerator * temp.Denominator).CompareTo(temp.Numerator * Denominator);
        }
        public override bool Equals(object obj)
        {
            return (CompareTo(obj)==0);
        }
        public override int GetHashCode()
        {
            return Numerator/Denominator;
        }
        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator);
        }
        private int GreatestCommonDivisor() //найбільший спільний дільник
        {
            int a = Math.Abs(Numerator);
            int b = Denominator;
            if (a == 0)
                return b;
            while (b != 0)
            {
                if (a > b)
                    a = a - b;
                else
                    b = b - a;
            }
            return a;
        }
        public Fraction Reduce() 
        {
            int gcd = GreatestCommonDivisor();
            return new Fraction(Numerator/gcd,Denominator/gcd);
        }
        public bool IsInteger()
        {
            return (Numerator % Denominator) == 0;
        }

        public static Fraction operator +(Fraction x1, Fraction x2)
        {
            Fraction res = new Fraction();
            if (x1.Denominator == x2.Denominator)
            {
                res.Numerator = x1.Numerator + x2.Numerator;
                res.Denominator = x1.Denominator;
            }
            else
            {
                res.Numerator = x1.Numerator * x2.Denominator + x2.Numerator * x1.Denominator;
                res.Denominator = x1.Denominator * x2.Denominator;
            }
            return res;
        }
        public static Fraction operator -(Fraction x1, Fraction x2)
        {
            Fraction temp = (Fraction)x2.Clone();
            temp.Numerator *= -1;
            return (x1 + temp);
        }
        public static Fraction operator *(Fraction x1, Fraction x2)
        {
            Fraction res = new Fraction();
            res.Numerator = x1.Numerator * x2.Numerator;
            res.Denominator = x1.Denominator * x2.Denominator;
            return res;
        }
        public static Fraction operator /(Fraction x1, Fraction x2)
        {
            if (x2 == 0)
                throw new DivideByZeroException();
            Fraction res = new Fraction();
            res.Numerator = x1.Numerator * x2.Denominator;
            res.Denominator = x1.Denominator * x2.Numerator;
            return res;
        }
        public static bool operator >(Fraction x1, Fraction x2)
        {
            return (x1.CompareTo(x2) == 1);
        }
        public static bool operator <(Fraction x1, Fraction x2)
        {
            return (x1.CompareTo(x2) == -1);
        }
        public static bool operator >=(Fraction x1, Fraction x2)
        {
            return ((x1.CompareTo(x2) == 1) || (x1.CompareTo(x2) == 0));
        }
        public static bool operator <=(Fraction x1, Fraction x2)
        {
            return ((x1.CompareTo(x2) == -1) || (x1.CompareTo(x2) == 0));
        }
        public static bool operator ==(Fraction x1, Fraction x2)
        {
            return (x1.CompareTo(x2) == 0);
        }
        public static bool operator !=(Fraction x1, Fraction x2)
        {
            return (x1.CompareTo(x2) != 0);
        }
        public static implicit operator Fraction (int x)
        {
            return new Fraction(x);
        }
        public static explicit operator int(Fraction x)
        {
            return x.Numerator / x.Denominator;
        }
        public Fraction Abs()
        {
            return new Fraction(Math.Abs(Numerator), Denominator);
        }
    }
}
