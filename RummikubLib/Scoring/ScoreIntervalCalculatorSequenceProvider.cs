using System.Collections.Generic;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoreIntervalCalculatorSequenceProvider : IScoreIntervalCalculatorSequenceProvider
    {
        public static IScoreIntervalCalculatorSequenceProvider Instance { get; } = new ScoreIntervalCalculatorSequenceProvider();

        const int SmallToLargeThreshold = 50;

        static readonly IScoreIntervalCalculator[] SequenceForSmallTileCollections =
        {
            ValueSummingScoreIntervalCalculator.Instance,
            CombinationSamplingScoreIntervalCalculator.Instance,
            KnownScoringSetsScoreIntervalCalculator.Instance
        };

        static readonly IScoreIntervalCalculator[] SequenceForLargeTileCollections =
        {
            KnownScoringSetsScoreIntervalCalculator.Instance
        };

        ScoreIntervalCalculatorSequenceProvider()
        {
        }

        public IEnumerable<IScoreIntervalCalculator> GetScoreIntervalCalculators(IReadOnlyMultiset<ITileClass> tiles)
        {
            return tiles.TotalCount < SmallToLargeThreshold
                ? SequenceForSmallTileCollections
                : SequenceForLargeTileCollections;
        }
    }
}