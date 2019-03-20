using System.Collections.Generic;

namespace RummikubLib.Game
{
    public interface IScoringSet
    {
        IReadOnlyCollection<ITile> Tiles { get; }

        ScoringSetType Type { get; }

        int GetScore();
    }
}