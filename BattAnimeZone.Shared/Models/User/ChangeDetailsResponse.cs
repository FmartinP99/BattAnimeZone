using BattAnimeZone.Shared.Models.User.BrowserStorageModels;

namespace BattAnimeZone.Shared.Models.User
{
    public class ChangeDetailsResponse
    {
        public bool result { get; set; } = false;
        public string Message { get; set; } = "";
        public UserSession? UserSession { get; set; }
    }
}
