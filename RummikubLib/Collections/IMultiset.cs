using System.Collections.Generic;

namespace RummikubLib.Collections
{
    public interface IMultiset<T> : IReadOnlyCollection<T>
    {
        int CountOf(T item);

        bool IsSubsetOf(IMultiset<T> other);

        bool IsSupersetOf(IMultiset<T> other);

        void UnionWith(IMultiset<T> other);

        void IntersectWith(IMultiset<T> other);
    }
}