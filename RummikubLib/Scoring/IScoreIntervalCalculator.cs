using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreIntervalCalculator
    {
        Range GetScoreInterval(IReadOnlyCollection<ITile> tiles);
    }
}