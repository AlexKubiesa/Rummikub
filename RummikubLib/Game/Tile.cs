using System;

namespace RummikubLib.Game
{
    class Tile : ITile
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
    }
}