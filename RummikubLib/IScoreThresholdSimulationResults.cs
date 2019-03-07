namespace RummikubLib
{
    public interface IScoreThresholdSimulationResults
    {
        int Successes { get; }

        int Failures { get; }

        int Uncertainty { get; }

        int Count { get; }

        Range GetSuccessRate();
    }
}