using BattAnimeZone.Shared.Models.User;
using BattAnimeZone.Shared.Models.User.BrowserStorageModels;

namespace BattAnimeZone.Services._Interfaces
{
    public interface IUserAccountService
    {
        public Task<bool> RegisterUser(RegisterRequest user);
        public Task<UserSession?> Login(LoginRequest loginRequest);
        public Task<bool> Logout(UserSession uSession);
        public Task<RefreshTokenDTO?> Refresh(RefreshTokenDTO refreshTokenDTO);
        public Task<ChangeDetailsResponse> ChangeDetails(ChangeDetailsRequest newUser, string token);
        public Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequest der, string token);
        public Task<bool> RateAnime(AnimeActionTransfer aat, string? token);
        public Task<Dictionary<int, InteractedAnime>?> GetInteractedAnimes(string UserName, string token);
        public Task<ProfilePageDTO?> GetProfileByUserName(string UserName);
    }
}
