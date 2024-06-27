using BattAnimeZone.DatabaseModels._IDataBaseModels;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("streaming")]
    public class StreamingSupabaseModel : BaseModel, IStreamingModel
    {
		[PrimaryKey("id", false)]
		public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("url")]
        public string Url { get; set; } = "";
    }
}
