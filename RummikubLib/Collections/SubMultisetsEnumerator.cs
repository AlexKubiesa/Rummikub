using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib.Collections
{
    public class SubMultisetsEnumerator<T> : IEnumerator<IMultiset<T>>
    {
        readonly IReadOnlyMultiset<T> multiset;

        readonly KeyValuePair<T, int>[] elementCounts;

        Multiset<T> current;

        public SubMultisetsEnumerator(IReadOnlyMultiset<T> multiset)
        {
            this.multiset = multiset ?? throw new ArgumentNullException(nameof(multiset));
            elementCounts = multiset
                .GetDistinctElements()
                .Select(x => new KeyValuePair<T, int>(x, 0))
                .ToArray();
        }

        object IEnumerator.Current => Current;

        public IMultiset<T> Current
        {
            get
            {
                if (current == null)
                {
                    throw new InvalidOperationException("There is no current element.");
                }

                return current;
            }
        }

        public bool MoveNext()
        {
            int index = elementCounts.FindIndex(x => x.Value < multiset.CountOf(x.Key));

            if (index == -1)
            {
                return false;
            }

            for (int i = 0; i < index; ++i)
            {
                elementCounts[i] = new KeyValuePair<T, int>(elementCounts[i].Key, 0);
            }

            var elementToIncrement = elementCounts[index];
            elementCounts[index] = new KeyValuePair<T, int>(elementToIncrement.Key, elementToIncrement.Value + 1);

            current = new Multiset<T>(elementCounts);
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}