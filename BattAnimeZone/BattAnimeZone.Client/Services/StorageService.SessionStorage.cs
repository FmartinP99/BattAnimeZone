﻿using BattAnimeZone.Client.Extensions;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;

namespace BattAnimeZone.Client.Services
{
    public partial class StorageService
    {

        public async Task SaveToSessionStorage<T>(string key, T value)
        {
            try
            {
                await _sessionStorage.SaveItemEncryptedAsync(key, value);
            }
            catch { }

        }

        public async Task<T?> GetFromSessionStorage<T>(string key)
        {
            try
            {
                return await _sessionStorage.ReadEncryptedItemAsync<T>(key);
            }
            catch
            {
                return default(T?);
            }
        }

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

        public async Task InsertOrUpdateInteractiveAnimeSessionStorage(int id, InteractedAnime intanime)
        {
            try
            {
                var userSession = await _sessionStorage.ReadEncryptedItemAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now.ToUniversalTime() < userSession.TokenExpiryTimeStamp)
                {
                    Dictionary<int, InteractedAnime> interacted_animes = await _sessionStorage.ReadEncryptedItemAsync<Dictionary<int, InteractedAnime>>("InteractedAnimes");
                    if (interacted_animes != null)
                        interacted_animes[id] = intanime;
                    else
                    {
                        interacted_animes = new Dictionary<int, InteractedAnime>() {
                            {
                                id, intanime
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