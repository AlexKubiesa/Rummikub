using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RummikubLib.Game
{
    public class ScoringSet : IScoringSet
    {
        public static IScoringSet CreateOrDefault(IReadOnlyCollection<ITile> tiles)
        {
            if (tiles == null)
            {
                throw new ArgumentNullException(nameof(tiles));
            }

            if (tiles.Count < 3 || tiles.Any(t => t.IsJoker))
            {
                return null;
            }

            if (ContainsEquivalentTiles(tiles))
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

        public static IScoringSet Create(IReadOnlyCollection<ITile> tiles)
        {
            var scoringSet = CreateOrDefault(tiles);

            if (scoringSet == null)
            {
                throw new ArgumentOutOfRangeException(nameof(tiles), "The tiles do not form a valid scoring set.");
            }

            return scoringSet;
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
                        .Distinct(TileValueEqualityComparer.Instance)
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
                        .Distinct(TileValueEqualityComparer.Instance)
                        .OrderBy(t => t.Value)
                        .ToArray();

                    int tileIndex =
                        tilesOfSameColor.FindIndex(t => TileValueEqualityComparer.Instance.Equals(t, tile));
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

        public static IEnumerable<IScoringSet> GetScoringSetsUpToEquivalence(IReadOnlyCollection<ITile> tiles)
        {
            return GetMaximalScoringSetsUpToEquivalence(tiles).SelectMany(GetScoringSetsUpToEquivalence);
        }

        static IEnumerable<IScoringSet> GetScoringSetsUpToEquivalence(IScoringSet scoringSet)
        {
            switch (scoringSet.Type)
            {
                case ScoringSetType.Group:
                    return GetContainedGroups(scoringSet);
                case ScoringSetType.Run:
                    return GetContainedRuns(scoringSet);
                default:
                    throw new InvalidEnumArgumentException("Invalid scoring set type.");
            }
        }

        static IEnumerable<IScoringSet> GetContainedGroups(IScoringSet group)
        {
            yield return group;

            if (group.Tiles.Count == 3)
            {
                yield break;
            }

            var tiles = group.Tiles.ToArray();
            yield return new ScoringSet(new[] { tiles[1], tiles[2], tiles[3] }, ScoringSetType.Group );
            yield return new ScoringSet(new[] { tiles[0], tiles[2], tiles[3] }, ScoringSetType.Group );
            yield return new ScoringSet(new[] { tiles[0], tiles[1], tiles[3] }, ScoringSetType.Group );
            yield return new ScoringSet(new[] { tiles[0], tiles[1], tiles[2] }, ScoringSetType.Group );
        }

        static IEnumerable<IScoringSet> GetContainedRuns(IScoringSet run)
        {
            var tiles = run.Tiles.ToArray();
            for (int runLength = 3; runLength <= tiles.Length; ++runLength)
            {
                for (int startIndex = 0; startIndex <= tiles.Length - runLength; ++startIndex)
                {
                    var containedRun = new ITile[runLength];
                    Array.Copy(tiles, startIndex, containedRun, 0, runLength);
                    yield return new ScoringSet(containedRun, ScoringSetType.Run);
                }
            }
        }

        static bool ContainsEquivalentTiles(IEnumerable<ITile> tiles)
        {
            ITile lastTile = null;
            foreach (var tile in tiles.OrderBy(t => t.Value).ThenBy(t => t.Color))
            {
                if (TileValueEqualityComparer.Instance.Equals(tile, lastTile))
                {
                    return true;
                }

                lastTile = tile;
            }

            return false;
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
