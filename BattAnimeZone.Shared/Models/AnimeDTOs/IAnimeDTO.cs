namespace BattAnimeZone.Shared.Models.AnimeDTOs

{
    public interface IAnimeDTO
    {
        string TitleEnglish { get; }
        string TitleJapanese { get; }
        public int Year { get; }
        float Score { get; }
        int Popularity { get; }
        string MediaType { get; }
    }
}
