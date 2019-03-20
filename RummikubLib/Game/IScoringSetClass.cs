using System.Collections.Generic;

namespace RummikubLib.Game
{
    public interface IScoringSetClass : IReadOnlyCollection<ITileClass>
    {
        ScoringSetType Type { get; }

        int GetScore();
    }
}