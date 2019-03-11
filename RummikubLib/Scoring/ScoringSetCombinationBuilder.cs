using System;
using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoringSetCombinationBuilder : IScoringSetCombinationBuilder
    {
        readonly List<IScoringSet> scoringSets = new List<IScoringSet>();

        readonly Dictionary<ITile, int> tileCountsForHand;

        readonly Dictionary<ITile, int> currentTileCounts;

        public ScoringSetCombinationBuilder(IReadOnlyCollection<ITile> hand)
        {
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));

            tileCountsForHand = hand.ToDictionary(tile => tile,
                tile => hand.Count(otherTile => TileEqualityComparerByValue.Instance.Equals(tile, otherTile)),
                TileEqualityComparerByValue.Instance);

            currentTileCounts = new Dictionary<ITile, int>(tileCountsForHand);
        }

        public IReadOnlyCollection<ITile> Hand { get; }

        public bool TryPush(IScoringSet scoringSet)
        {
            if (scoringSet.Tiles.Any(tile => currentTileCounts[tile] == tileCountsForHand[tile]))
            {
                return false;
            }

            scoringSets.Add(scoringSet);

            foreach (var tile in scoringSet.Tiles)
            {
                currentTileCounts[tile] += 1;
            }

            return true;
        }

        public IScoringSet Pop()
        {
            var scoringSet = scoringSets[scoringSets.Count - 1];

            scoringSets.RemoveAt(scoringSets.Count - 1);

            foreach (var tile in scoringSet.Tiles)
            {
                currentTileCounts[tile] -= 1;
            }

            return scoringSet;
        }

        public IEnumerable<IScoringSet> GetScoringSetCombination()
        {
            return scoringSets.ToArray();
        }
    }
}