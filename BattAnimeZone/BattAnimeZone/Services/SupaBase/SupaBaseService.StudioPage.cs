using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseService
    {
        public async Task<ProductionEntityPageDTO?> GetAnimesForProdEnt(int mal_id)
        {
            var response = await _client.Rpc("get_animes_for_prod_ent", new { _mal_id = mal_id });

            if (response.Content == "null") return null;

            if (response != null && response.ResponseMessage.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ProductionEntityPageDTO>(response.Content);

                result.StudioAnimes = result.StudioAnimes ?? new List<AnimeStudioPageDTO>();
                result.ProducedAnimes = result.ProducedAnimes ?? new List<AnimeStudioPageDTO>();
                result.LicensedAnimes = result.LicensedAnimes ?? new List<AnimeStudioPageDTO>();

                return result;
            }
            return null;

           

        }

    }
}
