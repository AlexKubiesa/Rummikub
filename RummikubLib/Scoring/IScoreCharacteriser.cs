using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreCharacteriser
    {
        Result IsScoreLessThanThreshold(IReadOnlyCollection<ITile> tiles, int threshold);
    }
}