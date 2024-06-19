using BattAnimeZone.Client.Extensions;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;

namespace BattAnimeZone.Client.Authentication
{
    public partial class CustomAuthenticationStateProvider
    {
        public async Task<string> GetTokenLocalStorage()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch { }

            return result;
        }

        public async Task<string> GetUsernameLocalStorage()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                    result = userSession.UserName;
            }
            catch { }

            return result;
        }

        public async Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimesLocalStorage()
        {
            Dictionary<int, InteractedAnime>? result = new Dictionary<int, InteractedAnime>();

            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)

                    result = await _localStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
            }
            catch { }

            return result;
        }

        public async Task<InteractedAnime?> GetInteractedAnimeByIdLocalStorage(int id)
        {
            InteractedAnime? result = null;

            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                {
                    Dictionary<int, InteractedAnime> interacted_animes = await _localStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
                    interacted_animes.TryGetValue(id, out result);
                }
            }
            catch { }

            return result;
        }

        public async Task InsertOrUpdateInteractiveAnimeLocalStorage(InteractedAnime intanime)
        {
            try
            {
                var userSession = await _localStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                {
                    Dictionary<int, InteractedAnime> interacted_animes = await _localStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
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
                    await _localStorage.SaveItemEncryptedAsync("InteractedAnimes", interacted_animes);

                }
            }
            catch { }

        }
    }
}
