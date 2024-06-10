using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.DatabaseModels.External
{
	[Table("External")]
	public class ExternalModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public int Id { get; set; }
		[Column("name")]
		public string Name { get; set; } = "";
		[Column("url")]
		public string Url { get; set; } = "";
		[Column("anime_id")]
		public int AnimeId { get; set; }
	}
}
