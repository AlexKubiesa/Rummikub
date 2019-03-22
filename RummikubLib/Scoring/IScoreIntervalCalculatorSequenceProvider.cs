using System.Collections.Generic;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreIntervalCalculatorSequenceProvider
    {
        IEnumerable<IScoreIntervalCalculator> GetScoreIntervalCalculators(IReadOnlyMultiset<ITileClass> tiles);
    }
}