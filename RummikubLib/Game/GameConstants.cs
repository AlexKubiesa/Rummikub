using System.Collections.Generic;

namespace RummikubLib.Game
{
    static class GameConstants
    {
        public static IReadOnlyCollection<TileColor> NumberedTileColors { get; } = new[]
        {
            TileColor.Black, TileColor.Blue, TileColor.Red, TileColor.Yellow
        };

        public static IReadOnlyCollection<int> NumberedTileValues { get; } = new[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13
        };
    }
}