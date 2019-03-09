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

        public static IEnumerable<IScoringSet> GetScoringSets(IReadOnlyCollection<ITile> tiles)
        {
            return tiles.GetSublists().Select(CreateOrDefault).Where(x => x != null);
        }

        public static IEnumerable<IScoringSet> GetScoringSetsUpToEquivalence(IReadOnlyCollection<ITile> tiles)
        {
            return tiles.Distinct(TileEqualityComparerByValue.Instance).GetSublists().Select(CreateOrDefault).Where(x => x != null);
        }

        public static IEnumerable<IScoringSet> GetMaximalScoringSetsUpToEquivalence(IReadOnlyCollection<ITile> tiles)
        {
            var maximalGroups = new List<IScoringSet>();
            var maximalRuns = new List<IScoringSet>();

            foreach (var tile in tiles.Where(t => !t.IsJoker))
            {
                if (!maximalGroups.Any(x => x.Tiles.Contains(tile)))
                {
                    var potentialGroup = tiles
                        .Where(t => t.Value == tile.Value)
                        .Distinct(TileEqualityComparerByValue.Instance)
                        .ToArray();

                    if (potentialGroup.Length >= 3)
                    {
                        maximalGroups.Add(new ScoringSet(potentialGroup, ScoringSetType.Group));
                    }
                }

                if (!maximalRuns.Any(x => x.Tiles.Contains(tile)))
                {
                    var tilesOfSameColor = tiles
                        .Where(t => t.Color == tile.Color)
                        .Distinct(TileEqualityComparerByValue.Instance)
                        .OrderBy(t => t.Value)
                        .ToArray();

                    int tileIndex = 
                        tilesOfSameColor.FindIndex(t => TileEqualityComparerByValue.Instance.Equals(t, tile));
                    int potentialRunFirstIndex =
                        tilesOfSameColor.FindIndex((t, i) => t.Value - i == tile.Value - tileIndex);
                    int indexAfterPotentialRun =
                        tilesOfSameColor.FindIndex((t, i) => t.Value - i > tile.Value - tileIndex);
                    int potentialRunLastIndex = indexAfterPotentialRun == -1
                        ? tilesOfSameColor.Length - 1
                        : indexAfterPotentialRun - 1;
                    int potentialRunLength = potentialRunLastIndex - potentialRunFirstIndex + 1;

                    if (potentialRunLength >= 3)
                    {
                        var run = tilesOfSameColor
                            .Skip(potentialRunFirstIndex)
                            .Take(potentialRunLength)
                            .ToArray();

                        maximalRuns.Add(new ScoringSet(run, ScoringSetType.Run));
                    }
                }
            }

            return maximalGroups.Union(maximalRuns);
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
