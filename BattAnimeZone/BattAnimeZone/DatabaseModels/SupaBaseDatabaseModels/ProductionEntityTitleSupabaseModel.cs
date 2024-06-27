using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("productionentitytitle")]
    public class ProductionEntityTitleSupabaseModel : BaseModel, IProductionEntityTitleModel
    {
		[PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("type")]
        public string Type { get; set; } = "";

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("parent_id")]
        public int ParentId { get; set; }

    }
}
