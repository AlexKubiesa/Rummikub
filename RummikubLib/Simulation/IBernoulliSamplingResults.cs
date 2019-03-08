namespace RummikubLib.Simulation
{
    public interface IBernoulliSamplingResults
    {
        int Successes { get; }

        int Failures { get; }

        int Count { get; }
    }
}