using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreCalculator
    {
        int GetScore(IReadOnlyMultiset<ITileClass> tiles);
    }
}