using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {
        public async Task<List<LiAnimeDTO>> GetFilteredAnimes(List<int>? genres, List<string>? mediaTypes, int? yearlower, int? yearupper)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                var query = (from a in _context.Animes
                             join ag in _context.AnimeGenres on a.Mal_id equals ag.AnimeId
                             where (genres == null || !genres.Any() || genres.Contains(ag.GenreId))
                             group ag by new { a.Mal_id, a.Title, a.TitleEnglish, a.TitleJapanese, a.MediaType, a.Episodes, a.Status, a.Rating, a.Score, a.Popularity, a.Year, a.ImageLargeWebpUrl } into g
                             where (genres == null || !genres.Any() || g.Select(ag => ag.GenreId).Distinct().Count() == genres.Count)
                             && (mediaTypes == null || !mediaTypes.Any() || mediaTypes.Contains(g.Key.MediaType))
                             && (!yearlower.HasValue || g.Key.Year >= yearlower.Value)
                             && (!yearupper.HasValue || g.Key.Year <= yearupper.Value)
                                             select new LiAnimeDTO
                             {
                                 Mal_id = g.Key.Mal_id,
                                 Title = g.Key.Title,
                                 TitleEnglish = g.Key.TitleEnglish,
                                 TitleJapanese = g.Key.TitleJapanese,
                                 MediaType = g.Key.MediaType,
                                 Episodes = g.Key.Episodes,
                                 Status = g.Key.Status,
                                 Rating = g.Key.Rating,
                                 Score = g.Key.Score,
                                 Popularity = g.Key.Popularity,
                                 Year = g.Key.Year,
                                 ImageLargeWebpUrl = g.Key.ImageLargeWebpUrl
                             }).ToList();

                return query;
            }

        }
    }
}
