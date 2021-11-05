using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace librational
{
    // this class is a mutable rational that is not necessarily normalized.
    // it exists to allow you to perform intermediate operations on a
    // rational without creating a lot of temporaries and performing gcd calculations
    internal partial class UnsafeBigRational: Rational
    {
        private NormalizedRational _lastNormalizedValue;

        new public BigInteger Numerator {
            get => base.Numerator;
            set { _numerator = value; _lastNormalizedValue = null; }
        }
        new public BigInteger Denominator {
            get => base.Denominator;
            set { _denominator = value; _lastNormalizedValue = null; }
        }

        new public static UnsafeBigRational FromPair(BigInteger num, BigInteger denom) => new UnsafeBigRational(num, denom);

        private UnsafeBigRational(long num = 0, long denom = 1):
            this(new BigInteger(num), new BigInteger(denom))
        {
        }

        private UnsafeBigRational(BigInteger num, BigInteger denom): base(num, denom)
        {
            _lastNormalizedValue = null;
        }

        internal NormalizedRational Normalize()
        {
            if (_lastNormalizedValue != null) return _lastNormalizedValue;
            else {
                var result = NormalizedRational.FromPair(Numerator, Denominator);
                this._numerator = result.Numerator;
                this._denominator = result.Denominator;
                return result;
            }
        }
    }
}
