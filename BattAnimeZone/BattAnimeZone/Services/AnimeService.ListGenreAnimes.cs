using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public async Task<List<LiGenreAnimeDTO>> GetAnimesForListGenreAnimes(int genre_id)
        {
            List<Anime> apgs;
            if (this.animesPerGenre.TryGetValue(genre_id, out apgs)) return animeMapper.Map<List<LiGenreAnimeDTO>>(apgs);
            return new List<LiGenreAnimeDTO> { new LiGenreAnimeDTO() };
        }

        public async Task<string> GetGenreNameById(int genre_id)
        {
            return this.genres[genre_id].Name;
        }

    }
}
