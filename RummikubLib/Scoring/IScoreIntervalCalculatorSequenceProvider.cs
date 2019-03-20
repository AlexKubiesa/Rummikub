using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreIntervalCalculatorSequenceProvider
    {
        IEnumerable<IScoreIntervalCalculator> GetScoreIntervalCalculators(IReadOnlyCollection<ITile> tiles);
    }
}