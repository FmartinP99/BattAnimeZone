using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.DatabaseModels;

namespace BattAnimeZone.Services
{
	public partial class DataBaseService
	{
		public async Task<IEnumerable<AnimeHomePageDTO>> GetAnimesForHomePageByYear(int year)
		{
			using (var _context = await _dbContextFactory.CreateDbContextAsync())
			{
				IEnumerable<AnimeModel> animes_by_year = _context.Animes.Where(anime => anime.Year == year).OrderBy(anime => anime.Popularity);
			
			return await Task.FromResult(dataBaseMapper.Map<IEnumerable<AnimeHomePageDTO>>(animes_by_year));
			}
		}
	}
}
