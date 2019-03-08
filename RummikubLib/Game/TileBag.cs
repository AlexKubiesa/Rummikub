using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

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

        readonly Stack<ITile> tiles;

        public TileBag(Random randomSource = null)
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

            tiles = new Stack<ITile>(unshuffledTiles.SelectPermutation(randomSource));
        }

        public int Count => tiles.Count;

        public ITile DrawTile()
        {
            return tiles.Pop();
        }
    }
}