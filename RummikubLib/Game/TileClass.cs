using System;

namespace RummikubLib.Game
{
    class TileClass : ITileClass, IEquatable<TileClass>
    {
        const int JokerHashCode = 1;

        public static ITileClass Joker { get; } = new TileClass(TileColor.None, 0, true);

        public static ITileClass CreateNumberedTileClass(TileColor color, int value)
        {
            if (color == TileColor.None)
            {
                throw new ArgumentOutOfRangeException(nameof(color));
            }

            if (value < 1 || value > 13)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new TileClass(color, value, false);
        }

        readonly int value;

        TileClass(TileColor color, int value, bool isJoker)
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

        public bool Equals(TileClass other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsJoker && other.IsJoker)
            {
                return true;
            }

            if (IsJoker || other.IsJoker)
            {
                return false;
            }

            return Color == other.Color && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TileClass) obj);
        }

        public override int GetHashCode()
        {
            if (IsJoker)
            {
                return JokerHashCode;
            }

            unchecked
            {
                return ((int)Color * 397) ^ Value;
            }
        }
    }
}