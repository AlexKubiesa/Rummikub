using System.Collections.Generic;

namespace RummikubLib.Collections
{
    public interface IReadOnlyMultiset<T> : IReadOnlyCollection<T>
    {
        bool Contains(T item);

        int CountOf(T item);

        bool IsSubsetOf(IMultiset<T> other);

        bool IsSupersetOf(IMultiset<T> other);
    }
}