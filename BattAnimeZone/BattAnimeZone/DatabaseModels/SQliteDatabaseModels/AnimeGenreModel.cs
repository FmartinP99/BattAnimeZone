using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("AnimeGenre")]
    public class AnimeGenreModel : IAnimeGenreModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("anime_id")]
        [Required]
        public int AnimeId { get; set; }

        [Column("genre_id")]
        [Required]
        public int GenreId { get; set; }

        [Column("is_theme")]
        [Required]
        public bool IsTheme { get; set; } = false;

        [ForeignKey("GenreId")]
        public GenreModel Genre { get; set; }

        [ForeignKey("AnimeId")]
        public AnimeModel Anime { get; set; }

    }
}
