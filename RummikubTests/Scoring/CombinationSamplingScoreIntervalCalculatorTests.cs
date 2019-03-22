using System.Linq;
using NUnit.Framework;
using RummikubLib;
using RummikubLib.Game;
using RummikubLib.Scoring;

namespace RummikubTests.Scoring
{
    [TestFixture]
    public class CombinationSamplingScoreIntervalCalculatorTests
    {
        IScoreIntervalCalculator calculator;

        [SetUp]
        public void SetUp()
        {
            calculator = CombinationSamplingScoreIntervalCalculator.Instance;
        }

        [Test]
        public void CheckResultsAreCorrect()
        {
            var tiles = new[]
            {
                Tile.CreateNumberedTile(1, TileColor.Red, 5),
                Tile.CreateNumberedTile(2, TileColor.Red, 6),
                Tile.CreateNumberedTile(1, TileColor.Red, 7),
                Tile.CreateNumberedTile(2, TileColor.Red, 8),
                Tile.CreateNumberedTile(1, TileColor.Red, 9),
                Tile.CreateNumberedTile(2, TileColor.Red, 10),
                Tile.CreateNumberedTile(1, TileColor.Red, 11),
                Tile.CreateNumberedTile(2, TileColor.Blue, 8),
                Tile.CreateNumberedTile(1, TileColor.Black, 8),
                Tile.CreateNumberedTile(2, TileColor.Blue, 6),
                Tile.CreateNumberedTile(1, TileColor.Black, 6),
                Tile.CreateNumberedTile(2, TileColor.Red, 10),

            }.Select(x => x.Class).ToMultiset();

            var scoreInterval = calculator.GetScoreInterval(tiles);
            Assert.That(scoreInterval.Min, Is.AtLeast(30));
        }
    }
}