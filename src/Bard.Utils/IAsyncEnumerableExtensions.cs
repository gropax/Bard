using System;
using System.Collections.Generic;
using System.Linq;

namespace Bard.Utils
{
    public static class IAsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<TSource[]> Batch<TSource>(
            this IAsyncEnumerable<TSource> source,
            int size)
        {
            TSource[] bucket = null;
            var count = 0;

            await foreach (var item in source)
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
    }
}
