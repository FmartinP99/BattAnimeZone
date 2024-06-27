using BattAnimeZone.Shared.Models.AnimeDTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace BattAnimeZone.Services
{
    public partial class SupaBaseService
    {
        public async Task<LiGenreAnimeDTOContainer?> GetAnimesForListGenreAnimes(int genre_id)
        {

            var rpcParameters = new
            {
                _genre_id = genre_id,
            };

            var response = await _client.Rpc("get_genre_animes", rpcParameters);


            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var return_list = JsonSerializer.Deserialize<List<LiGenreAnimeDTOContainer>>(response.Content, jsonOptions).FirstOrDefault();
                return return_list;
            }

            return null;



        }
    }
}
