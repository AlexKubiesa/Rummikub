﻿namespace RummikubLib.Simulation
{
    public interface IBernoulliSamplingPartialResults
    {
        int Successes { get; }

        int Failures { get; }

        int Inconclusive { get; }

        int Count { get; }
    }

    public static class BernoulliSamplingPartialResultsExtensions
    {
        public static Range GetSuccessRate(this IBernoulliSamplingPartialResults partialResults)
        {
            return new Range(partialResults.Successes, partialResults.Successes + partialResults.Inconclusive) / partialResults.Count;
        }

        public static IBernoulliSamplingResults TreatInconclusiveAsSuccess(
            this IBernoulliSamplingPartialResults partialResults)
        {
            return new BernoulliSamplingResults(partialResults.Successes + partialResults.Inconclusive, partialResults.Failures);
        }

        public static IBernoulliSamplingResults TreatInconclusiveAsFailure(
            this IBernoulliSamplingPartialResults partialResults)
        {
            return new BernoulliSamplingResults(partialResults.Successes, partialResults.Failures + partialResults.Inconclusive);
        }
    }
}