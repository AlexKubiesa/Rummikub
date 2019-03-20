namespace RummikubLib.Game
{
    public interface ITileClass
    {
        TileColor Color { get; }

        int Value { get; }

        bool IsJoker { get; }
    }
}