using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.DatabaseModels
{
    [Table("ProductionEntityTitle")]
    public class ProductionEntityTitleModel
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
