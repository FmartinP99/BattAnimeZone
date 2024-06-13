using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.DatabaseModels
{
    [Table("ProductionEntity")]
    public class ProductionEntityModel
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
