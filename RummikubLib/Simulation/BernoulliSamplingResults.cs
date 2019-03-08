using System;

namespace RummikubLib.Simulation
{
    public class BernoulliSamplingResults : IBernoulliSamplingResults
    {
        public BernoulliSamplingResults(int successes, int failures)
        {
            if (successes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(successes));
            }

            if (failures < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(failures));
            }

            Successes = successes;
            Failures = failures;
            Count = successes + failures;
        }

        public int Successes { get; }

        public int Failures { get; }

        public int Count { get; }
    }
}