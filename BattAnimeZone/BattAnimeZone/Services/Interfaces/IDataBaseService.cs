using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.GenreDTOs;

namespace BattAnimeZone.Services.Interfaces
{
	public interface IDataBaseService
	{
		public Task<List<string?>?> GetDistinctMediaTypes();
		public Task<List<int?>?> GetDistinctYears();
		public Task<List<AnimeGenreDTO>?> GetGenres();
		public Task<Dictionary<int, int>?> GetAnimesPerGenreIdCount();
		public Task<AnimeRelationsKeyDTO?> GetRelations(int mal_id);
		public Task<List<LiAnimeDTO>?> GetFilteredAnimes(List<int>? genres, List<string>? mediaTypes, int? yearlower, int? yearupper);
		public Task<LiGenreAnimeDTOContainer?> GetAnimesForListGenreAnimes(int genre_id);
    }
}
