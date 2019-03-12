using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Model
{
    public interface IScoringSet
    {
        IReadOnlyCollection<ITile> Tiles { get; }

        ScoringSetType Type { get; }

        int GetScore();
    }
}