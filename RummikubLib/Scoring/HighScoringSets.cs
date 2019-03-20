using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public static class HighScoringSets
    {
        public static readonly IEnumerable<IScoringSet> ScoringSets = GetScoringSets();

        static readonly IEnumerable<TileColor> Colors = new[]
        {
            TileColor.Black, TileColor.Blue, TileColor.Red, TileColor.Yellow
        };

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

        static IEnumerable<IScoringSet> GetScoringSets()
        {
            foreach (var colorCombination in CombinationsOfThreeTileColors)
            {
                for (int value = 10; value <= 13; ++value)
                {
                    yield return ScoringSet.Create(new[]
                    {
                        Tile.CreateNumberedTile(colorCombination[0], value),
                        Tile.CreateNumberedTile(colorCombination[1], value),
                        Tile.CreateNumberedTile(colorCombination[2], value)
                    });
                }
            }

            foreach (var numberSequence in SequencesOfThreeNumbers)
            {
                foreach (var color in Colors)
                {
                    yield return ScoringSet.Create(new[]
                    {
                        Tile.CreateNumberedTile(color, numberSequence[0]),
                        Tile.CreateNumberedTile(color, numberSequence[1]),
                        Tile.CreateNumberedTile(color, numberSequence[2])
                    });
                }
            }
        }
    }
}