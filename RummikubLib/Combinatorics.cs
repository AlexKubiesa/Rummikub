using System;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib
{
    public static class Combinatorics
    {
        public static IEnumerable<Tuple<T, T>> GetPairs<T>(this IEnumerable<T> source)
        {
            if (!source.Any())
            {
                yield break;
            }

            var first = source.First();

            foreach (var element in source.Skip(1))
            {
                yield return Tuple.Create(first, element);
            }

            foreach (var pair in source.Skip(1).GetPairs())
            {
                yield return pair;
            }
        }

        public static IEnumerable<List<T>> GetCombinations<T>(this IEnumerable<T> source, int size)
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

            if (source.Count() < size)
            {
                yield break;
            }

            var first = source.First();

            foreach (var combinationWithFirstElement in source.Skip(1).GetCombinations(size - 1))
            {
                combinationWithFirstElement.Add(first);
                yield return combinationWithFirstElement;
            }

            foreach (var combinationWithoutFirstElement in source.Skip(1).GetCombinations(size))
            {
                yield return combinationWithoutFirstElement;
            }
        }

        public static IEnumerable<List<T>> GetSubsets<T>(this IEnumerable<T> source)
        {
            var sourceStack = new Stack<T>(source);

            yield return new List<T>();

            if (sourceStack.Count == 0)
            {
                yield break;
            }

            var first = sourceStack.Pop();

            foreach (var subsetWithoutFirstElement in sourceStack.GetSubsets())
            {
                var subsetWithFirstElement = new List<T>(subsetWithoutFirstElement) {first};
                yield return subsetWithoutFirstElement;
                yield return subsetWithFirstElement;
            }
        }
    }
}
