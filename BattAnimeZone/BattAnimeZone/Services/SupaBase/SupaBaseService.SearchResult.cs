using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseService
    {

        public async Task<List<AnimeSearchResultDTO>?> GetSimilarAnimesForSearchResult(int n, string name)
        {

            int[] similar_anime_ids = _ssService.GetSimilarAnimesForSearchResult(n, name);
            if(similar_anime_ids.Count() == 0) return new List<AnimeSearchResultDTO>();
            var response = await _client.Rpc("get_similar_animes", new { similar_anime_ids = similar_anime_ids });
          

            if (response != null && response.ResponseMessage.IsSuccessStatusCode)
            {
                var return_list = JsonSerializer.Deserialize<List<AnimeSearchResultDTO>?>(response.Content, jsonOptions);
                return return_list;
            }

            return null;
        }
    }
}
