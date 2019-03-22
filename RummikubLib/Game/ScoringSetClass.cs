using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RummikubLib.Collections;

namespace RummikubLib.Game
{
    public static class ScoringSetClass
    {
        public static IScoringSetClass CreateOrDefault(IReadOnlyMultiset<ITileClass> tiles)
        {
            if (tiles == null)
            {
                throw new ArgumentNullException(nameof(tiles));
            }

            if (tiles.Count < 3 || tiles.Contains(TileClass.Joker))
            {
                return null;
            }

            if (tiles.Any(x => tiles.CountOf(x) != 1))
            {
                return null;
            }

            var colors = tiles.Select(t => t.Color).Distinct().ToArray();
            var values = tiles.Select(t => t.Value).OrderBy(x => x).Distinct().ToArray();

            if (colors.Length == 1)
            {
                if (values.Length != tiles.Count)
                {
                    return null;
                }

                int valueRange = values[values.Length - 1] - values[0];

                return valueRange == values.Length - 1
                    ? new RunClass(colors[0], values[0], values[values.Length - 1])
                    : null;
            }

            if (colors.Length != tiles.Count)
            {
                return null;
            }

            return values.Length == 1
                ? new GroupClass(colors, values[0])
                : null;
        }

        public static IScoringSetClass Create(IReadOnlyMultiset<ITileClass> tiles)
        {
            var scoringSet = CreateOrDefault(tiles);

            if (scoringSet == null)
            {
                throw new ArgumentOutOfRangeException(nameof(tiles), "The tiles do not form a valid scoring set.");
            }

            return scoringSet;
        }

        public static IEnumerable<IScoringSetClass> GetMaximalScoringSetClasses(IReadOnlyMultiset<ITileClass> tiles)
        {
            var maximalGroups = new List<IScoringSetClass>();
            var maximalRuns = new List<IScoringSetClass>();

            foreach (var tile in tiles.Where(t => !t.IsJoker))
            {
                if (!maximalGroups.Any(x => x.Contains(tile)))
                {
                    var potentialGroupColors = tiles
                        .Where(t => t.Value == tile.Value)
                        .Select(t => t.Color)
                        .ToArray();

                    if (potentialGroupColors.Length >= 3)
                    {
                        maximalGroups.Add(new GroupClass(potentialGroupColors, tile.Value));
                    }
                }

                if (!maximalRuns.Any(x => x.Contains(tile)))
                {
                    var potentialRunValues = tiles
                        .Where(t => t.Color == tile.Color)
                        .Select(t => t.Value)
                        .OrderBy(x => x)
                        .ToArray();

                    if (potentialRunValues.Length >= 3 &&
                        potentialRunValues[potentialRunValues.Length - 1] - potentialRunValues[0] ==
                        potentialRunValues.Length - 1)
                    {
                        maximalRuns.Add(new RunClass(tile.Color, potentialRunValues[0], potentialRunValues[potentialRunValues.Length - 1]));
                    }
                }
            }

            return maximalGroups.Union(maximalRuns);
        }

        public static IReadOnlyMultiset<IScoringSetClass> GetScoringSetClasses(IReadOnlyMultiset<ITileClass> tiles)
        {
            var scoringSetClasses = new Multiset<IScoringSetClass>();

            foreach (var scoringSetClass in GetMaximalScoringSetClasses(tiles).SelectMany(GetScoringSetClasses))
            {
                int multiplicity = scoringSetClass.Min(tiles.CountOf);
                scoringSetClasses.AddMany(scoringSetClass, multiplicity);
            }

            return scoringSetClasses;
        }

        static IEnumerable<IScoringSetClass> GetScoringSetClasses(IScoringSetClass scoringSetClass)
        {
            switch (scoringSetClass)
            {
                case GroupClass groupClass:
                    return GetContainedGroups(groupClass);
                case RunClass runClass:
                    return GetContainedRuns(runClass);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoringSetClass));
            }
        }

        static IEnumerable<IScoringSetClass> GetContainedGroups(GroupClass groupClass)
        {
            yield return groupClass;

            if (groupClass.Count == 3)
            {
                yield break;
            }

            var colors = groupClass.Select(t => t.Color).ToArray();
            yield return new GroupClass(new[] { colors[1], colors[2], colors[3] }, groupClass.Value);
            yield return new GroupClass(new[] { colors[0], colors[2], colors[3] }, groupClass.Value);
            yield return new GroupClass(new[] { colors[0], colors[1], colors[3] }, groupClass.Value);
            yield return new GroupClass(new[] { colors[0], colors[1], colors[2] }, groupClass.Value);
        }

        static IEnumerable<IScoringSetClass> GetContainedRuns(RunClass runClass)
        {
            var tiles = runClass.ToArray();
            for (int runLength = 3; runLength <= tiles.Length; ++runLength)
            {
                for (int startIndex = 0; startIndex <= tiles.Length - runLength; ++startIndex)
                {
                    var containedRun = new ITile[runLength];
                    Array.Copy(tiles, startIndex, containedRun, 0, runLength);
                    yield return new RunClass(runClass.Color, tiles[startIndex].Value, tiles[startIndex + runLength - 1].Value);
                }
            }
        }
    }
}