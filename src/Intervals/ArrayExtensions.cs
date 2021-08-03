using System;
using System.Collections.Generic;
using System.Text;

namespace Intervals
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(this T[] data, int index)
        {
            int length = data.Length - index;
            return data.SubArray(index, length);
        }

        public static T[] SubArray<T>(this T[] data, IInterval interval)
        {
            var i = interval.ToInterval();
            return data.SubArray(i.Start, i.Length);
        }
    }
}
