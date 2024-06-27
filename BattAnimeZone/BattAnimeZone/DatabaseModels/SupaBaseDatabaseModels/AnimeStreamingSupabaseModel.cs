using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("animestreaming")]
    public class AnimeStreamingSupabaseModel : BaseModel, IAnimeStreamingModel
    {
		[PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Column("streaming_id")]
        public int StreamingId { get; set; }
    }
}
