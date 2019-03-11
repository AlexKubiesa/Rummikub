using System;
using System.Collections;
using System.Collections.Generic;

namespace RummikubLib.Collections
{
    class Sublists<T> : IEnumerable<List<T>>
    {
        readonly IReadOnlyList<T> list;

        public Sublists(IReadOnlyList<T> list)
        {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public IEnumerator<List<T>> GetEnumerator()
        {
            return new SublistsEnumerator<T>(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}