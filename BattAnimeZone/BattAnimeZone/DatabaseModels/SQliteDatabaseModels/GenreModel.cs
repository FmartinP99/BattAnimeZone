using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("Genre")]
    public class GenreModel : IGenreModel
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
