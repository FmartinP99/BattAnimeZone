using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.DatabaseModels
{
    [Table("AnimeUser")]
    public class AnimeUserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("anime_id")]
        [ForeignKey("anime_id")]
        [Required]
        public int AnimeId { get; set; }

        [Column("user_id")]
        [ForeignKey("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("Favorite")]
        [Required]
        public bool favorite { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; }

        [Column("rating")]
        [Required]
        public int Rating { get; set; } = 0;

        [Column("date")]
        [Required]
        public string Date { get; set; }

    }
}
