using System.Collections.Generic;
using NUnit.Framework;
using RummikubLib.Game;
using RummikubLib.Scoring;

namespace RummikubTests.Scoring
{
    [TestFixture]
    public class ScoreCharacteriserTests
    {
        public static IEnumerable<TestCaseData> IsUnderThresholdTestCases = new[]
        {
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
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Black, 1),
                        Tile.CreateNumberedTile(TileColor.Yellow, 2),
                        Tile.CreateNumberedTile(TileColor.Yellow, 2),
                        Tile.CreateNumberedTile(TileColor.Yellow, 2)
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
                .SetName("Overlapping run and group, where the group is better")
        };

        ScoreCharacteriser scoreCharacteriser;

        [SetUp]
        public void SetUp()
        {
            scoreCharacteriser = new ScoreCharacteriser();
        }

        [Test]
        [TestCaseSource(nameof(IsUnderThresholdTestCases))]
        public Result CheckResultsAreCorrect(IReadOnlyCollection<ITile> tiles, int threshold)
        {
            return scoreCharacteriser.IsScoreLessThanThreshold(tiles, threshold);
        }
    }
}