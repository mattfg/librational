using System;
using System.Collections.Generic;
using System.Linq;

namespace librational
{
    public static class IEnumerableExtensions
    {
        /** aggregate more performantly by using an UnsafeBigRational as the intermediate value */
        public static Rational Aggregate<T, TAccumulate, TResult>(this IEnumerable<T> rat,
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
            where T: Rational
            where TAccumulate : Rational
            where TResult: Rational
        {
            UnsafeBigRational temp = UnsafeBigRational.FromPair(seed.Numerator, seed.Denominator);
            return rat.Aggregate<T, UnsafeBigRational, TResult>(
                temp,
                (tIn, tCarry) => func(UnsafeBigRational.FromPair(tIn.Numerator, tIn.Denominator), tCarry),
                tResult => resultSelector(tResult.Normalize()));
        }
    }
}
