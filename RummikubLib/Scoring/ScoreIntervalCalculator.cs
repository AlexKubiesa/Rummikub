using System;
using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoreIntervalCalculator : IScoreIntervalCalculator
    {
        public static IScoreIntervalCalculator Instance { get; } = new ScoreIntervalCalculator();

        const int MaximumNumberOfScoringSetCombinationsToCheck = 5;

        ScoreIntervalCalculator()
        {
        }

        public Range GetScoreInterval(IReadOnlyCollection<ITile> tiles)
        {
            var combinations = ScoreCalculationHelper.GetScoringSetCombinations(tiles);

            int encounteredCombinations = 0;
            int bestScoreSoFar = 0;
            foreach (var combination in combinations.Take(MaximumNumberOfScoringSetCombinationsToCheck + 1))
            {
                ++encounteredCombinations;

                if (encounteredCombinations > MaximumNumberOfScoringSetCombinationsToCheck)
                {
                    break;
                }

                bestScoreSoFar = Math.Max(bestScoreSoFar, ScoreCalculationHelper.GetScoreForCombination(combination));
            }

            return (encounteredCombinations <= MaximumNumberOfScoringSetCombinationsToCheck)
                ? new Range(bestScoreSoFar, bestScoreSoFar)
                : new Range(bestScoreSoFar, double.PositiveInfinity);
        }
    }
}