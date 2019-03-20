namespace RummikubLib.Game
{
    public interface ITile
    {
        int Id { get; }

        ITileClass Class { get; }

        TileColor Color { get; }

        int Value { get; }

        bool IsJoker { get; }
    }
}