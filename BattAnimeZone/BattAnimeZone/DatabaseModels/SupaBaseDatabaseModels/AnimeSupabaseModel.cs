using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("anime")]
    public class AnimeSupabaseModel : BaseModel , IAnimeModel
	{
        [PrimaryKey("id", true)]
        public int Mal_id { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("title_english")]
        public string TitleEnglish { get; set; } = "";

        [Column("title_japanese")]
        public string TitleJapanese { get; set; } = "";

        [Column("title_synonyms")]
        public string TitleSynonyms { get; set; } = "";

        [Column("media_type")]
        public string MediaType { get; set; } = "";

        [Column("source")]
        public string Source { get; set; } = "";

        [Column("episodes")]
        public int Episodes { get; set; } = 0;
        [Column("status")]
		public string Status { get; set; } = string.Empty;

		[Column("duration")]
        public string Duration { get; set; } = "";

        [Column("rating")]
        public string Rating { get; set; } = "";

        [Column("score")]
        public float Score { get; set; } = -1;

        [Column("scored_by")]
        public int ScoredBy { get; set; } = -1;

        [Column("rank")]
        public int Rank { get; set; } = -1;

        [Column("popularity")]
        public int Popularity { get; set; } = -1;

        [Column("members")]
        public int Members { get; set; } = -1;

        [Column("favorites")]
        public int Favorites { get; set; } = -1;

        [Column("synopsis")]
        public string Synopsis { get; set; } = "";

        [Column("background")]
        public string Background { get; set; } = "";

        [Column("season")]
        public string Season { get; set; } = "";

        [Column("year")]
        public int Year { get; set; } = -1;

        [Column("image_jpg_url")]
        public string ImageJpgUrl { get; set; } = "";

        [Column("image_small_jpg_url")]
        public string ImageSmallJpgUrl { get; set; } = "";

        [Column("image_large_jpg_url")]
        public string ImageLargeJpgUrl { get; set; } = "";

        [Column("image_webp_url")]
        public string ImageWebpUrl { get; set; } = "";

        [Column("image_small_webp_url")]
        public string ImageSmallWebpUrl { get; set; } = "";

        [Column("image_large_webp_url")]
        public string ImageLargeWebpUrl { get; set; } = "";

        [Column("trailer_url")]
        public string TrailerUrl { get; set; } = "";

        [Column("trailer_embed_url")]
        public string TrailerEmbedUrl { get; set; } = "";

        [Column("trailer_image_url")]
        public string TrailerImageUrl { get; set; } = "";

        [Column("trailer_image_small_url")]
        public string TrailerImageSmallUrl { get; set; } = "";

        [Column("trailer_image_medium_url")]
        public string TrailerImageMediumUrl { get; set; } = "";

        [Column("trailer_image_large_url")]
        public string TrailerImageLargeUrl { get; set; } = "";

        [Column("trailer_image_maximum_url")]
        public string TrailerImageMaximumUrl { get; set; } = "";

        [Column("aired_from_day")]
        public int AiredFromDay { get; set; } = -1;

        [Column("aired_from_month")]
        public int AiredFromMonth { get; set; } = -1;

        [Column("aired_from_year")]
        public int AiredFromYear { get; set; } = -1;

        [Column("aired_to_day")]
        public int AiredToDay { get; set; } = -1;

        [Column("aired_to_month")]
        public int AiredToMonth { get; set; } = -1;

        [Column("aired_to_year")]
        public int AiredToYear { get; set; } = -1;

        [Column("aired_string")]
        public string AiredString { get; set; } = "";
    }
}
