using BattAnimeZone.Client.Extensions;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;
using Microsoft.AspNetCore.Components.Authorization;

namespace BattAnimeZone.Client.Authentication
{
    public partial class CustomAuthenticationStateProvider
    {
        public async Task<string> GetTokenSessionStorage()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch { }

            return result;
        }

        public async Task<string> GetUsernameSessionStorage()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                    result = userSession.UserName;
            }
            catch { }

            return result;
        }

        public async Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimesSessionStorage()
        {
            Dictionary<int, InteractedAnime>? result = new Dictionary<int, InteractedAnime>();

            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)

                    result = await _sessionStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
            }
            catch { }

            return result;
        }

        public async Task<InteractedAnime?> GetInteractedAnimeByIdSessionStorage(int id)
        {
            InteractedAnime? result = null;

            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                {
                    Dictionary<int, InteractedAnime> interacted_animes = await _sessionStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
                    interacted_animes.TryGetValue(id, out result);
                }
            }
            catch { }

            return result;
        }

        public async Task InsertOrUpdateInteractiveAnimeSessionStorage(InteractedAnime intanime)
        {
            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                {
                    Dictionary<int, InteractedAnime> interacted_animes = await _sessionStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
                    if (interacted_animes != null)
                        interacted_animes[intanime.MalId] = intanime;
                    else
                    {
                        interacted_animes = new Dictionary<int, InteractedAnime>() {
                            {
                                intanime.MalId, intanime
                            }
                        };
                    }
                    await _sessionStorage.SaveItemEncryptedAsync("InteractedAnimes", interacted_animes);

                }
            }
            catch { }

        }
    }
}
