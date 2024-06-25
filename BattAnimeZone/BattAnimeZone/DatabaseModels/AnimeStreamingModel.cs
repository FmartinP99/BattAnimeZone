using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.DatabaseModels
{
    [Table("AnimeStreaming")]
    public class AnimeStreamingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("anime_id")]
        public int AnimeId { get; set; }

        [Required]
        [Column("streaming_id")]
        public int StreamingId { get; set; }

        [ForeignKey("AnimeId")]
        public AnimeModel Anime { get; set; }

        [ForeignKey("StreamingId")]
        public StreamingModel Streaming { get; set; }
    }
}
