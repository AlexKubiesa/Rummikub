using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    class ScoreCalculationHelper
    {
        public static IEnumerable<List<IScoringSet>> GetScoringSetCombinations(IReadOnlyCollection<ITile> tiles)
        {
            var scoringSetsUpToEquivalence = ScoringSet.GetScoringSetsUpToEquivalence(tiles).ToArray();

            return scoringSetsUpToEquivalence
                .Concat(scoringSetsUpToEquivalence)
                .ToArray()
                .GetSublists()
                .Where(combination =>
                    tiles.All(tile =>
                        combination.Count(scoringSet =>
                            scoringSet.Tiles.Contains(tile, TileEqualityComparerByValue.Instance)) <=
                        tiles.Count(otherTile => TileEqualityComparerByValue.Instance.Equals(tile, otherTile))));
        }

        public static int GetScoreForCombination(IReadOnlyCollection<IScoringSet> scoringSetCombination)
        {
            return scoringSetCombination.Sum(scoringSet => scoringSet.GetScore());
        }
    }
}