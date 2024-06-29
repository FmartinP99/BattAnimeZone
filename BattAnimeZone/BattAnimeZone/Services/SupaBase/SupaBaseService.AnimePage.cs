using BattAnimeZone.Shared.Models.Anime;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseService
    {
        public async Task<AnimePageDTO?> GetAnimePageDTOByID(int mal_id)
        {
            var response = await _client.Rpc("get_anime_page_details", new {_mal_id = mal_id});

            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var returnDto = JsonSerializer.Deserialize<AnimePageDTO>(response.Content, jsonOptions);
                return returnDto;
            }
            return null;

        }
    }
}
