using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("animeuser")]
    public class AnimeUserSupabaseModel : BaseModel, IAnimeUserModel
	{
		[PrimaryKey("id", false)]
		public int Id { get; set; }

        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("favorite")]
        public bool favorite { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("rating")]
        public int Rating { get; set; } = 0;

        [Column("date")]
        public string Date { get; set; }
    }
}
