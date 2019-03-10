using RummikubLib.Simulation;

namespace RummikubLib.Statistics
{
    public interface IScoreThresholdAnalysis
    {
        IScoreThresholdSimulation Simulation { get; }

        double ConfidenceLevel { get; }

        bool IsComplete { get; }

        IScoreThresholdAnalysisResult Result { get; }

        IScoreThresholdAnalysisResult Run();
    }
}