using System;
using System.Collections.Generic;
using System.Linq;
using RummikubLib.Collections;

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

        public static IEnumerable<List<T>> GetSublists<T>(this IReadOnlyList<T> source)
        {
            return new Sublists<T>(source);
        }

        public static Multiset<T> ToMultiset<T>(this IEnumerable<T> source)
        {
            return new Multiset<T>(source);
        }

        public static IEnumerable<IReadOnlyMultiset<T>> GetSubMultisets<T>(this IReadOnlyMultiset<T> multiset)
        {
            return new SubMultisets<T>(multiset);
        }

        public static IMultiset<T> Intersect<T>(this IReadOnlyMultiset<T> multiset, IReadOnlyMultiset<T> other)
        {
            var elementCounts = multiset.GetDistinctElements()
                .ToDictionary(x => x, x => Math.Min(multiset.CountOf(x), other.CountOf(x)));
            return new Multiset<T>(elementCounts);
        }
    }
}