using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoringSet : IScoringSet
    {
        public static IScoringSet CreateOrDefault(IReadOnlyCollection<ITile> tiles)
        {
            if (tiles.Count < 3 || tiles.Any(t => t.IsJoker))
            {
                return null;
            }

            if (ContainsDuplicates(tiles))
            {
                return null;
            }

            var colors = tiles.Select(t => t.Color).Distinct().ToArray();

            if (colors.Length == 1)
            {
                return IsConsecutiveSetOfIntegers(tiles.Select(t => t.Value))
                    ? new ScoringSet(tiles, ScoringSetType.Run)
                    : null;
            }

            if (colors.Length != tiles.Count)
            {
                return null;
            }

            var values = tiles.Select(t => t.Value).Distinct().ToArray();

            return values.Length == 1
                ? new ScoringSet(tiles, ScoringSetType.Group)
                : null;
        }

        static bool ContainsDuplicates(IEnumerable<ITile> tiles)
        {
            return tiles.GetPairs().Any(pair => TileEqualityComparerByValue.Instance.Equals(pair.Item1, pair.Item2));
        }

        static bool IsConsecutiveSetOfIntegers(IEnumerable<int> integers)
        {
            return integers.OrderBy(x => x).Select((x, i) => x - i).Distinct().Count() <= 1;
        }

        ScoringSet(IReadOnlyCollection<ITile> tiles, ScoringSetType type)
        {
            Tiles = tiles;
            Type = type;
        }

        public IReadOnlyCollection<ITile> Tiles { get; }

        public ScoringSetType Type { get; }

        public int GetScore()
        {
            return Tiles.Sum(x => x.Value);
        }
    }
}
