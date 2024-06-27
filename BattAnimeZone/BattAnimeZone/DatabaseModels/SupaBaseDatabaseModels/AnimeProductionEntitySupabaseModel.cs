using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("animeproductionentity")]
    public class AnimeProductionEntitySupabaseModel : BaseModel, IAnimeProductionEntityModel
    {
		[PrimaryKey("id", false)]
		public int Id { get; set; }

        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Column("productionentity_id")]
        public int ProductionEntityId { get; set; }

        [Column("type")]
        public string Type { get; set; }
    }
}
