using System;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib.Collections
{
    public class Multiset<T> : IMultiset<T>
    {
        readonly Dictionary<T, int> dict;

        public Multiset()
        {
            dict = new Dictionary<T, int>();
        }

        public Multiset(IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            dict = new Dictionary<T, int>();

            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        public Multiset(IEnumerable<KeyValuePair<T, int>> elementCounts)
        {
            if (elementCounts == null)
            {
                throw new ArgumentNullException(nameof(elementCounts));
            }

            dict = new Dictionary<T, int>();

            foreach (var kvp in elementCounts)
            {
                AddItem(kvp.Key, kvp.Value);
            }
        }

        public int DistinctCount => dict.Count(x => x.Value != 0);

        public int CountWithMultiplicity { get; private set; }

        public bool Contains(T item)
        {
            return CountOf(item) != 0;
        }

        public int CountOf(T item)
        {
            return dict.TryGetValue(item, out int value) ? value : 0;
        }

        public bool SetEquals(IReadOnlyMultiset<T> other)
        {
            return GetDistinctElements().Union(other.GetDistinctElements()).All(x => CountOf(x) == other.CountOf(x));
        }

        public bool IsSubsetOf(IReadOnlyMultiset<T> other)
        {
            return GetDistinctElements().All(x => CountOf(x) <= other.CountOf(x));
        }

        public bool IsSupersetOf(IReadOnlyMultiset<T> other)
        {
            return other.GetDistinctElements().All(x => CountOf(x) >= other.CountOf(x));
        }

        public void UnionWith(IMultiset<T> other)
        {
            foreach (var item in other.GetDistinctElements())
            {
                AddItem(item, other.CountOf(item));
            }
        }

        public void IntersectWith(IMultiset<T> other)
        {
            foreach (var item in other.GetDistinctElements())
            {
                RemoveItem(item, other.CountOf(item));
            }
        }

        public IEnumerable<T> GetDistinctElements()
        {
            return dict.Where(x => x.Value != 0).Select(x => x.Key);
        }

        public IEnumerable<T> GetElementsWithMultiplicity()
        {
            return dict.Where(x => x.Value != 0).SelectMany(x => Enumerable.Repeat(x.Key, x.Value));
        }

        public void AddOne(T item)
        {
            AddItem(item);
        }

        public void AddMany(T item, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            AddItem(item, count);
        }

        void AddItem(T item, int count = 1)
        {
            CountWithMultiplicity += count;
            dict[item] = dict.TryGetValue(item, out int oldCount) ? oldCount + count : count;
        }

        void RemoveItem(T item, int multiplicity)
        {
            if (!dict.TryGetValue(item, out int count))
            {
                return;
            }

            if (multiplicity >= count)
            {
                dict.Remove(item);
            }
            else
            {
                dict[item] = count - multiplicity;
            }
        }
    }
}