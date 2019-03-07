namespace RummikubLib
{
    public interface IScoreThresholdSimulator
    {
        IScoreThresholdSimulationResults Run(int trialCount, int tileCount, int threshold);
    }
}