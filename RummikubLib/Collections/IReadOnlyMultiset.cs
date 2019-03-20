using System.Collections.Generic;

namespace RummikubLib.Collections
{
    public interface IReadOnlyMultiset<T> : IReadOnlyCollection<T>
    {
        bool Contains(T item);

        int CountOf(T item);

        bool SetEquals(IReadOnlyMultiset<T> other);

        bool IsSubsetOf(IReadOnlyMultiset<T> other);

        bool IsSupersetOf(IReadOnlyMultiset<T> other);
    }
}