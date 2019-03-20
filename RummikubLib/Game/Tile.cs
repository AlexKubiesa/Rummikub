using System;

namespace RummikubLib.Game
{
    class Tile : ITile, IEquatable<Tile>
    {
        public static ITile Create(int id, ITileClass @class)
        {
            if (@class == null)
            {
                throw new ArgumentNullException(nameof(@class));
            }

            return new Tile(id, @class);
        }

        Tile(int id, ITileClass @class)
        {
            Id = id;
            Class = @class;
        }

        public int Id { get; }

        public ITileClass Class { get; }

        public TileColor Color => Class.Color;

        public int Value => Class.Value;

        public bool IsJoker => Class.IsJoker;

        public bool Equals(Tile other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id && Equals(Class, other.Class);
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

            return Equals((Tile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Class != null ? Class.GetHashCode() : 0);
            }
        }
    }
}