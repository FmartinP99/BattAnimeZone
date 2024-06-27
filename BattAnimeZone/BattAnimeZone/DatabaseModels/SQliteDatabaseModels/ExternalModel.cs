using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("External")]
    public class ExternalModel : IExternalModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = "";

        [Required]
        [Column("url")]
        public string Url { get; set; } = "";

        [Required]
        [Column("anime_id")]
        public int AnimeId { get; set; }

        [ForeignKey("AnimeId")]
        public AnimeModel Anime { get; set; }
    }
}
