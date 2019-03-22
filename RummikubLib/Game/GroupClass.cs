using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RummikubLib.Game
{
    class GroupClass : IScoringSetClass
    {
        readonly ITileClass[] tiles;

        public GroupClass(IReadOnlyCollection<TileColor> colors, int value)
        {
            if (colors.Count < 3 || colors.Count > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(colors));
            }

            if (value < 1 || value > 13)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (colors.Any(x => x == TileColor.None))
            {
                throw new ArgumentOutOfRangeException(nameof(colors));
            }

            var distinctColors = colors.Distinct().ToArray();

            if (distinctColors.Length != colors.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(colors));
            }

            tiles = distinctColors
                .Select(color => TileClass.CreateNumberedTileClass(color, value))
                .ToArray();
        }

        public int Count => tiles.Length;

        public ScoringSetType Type => ScoringSetType.Group;

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