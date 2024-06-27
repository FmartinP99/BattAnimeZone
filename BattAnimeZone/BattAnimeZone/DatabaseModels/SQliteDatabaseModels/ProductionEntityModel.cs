using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("ProductionEntity")]
    public class ProductionEntityModel : IProductionEntityModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Required]
        [Column("favorites")]
        public int Favorites { get; set; } = -1;

        [Column("established")]
        public string Established { get; set; } = "";

        [Column("about")]
        public string About { get; set; } = "";

        [Required]
        [Column("count")]
        public int Count { get; set; } = -1;

        [Column("image_url")]
        public string ImageUrl { get; set; } = "";
    }
}
