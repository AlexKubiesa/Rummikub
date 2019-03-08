using RummikubLib.Simulation;

namespace RummikubLib.Statistics
{
    public interface IBernoulliConfidenceIntervalProvider
    {
        Range GetConfidenceInterval(IBernoulliSamplingPartialResults results, double confidenceLevel);

        Range GetConfidenceInterval(IBernoulliSamplingResults results, double confidenceLevel);
    }
}