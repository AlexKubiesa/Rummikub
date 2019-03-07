using System.Collections.Generic;
using System.Linq;

namespace RummikubLib.Game
{
    public class TileBag : ITileBag
    {
        static readonly TileColor[] ColorsForNumberedTiles =
        {
            TileColor.Black,
            TileColor.Blue,
            TileColor.Red,
            TileColor.Yellow
        };

        static readonly int[] ValuesForNumberedTiles = Enumerable.Range(1, 13).ToArray();

        readonly IList<ITile> tiles;

        public TileBag()
        {
            var unshuffledTiles = new List<ITile>(2 * ColorsForNumberedTiles.Length * ValuesForNumberedTiles.Length + 2);

            foreach (var color in ColorsForNumberedTiles)
            {
                foreach (int value in ValuesForNumberedTiles)
                {
                    unshuffledTiles.Add(Tile.CreateNumberedTile(color, value));
                    unshuffledTiles.Add(Tile.CreateNumberedTile(color, value));
                }
            }

            unshuffledTiles.Add(Tile.CreateJoker());
            unshuffledTiles.Add(Tile.CreateJoker());

            tiles = unshuffledTiles.Shuffle();
        }

        public int Count => tiles.Count;

        public ITile DrawTile()
        {
            var tile = tiles[tiles.Count - 1];
            tiles.RemoveAt(tiles.Count - 1);
            return tile;
        }
    }
}