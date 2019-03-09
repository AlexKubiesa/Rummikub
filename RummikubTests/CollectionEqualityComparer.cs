using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace RummikubTests
{
    public class CollectionEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var tally = new CollectionTally(new NUnitEqualityComparer(), x);
            tally.TryRemove(y);
            var tallyResult = tally.Result;
            return (tallyResult.ExtraItems.Count == 0) && (tallyResult.MissingItems.Count == 0);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            return obj.Sum(x => x.GetHashCode());
        }
    }
}