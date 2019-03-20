namespace RummikubLib.Collections
{
    public interface IMultiset<T> : IReadOnlyMultiset<T>
    {
        void UnionWith(IMultiset<T> other);

        void IntersectWith(IMultiset<T> other);
    }
}