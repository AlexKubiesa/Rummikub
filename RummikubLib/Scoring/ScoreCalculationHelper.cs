using System.Collections.Generic;
using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    class ScoreCalculationHelper
    {
        public static IEnumerable<IReadOnlyMultiset<IScoringSetClass>> GetScoringSetCombinations(IReadOnlyMultiset<ITileClass> tiles)
        {
            var scoringSetClasses = ScoringSetClass.GetScoringSetClasses(tiles);

            return scoringSetClasses
                .GetSubMultisets()
                .Where(combination =>
                    tiles.GetDistinctElements().All(tileClass =>
                        combination.GetElementsWithMultiplicity().Count(scoringSet =>
                            scoringSet.Contains(tileClass)) <=
                        tiles.CountOf(tileClass)));
        }

        public static int GetScoreForCombination(IReadOnlyMultiset<IScoringSetClass> scoringSetCombination)
        {
            return scoringSetCombination.GetElementsWithMultiplicity().Sum(scoringSet => scoringSet.GetScore());
        }
    }
}