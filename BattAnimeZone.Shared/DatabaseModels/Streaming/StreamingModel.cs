using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.DatabaseModels.Streaming
{
	[Table("Streaming")]
	public class StreamingModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public int Id { get; set; }
		[Column("name")]
		public string Name { get; set; } = "";
		[Column("url")]
		public string Url { get; set; } = "";
	}
}
