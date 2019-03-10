using System;
using RummikubLib.Simulation;

namespace RummikubLib.Statistics
{
    public class ScoreThresholdAnalysis : IScoreThresholdAnalysis
    {
        public ScoreThresholdAnalysis(IScoreThresholdSimulation simulation, double confidenceLevel)
        {
            if (confidenceLevel <= 0 || confidenceLevel >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceLevel));
            }

            Simulation = simulation ?? throw new ArgumentNullException(nameof(simulation));
            ConfidenceLevel = confidenceLevel;
        }

        public ScoreThresholdAnalysis(int trialCount, int tileCount, int threshold, double confidenceLevel)
        {
            if (confidenceLevel <= 0 || confidenceLevel >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceLevel));
            }

            Simulation = new ScoreThresholdSimulation(trialCount, tileCount, threshold);
            ConfidenceLevel = confidenceLevel;
        }

        public IScoreThresholdSimulation Simulation { get; }

        public double ConfidenceLevel { get; }

        public bool IsComplete => Result != null;

        public IScoreThresholdAnalysisResult Result { get; private set; }

        public IScoreThresholdAnalysisResult Run()
        {
            if (Result != null)
            {
                throw new InvalidOperationException("The analysis has already been run.");
            }

            if (!Simulation.IsComplete)
            {
                Simulation.Run();
            }

            var confidenceInterval = BernoulliConfidenceIntervalProvider.Instance
                .GetConfidenceInterval(Simulation.Results, ConfidenceLevel);

            Result = new ScoreThresholdAnalysisResult(confidenceInterval);
            return Result;
        }
    }
}