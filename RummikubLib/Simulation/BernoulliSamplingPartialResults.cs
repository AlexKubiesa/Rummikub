using System;
using RummikubLib.Scoring;

namespace RummikubLib.Simulation
{
    public class BernoulliSamplingPartialResults : IBernoulliSamplingPartialResults
    {
        public BernoulliSamplingPartialResults()
        {
            Successes = 0;
            Failures = 0;
            Inconclusive = 0;
            Count = 0;
        }

        public int Successes { get; private set; }

        public int Failures { get; private set; }

        public int Inconclusive { get; private set; }

        public int Count { get; private set; }

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
                    Inconclusive += 1;
                    Count += 1;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}