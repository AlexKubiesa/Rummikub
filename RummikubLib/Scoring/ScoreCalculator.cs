using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoreCalculator : IScoreCalculator
    {
        public static IScoreCalculator Instance { get; } = new ScoreCalculator();

        ScoreCalculator()
        {
        }

        public int GetScore(IReadOnlyMultiset<ITileClass> tiles)
        {
            return ScoreCalculationHelper.GetScoringSetCombinations(tiles)
                .Select(ScoreCalculationHelper.GetScoreForCombination)
                .Max();
        }
    }
}