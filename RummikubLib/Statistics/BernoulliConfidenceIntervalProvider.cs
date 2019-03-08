using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using RummikubLib.Simulation;
using MathNet.Numerics.Statistics;

namespace RummikubLib.Statistics
{
    public class BernoulliConfidenceIntervalProvider : IBernoulliConfidenceIntervalProvider
    {
        public static IBernoulliConfidenceIntervalProvider Instance { get; } = new BernoulliConfidenceIntervalProvider();

        BernoulliConfidenceIntervalProvider()
        {
        }

        public Range GetConfidenceInterval(IBernoulliSamplingPartialResults results, double confidenceLevel)
        {
            var lowerInterval = GetConfidenceInterval(results.TreatInconclusiveAsFailure(), confidenceLevel);
            var higherInterval = GetConfidenceInterval(results.TreatInconclusiveAsSuccess(), confidenceLevel);
            return lowerInterval.Union(higherInterval);

        }

        public Range GetConfidenceInterval(IBernoulliSamplingResults results, double confidenceLevel)
        {
            // Use the normal approximation to the binomial distribution, assuming the sample size is large.
            double sampleMean = ConvertToSamples(results).Mean();
            var standardNormal = new Normal();
            double quantile = standardNormal.InverseCumulativeDistribution(1.0 - confidenceLevel / 2.0);
            double radius = quantile * Math.Sqrt(sampleMean * (1.0 - sampleMean) / results.Count);
            return new Range(sampleMean - radius, sampleMean + radius);
        }

        static IEnumerable<double> ConvertToSamples(IBernoulliSamplingResults results)
        {
            return Enumerable.Repeat(0.0, results.Failures).Concat(Enumerable.Repeat(1.0, results.Successes));
        }
    }
}