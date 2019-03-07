using System;
using RummikubLib;
using RummikubLib.Scoring;

namespace Rummikub
{
    class Program
    {
        static void Main(string[] args)
        {
            const int trialCount = 1000;
            const int tileCount = 10;
            const int threshold = 30;

            Console.WriteLine($"Number of trials: {trialCount}");
            Console.WriteLine($"Tiles to draw: {tileCount}");
            Console.WriteLine($"Score threshold: {threshold}");
            Console.WriteLine("Running simulation...");

            var simulator = new ScoreThresholdSimulator(new ScoreCharacteriser());
            var results = simulator.Run(trialCount, tileCount, threshold);

            Console.WriteLine("Simulation finished!");
            Console.WriteLine($"Trials under threshold: {results.Successes}");
            Console.WriteLine($"Trials over threshold: {results.Failures}");
            Console.WriteLine($"Inconclusive trials: {results.Uncertainty}");

            var successRate = results.GetSuccessRate() * 100;

            Console.WriteLine($"Success rate: {successRate.Min}% - {successRate.Max}%");

            Console.ReadLine();
        }
    }
}
