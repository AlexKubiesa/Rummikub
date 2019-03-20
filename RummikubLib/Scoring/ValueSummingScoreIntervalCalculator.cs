using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ValueSummingScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new ValueSummingScoreIntervalCalculator();

        ValueSummingScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyCollection<ITile> tiles)
        {
            return new Range(0, tiles.Sum(t => t.Value));
        }
    }
}