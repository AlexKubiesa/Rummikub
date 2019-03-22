﻿using System.Collections.Generic;

namespace RummikubLib.Game
{
    public class TileEqualityComparerByValue : IEqualityComparer<ITile>
    {
        public static IEqualityComparer<ITile> Instance { get; } = new TileEqualityComparerByValue();

        TileEqualityComparerByValue()
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

            return x.Color == y.Color && x.Value == y.Value;
        }

        public int GetHashCode(ITile obj)
        {
            unchecked
            {
                int hashCode = (int)obj.Color;
                hashCode = (hashCode * 397) ^ obj.Value;
                return hashCode;
            }
        }
    }
}