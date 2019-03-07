using System;
using RummikubLib.Scoring;

namespace RummikubLib
{
    public class ScoreThresholdSimulationResults : IScoreThresholdSimulationResults
    {
        public ScoreThresholdSimulationResults()
        {
            Successes = 0;
            Failures = 0;
            Uncertainty = 0;
            Count = 0;
        }

        public int Successes { get; private set; }

        public int Failures { get; private set; }

        public int Uncertainty { get; private set; }

        public int Count { get; private set; }

        public Range GetSuccessRate()
        {
            return new Range(Successes, Successes + Uncertainty) / Count;
        }

        public void AddResult(Result result)
        {
            switch (result)
            {
                case Result.Yes:
                    Successes += 1;
                    Count += 1;
                    return;

                case Result.No:
                    Failures += 1;
                    Count += 1;
                    return;

                case Result.Maybe:
                    Uncertainty += 1;
                    Count += 1;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}