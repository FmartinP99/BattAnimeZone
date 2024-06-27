using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("DistinctYears")]
    public class DistinctYearModel : IDistinctYearModel
	{
        [Key]
        [Column("year")]
        public int? Year { get; set; }
    }
}
