using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    class ScoreCharacteriser : IScoreCharacteriser
    {
        public static IScoreCharacteriser Instance { get; } = new ScoreCharacteriser();

        const int MaximumNumberOfScoringSetsForExplicitScoreCalculation = 5;

        ScoreCharacteriser()
        {
        }

        public Result IsScoreLessThanThreshold(IReadOnlyCollection<ITile> tiles, int threshold)
        {
            return IsScoreLessThanThresholdDestructive(tiles.ToList(), threshold);
        }

        static Result IsScoreLessThanThresholdDestructive(List<ITile> tiles, int threshold)
        {
            int scoreSoFar = 0;

            if (scoreSoFar >= threshold)
            {
                return Result.No;
            }

            RemoveJokers(tiles);

            var partition = PartitionProvider.Instance.GetPartition(tiles);

            var inconclusiveComponents = new List<IReadOnlyCollection<ITile>>();
            foreach (var component in partition)
            {
                if (TryGetScoreForComponent(component, out int scoreFromComponent))
                {
                    scoreSoFar += scoreFromComponent;
                }
                else
                {
                    inconclusiveComponents.Add(component);
                }
            }

            if (scoreSoFar >= threshold)
            {
                return Result.No;
            }

            if (inconclusiveComponents.Count == 0 && scoreSoFar < threshold)
            {
                return Result.Yes;
            }

            return Result.Maybe;
        }

        static void RemoveJokers(ICollection<ITile> tiles)
        {
            var jokers = tiles.Where(t => t.IsJoker).ToArray();

            foreach (var joker in jokers)
            {
                tiles.Remove(joker);
            }
        }

        static bool TryGetScoreForComponent(IReadOnlyCollection<ITile> component, out int score)
        {
            if (component.Count < 3)
            {
                score = 0;
                return true;
            }

            var scoringSet = ScoringSet.CreateOrDefault(component);

            if (scoringSet != null)
            {
                score = scoringSet.GetScore();
                return true;
            }

            // If doing an explicit score calculation would take too long, don't bother.
            if (ScoringSet.GetScoringSetsUpToEquivalence(component).Skip(MaximumNumberOfScoringSetsForExplicitScoreCalculation).Any())
            {
                score = 0;
                return false;
            }

            score = ScoreCalculator.Instance.GetScore(component);
            return true;
        }
    }
}