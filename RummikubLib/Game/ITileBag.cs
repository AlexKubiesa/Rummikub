namespace RummikubLib.Game
{
    public interface ITileBag
    {
        int Count { get; }

        ITile DrawTile();
    }
}