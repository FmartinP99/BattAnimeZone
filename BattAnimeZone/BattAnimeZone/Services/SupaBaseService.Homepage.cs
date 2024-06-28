using BattAnimeZone.DatabaseModels.SQliteDatabaseModels;
using BattAnimeZone.Shared.Models.AnimeDTOs;
using System.Text.Json;

namespace BattAnimeZone.Services
{
    public partial class SupaBaseService
    {

        public async Task<IEnumerable<AnimeHomePageDTO>?> GetAnimesForHomePageByYear(int year)
        {
            
            var response = await _client.Rpc("get_animes_by_year_with_genres", new { _year = year });
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var returnDto = JsonSerializer.Deserialize<IEnumerable<AnimeHomePageDTO>>(response.Content, jsonOptions);

                return returnDto;
            }
            return null;

        }

    }
}
