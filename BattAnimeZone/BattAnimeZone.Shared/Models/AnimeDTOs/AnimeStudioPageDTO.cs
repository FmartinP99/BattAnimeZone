using BattAnimeZone.Shared.Models.Anime;

namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class AnimeStudioPageDTO
    {
        public int Mal_id { get; set; } = -1;
        public string TitleEnglish { get; set; } = string.Empty;
        public string TitleJapanese { get; set; } = string.Empty;
        public string ImageLargeWebpUrl { get; set; } = string.Empty;
    }
}
