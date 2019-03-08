using NUnit.Framework;
using System;
using RummikubLib.Simulation;

namespace RummikubTests.Simulation
{
    [TestFixture]
    public class ScoreThresholdSimulationTests
    {
        const double InconclusiveRateThreshold = 0.1;

        [Test]
        public void CheckThatInconclusiveRateIsLow()
        {
            var simulation = new ScoreThresholdSimulation(1000, 15, 30, new Random(123));
            var results = simulation.Run();
            double inconclusiveRate = results.Inconclusive / (double) results.Count;
            Assert.That(inconclusiveRate, Is.LessThan(InconclusiveRateThreshold));
        }
    }
}
