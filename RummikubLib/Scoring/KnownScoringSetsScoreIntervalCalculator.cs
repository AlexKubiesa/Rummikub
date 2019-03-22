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
            foreach (var scoringSetClass in HighScoringSetClasses.ScoringSetClasses)
            {
                if (scoringSetClass.All(tileClass => tiles.Any(tile => tile.Class.Equals(tileClass))))
                {
                    return new Range(scoringSetClass.GetScore(), double.PositiveInfinity);
                }
            }

            return new Range(0, double.PositiveInfinity);
        }
    }
}