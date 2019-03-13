using System.Collections.Generic;
using NUnit.Framework;
using RummikubLib.Game;
using RummikubLib.Scoring.Calculation;
using RummikubLib.Scoring.Model;

namespace RummikubTests.Scoring
{
    [TestFixture]
    public class ScoreCharacteriserTests
    {
        public static IEnumerable<TestCaseData> IsUnderThresholdTestCases = new[]
        {
            new TestCaseData(
                    new[] { Tile.CreateNumberedTile(TileColor.Black, 1) },
                    0)
                .Returns(Result.No)
                .SetName("No tiles (threshold = 0)"),

            new TestCaseData(
                    new[] { Tile.CreateNumberedTile(TileColor.Black, 1) },
                    1)
                .Returns(Result.Yes)
                .SetName("No tiles (threshold = 1)"),

            new TestCaseData(
                    new[] { Tile.CreateNumberedTile(TileColor.Black, 1) },
                    1)
                .Returns(Result.Yes)
                .SetName("Single tile"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateJoker()
                    },
                    2)
                .Returns(Result.Yes)
                .SetName("Run involving joker"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateJoker()
                    },
                    2)
                .Returns(Result.Yes)
                .SetName("Group involving joker"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3)
                    },
                    5)
                .Returns(Result.No)
                .SetName("Run of 3"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Black, 4)
                    },
                    10)
                .Returns(Result.No)
                .SetName("Run of 4"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 1)
                    },
                    3)
                .Returns(Result.No)
                .SetName("Group of 3"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 1),
                        Tile.CreateNumberedTile(TileColor.Yellow, 1)
                    },
                    4)
                .Returns(Result.No)
                .SetName("Group of 4"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 2),
                        Tile.CreateNumberedTile(TileColor.Blue, 3)
                    },
                    12)
                .Returns(Result.No)
                .SetName("Disjoint runs"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Blue, 2),
                        Tile.CreateNumberedTile(TileColor.Red, 2)
                    },
                    9)
                .Returns(Result.No)
                .SetName("Disjoint groups"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Yellow, 1)
                    },
                    7)
                .Returns(Result.Yes)
                .SetName("Run with extra tile"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Black, 4)
                    },
                    15)
                .Returns(Result.No)
                .SetName("Overlapping runs"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 1),
                    },
                    6)
                .Returns(Result.No)
                .SetName("Overlapping run and group, where the run is better"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Blue, 3),
                        Tile.CreateNumberedTile(TileColor.Red, 3),
                    },
                    9)
                .Returns(Result.No)
                .SetName("Overlapping run and group, where the group is better"),

            new TestCaseData(
                    new[]
                    {
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 2),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Black, 3),
                        Tile.CreateNumberedTile(TileColor.Black, 4),
                        Tile.CreateNumberedTile(TileColor.Black, 4),
                        Tile.CreateNumberedTile(TileColor.Black, 5),
                        Tile.CreateNumberedTile(TileColor.Black, 5),
                        Tile.CreateNumberedTile(TileColor.Black, 6),
                        Tile.CreateNumberedTile(TileColor.Black, 6),
                        Tile.CreateNumberedTile(TileColor.Black, 7),
                        Tile.CreateNumberedTile(TileColor.Black, 7),
                        Tile.CreateNumberedTile(TileColor.Black, 8),
                        Tile.CreateNumberedTile(TileColor.Black, 8),
                        Tile.CreateNumberedTile(TileColor.Black, 9),
                        Tile.CreateNumberedTile(TileColor.Black, 9),
                        Tile.CreateNumberedTile(TileColor.Black, 10),
                        Tile.CreateNumberedTile(TileColor.Black, 10),
                        Tile.CreateNumberedTile(TileColor.Black, 11),
                        Tile.CreateNumberedTile(TileColor.Black, 11),
                        Tile.CreateNumberedTile(TileColor.Black, 12),
                        Tile.CreateNumberedTile(TileColor.Black, 12),
                        Tile.CreateNumberedTile(TileColor.Black, 13),
                        Tile.CreateNumberedTile(TileColor.Black, 13),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 1),
                        Tile.CreateNumberedTile(TileColor.Blue, 2),
                        Tile.CreateNumberedTile(TileColor.Blue, 2),
                        Tile.CreateNumberedTile(TileColor.Blue, 3),
                        Tile.CreateNumberedTile(TileColor.Blue, 3),
                        Tile.CreateNumberedTile(TileColor.Blue, 4),
                        Tile.CreateNumberedTile(TileColor.Blue, 4),
                        Tile.CreateNumberedTile(TileColor.Blue, 5),
                        Tile.CreateNumberedTile(TileColor.Blue, 5),
                        Tile.CreateNumberedTile(TileColor.Blue, 6),
                        Tile.CreateNumberedTile(TileColor.Blue, 6),
                        Tile.CreateNumberedTile(TileColor.Blue, 7),
                        Tile.CreateNumberedTile(TileColor.Blue, 7),
                        Tile.CreateNumberedTile(TileColor.Blue, 8),
                        Tile.CreateNumberedTile(TileColor.Blue, 8),
                        Tile.CreateNumberedTile(TileColor.Blue, 9),
                        Tile.CreateNumberedTile(TileColor.Blue, 9),
                        Tile.CreateNumberedTile(TileColor.Blue, 10),
                        Tile.CreateNumberedTile(TileColor.Blue, 10),
                        Tile.CreateNumberedTile(TileColor.Blue, 11),
                        Tile.CreateNumberedTile(TileColor.Blue, 11),
                        Tile.CreateNumberedTile(TileColor.Blue, 12),
                        Tile.CreateNumberedTile(TileColor.Blue, 12),
                        Tile.CreateNumberedTile(TileColor.Blue, 13),
                        Tile.CreateNumberedTile(TileColor.Blue, 13),
                        Tile.CreateNumberedTile(TileColor.Red, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 1),
                        Tile.CreateNumberedTile(TileColor.Red, 2),
                        Tile.CreateNumberedTile(TileColor.Red, 2),
                        Tile.CreateNumberedTile(TileColor.Red, 3),
                        Tile.CreateNumberedTile(TileColor.Red, 3),
                        Tile.CreateNumberedTile(TileColor.Red, 4),
                        Tile.CreateNumberedTile(TileColor.Red, 4),
                        Tile.CreateNumberedTile(TileColor.Red, 5),
                        Tile.CreateNumberedTile(TileColor.Red, 5),
                        Tile.CreateNumberedTile(TileColor.Red, 6),
                        Tile.CreateNumberedTile(TileColor.Red, 6),
                        Tile.CreateNumberedTile(TileColor.Red, 7),
                        Tile.CreateNumberedTile(TileColor.Red, 7),
                        Tile.CreateNumberedTile(TileColor.Red, 8),
                        Tile.CreateNumberedTile(TileColor.Red, 8),
                        Tile.CreateNumberedTile(TileColor.Red, 9),
                        Tile.CreateNumberedTile(TileColor.Red, 9),
                        Tile.CreateNumberedTile(TileColor.Red, 10),
                        Tile.CreateNumberedTile(TileColor.Red, 10),
                        Tile.CreateNumberedTile(TileColor.Red, 11),
                        Tile.CreateNumberedTile(TileColor.Red, 11),
                        Tile.CreateNumberedTile(TileColor.Red, 12),
                        Tile.CreateNumberedTile(TileColor.Red, 12),
                        Tile.CreateNumberedTile(TileColor.Red, 13),
                        Tile.CreateNumberedTile(TileColor.Red, 13),
                        Tile.CreateNumberedTile(TileColor.Yellow, 1),
                        Tile.CreateNumberedTile(TileColor.Yellow, 1),
                        Tile.CreateNumberedTile(TileColor.Yellow, 2),
                        Tile.CreateNumberedTile(TileColor.Yellow, 2),
                        Tile.CreateNumberedTile(TileColor.Yellow, 3),
                        Tile.CreateNumberedTile(TileColor.Yellow, 3),
                        Tile.CreateNumberedTile(TileColor.Yellow, 4),
                        Tile.CreateNumberedTile(TileColor.Yellow, 4),
                        Tile.CreateNumberedTile(TileColor.Yellow, 5),
                        Tile.CreateNumberedTile(TileColor.Yellow, 5),
                        Tile.CreateNumberedTile(TileColor.Yellow, 6),
                        Tile.CreateNumberedTile(TileColor.Yellow, 6),
                        Tile.CreateNumberedTile(TileColor.Yellow, 7),
                        Tile.CreateNumberedTile(TileColor.Yellow, 7),
                        Tile.CreateNumberedTile(TileColor.Yellow, 8),
                        Tile.CreateNumberedTile(TileColor.Yellow, 8),
                        Tile.CreateNumberedTile(TileColor.Yellow, 9),
                        Tile.CreateNumberedTile(TileColor.Yellow, 9),
                        Tile.CreateNumberedTile(TileColor.Yellow, 10),
                        Tile.CreateNumberedTile(TileColor.Yellow, 10),
                        Tile.CreateNumberedTile(TileColor.Yellow, 11),
                        Tile.CreateNumberedTile(TileColor.Yellow, 11),
                        Tile.CreateNumberedTile(TileColor.Yellow, 12),
                        Tile.CreateNumberedTile(TileColor.Yellow, 12),
                        Tile.CreateNumberedTile(TileColor.Yellow, 13),
                        Tile.CreateNumberedTile(TileColor.Yellow, 13),
                        Tile.CreateJoker(),
                        Tile.CreateJoker()
                    },
                    30)
                .Returns(Result.No)
                .SetName("All the tiles"),
        };

        IScoreCharacteriser scoreCharacteriser;

        [SetUp]
        public void SetUp()
        {
            scoreCharacteriser = ScoreCharacteriser.Instance;
        }

        [Test]
        [TestCaseSource(nameof(IsUnderThresholdTestCases))]
        public Result CheckResultsAreCorrect(IReadOnlyCollection<ITile> tiles, int threshold)
        {
            return scoreCharacteriser.IsScoreLessThanThreshold(tiles, threshold);
        }
    }
}