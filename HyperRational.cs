using System;
using System.Numerics;

namespace librational
{
    public abstract partial class Rational
    {
        bool IsPositiveInfinity => this == HyperRational.PositiveInfinity || _numerator > 0 && _denominator == 0;
        bool IsNegativeInfinity => this == HyperRational.NegativeInfinity || _numerator < 0 && _denominator == 0;
        bool IsNotANumber => this == HyperRational.NotANumber || _numerator == 0 && _denominator == 0;

        private class HyperRational : Rational
        {
            internal static readonly Rational PositiveInfinity = new HyperRational(1, 0); // need a new class because BigRational's ctor does a divide-by-0 if denom is 0
            internal static readonly Rational NegativeInfinity = new HyperRational(-1, 0);
            internal static readonly Rational NotANumber = new HyperRational(0, 0);

            private HyperRational(BigInteger n, BigInteger d): base(n, d)
            {
            }

            internal static Rational Get(BigInteger n)
            {
                if (n.Sign < 0) return NegativeInfinity;
                else if (n.Sign == 0) return NotANumber;
                else if (n.Sign > 0) return PositiveInfinity;
                else throw new Exception("And you may ask yourself, well, how did I get here?");
            }
        }
    }
}
