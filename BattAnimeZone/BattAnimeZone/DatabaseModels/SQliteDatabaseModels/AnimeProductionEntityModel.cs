using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("AnimeProductionEntity")]
    public class AnimeProductionEntityModel : IAnimeProductionEntityModel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Required]
        [Column("productionEntity_id")]
        public int ProductionEntityId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(1)]
        public string Type { get; set; }


        [ForeignKey("AnimeId")]
        public AnimeModel Anime { get; set; }

        [ForeignKey("ProductionEntityId")]
        public ProductionEntityModel ProductionEntity { get; set; }
    }
}
