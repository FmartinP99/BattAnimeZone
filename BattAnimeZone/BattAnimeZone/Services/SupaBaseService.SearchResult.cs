using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Text.Json;

namespace BattAnimeZone.Services
{
    public partial class SupaBaseService
    {

        public async Task<List<AnimeSearchResultDTO>?> GetSimilarAnimesForSearchResult(int n, string name)
        {

            int[] similar_anime_ids = _ssService.GetSimilarAnimesForSearchResult(n, name);
            var response = await _client.Rpc("get_similar_animes", new { similar_anime_ids = similar_anime_ids });
            await Console.Out.WriteLineAsync(response.Content);
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var return_list = JsonSerializer.Deserialize<List<AnimeSearchResultDTO>?>(response.Content, jsonOptions);
                if(return_list.Count > 0) return return_list;
                return new List<AnimeSearchResultDTO>();

            }

            return null;
        }
    }
}
