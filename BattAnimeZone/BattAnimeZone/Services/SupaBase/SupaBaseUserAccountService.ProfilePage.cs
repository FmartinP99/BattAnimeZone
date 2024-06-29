using BattAnimeZone.Shared.Models.AnimeDTOs;
using BattAnimeZone.Shared.Models.User;
using System.Text.Json;

namespace BattAnimeZone.Services.SupaBase
{
    public partial class SupaBaseUserAccountService
    {

        public async Task<ProfilePageDTO?> GetProfileByUserName(string UserName)
        {
            var response = await _client.Rpc("get_user_profile_data", new {_username = UserName});

            if(response == null || !response.ResponseMessage.IsSuccessStatusCode) return null;
            
            using (JsonDocument document = JsonDocument.Parse(response.Content))
            {
                JsonElement root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Null) return null;
                    
                if (root.TryGetProperty("Animes", out JsonElement animesElement) && !(animesElement.ValueKind == JsonValueKind.Null))
                {
                    var returnDto = JsonSerializer.Deserialize<ProfilePageDTO>(response.Content, jsonOptions);
                    return returnDto;
                }
                else
                {
                    return new ProfilePageDTO
                    {
                        Name = root.GetProperty("Name").ToString(),
                        RegisteredAt = root.GetProperty("RegisteredAt").ToString(),
                        Animes = new List<AnimeProfilePageDTO>()
                    };
                }
            }
            

        }

    }
}
