using System.Collections.Generic;
using System.Linq;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public class ScoreCharacteriser : IScoreCharacteriser
    {
        public Result IsScoreLessThanThreshold(IReadOnlyCollection<ITile> tiles, int threshold)
        {
            return new Calculation(tiles.ToList(), threshold).Execute();
        }

        static IScoringSet GetMinimalScoringSetOrDefault(IEnumerable<ITile> tiles)
        {
            return tiles.GetCombinations(3).Select(ScoringSet.CreateOrDefault).FirstOrDefault(x => x != null);
        }

        static IScoringSet GetNonConflictingScoringSetOrDefault(IEnumerable<ITile> tiles)
        {
            var minimalScoringSets = tiles.GetCombinations(3).Select(ScoringSet.CreateOrDefault).Where(x => x != null);
            return minimalScoringSets.FirstOrDefault(set =>
                minimalScoringSets.All(
                    other => other.Tiles.SequenceEqual(set.Tiles) || !ScoringSetsConflict(set, other)));
        }

        static bool ScoringSetsConflict(IScoringSet first, IScoringSet second)
        {
            return first.Tiles.Intersect(second.Tiles).Any();
        }

        class Calculation
        {
            readonly ICollection<ITile> tiles;

            int threshold;

            public Calculation(ICollection<ITile> tiles, int threshold)
            {
                this.tiles = tiles;
                this.threshold = threshold;
            }

            public Result Execute()
            {
                if (threshold <= 0)
                {
                    return Result.No;
                }

                RemoveJokers();

                var setToRemove = GetNonConflictingScoringSetOrDefault(tiles);
                while (setToRemove != null)
                {
                    RemoveScoringSet(setToRemove);

                    if (threshold <= 0)
                    {
                        return Result.No;
                    }

                    setToRemove = GetNonConflictingScoringSetOrDefault(tiles);
                }

                return GetMinimalScoringSetOrDefault(tiles) == null
                    ? Result.Yes
                    : Result.Maybe;
            }

            void RemoveJokers()
            {
                var jokers = tiles.Where(t => t.IsJoker).ToArray();

                foreach (var joker in jokers)
                {
                    tiles.Remove(joker);
                }
            }

            void RemoveScoringSet(IScoringSet scoringSet)
            {
                foreach (var tile in scoringSet.Tiles)
                {
                    tiles.Remove(tile);
                }

                threshold -= scoringSet.GetScore();
            }
        }
    }
}