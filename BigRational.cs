using System;
using System.Numerics;

namespace librational
{
    public partial struct BigRational: IRational
    {
        public readonly BigInteger Numerator { get; }
        public readonly BigInteger Denominator { get; }

        public override bool Equals(object obj) => base.Equals(obj);
        public bool Equals(BigRational b) => Numerator == b.Numerator && Denominator == b.Denominator;
        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
        public override string ToString() => $"{Numerator}/{Denominator}";

        public BigRational(long num = 0, long denom = 1):
            this(new BigInteger(num), new BigInteger(denom))
        {
        }

        public BigRational(double value)
        {
            var whole = Math.Truncate(value);
            var wholeRat = new UnsafeBigRational((BigInteger)whole, 1);
            var frac = value - whole;
            var scale = Math.Pow(10, frac.ToString().Length - 1); // a power of 10 that moves the fractional part into a whole number
            var b = new UnsafeBigRational((long)(frac * scale), // multiply frac by scale to get the fractional part as a whole number
                (long)scale) // then divide the scale back out to turn it back into a fraction... but this time, we're in a rational, so we don't lose info
                + wholeRat; // Finally, add the whole portion back

            var gcd = BigInteger.GreatestCommonDivisor(b.Numerator, b.Denominator); // then reduce the resultant fraction
            this.Numerator = b.Numerator / gcd;
            this.Denominator = b.Denominator / gcd;
        }

        public BigRational(BigInteger num, BigInteger denom)
        {
            var gcd = BigInteger.GreatestCommonDivisor(num, denom);
            this.Numerator = num / gcd;
            this.Denominator = denom / gcd;
        }
    }
}
