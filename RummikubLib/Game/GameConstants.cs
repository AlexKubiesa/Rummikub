using System.Collections.Generic;
using System.Linq;
using RummikubLib.Collections;

namespace RummikubLib.Game
{
    static class GameConstants
    {
        public const int TileCount = 106;

        static GameConstants()
        {
            MaximalRunClasses = GetMaximalRunClasses().ToArray();
            MaximalGroupClasses = GetMaximalGroupClasses().ToArray();
            MaximalScoringSetClasses = MaximalRunClasses.Concat(MaximalGroupClasses).ToArray();
        }

        public static IReadOnlyCollection<TileColor> NumberedTileColors { get; } = new[]
        {
            TileColor.Black, TileColor.Blue, TileColor.Red, TileColor.Yellow
        };

        public static IReadOnlyCollection<int> NumberedTileValues { get; } = new[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13
        };

        public static IReadOnlyCollection<IReadOnlyMultiset<ITileClass>> MaximalRunClasses { get; }

        public static IReadOnlyCollection<IReadOnlyMultiset<ITileClass>> MaximalGroupClasses { get; }

        public static IReadOnlyCollection<IReadOnlyMultiset<ITileClass>> MaximalScoringSetClasses { get; }

        static IEnumerable<IReadOnlyMultiset<ITileClass>> GetMaximalRunClasses()
        {
            return NumberedTileColors.Select(color => NumberedTileValues
                .Select(value => TileClass.CreateNumberedTileClass(color, value))
                .ToMultiset());
        }

        static IEnumerable<IReadOnlyMultiset<ITileClass>> GetMaximalGroupClasses()
        {
            return NumberedTileValues.Select(value => NumberedTileColors
                .Select(color => TileClass.CreateNumberedTileClass(color, value))
                .ToMultiset());
        }
    }
}