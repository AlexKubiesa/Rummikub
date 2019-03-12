using System.Collections.Generic;
using RummikubLib.Game;
using RummikubLib.Scoring.Model;

namespace RummikubLib.Scoring.Calculation
{
    public interface IScoreCharacteriser
    {
        Result IsScoreLessThanThreshold(IReadOnlyCollection<ITile> tiles, int threshold);
    }
}