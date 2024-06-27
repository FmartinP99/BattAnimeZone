using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("animegenre")]
    public class AnimeGenreSupabaseModel : BaseModel, IAnimeGenreModel
    {
		[PrimaryKey("id", false)]
		public int Id { get; set; }

        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Column("genre_id")]
        public int GenreId { get; set; }

        [Column("is_theme")]
        public bool IsTheme { get; set; } = false;
    }
}
