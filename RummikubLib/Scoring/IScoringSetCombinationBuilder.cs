using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    public interface IScoringSetCombinationBuilder
    {
        IReadOnlyCollection<ITile> Hand { get; }

        bool TryPush(IScoringSet scoringSet);

        IScoringSet Pop();

        IEnumerable<IScoringSet> GetScoringSetCombination();
    }
}