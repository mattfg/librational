using System;
using System.Numerics;

namespace librational
{
    // this class is a mutable rational that is not necessarily normalized.
    // it exists to allow you to perform intermediate operations on a
    // rational without creating a lot of temporaries and performing gcd calculations
    public partial struct UnsafeBigRational: IRational
    {
        public BigInteger Numerator { get; set; }
        public BigInteger Denominator { get; set; }

        public override bool Equals(object obj) => base.Equals(obj);
        public bool Equals(BigRational b) => Normalize().Equals(b);
        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
        public override string ToString() => $"{Numerator}/{Denominator}";

        public UnsafeBigRational(long num = 0, long denom = 1):
            this(new BigInteger(num), new BigInteger(denom))
        {
        }

        public UnsafeBigRational(BigInteger num, BigInteger denom)
        {
            this.Numerator = num;
            this.Denominator = denom;
        }

        public BigRational Normalize()
        {
            var result = new BigRational(Numerator, Denominator);
            this.Numerator = result.Numerator;
            this.Denominator = result.Denominator;
            return result;
        }
    }
}
