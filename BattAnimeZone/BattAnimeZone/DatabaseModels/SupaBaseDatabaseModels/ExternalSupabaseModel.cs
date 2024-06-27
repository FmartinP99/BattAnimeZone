using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("external")]
    public class ExternalSupabaseModel : BaseModel, IExternalModel
    {
		[PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("url")]
        public string Url { get; set; } = "";

        [Column("anime_id")]
        public int AnimeId { get; set; }

    }
}
