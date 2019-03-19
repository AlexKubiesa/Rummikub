using System.Collections.Generic;

namespace RummikubLib.Game
{
    public class TileValueEqualityComparer : IEqualityComparer<ITile>
    {
        public static IEqualityComparer<ITile> Instance { get; } = new TileValueEqualityComparer();

        TileValueEqualityComparer()
        {
        }

        public bool Equals(ITile x, ITile y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.IsJoker && y.IsJoker)
            {
                return true;
            }

            if (x.IsJoker || y.IsJoker)
            {
                return false;
            }

            return x.Color == y.Color && x.Value == y.Value;
        }

        public int GetHashCode(ITile obj)
        {
            if (obj.IsJoker)
            {
                return 1;
            }

            unchecked
            {
                int hashCode = (int)obj.Color;
                hashCode = (hashCode * 397) ^ obj.Value;
                return hashCode;
            }
        }
    }
}