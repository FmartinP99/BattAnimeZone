using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using DotNetEnv;
using BattAnimeZone.DatabaseModels.SQliteDatabaseModels;

namespace BattAnimeZone.Services.DataBase
{
    public partial class DataBaseService
	{

		public async Task<IEnumerable<AnimeHomePageDTO>?> GetAnimesForHomePageByYear(int year)
		{
			
			using (var _context = await _dbContextFactory.CreateDbContextAsync())
			{
				IEnumerable<AnimeModel> animes_by_year = _context.Animes.Where(anime => anime.Year == year).OrderBy(anime => anime.Popularity);

                var animeIds = animes_by_year.Select(anime => anime.Mal_id).ToList();

                var animeGenres = _context.AnimeGenres
                    .Where(ag => animeIds.Contains(ag.AnimeId) && ag.IsTheme == false)
                    .Join(_context.Genres, ag => ag.GenreId, g => g.Mal_id, (ag, g) => new
                    {
                        ag.AnimeId,
                        GenreName = g.Name
                    })
                    .GroupBy(ag => ag.AnimeId)
                    .ToList();

                foreach (var anime in animes_by_year)
                {
                    var genres = animeGenres
                        .Where(ag => ag.Key == anime.Mal_id)
                        .SelectMany(ag => ag.Select(g => g.GenreName))
                        .ToList();

                    anime.Genres = genres;
                }

                return await Task.FromResult(dataBaseMapper.Map<IEnumerable<AnimeHomePageDTO>>(animes_by_year));
			}
            
		}
	}
}
