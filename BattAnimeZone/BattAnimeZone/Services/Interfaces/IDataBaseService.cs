using BattAnimeZone.Shared.Models.GenreDTOs;

namespace BattAnimeZone.Services.Interfaces
{
	public interface IDataBaseService
	{
		public Task<List<AnimeGenreDTO>?> GetGenres();
		public Task<Dictionary<int, int>> GetAnimesPerGenreIdCount();

	}
}
