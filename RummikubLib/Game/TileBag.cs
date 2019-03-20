using System;
using System.Collections.Generic;
using MathNet.Numerics;

namespace RummikubLib.Game
{
    public class TileBag : ITileBag
    {
        readonly Stack<ITile> tiles;

        public TileBag(Random randomSource = null)
        {
            var unshuffledTiles = new List<ITile>(GameConstants.TileCount);

            foreach (var color in GameConstants.NumberedTileColors)
            {
                foreach (int value in GameConstants.NumberedTileValues)
                {
                    var tileClass = TileClass.CreateNumberedTileClass(color, value);
                    unshuffledTiles.Add(Tile.Create(1, tileClass));
                    unshuffledTiles.Add(Tile.Create(2, tileClass));
                }
            }

            unshuffledTiles.Add(Tile.Create(1, TileClass.Joker));
            unshuffledTiles.Add(Tile.Create(2, TileClass.Joker));

            tiles = new Stack<ITile>(unshuffledTiles.SelectPermutation(randomSource));
        }

        public int Count => tiles.Count;

        public ITile DrawTile()
        {
            return tiles.Pop();
        }
    }
}