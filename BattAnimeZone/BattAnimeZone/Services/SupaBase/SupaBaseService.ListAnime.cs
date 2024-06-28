using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
	public partial class SupaBaseService
	{

		public async Task<List<LiAnimeDTO>?> GetFilteredAnimes(List<int>? genres, List<string>? mediaTypes, int? yearlower, int? yearupper)
		{
			var rpcParameters = new
			{
				genres = genres.Count > 0 ? genres : null,
				media_types = mediaTypes.Count > 0 ? mediaTypes : null,
				year_lower = yearlower,
				year_upper = yearupper
			};

			var response = await _client.Rpc("get_filtered_animes", rpcParameters);

            if (response.ResponseMessage.IsSuccessStatusCode)
			{
                
                var return_list = JsonSerializer.Deserialize<List<LiAnimeDTO>>(response.Content, jsonOptions);
				return return_list;
			}

			return null;

		}

	}
}
