using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class KnownScoringSetsScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new KnownScoringSetsScoreIntervalCalculator();

        KnownScoringSetsScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyCollection<ITile> tiles)
        {
            foreach (var scoringSet in HighScoringSetClasses.ScoringSetClasses)
            {
                if (scoringSet.Tiles.All(t => tiles.Contains(t, TileValueEqualityComparer.Instance)))
                {
                    return new Range(scoringSet.GetScore(), double.PositiveInfinity);
                }
            }

            return new Range(0, double.PositiveInfinity);
        }
    }
}