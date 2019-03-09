using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class PartitionProvider : IPartitionProvider
    {
        public static IPartitionProvider Instance { get; } = new PartitionProvider();

        PartitionProvider()
        {
        }

        public IEnumerable<IReadOnlyCollection<ITile>> GetPartition(IReadOnlyCollection<ITile> tiles)
        {
            var partition = new List<HashSet<ITile>>();

            var maximalScoringSets = ScoringSet.GetMaximalScoringSetsUpToEquivalence(tiles).ToList();

            while (maximalScoringSets.Count > 0)
            {
                var newComponent = new HashSet<ITile>(maximalScoringSets[maximalScoringSets.Count - 1].Tiles);
                partition.Add(newComponent);
                maximalScoringSets.RemoveAt(maximalScoringSets.Count - 1);

                bool newScoringSetFound;
                do
                {
                    var scoringSetsToMerge = maximalScoringSets.Where(x =>
                        newComponent.Intersect(x.Tiles, TileEqualityComparerByValue.Instance).Any())
                        .ToArray();

                    foreach (var scoringSet in scoringSetsToMerge)
                    {
                        newComponent.UnionWith(scoringSet.Tiles);
                        maximalScoringSets.Remove(scoringSet);
                    }

                    newScoringSetFound = scoringSetsToMerge.Length != 0;
                } while (newScoringSetFound);
            }

            var singletonTiles = new List<ITile>();
            foreach (var tile in tiles)
            {
                // ReSharper disable once PossibleUnintendedLinearSearchInSet
                // We want to use a specific equality comparer here, so we have to use the LINQ Contains method.
                var component = partition.FirstOrDefault(x => x.Contains(tile, TileEqualityComparerByValue.Instance));

                if (component == null)
                {
                    // The tile will be in its own component.
                    singletonTiles.Add(tile);
                }
                else
                {
                    // Add the tile, in case the component contains an equivalent tile but does not contain this tile.
                    component.Add(tile);
                }
            }

            foreach (var tile in singletonTiles)
            {
                partition.Add(new HashSet<ITile> { tile });
            }

            return partition;
        }
    }
}