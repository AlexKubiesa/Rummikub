using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public static class HighScoringSetClasses
    {
        public static readonly IEnumerable<IScoringSetClass> ScoringSetClasses = GetScoringSetClasses();

        static readonly IEnumerable<TileColor[]> CombinationsOfThreeTileColors = new[]
        {
            new[] { TileColor.Black, TileColor.Blue, TileColor.Red },
            new[] { TileColor.Yellow, TileColor.Blue, TileColor.Red },
            new[] { TileColor.Black, TileColor.Yellow, TileColor.Red },
            new[] { TileColor.Black, TileColor.Blue, TileColor.Yellow }
        };

        static readonly IEnumerable<int[]> SequencesOfThreeNumbers = new[]
        {
            new[] { 9, 10, 11 },
            new[] { 10, 11, 12 },
            new[] { 11, 12, 13 }
        };

        static IEnumerable<IScoringSetClass> GetScoringSetClasses()
        {
            foreach (var colorCombination in CombinationsOfThreeTileColors)
            {
                for (int value = 10; value <= 13; ++value)
                {
                    yield return new GroupClass(colorCombination, value);
                }
            }

            foreach (var numberSequence in SequencesOfThreeNumbers)
            {
                foreach (var color in GameConstants.NumberedTileColors)
                {
                    yield return new RunClass(color, numberSequence[0], numberSequence[2]);
                }
            }
        }
    }
}