using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoreCalculator : IScoreCalculator
    {
        public static IScoreCalculator Instance { get; } = new ScoreCalculator();

        ScoreCalculator()
        {
        }

        public int GetScore(IReadOnlyCollection<ITile> tiles)
        {
            var scoringSetsUpToEquivalence = ScoringSet.GetScoringSetsUpToEquivalence(tiles).ToArray();

            var scoringSetCombinations = scoringSetsUpToEquivalence
                .Concat(scoringSetsUpToEquivalence)
                .GetSublists()
                .Where(combination =>
                    tiles.All(tile => 
                        combination.Count(scoringSet =>
                            scoringSet.Tiles.Contains(tile, TileEqualityComparerByValue.Instance)) <=
                        tiles.Count(otherTile => TileEqualityComparerByValue.Instance.Equals(tile, otherTile))));

            return scoringSetCombinations
                .Select(combination => combination.Sum(scoringSet => scoringSet.GetScore()))
                .Max();
        }
    }
}