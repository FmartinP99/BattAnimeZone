namespace BattAnimeZone.Shared.Models.User.BrowserStorageModels
{
    public class UserSession
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime TokenExpiryTimeStamp { get; set; }
        public DateTime RefreshTokenExpiryTimestamp { get; set; }
    }
}
