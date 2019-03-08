using System;
using System.Collections.Generic;
using RummikubLib.Game;
using RummikubLib.Scoring;

namespace RummikubLib.Simulation
{
    public class ScoreThresholdSimulation : IBernoulliSamplingSimulation
    {
        readonly Random randomSource;

        public ScoreThresholdSimulation(int trialCount, int tileCount, int threshold, Random randomSource = null)
        {
            if (trialCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(trialCount));
            }

            if (tileCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tileCount));
            }

            if (threshold <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold));
            }

            this.randomSource = randomSource ?? new Random();

            TrialCount = trialCount;
            TileCount = tileCount;
            Threshold = threshold;
        }

        public int TrialCount { get; }

        public int TileCount { get; }

        public int Threshold { get; }

        public bool IsComplete => Results != null;

        public IBernoulliSamplingPartialResults Results { get; private set; }

        public IBernoulliSamplingPartialResults Run()
        {
            if (Results != null)
            {
                throw new InvalidOperationException("The simulation has already been run.");
            }

            var results = new BernoulliSamplingPartialResults();

            for (int i = 0; i < TrialCount; ++i)
            {
                var result = new Trial(TileCount, Threshold, randomSource).Run();
                results.AddResult(result);
            }

            Results = results;
            return Results;
        }

        class Trial
        {
            readonly int tileCount;
            readonly int threshold;
            readonly Random randomSource;

            public Trial(int tileCount, int threshold, Random randomSource)
            {
                this.tileCount = tileCount;
                this.threshold = threshold;
                this.randomSource = randomSource;
            }

            public Result Run()
            {
                var bag = new TileBag(randomSource);
                var tiles = new List<ITile>(tileCount);

                for (int i = 0; i < tileCount; ++i)
                {
                    tiles.Add(bag.DrawTile());
                }

                return ScoreCharacteriser.Instance.IsScoreLessThanThreshold(tiles, threshold);
            }
        }
    }
}