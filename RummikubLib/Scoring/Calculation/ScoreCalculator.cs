using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    public class ScoreCalculator : IScoreCalculator
    {
        public static IScoreCalculator Instance { get; } = new ScoreCalculator();

        ScoreCalculator()
        {
        }

        public int GetScore(IReadOnlyCollection<ITile> tiles)
        {
            return ScoreCalculationHelper.GetScoringSetCombinations(tiles)
                .Select(ScoreCalculationHelper.GetScoreForCombination)
                .Max();
        }
    }
}