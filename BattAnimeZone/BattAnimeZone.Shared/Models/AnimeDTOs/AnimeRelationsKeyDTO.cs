using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class AnimeRelationsKeyDTO
    {
        public int Mal_id { get; set; } = -1;
        public string TitleEnglish { get; set; } = string.Empty;
        public string TitleJapanese { get; set; } = string.Empty;
        public List<AnimeRelationDTO> Relations { get; set; }
    }
}
