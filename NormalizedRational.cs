using System;
using System.Numerics;

namespace librational
{
    internal class NormalizedRational: Rational
    {
        new static internal NormalizedRational FromPair(BigInteger num, BigInteger denom) => new NormalizedRational(num, denom);

        private NormalizedRational(BigInteger num, BigInteger denom, BigInteger gcd): base(num/gcd, denom/gcd)
        {
        }
        private NormalizedRational(BigInteger num, BigInteger denom) : this(num, denom, BigInteger.GreatestCommonDivisor(num, denom))
        {
        }
    }
}
