using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BattAnimeZone.DatabaseModels
{
	[Table("DistinctYears")]
	public class DistinctYearModel
	{
		[Key]
		[Column("year")]
		public int Year { get; set; }
	}
}
