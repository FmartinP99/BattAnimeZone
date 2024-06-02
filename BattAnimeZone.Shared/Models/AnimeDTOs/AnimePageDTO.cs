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
        public float Episodes { get; set; } = -1;
        public string Status { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public float Score { get; set; } = -1;
        public float Rank { get; set; } = -1;
        public float Popularity { get; set; } = -1;
        public string Synopsis { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public float Year { get; set; } = -1;
        public List<Producer> Producers { get; set; }
        public List<Licensor> Licensors { get; set; }
        public List<Studio> Studios { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Theme> Themes { get; set; }
        public string Image_large_webp_url { get; set; } = string.Empty;
        public string Aired_string { get; set; } = string.Empty;
    }
}
