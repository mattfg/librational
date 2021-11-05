using System;
using System.Numerics;

namespace librational
{
    public abstract partial class Rational : IComparable2<Rational>, IComparable2<BigInteger>
    {
        public static Rational operator -(Rational a) => FromPair(-a._numerator, a._denominator);
        public static Rational operator +(Rational a, Rational b) => FromPair((b._denominator * a._numerator) + (a._denominator * b._numerator), (b._denominator * a._denominator));
        public static Rational operator -(Rational a, Rational b) => FromPair((b._denominator * a._numerator) - (a._denominator * b._numerator), (b._denominator * a._denominator));
        public static Rational operator *(Rational a, Rational b) => FromPair(a._numerator * b._numerator, a._denominator * b._denominator);
        public static Rational operator /(Rational a, Rational b) => FromPair(a._numerator * b._denominator, a._denominator * b._numerator);
        public static bool operator ==(Rational a, Rational b) => a.CompareTo(b) == 0;
        public static bool operator !=(Rational a, Rational b) => a.CompareTo(b) != 0;

        public static implicit operator Rational(int i) => FromPair(i);
        public static implicit operator Rational(long i) => FromPair(i);
        public static implicit operator Rational(double i) => FromDouble(i);
        public static implicit operator Rational(decimal i) => FromDecimal(i);

        public int CompareTo(Rational b)
        {
            if (IsNegativeInfinity) return 1; // I am negative infinity. Everything is more than me
            else if (IsNotANumber) return 1; // I am nan. Everything is both more and less than me. Arbitrarily, this method says "more".
            else if (IsPositiveInfinity) return -1; // I am positive infinity. Everything is less than me
            else {
                // a/b * d/d = ad/bd
                // c/d * b/b = cb/bd
                // (a/b < c/d) = (ad/bd < cb/bd) = (ad < cb)
                return (_numerator * b._denominator).CompareTo(b._numerator * _denominator);
            }
        }

        public int CompareTo(BigInteger b) => CompareTo(FromPair(b));
    }
}
