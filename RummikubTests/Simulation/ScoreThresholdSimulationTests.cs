using NUnit.Framework;
using System;
using System.Timers;
using RummikubLib.Simulation;

namespace RummikubTests.Simulation
{
    [TestFixture]
    public class ScoreThresholdSimulationTests
    {
        const double InconclusiveRateThreshold = 0.1;

        double elapsedTime;

        [SetUp]
        public void SetUp()
        {
            elapsedTime = 0;
        }

        [Test]
        public void CheckThatInconclusiveRateIsLow()
        {
            var simulation = new ScoreThresholdSimulation(1000, 15, 30, new Random(123));
            var results = simulation.Run();
            double inconclusiveRate = results.Inconclusive / (double) results.Count;
            Assert.That(inconclusiveRate, Is.LessThan(InconclusiveRateThreshold));
        }

        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        [TestCase(50)]
        [TestCase(60)]
        [TestCase(70)]
        [TestCase(80)]
        [TestCase(90)]
        [TestCase(100)]
        public void CheckPerformance(int tileCount)
        {
            using (var timer = new Timer(100))
            {
                timer.Elapsed += (sender, args) => elapsedTime += 100;
                timer.Start();
                var simulation = new ScoreThresholdSimulation(1000, tileCount, 30, new Random(343));
                var results = simulation.Run();
                timer.Stop();
                Console.WriteLine($"Elapsed time is {elapsedTime}ms.");
                Assert.That(elapsedTime, Is.LessThan(1000));
            }
        }
    }
}
