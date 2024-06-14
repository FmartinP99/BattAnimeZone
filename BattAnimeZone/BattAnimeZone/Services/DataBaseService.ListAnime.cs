using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Services
{
    public partial class DataBaseService
    {
        public async Task<List<LiAnimeDTO>> GetFilteredAnimes(List<int>? genres, List<string>? mediaTypes)
        {
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {


                List<LiAnimeDTO> query;
                if ((genres == null || !genres.Any()) && (mediaTypes == null || !mediaTypes.Any()))
                {
                    query = (from a in _context.Animes
                             join ag in _context.AnimeGenres on a.Mal_id equals ag.AnimeId
                             select new LiAnimeDTO
                             {
                                 Mal_id = a.Mal_id,
                                 TitleEnglish = a.TitleEnglish,
                                 TitleJapanese = a.TitleJapanese,
                                 MediaType = a.MediaType,
                                 Episodes = a.Episodes,
                                 Status = a.Status,
                                 Rating = a.Rating,
                                 Score = a.Score,
                                 Popularity = a.Popularity,
                                 Year = a.Year,
                                 ImageLargeWebpUrl = a.ImageLargeWebpUrl
                             }).Distinct().ToList();

                }

                else if (genres == null || !genres.Any())
                {
                    query = (from a in _context.Animes
                             join ag in _context.AnimeGenres on a.Mal_id equals ag.AnimeId
                             where mediaTypes.Contains(a.MediaType)
                             select new LiAnimeDTO
                             {
                                 Mal_id = a.Mal_id,
                                 TitleEnglish = a.TitleEnglish,
                                 TitleJapanese = a.TitleJapanese,
                                 MediaType = a.MediaType,
                                 Episodes = a.Episodes,
                                 Status = a.Status,
                                 Rating = a.Rating,
                                 Score = a.Score,
                                 Popularity = a.Popularity,
                                 Year = a.Year,
                                 ImageLargeWebpUrl = a.ImageLargeWebpUrl
                             }).Distinct().ToList();

                }

                else if (mediaTypes == null || !mediaTypes.Any())
                {

                    query = (from a in _context.Animes
                             join ag in _context.AnimeGenres on a.Mal_id equals ag.AnimeId
                             where genres.Contains(ag.GenreId)
                             select new LiAnimeDTO
                             {
                                 Mal_id = a.Mal_id,
                                 TitleEnglish = a.TitleEnglish,
                                 TitleJapanese = a.TitleJapanese,
                                 MediaType = a.MediaType,
                                 Episodes = a.Episodes,
                                 Status = a.Status,
                                 Rating = a.Rating,
                                 Score = a.Score,
                                 Popularity = a.Popularity,
                                 Year = a.Year,
                                 ImageLargeWebpUrl = a.ImageLargeWebpUrl
                             }).Distinct().ToList();
                }
                else
                {
                    query = (from a in _context.Animes
                             join ag in _context.AnimeGenres on a.Mal_id equals ag.AnimeId
                             where genres.Contains(ag.GenreId) && mediaTypes.Contains(a.MediaType)
                             select new LiAnimeDTO
                             {
                                 Mal_id = a.Mal_id,
                                 TitleEnglish = a.TitleEnglish,
                                 TitleJapanese = a.TitleJapanese,
                                 MediaType = a.MediaType,
                                 Episodes = a.Episodes,
                                 Status = a.Status,
                                 Rating = a.Rating,
                                 Score = a.Score,
                                 Popularity = a.Popularity,
                                 Year = a.Year,
                                 ImageLargeWebpUrl = a.ImageLargeWebpUrl
                             }).Distinct().ToList();
                }

                return query;
            }

        }
    }
}
