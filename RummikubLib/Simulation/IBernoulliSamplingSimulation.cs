namespace RummikubLib.Simulation
{
    public interface IBernoulliSamplingSimulation
    {
        int TrialCount { get; }

        bool IsComplete { get; }

        IBernoulliSamplingPartialResults Results { get; }

        IBernoulliSamplingPartialResults Run();
    }
}