using System;
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

            if (tiles.Count == 0)
            {
                return Result.Yes;
            }

            RemoveJokers(tiles);

            var partition = PartitionProvider.Instance.GetPartition(tiles);

            var calculations = partition
                .Select(component => new ScoreIntervalCalculation(component))
                .ToArray();

            while (calculations.Any(x => x.Step()))
            {
                var scoreInterval = calculations.Sum(x => x.ResultSoFar);

                if (scoreInterval.Min >= threshold)
                {
                    return Result.No;
                }

                if (scoreInterval.Max < threshold)
                {
                    return Result.Yes;
                }
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

        class ScoreIntervalCalculation
        {
            readonly IReadOnlyCollection<ITile> component;

            readonly IEnumerator<IScoreIntervalCalculator> enumerator;

            bool finished;

            public ScoreIntervalCalculation(IReadOnlyCollection<ITile> component)
            {
                this.component = component ?? throw new ArgumentNullException(nameof(component));
                enumerator = ScoreIntervalCalculatorSequenceProvider.Instance
                    .GetScoreIntervalCalculators(component)
                    .GetEnumerator();
                ResultSoFar = new Range(0, double.PositiveInfinity);
            }

            public Range ResultSoFar { get; private set; }

            public bool Step()
            {
                if (finished)
                {
                    return false;
                }

                if (!enumerator.MoveNext())
                {
                    enumerator.Dispose();
                    finished = true;
                    return false;
                }

                if (enumerator.Current == null)
                {
                    return true;
                }

                ResultSoFar = ResultSoFar.Intersect(enumerator.Current.GetScoreInterval(component));
                return true;
            }
        }
    }
}