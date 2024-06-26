﻿using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.GenreDTOs;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;

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
		public Task<List<LiProductionEntityDTO>?> GetProductionEntitiesDTO();
		public Task<ProductionEntityPageDTO?> GetAnimesForProdEnt(int mal_id);
		public Task<List<AnimeSearchResultDTO>?> GetSimilarAnimesForSearchResult(int n, string name);
		public Task<AnimePageDTO?> GetAnimePageDTOByID(int mal_id);
		public Task<IEnumerable<AnimeHomePageDTO>?> GetAnimesForHomePageByYear(int year);
    }
}
