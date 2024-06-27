using BattAnimeZone.DatabaseModels._IDataBaseModels;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;


namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("useraccount")]
    public class UserAccountSupabaseModel : BaseModel, IUserAccountModel
	{
		[PrimaryKey("id", false)]
		public int Id { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("registered_at")]
        public string RegisteredAt { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("token")]
        public string? Token { get; set; } = null;

        [Column("refreshtoken")]
        public string? RefreshToken { get; set; } = null;

        [Column("refreshtokenexpirytime")]
        public string? RefreshTokenExpiryTime { get; set; } = null;

    }
}
