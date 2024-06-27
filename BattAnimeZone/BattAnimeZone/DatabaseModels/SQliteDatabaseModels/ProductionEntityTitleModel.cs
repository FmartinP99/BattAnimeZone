using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("ProductionEntityTitle")]
    public class ProductionEntityTitleModel : IProductionEntityTitleModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("type")]
        public string Type { get; set; } = "";

        [Required]
        [Column("title")]
        public string Title { get; set; } = "";

        [Required]
        [Column("parent_id")]
        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public ProductionEntityModel ProductionEntity { get; set; }
    }
}
