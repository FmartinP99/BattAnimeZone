using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.DatabaseModels;

namespace BattAnimeZone.Services
{
	public partial class DataBaseService
	{
		public Task<IEnumerable<AnimeHomePageDTO>> GetAnimesForHomePageByYear(int year)
		{

			using (var _context = _dbContextFactory.CreateDbContext())
			{
				IEnumerable<AnimeModel> animes_by_year = _context.Animes.Where(anime => anime.Year == year).OrderBy(anime => anime.Popularity);
				return Task.FromResult(dataBaseMapper.Map<IEnumerable<AnimeHomePageDTO>>(animes_by_year));
			}
		}
	}
}
