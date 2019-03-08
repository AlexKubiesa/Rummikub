using System;

namespace RummikubLib.Game
{
    public class Tile : ITile
    {
        public static ITile CreateNumberedTile(TileColor color, int value)
        {
            if (value < 1 || value > 13)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new Tile(color, value, false);
        }

        public static ITile CreateJoker()
        {
            return new Tile(TileColor.None, 0, true);
        }

        readonly int value;

        Tile(TileColor color, int value, bool isJoker)
        {
            Color = color;
            this.value = value;
            IsJoker = isJoker;
        }

        public TileColor Color { get; }

        public int Value
        {
            get
            {
                if (IsJoker)
                {
                    throw new InvalidOperationException("Jokers do not have a numeric value.");
                }

                return value;
            }
        }

        public bool IsJoker { get; }
    }
}