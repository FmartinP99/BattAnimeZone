using BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels;
using BattAnimeZone.Services.Interfaces;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.GenreDTOs;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
	public partial class SupaBaseService : IDataBaseService
	{
		private Supabase.Client? _client;
		private JsonSerializerOptions jsonOptions;
        private SingletonSearchService _ssService;

        private struct AnimeCountByGenreDTO
		{
			public int GenreId { get; set; }
			public int AnimeCount { get; set; }
		}


		public SupaBaseService(Supabase.Client? client, SingletonSearchService ssService)
        {
			_client = client;

			jsonOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true, 
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
			};

            _ssService = ssService;
        }





		public async Task<List<string?>?> GetDistinctMediaTypes()
		{
            var response = await _client.Rpc("get_distinct_mediatypes", null);

            if (response != null)
            {
                var responseDto = JsonSerializer.Deserialize<List<DistinctMediaTypesSupabasaeModel>>(response.Content, jsonOptions);
                var returnDto = responseDto.Where(x => string.IsNullOrEmpty(x.mediaType) == false).Select(x => x.mediaType).ToList();
                return returnDto;
            }
            else
            {
                return new List<string?>();
            }
        }

		public async Task<List<int?>> GetDistinctYears()
		{
            var response = await _client.Rpc("get_distinct_years", null);

            if (response != null)
            {
                var responseDto = JsonSerializer.Deserialize<List<DistinctYearSupabaseModel>>(response.Content, jsonOptions);
				var returnDto = responseDto.Select(x => x.Year).ToList();
				return returnDto;
            }
            else
            {
                return new List<int?>();
            }

        }




		public async Task<List<AnimeGenreDTO>?> GetGenres()
		{
			var response = await _client.From<GenreSupabaseModel>()
									.Select(x => new object[] { x.Mal_id, x.Name }).Get();
			if (response == null) return null;

			List<AnimeGenreDTO>? animeGenreDTOs = response.Models.Select(response => new AnimeGenreDTO
			{
				Mal_id = response.Mal_id,
				Name = response.Name,
			}).ToList();

			return animeGenreDTOs;

		}

		public async Task<Dictionary<int, int>?> GetAnimesPerGenreIdCount()
		{
			

			Dictionary<int, int> return_dict = new Dictionary<int, int>();
			var response = await _client.Rpc("get_anime_count_by_genre", null);

			var genreCounts = JsonSerializer.Deserialize<List<AnimeCountByGenreDTO>>(response.Content, jsonOptions);

			foreach(var item in genreCounts)
			{
				return_dict[item.GenreId] = item.AnimeCount;
			}

			return return_dict;

		}

	}
}
