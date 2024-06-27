using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BattAnimeZone.DatabaseModels._IDataBaseModels;
using Supabase.Postgrest.Models;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("distinctyears")]
    public class DistinctYearSupabaseModel : BaseModel, IDistinctYearModel
	{
        [Key]
        [Column("year")]
        public int? Year { get; set; }
    }
}
