using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BattAnimeZone.DatabaseModels
{
	[Table("AnimeGenre")]
	public class AnimeGenreModel
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("anime_id")]
		[ForeignKey("anime_id")]
		[Required]
		public int AnimeId { get; set; }

		[Column("genre_id")]
		[ForeignKey("genre_id")]
		[Required]
		public int GenreId { get; set; }

		[Column("is_theme")]
		[Required]
		public bool IsTheme { get; set; } = false;

	}
}
