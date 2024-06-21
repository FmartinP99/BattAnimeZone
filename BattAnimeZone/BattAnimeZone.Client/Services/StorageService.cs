using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace BattAnimeZone.Client.Services
{
    public partial class StorageService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService _sessionStorage;

        public StorageService(ILocalStorageService localStorage,
            ISessionStorageService sessionStorage)
        {
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
        }
    }
}
