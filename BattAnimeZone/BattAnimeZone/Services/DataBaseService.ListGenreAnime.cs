using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {
        public async Task<LiGenreAnimeDTOContainer?> GetAnimesForListGenreAnimes(int genre_id)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync()) {

                var genreAnimes = await (from ag in _context.AnimeGenres
                                         join g in _context.Genres on ag.GenreId equals g.Mal_id
                                         join a in _context.Animes on ag.AnimeId equals a.Mal_id
                                         where ag.GenreId == genre_id
                                         group new { a, g, ag } by new
                                         {
                                             g.Name
                                         } into grp
                                         select new LiGenreAnimeDTOContainer
                                         {
                                             GenreName = grp.Key.Name,
                                             Animes = grp.Select(x => new LiGenreAnimeDTO
                                             {
                                                 Mal_id = x.a.Mal_id,
                                                 Title = x.a.Title,
                                                 TitleEnglish = x.a.TitleEnglish,
                                                 TitleJapanese = x.a.TitleJapanese,
                                                 MediaType = x.a.MediaType,
                                                 Episodes = x.a.Episodes,
                                                 Score = x.a.Score,
                                                 Popularity = x.a.Popularity,
                                                 Year = x.a.Year,
                                                 ImageLargeWebpUrl = x.a.ImageLargeWebpUrl
                                             }).ToList()
                                         }
                                         ).FirstOrDefaultAsync();
                return genreAnimes;

            }
        }
    }
}
