using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Services
{
    public partial class AnimeService
    {
        public Task<IEnumerable<AnimeHomePageDTO>> GetAnimesForHomePageByYear(int year)
        {
            IEnumerable<Anime> animes_by_year = this.animes.Where(anime => anime.Value.Year == year).OrderBy(anime => anime.Value.Popularity).Select(anime => anime.Value);
            return Task.FromResult(animeMapper.Map<IEnumerable<AnimeHomePageDTO>>(animes_by_year));
        }
    }
}
