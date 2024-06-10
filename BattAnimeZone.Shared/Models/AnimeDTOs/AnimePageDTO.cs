namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    using BattAnimeZone.Shared.Models.Anime;
    public class AnimePageDTO
    {
        public int Mal_id { get; set; } = -1;
        public string Title { get; set; } = string.Empty;
        public string Title_english { get; set; } = string.Empty;
        public string Title_japanese { get; set; } = string.Empty;
        public string Media_type { get; set; } = string.Empty;
        public int Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public float Score { get; set; } = -1;
        public float Rank { get; set; } = -1;
        public float Popularity { get; set; } = -1;
        public string Synopsis { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public int Year { get; set; } = -1;
        public List<Entry> Producers { get; set; }
        public List<Entry> Licensors { get; set; }
        public List<Entry> Studios { get; set; }
        public List<Entry> Genres { get; set; }
        public List<Entry> Themes { get; set; }
        public string Image_large_webp_url { get; set; } = string.Empty;
        public string Aired_string { get; set; } = string.Empty;
    }
}
