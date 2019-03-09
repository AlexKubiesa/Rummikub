using System;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib
{
    public static class Combinatorics
    {
        public static IEnumerable<Tuple<T, T>> GetPairs<T>(this IEnumerable<T> source)
        {
            return source.GetSublistsOfSize(2).Select(list => Tuple.Create(list[0], list[1]));
        }

        public static IEnumerable<Tuple<T, T, T>> GetTriples<T>(this IEnumerable<T> source)
        {
            return source.GetSublistsOfSize(3).Select(list => Tuple.Create(list[0], list[1], list[2]));
        }

        public static IEnumerable<List<T>> GetSublistsOfSize<T>(this IEnumerable<T> source, int size)
        {
            if (size < 0)
            {
                yield break;
            }

            if (size == 0)
            {
                yield return new List<T>();
                yield break;
            }

            var sourceStack = new Stack<T>(source);

            if (sourceStack.Count < size)
            {
                yield break;
            }

            var first = sourceStack.Pop();

            foreach (var subsetWithoutFirstElement in sourceStack.GetSublistsOfSize(size - 1))
            {
                var subsetWithFirstElement = new List<T>(subsetWithoutFirstElement) { first };
                yield return subsetWithFirstElement;
            }

            foreach (var subsetWithoutFirstElement in sourceStack.GetSublistsOfSize(size))
            {
                yield return subsetWithoutFirstElement;
            }
        }

        public static IEnumerable<List<T>> GetSublists<T>(this IEnumerable<T> source)
        {
            var sourceStack = new Stack<T>(source);

            if (sourceStack.Count == 0)
            {
                yield return new List<T>();
                yield break;
            }

            var first = sourceStack.Pop();

            foreach (var subsetWithoutFirstElement in sourceStack.GetSublists())
            {
                var subsetWithFirstElement = new List<T>(subsetWithoutFirstElement) {first};
                yield return subsetWithoutFirstElement;
                yield return subsetWithFirstElement;
            }
        }
    }
}
