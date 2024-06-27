using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;


namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("genre")]
    public class GenreSupabaseModel : BaseModel, IGenreModel
    {
		[PrimaryKey("id", true)]
		public int Mal_id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("url")]
        public string Url { get; set; } = "";

    }
}
