namespace BattAnimeZone.Shared.Models.AnimeDTOs

{
    public class AnimeRelationDTO
    {
        public int Mal_id { get; set; } = -1;
        public string Title_english { get; set; } = string.Empty;
        public string Title_japanese { get; set; } = string.Empty;
        public string Media_type { get; set; } = string.Empty;
        public int Episodes { get; set; } = -1;
        public float Score { get; set; } = -1;
        public int Popularity { get; set; } = -1;
        public int Year { get; set; } = -1;
        public string Image_large_webp_url { get; set; } = string.Empty;
    }
}
