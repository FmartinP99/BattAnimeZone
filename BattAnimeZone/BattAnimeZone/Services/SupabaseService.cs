using BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels;
using BattAnimeZone.Services.Interfaces;
using BattAnimeZone.Shared.Models.GenreDTOs;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BattAnimeZone.Services
{
	public partial class SupaBaseService : IDataBaseService
	{
		private Supabase.Client? _client;
		private JsonSerializerOptions jsonOptions;

		private struct AnimeCountByGenreDTO
		{
			public int GenreId { get; set; }
			public int AnimeCount { get; set; }
		}




		public SupaBaseService(Supabase.Client? client)
        {
			_client = client;

			jsonOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true, 
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
			};
		}





		public async Task<List<string?>> GetDistinctMediaTypes()
		{
			var response = await _client.From<DistinctMediaTypesSupabasaeModel>()
									.Select("media_type").Get();

			var media_types = response.Models.Select(x => x.mediaType).ToList();

			return media_types;
		}

		public async Task<List<int?>> GetDistinctYears()
		{
			var response = await _client.From<DistinctYearSupabaseModel>()
									.Select("year").Get();

			var years = response.Models.Select(x=> x.Year).ToList();

			return years;

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

		public async Task<Dictionary<int, int>> GetAnimesPerGenreIdCount()
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
