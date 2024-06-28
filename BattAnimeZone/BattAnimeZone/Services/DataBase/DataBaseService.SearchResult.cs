using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.EntityFrameworkCore;

namespace BattAnimeZone.Services.DataBase
{
    public partial class DataBaseService
    {
        public async Task<List<AnimeSearchResultDTO>?> GetSimilarAnimesForSearchResult(int n, string name)
        {
			int[] similar_anime_ids = _ssService.GetSimilarAnimesForSearchResult(n, name);
            using (var _context = await _dbContextFactory.CreateDbContextAsync())
            {
                List<AnimeSearchResultDTO>? animes = await _context.Animes
                   .Where(a => similar_anime_ids.Contains(a.Mal_id))
                   .Select(a => new AnimeSearchResultDTO
                   {
                       Mal_id = a.Mal_id,
                       Title = a.Title,
                       TitleEnglish = a.TitleEnglish,
                       TitleJapanese = a.TitleJapanese,
                       MediaType = a.MediaType,
                       Episodes = a.Episodes,
                       Status = a.Status,
                       Score = a.Score,
                       Season = a.Season,
                       Year = a.Year,
                       ImageLargeWebpUrl = a.ImageLargeWebpUrl
                   })
                   .ToListAsync();

                if (animes != null)
                {
                    animes = animes
                        .OrderBy(a => Array.IndexOf(similar_anime_ids, a.Mal_id))
                        .ToList();
                }
                return animes;
            }


        }
    }
}
