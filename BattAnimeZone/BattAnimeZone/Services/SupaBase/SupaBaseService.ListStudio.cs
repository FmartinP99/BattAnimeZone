using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.ProductionEntity;
using BattAnimeZone.Shared.Models.ProductionEntityDTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseService
    {
        public async Task<List<LiProductionEntityDTO>?> GetProductionEntitiesDTO()
        {
            Dictionary<int, LiProductionEntityDTO> return_dict = new Dictionary<int, LiProductionEntityDTO> ();
            var response = await _client.Rpc("get_production_entities", null);

            if (response != null && response.ResponseMessage.IsSuccessStatusCode)
            {
                var response_list = JsonSerializer.Deserialize<List<LiProductionEntityDTO>?>(response.Content, jsonOptions);
                return response_list;
            }

            return null;
        }
    }
}
