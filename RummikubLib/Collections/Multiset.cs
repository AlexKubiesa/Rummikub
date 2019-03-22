﻿using System;
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

        public int TotalCount { get; private set; }

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
                SetItemCount(item, Math.Max(CountOf(item), other.CountOf(item)));
            }
        }

        public void IntersectWith(IMultiset<T> other)
        {
            foreach (var item in other.GetDistinctElements())
            {
                SetItemCount(item, Math.Min(CountOf(item), other.CountOf(item)));
            }
        }

        public void SumWith(IMultiset<T> other)
        {
            foreach (var item in other.GetDistinctElements())
            {
                AddItem(item, other.CountOf(item));
            }
        }

        public void ExceptWith(IMultiset<T> other)
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

        public void RemoveAll(T item)
        {
            RemoveItem(item, CountOf(item));
        }

        void AddItem(T item, int count = 1)
        {
            TotalCount += count;
            dict[item] = dict.TryGetValue(item, out int oldCount) ? oldCount + count : count;
        }

        void RemoveItem(T item, int count)
        {
            if (!dict.TryGetValue(item, out int oldCount))
            {
                return;
            }

            if (count >= oldCount)
            {
                TotalCount -= oldCount;
                dict.Remove(item);
            }
            else
            {
                TotalCount -= count;
                dict[item] = oldCount - count;
            }
        }

        void SetItemCount(T item, int count)
        {
            if (!dict.TryGetValue(item, out int oldCount))
            {
                if (count == 0)
                {
                    return;
                }

                TotalCount += count;
                dict[item] = count;
                return;
            }

            TotalCount += count - oldCount;

            if (count == 0)
            {
                dict.Remove(item);
            }
        }
    }
}