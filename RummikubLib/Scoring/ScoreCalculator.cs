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
            var scoringSetCombinations = 
                ScoringSet.GetScoringSets(tiles)
                .GetSubsets()
                .Where(x => !ContainsOverlappingScoringSets(x));

            return scoringSetCombinations
                .Select(x => x.Sum(scoringSet => scoringSet.GetScore()))
                .Max();
        }

        static bool ContainsOverlappingScoringSets(IEnumerable<IScoringSet> scoringSetCombination)
        {
            return scoringSetCombination
                .GetPairs()
                .Any(pair => pair.Item1.Tiles.Intersect(pair.Item2.Tiles).Any());
        }
    }
}