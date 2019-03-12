using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    public class KnownScoringSetsScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new KnownScoringSetsScoreIntervalCalculator();

        KnownScoringSetsScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyCollection<ITile> tiles)
        {
            foreach (var scoringSet in HighScoringSets.ScoringSets)
            {
                if (scoringSet.Tiles.All(t => tiles.Contains(t, TileEqualityComparerByValue.Instance)))
                {
                    return new Range(scoringSet.GetScore(), double.PositiveInfinity);
                }
            }

            return new Range(0, double.PositiveInfinity);
        }
    }
}