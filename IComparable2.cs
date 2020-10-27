using System;

namespace librational
{
    public interface IComparable2<T>: IComparable<T>
    {
        public static bool operator >(IComparable2<T> a, T b) => a.CompareTo(b) > 0;
        public static bool operator <=(IComparable2<T> a, T b) => a.CompareTo(b) <= 0;
        public static bool operator >=(IComparable2<T> a, T b) => a.CompareTo(b) >= 0;
        public static bool operator <(IComparable2<T> a, T b) => a.CompareTo(b) < 0;
    }
}
