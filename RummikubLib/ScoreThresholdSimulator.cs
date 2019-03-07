using System;
using System.Collections.Generic;
using RummikubLib.Game;
using RummikubLib.Scoring;

namespace RummikubLib
{
    public class ScoreThresholdSimulator : IScoreThresholdSimulator
    {
        readonly IScoreCharacteriser scoreCharacteriser;

        public ScoreThresholdSimulator(IScoreCharacteriser scoreCharacteriser)
        {
            this.scoreCharacteriser = scoreCharacteriser ?? throw new ArgumentNullException(nameof(scoreCharacteriser));
        }

        public IScoreThresholdSimulationResults Run(int trialCount, int tileCount, int threshold)
        {
            var results = new ScoreThresholdSimulationResults();

            for (int i = 0; i < trialCount; ++i)
            {
                var result = new Trial(tileCount, threshold, scoreCharacteriser).Run();
                results.AddResult(result);
            }

            return results;
        }

        class Trial
        {
            readonly int tileCount;
            readonly int threshold;
            readonly IScoreCharacteriser scoreCharacteriser;

            public Trial(int tileCount, int threshold, IScoreCharacteriser scoreCharacteriser)
            {
                this.tileCount = tileCount;
                this.threshold = threshold;
                this.scoreCharacteriser = scoreCharacteriser;
            }

            public Result Run()
            {
                var bag = new TileBag();
                var tiles = new List<ITile>(tileCount);

                for (int i = 0; i < tileCount; ++i)
                {
                    tiles.Add(bag.DrawTile());
                }

                return scoreCharacteriser.IsScoreLessThanThreshold(tiles, threshold);
            }
        }
    }
}