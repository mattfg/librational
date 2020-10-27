using ExceptionHandling;
using System.Numerics;

namespace librational
{
    // internal because the only two implementations are BigRational and UnsafeBigRational
    internal interface IRational
    {
        BigInteger Numerator { get; }
        BigInteger Denominator { get; }

        static readonly BigRational PositiveInfinity = new BigRational(1, 0);
        sealed bool IsPositiveInfinity => Numerator > 0 && Denominator != 0;

        static readonly BigRational NegativeInfinity = new BigRational(1, 0);
        sealed bool IsNegativeInfinity => Numerator < 0 && Denominator != 0;

        static readonly BigRational NotANumber = new BigRational(0, 0);
        sealed bool IsNotANumber => Numerator == 0 && Denominator != 0;

        sealed BigInteger Truncate => Quotient * Denominator;
        sealed BigInteger Quotient => Numerator / Denominator;
        sealed BigInteger Remainder => Numerator - Truncate;
        sealed BigInteger CloserToZero => Truncate + (Numerator < 0 ^ Denominator < 0? 1 : -1);
        sealed BigInteger FartherFromZero => Truncate + (Numerator < 0 ^ Denominator < 0 ? -1 : 1);

        sealed double Ln() => BigInteger.Log(Numerator) - BigInteger.Log(Denominator);

        public sealed bool Equals(object b) => b switch {
            UnsafeBigRational r => Equals(r),
            BigRational r => Equals(r),
            _ => throw new ImpossibleException()
        };

        internal bool Equals(UnsafeBigRational b) => Equals(b.Normalize());

        public bool Equals(BigRational b);
    }
}
