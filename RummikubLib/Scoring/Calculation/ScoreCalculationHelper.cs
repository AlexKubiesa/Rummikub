using System.Collections.Generic;
using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;
using RummikubLib.Scoring.Model;

namespace RummikubLib.Scoring.Calculation
{
    class ScoreCalculationHelper
    {
        public static IEnumerable<List<IScoringSet>> GetScoringSetCombinations(IMultiset<ITile> tiles)
        {
            var scoringSetsUpToEquivalence = ScoringSet.GetScoringSetsUpToEquivalence(tiles).ToArray();

            return scoringSetsUpToEquivalence
                .Concat(scoringSetsUpToEquivalence)
                .ToArray()
                .GetSublists()
                .Where(combination =>
                    tiles.All(tile =>
                        combination.Count(scoringSet =>
                            scoringSet.Tiles.Contains(tile, TileValueEqualityComparer.Instance)) <=
                        tiles.CountOf(otherTile => TileValueEqualityComparer.Instance.Equals(tile, otherTile))));
        }

        public static int GetScoreForCombination(IReadOnlyCollection<IScoringSet> scoringSetCombination)
        {
            return scoringSetCombination.Sum(scoringSet => scoringSet.GetScore());
        }
    }
}