using System;
using RummikubLib.Simulation;
using RummikubLib.Statistics;

namespace Rummikub
{
    class Program
    {
        static void Main(string[] args)
        {
            const int trialCount = 2000;
            const int tileCount = 15;
            const int threshold = 30;
            const double confidenceLevel = 0.95;

            Console.WriteLine($"Number of trials: {trialCount}");
            Console.WriteLine($"Tiles to draw: {tileCount}");
            Console.WriteLine($"Score threshold: {threshold}");
            Console.WriteLine($"Confidence level: {confidenceLevel:P0}");
            Console.WriteLine("Running simulation...");

            var simulation = new ScoreThresholdSimulation(trialCount, tileCount, threshold);
            var results = simulation.Run();

            Console.WriteLine("Simulation finished!");
            Console.WriteLine($"Trials under threshold: {results.Successes}");
            Console.WriteLine($"Trials over threshold: {results.Failures}");
            Console.WriteLine($"Inconclusive trials: {results.Inconclusive}");

            var successRate = results.GetSuccessRate();

            Console.WriteLine($"Success rate: {successRate.Min:P1} - {successRate.Max:P1}");
            Console.WriteLine($"Computing a {confidenceLevel * 100}% confidence interval for the probability that the score is under the threshold...");

            var confidenceInterval = BernoulliConfidenceIntervalProvider.Instance.GetConfidenceInterval(results, confidenceLevel);

            Console.WriteLine($"Confidence interval: {confidenceInterval.Min:N3} - {confidenceInterval.Max:N3}");

            Console.ReadLine();
        }
    }
}
