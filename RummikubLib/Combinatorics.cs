using System;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib
{
    public static class Combinatorics
    {
        public static IEnumerable<IReadOnlyCollection<T>> GetSubsets<T>(this IEnumerable<T> source)
        {
            if (!source.Any())
            {
                yield return new T[0];
                yield break;
            }

            var first = source.First();

            foreach (var subset in source.Skip(1).GetSubsets())
            {
                yield return subset;

                var subsetPlusFirst = new T[subset.Count + 1];
                subsetPlusFirst[0] = first;

                int i = 1;
                foreach (var element in subset)
                {
                    subsetPlusFirst[i] = element;
                    ++i;
                }

                yield return subsetPlusFirst;
            }
        }

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

        /// <summary>
        /// Returns a shuffled list where the index of each element is uniformly distributed.
        /// </summary>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var random = new Random();
            var freeIndices = Enumerable.Range(0, list.Count).ToList();
            var shuffledList = Enumerable.Repeat(default(T), list.Count).ToList();
            
            foreach (var item in list)
            {
                int newIndexSeed = random.Next(0, freeIndices.Count);
                int newIndex = freeIndices[newIndexSeed];
                shuffledList[newIndex] = item;
                freeIndices.RemoveAt(newIndexSeed);
            }

            return shuffledList;
        }
    }
}
