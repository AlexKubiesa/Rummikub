using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoreIntervalCalculator
    {
        Range GetScoreInterval(IReadOnlyMultiset<ITileClass> tiles);
    }
}