using NUnit.Framework;
using RummikubLib.Game;
using RummikubLib.Scoring.Calculation;

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
                Tile.CreateNumberedTile(TileColor.Red, 5),
                Tile.CreateNumberedTile(TileColor.Red, 6),
                Tile.CreateNumberedTile(TileColor.Red, 7),
                Tile.CreateNumberedTile(TileColor.Red, 8),
                Tile.CreateNumberedTile(TileColor.Red, 9),
                Tile.CreateNumberedTile(TileColor.Red, 10),
                Tile.CreateNumberedTile(TileColor.Red, 11),
                Tile.CreateNumberedTile(TileColor.Blue, 8),
                Tile.CreateNumberedTile(TileColor.Black, 8),
                Tile.CreateNumberedTile(TileColor.Blue, 6),
                Tile.CreateNumberedTile(TileColor.Black, 6),
                Tile.CreateNumberedTile(TileColor.Red, 10),

            };

            var scoreInterval = calculator.GetScoreInterval(tiles);
            Assert.That(scoreInterval.Min, Is.AtLeast(30));
        }
    }
}