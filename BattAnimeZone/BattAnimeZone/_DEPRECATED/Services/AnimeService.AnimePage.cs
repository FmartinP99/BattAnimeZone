using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;


#pragma warning disable CS1998
namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public async Task<AnimePageDTO> GetAnimePageDTOByID(int mal_id)
        {
            Anime? return_anime;
            if (this.animes.TryGetValue(mal_id, out return_anime)) return animeMapper.Map<AnimePageDTO>(return_anime);
            return new AnimePageDTO();
        }
    }
}
