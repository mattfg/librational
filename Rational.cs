using System;
using System.Collections.Generic;
using System.Numerics;

namespace librational
{
    // wanted this to be a struct but for inheritance reasons it had to be an interface. but then because interfaces can't override ==, it had to be an abstract class
    public abstract partial class Rational
    {
        public BigInteger Numerator => _numerator;
        public BigInteger Denominator => _denominator;
        protected BigInteger _numerator;
        protected BigInteger _denominator;

        protected Rational(BigInteger n, BigInteger d)
        {
            _numerator = n;
            _denominator = d;
        }

        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);
        public override string ToString() => $"{_numerator}/{_denominator}";

        public Rational Reciprocal => FromPair(_denominator, _numerator);
        public BigInteger Truncate => Quotient * _denominator;
        public BigInteger Quotient => _numerator / _denominator;
        public Rational Remainder => FromPair(_numerator - Truncate, _denominator);
        public BigInteger CloserToZero => Truncate;
        public BigInteger FartherFromZero => CloserToZero + (IsNegative ? -1 : 1);

        public bool IsNegative => _numerator < 0 != _denominator < 0;

        public static Rational FromDecimal(decimal value)
        {
            // this is a dumb way to do it but it's easy
            var strep = value.ToString();
            var parts = strep.Split('.');
            return FromPair(BigInteger.Parse(parts[0] + parts[1]), BigInteger.Pow(new BigInteger(10), parts[1].Length));
        }

        public static Rational FromDouble(double value)
        {
            // based on https://github.com/microsoftarchive/bcl/blob/master/Libraries/BigRational/BigRationalLibrary/BigRational.cs
            if (double.IsNaN(value)) return HyperRational.NotANumber;
            else if (double.IsPositiveInfinity(value)) return HyperRational.PositiveInfinity;
            else if (double.IsNegativeInfinity(value)) return HyperRational.NegativeInfinity;
            else {
                (int exponent, long mantissa) GetDoubleParts(double value)
                {
                    // https://stackoverflow.com/questions/389993/extracting-mantissa-and-exponent-from-double-in-c-sharp
                    var bits = BitConverter.DoubleToInt64Bits(value);
                    bool negative = (bits & (1L << 63)) != 0;
                    int exponent = (int)((bits >> 52) & 0x7ffL);
                    long mantissa = bits & 0xf_ffff_ffff_ffffL; // 13 rightmost bits

                    // not necessary to normalize since this is just an intermediate value
                    return (negative? -exponent : exponent, mantissa);
                }

                BigInteger numerator, denominator;
                var (exponent, mantissa) = GetDoubleParts(value);
                if (exponent >= 0) {
                    numerator = BigInteger.Pow(mantissa, exponent);
                    denominator = 1;
                } else /* (exponent < 0) */ {
                    numerator = 1;
                    denominator = BigInteger.Pow(mantissa, -exponent); //-exponent makes it positive, since we already know it's <0
                }
                return NormalizedRational.FromPair(numerator, denominator);
            }
        }
        public static Rational FromPair(long num = 0, long denom = 1) => FromPair(new BigInteger(num), new BigInteger(denom));
        public static Rational FromPair(BigInteger num) => FromPair(num, BigInteger.One);
        public static Rational FromPair(BigInteger num, BigInteger denom)
        {
            if (denom.IsZero) return HyperRational.Get(num);
            else return NormalizedRational.FromPair(num, denom);
        }

        // maybe instead of using a/b as my representation I should have designed this library around representing natively in IEnumerable<BigInteger>...
        // seeya in version 3 maybe
        public IEnumerable<BigInteger> ToContinuedFraction {
            get {
                var current = this;
                while (current.Denominator != 1) {
                    yield return current.Truncate;
                    current = current.Remainder.Reciprocal;
                }
            }
        }

        public Logarithmed Ln() => new Logarithmed {
            
        };

        public override bool Equals(object b) => b switch {
            UnsafeBigRational r => Equals(r),
            NormalizedRational r => Equals(r),
            _ => false // hyperrationals never compare as equal to anything, even themselves
        };

        internal bool Equals(UnsafeBigRational b) => Equals(b.Normalize());
    }
}
