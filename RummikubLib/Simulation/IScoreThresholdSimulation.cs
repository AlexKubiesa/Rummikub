namespace RummikubLib.Simulation
{
    public interface IScoreThresholdSimulation : IBernoulliSamplingSimulation
    {
        int TileCount { get; }

        int Threshold { get; }
    }
}