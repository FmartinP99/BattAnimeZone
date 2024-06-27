

namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IUserAccountModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RegisteredAt { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; } 
        public string? RefreshToken { get; set; } 
        public string? RefreshTokenExpiryTime { get; set; } 

    }
}
