using System;
using System.Numerics;

namespace librational
{
    public partial struct BigRational: IComparable2<BigRational>, IComparable2<BigInteger>
    {
        public static BigRational operator -(BigRational a) => new BigRational(-a.Numerator, a.Denominator);
        public static BigRational operator +(BigRational a, BigRational b) => new BigRational((b.Denominator * a.Numerator) + (a.Denominator * b.Numerator), (b.Denominator * a.Denominator));
        public static BigRational operator -(BigRational a, BigRational b) => new BigRational((b.Denominator * a.Numerator) - (a.Denominator * b.Numerator), (b.Denominator * a.Denominator));
        public static BigRational operator *(BigRational a, BigRational b) => new BigRational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        public static BigRational operator /(BigRational a, BigRational b) => new BigRational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        public static bool operator ==(BigRational a, BigRational b) => a.CompareTo(b) == 0;
        public static bool operator !=(BigRational a, BigRational b) => a.CompareTo(b) != 0;

        public static implicit operator BigRational(int i) => new BigRational(i);
        public static implicit operator BigRational(long i) => new BigRational(i);

        public int CompareTo(BigRational b)
        {
            // this is inefficient but I'm not great at math so instead of figuring out how to normalize these
            // denominators efficiently, I'm letting the constructor do the work
            var left = new BigRational(Numerator * b.Denominator, Denominator * b.Denominator).Numerator;
            var right = new BigRational(b.Numerator * Denominator, Denominator * b.Denominator).Numerator;
            return left.CompareTo(right);
        }

        /* Numerator / Denominator gives the quotient
         * the quotient times the denominator gives back not the original value (the remainder is lost)
         * but the truncated value. For some reason this partial class can see the Numerator and Denominator
         * from here but not the Truncate property; I guess because Numerator and Denominator are
         * overridden in BigRational but Truncate is on IRational (and sealed).
         */
        public int CompareTo(BigInteger b) => (Numerator / Denominator * Denominator).CompareTo(b);
    }

    public partial struct UnsafeBigRational: IComparable2<UnsafeBigRational>
    {
        public static UnsafeBigRational operator -(UnsafeBigRational a) => new UnsafeBigRational(-a.Numerator, a.Denominator);
        public static UnsafeBigRational operator +(UnsafeBigRational a, UnsafeBigRational b) => new UnsafeBigRational((b.Denominator * a.Numerator) + (a.Denominator * b.Numerator), (b.Denominator * a.Denominator));
        public static UnsafeBigRational operator -(UnsafeBigRational a, UnsafeBigRational b) => new UnsafeBigRational((b.Denominator * a.Numerator) - (a.Denominator * b.Numerator), (b.Denominator * a.Denominator));
        public static UnsafeBigRational operator *(UnsafeBigRational a, UnsafeBigRational b) => new UnsafeBigRational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        public static UnsafeBigRational operator /(UnsafeBigRational a, UnsafeBigRational b) => new UnsafeBigRational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        public static bool operator ==(UnsafeBigRational a, UnsafeBigRational b) => a.CompareTo(b) == 0;
        public static bool operator !=(UnsafeBigRational a, UnsafeBigRational b) => a.CompareTo(b) != 0;

        public int CompareTo(UnsafeBigRational b)
        {
            // this is inefficient but I'm not great at math so instead of figuring out how to normalize these
            // denominators efficiently, I'm letting the constructor do the work
            var left = new BigRational(Numerator * b.Denominator, Denominator * b.Denominator).Numerator;
            var right = new BigRational(b.Numerator * Denominator, Denominator * b.Denominator).Numerator;
            return left.CompareTo(right);
        }
    }
}
