using System;
using System.Collections.Generic;
using System.Linq;

namespace Bard.Utils
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TSource[]> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;
                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count).ToArray();
        }

                public static IEnumerable<Tuple<T, T>> UnorderedPairs<T>(this IEnumerable<T> ts)
        {
            var ary = ts.ToArray();
            for (int i = 0; i < ary.Length; i++)
                for (int j = 0; j < i; j++)
                    yield return Tuple.Create(ary[i], ary[j]);
        }

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> ts)
        {
            return
                from t1 in ts
                from t2 in ts
                select Tuple.Create(t1, t2);
        }

        public static IEnumerable<Tuple<T, T, T>> Triplets<T>(this IEnumerable<T> ts)
        {
            return
                from t1 in ts
                from t2 in ts
                from t3 in ts
                select Tuple.Create(t1, t2, t3);
        }

        public static IEnumerable<U> ConsecutivePairs<T, U>(this IEnumerable<T> ts, Func<T, T, U> reduce)
        {
            T last = default;
            bool hasLast = false;

            foreach (var t in ts)
            {
                if (hasLast)
                    yield return reduce(last, t);
                else
                    hasLast = true;

                last = t;
            }
        }


        public static void Populate<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static IEnumerable<T[]> Pad<T>(this T[] phonemes, T value, int length)
        {
            int diff = length - phonemes.Length;
            for (int i = 0; i <= diff; i++)
            {
                var before = new T[i];
                var after = new T[diff - i];

                before.Populate(value);
                after.Populate(value);

                yield return before.Concat(phonemes).Concat(after).ToArray();
            }
        }

        public static string[] SplitPhonemes(this string phonemesStr)
        {
            return phonemesStr.Select(c => c.ToString()).ToArray();
        }
    }
}
