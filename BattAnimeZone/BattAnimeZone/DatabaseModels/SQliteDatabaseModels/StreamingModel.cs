using BattAnimeZone.DatabaseModels._IDataBaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("Streaming")]
    public class StreamingModel : IStreamingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; } = "";

        [Column("url")]
        [Required]
        public string Url { get; set; } = "";
    }
}
