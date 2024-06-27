using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("AnimeUser")]
    public class AnimeUserModel : IAnimeUserModel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("anime_id")]
        [Required]
        public int AnimeId { get; set; }

        [Column("user_id")]
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

        [ForeignKey("AnimeId")]
        public AnimeModel Anime { get; set; }

        [ForeignKey("UserId")]
        public UserAccountModel UserAccount { get; set; }

    }
}
