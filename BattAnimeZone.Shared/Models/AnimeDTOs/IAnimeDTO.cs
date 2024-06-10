namespace BattAnimeZone.Shared.Models.AnimeDTOs

{
    public interface IAnimeDTO
    {
        string Title_english { get; }
        string Title_japanese { get; }
        public int Year { get; }
        float Score { get; }
        int Popularity { get; }
        string Media_type { get; }
    }
}
