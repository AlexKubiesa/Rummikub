namespace RummikubLib.Game
{
    public interface ITile
    {
        TileColor Color { get; }

        int Value { get; }

        bool IsJoker { get; }
    }
}