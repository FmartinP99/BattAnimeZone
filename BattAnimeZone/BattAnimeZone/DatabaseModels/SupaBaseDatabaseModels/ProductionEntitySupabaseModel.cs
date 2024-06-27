using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("productionentity")]
    public class ProductionEntitySupabaseModel : BaseModel, IProductionEntityModel
    {
		[PrimaryKey("id", true)]
		public int Id { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("favorites")]
        public int Favorites { get; set; } = -1;

        [Column("established")]
        public string Established { get; set; } = "";

        [Column("about")]
        public string About { get; set; } = "";

        [Column("count")]
        public int Count { get; set; } = -1;

        [Column("image_url")]
        public string ImageUrl { get; set; } = "";
    }
}
