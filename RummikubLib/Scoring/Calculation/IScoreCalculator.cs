using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    public interface IScoreCalculator
    {
        int GetScore(IReadOnlyCollection<ITile> tiles);
    }
}