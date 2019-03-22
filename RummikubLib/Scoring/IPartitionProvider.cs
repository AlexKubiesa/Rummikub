using System.Collections.Generic;
using RummikubLib.Collections;
using RummikubLib.Game;

namespace RummikubLib.Scoring
{
    /// <summary>
    /// Partitions the hand into a set of components whose scores can be computed individually.
    /// </summary>
    public interface IPartitionProvider
    {
        IEnumerable<IMultiset<ITileClass>> GetPartition(IReadOnlyMultiset<ITileClass> tiles);
    }
}