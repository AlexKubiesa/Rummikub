using System;
using System.Collections;
using System.Collections.Generic;

namespace RummikubLib.Collections
{
    public class SubMultisets<T> : IEnumerable<IMultiset<T>>
    {
        readonly IReadOnlyMultiset<T> multiset;

        public SubMultisets(IReadOnlyMultiset<T> multiset)
        {
            this.multiset = multiset ?? throw new ArgumentNullException(nameof(multiset));
        }

        public IEnumerator<IMultiset<T>> GetEnumerator()
        {
            return new SubMultisetsEnumerator<T>(multiset);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}