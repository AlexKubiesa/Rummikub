using System.Collections.Generic;
using System.Linq;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class PartitionProvider : IPartitionProvider
    {
        public static IPartitionProvider Instance { get; } = new PartitionProvider();

        PartitionProvider()
        {
        }

        public IEnumerable<IMultiset<ITileClass>> GetPartition(IReadOnlyMultiset<ITileClass> tiles)
        {
            var partition = new List<Multiset<ITileClass>>();

            var maximalScoringSetClasses = ScoringSetClass.GetMaximalScoringSetClasses(tiles).ToList();

            while (maximalScoringSetClasses.Count > 0)
            {
                var newComponentElementCounts = maximalScoringSetClasses[maximalScoringSetClasses.Count - 1]
                    .ToDictionary(x => x, tiles.CountOf);
                var newComponent = new Multiset<ITileClass>(newComponentElementCounts);
                partition.Add(newComponent);
                maximalScoringSetClasses.RemoveAt(maximalScoringSetClasses.Count - 1);

                bool newScoringSetFound;
                do
                {
                    var scoringSetClassesToMerge = maximalScoringSetClasses
                        .Where(x => newComponent.Intersect(x.ToMultiset()).DistinctCount != 0)
                        .ToArray();

                    foreach (var scoringSetClass in scoringSetClassesToMerge)
                    {
                        var scoringSetClassElementCounts = scoringSetClass
                            .ToDictionary(x => x, tiles.CountOf);
                        var multisetToMerge = new Multiset<ITileClass>(scoringSetClassElementCounts);

                        newComponent.UnionWith(multisetToMerge);
                        maximalScoringSetClasses.Remove(scoringSetClass);
                    }

                    newScoringSetFound = scoringSetClassesToMerge.Length != 0;
                } while (newScoringSetFound);
            }

            // For each tile class not yet in a component, put it into its own component.
            var singletonTileClasses = new List<ITileClass>();
            foreach (var tileClass in tiles.GetDistinctElements())
            {
                var component = partition.FirstOrDefault(x => x.Contains(tileClass));

                if (component == null)
                {
                    singletonTileClasses.Add(tileClass);
                }
            }

            foreach (var tileClass in singletonTileClasses)
            {
                var elementCounts = new Dictionary<ITileClass, int> { { tileClass, tiles.CountOf(tileClass) } };
                partition.Add(new Multiset<ITileClass>(elementCounts));
            }

            return partition;
        }
    }
}