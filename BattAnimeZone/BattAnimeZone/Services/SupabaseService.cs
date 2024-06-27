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
	public class SupabaseService : IDataBaseService
	{
		private Supabase.Client? _client;
		JsonSerializerOptions jsonOptions;

		private struct GenreCountDTO
		{
			public int GenreId { get; set; }
			public int AnimeCount { get; set; }
		}

		public SupabaseService(Supabase.Client? client)
        {
			_client = client;

			jsonOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true, 
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
			};
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
            await Console.Out.WriteLineAsync(response.Content);

			var genreCounts = JsonSerializer.Deserialize<List<GenreCountDTO>>(response.Content, jsonOptions);

			foreach(var item in genreCounts)
			{
				return_dict[item.GenreId] = item.AnimeCount;
			}

			return return_dict;

		}

	}
}
