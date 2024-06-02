namespace BattAnimeZone.Shared.Models.AnimeDTOs

{
    public interface IAnimeDTO
    {
        string Title_english { get; }
        string Title_japanese { get; }
        public float Year { get; }
        float Score { get; }
        float Popularity { get; }
        string Media_type { get; }
    }
}
