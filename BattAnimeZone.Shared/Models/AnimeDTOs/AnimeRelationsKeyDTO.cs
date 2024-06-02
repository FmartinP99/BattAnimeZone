using BattAnimeZone.Shared.Models.Anime;

namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class AnimeRelationsKeyDTO
    {
        public int Mal_id { get; set; } = -1;
        public string Title_english { get; set; } = string.Empty;
        public string Title_japanese { get; set; } = string.Empty;
        public List<Relations> Relations { get; set; }
    }
}
