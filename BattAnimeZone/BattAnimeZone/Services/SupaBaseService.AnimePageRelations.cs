using BattAnimeZone.Services.Interfaces;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.Json;

namespace BattAnimeZone.Services
{
	public partial class SupaBaseService
	{
		public async Task<AnimeRelationsKeyDTO?> GetRelations(int mal_id)
		{
			var response = await _client.Rpc("get_anime_relations_by_parent_id", new { _parent_id = mal_id });
			var returnDto = JsonSerializer.Deserialize<List<AnimeRelationsKeyDTO>>(response.Content, jsonOptions)?.FirstOrDefault();
			return returnDto;

		}
	}
}
