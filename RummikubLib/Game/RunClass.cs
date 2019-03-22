using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib.Game
{
    class RunClass : IScoringSetClass
    {
        readonly ITileClass[] tiles;

        public RunClass(TileColor color, int startValue, int endValue)
        {
            if (color == TileColor.None)
            {
                throw new ArgumentOutOfRangeException(nameof(color));
            }

            if (startValue < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(startValue));
            }

            if (endValue > 13)
            {
                throw new ArgumentOutOfRangeException(nameof(endValue));
            }

            if (startValue > endValue)
            {
                throw new ArgumentException("Start value cannot be greater than end value.");
            }

            int length = endValue - startValue + 1;

            if (length < 3)
            {
                throw new ArgumentException("A run must be of length at least 3.");
            }

            tiles = Enumerable.Range(startValue, length)
                .Select(value => TileClass.CreateNumberedTileClass(color, value))
                .ToArray();
        }

        public int Count => tiles.Length;

        public ScoringSetType Type => ScoringSetType.Run;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ITileClass> GetEnumerator()
        {
            return ((IEnumerable<ITileClass>)tiles).GetEnumerator();
        }

        public int GetScore()
        {
            return tiles.Sum(x => x.Value);
        }
    }
}