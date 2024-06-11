using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BattAnimeZone.DatabaseModels
{
	[Table("Genre")]
	public class GenreModel
	{
		[Key]
		[Column("id")]
		public int Mal_id { get; set; }

		[Column("name")]
		[Required]
		public string Name { get; set; } = "";

		[Column("url")]
		[Required]
		public string Url { get; set; } = "";

	}
}
