using System.Collections.Generic;
using RummikubLib.Game;

namespace RummikubLib.Scoring.Calculation
{
    /// <summary>
    /// Partitions the hand into a set of components whose scores can be computed individually.
    /// </summary>
    public interface IPartitionProvider
    {
        IEnumerable<IReadOnlyCollection<ITile>> GetPartition(IReadOnlyCollection<ITile> tiles);
    }
}