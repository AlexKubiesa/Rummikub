using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    class ScoreCharacteriser : IScoreCharacteriser
    {
        public static IScoreCharacteriser Instance { get; } = new ScoreCharacteriser();

        ScoreCharacteriser()
        {
        }

        public Result IsScoreLessThanThreshold(IReadOnlyCollection<ITile> tiles, int threshold)
        {
            return IsScoreLessThanThresholdDestructive(tiles.ToList(), threshold);
        }

        static Result IsScoreLessThanThresholdDestructive(List<ITile> tiles, int threshold)
        {
            if (threshold <= 0)
            {
                return Result.No;
            }

            RemoveJokers(tiles);

            var partition = PartitionProvider.Instance.GetPartition(tiles);

            var scoreInterval = partition.Sum(GetScoreIntervalForComponent);

            if (scoreInterval.Min >= threshold)
            {
                return Result.No;
            }

            if (scoreInterval.Max < threshold)
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

        static Range GetScoreIntervalForComponent(IReadOnlyCollection<ITile> component)
        {
            if (component.Count < 3)
            {
                return new Range(0, 0);
            }

            var scoringSet = ScoringSet.CreateOrDefault(component);

            if (scoringSet != null)
            {
                int score = scoringSet.GetScore();
                return new Range(score, score);
            }

            var range = new Range(0, double.PositiveInfinity);

            range = range.Intersect(KnownScoringSetsScoreIntervalCalculator.Instance.GetScoreInterval(component));
            range = range.Intersect(CombinationSamplingScoreIntervalCalculator.Instance.GetScoreInterval(component));

            return range;
        }
    }
}