namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class AnimeSearchResultDTO
    {
        public int Mal_id { get; set; } = -1;
        public string Title_english { get; set; } = string.Empty;
        public string Title_japanese { get; set; } = string.Empty;
        public string Media_type { get; set; } = string.Empty;
        public int Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
        public float Score { get; set; } = -1;
        public string Season { get; set; } = string.Empty;
        public int Year { get; set; } = -1;
        public string Image_large_webp_url { get; set; } = string.Empty;

    }
}
