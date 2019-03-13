using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    public interface IScoreIntervalCalculatorSequenceProvider
    {
        IEnumerable<IScoreIntervalCalculator> GetScoreIntervalCalculators(IReadOnlyCollection<ITile> tiles);
    }
}