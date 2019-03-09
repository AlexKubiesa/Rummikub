using System;
using System.Collections.Generic;

namespace RummikubLib
{
    public static class CollectionsHelper
    {
        public static int FindIndex<T>(this IReadOnlyList<T> source, Func<T, bool> predicate)
        {
            for (int i = 0; i < source.Count; ++i)
            {
                if (predicate(source[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindIndex<T>(this IReadOnlyList<T> source, Func<T, int, bool> predicate)
        {
            for (int i = 0; i < source.Count; ++i)
            {
                if (predicate(source[i], i))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}