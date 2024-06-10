using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.DatabaseModels.AnimeStreaming
{
	[Table("AnimeStreaming")]
	public class AnimeStreamingModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public int Id { get; set; }
		[Column("anime_id")]
		public int AnimeId { get; set; }
		[Column("streaming_id")]
		public int StreamingId { get; set; }
	}
}
