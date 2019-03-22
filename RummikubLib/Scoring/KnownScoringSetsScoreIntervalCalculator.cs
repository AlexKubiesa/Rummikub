using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class KnownScoringSetsScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new KnownScoringSetsScoreIntervalCalculator();

        KnownScoringSetsScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyMultiset<ITileClass> tiles)
        {
            foreach (var scoringSetClass in HighScoringSetClasses.ScoringSetClasses)
            {
                if (scoringSetClass.All(tiles.Contains))
                {
                    return new Range(scoringSetClass.GetScore(), double.PositiveInfinity);
                }
            }

            return new Range(0, double.PositiveInfinity);
        }
    }
}