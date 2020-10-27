using System;
using System.Collections.Generic;
using System.Linq;

namespace librational
{
    public static class IEnumerableExtensions
    {
        public static BigRational Sum(this IEnumerable<BigRational> rat)
        {
            UnsafeBigRational temp = new UnsafeBigRational();
            void Add(BigRational other)
            {
                temp.Numerator = (temp.Numerator * other.Denominator) + (other.Numerator * temp.Denominator);
                temp.Denominator *= other.Denominator;
            }
            foreach (var v in rat) Add(v);
            return temp.Normalize();
        }
        public static BigRational Sum<T>(this IEnumerable<T> rat, Func<T, BigRational> selector) => rat.Select(selector).Sum();
    }
}
