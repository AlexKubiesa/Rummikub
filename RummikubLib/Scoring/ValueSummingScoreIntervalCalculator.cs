using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ValueSummingScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new ValueSummingScoreIntervalCalculator();

        ValueSummingScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyMultiset<ITileClass> tiles)
        {
            return new Range(0, tiles.GetElementsWithMultiplicity().Sum(t => t.Value));
        }
    }
}