using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    public interface IScoreIntervalCalculator
    {
        Range GetScoreInterval(IReadOnlyCollection<ITile> tiles);
    }
}