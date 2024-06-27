using BattAnimeZone.DatabaseModels._IDataBaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("DistinctMediaTypes")]
    public class DistinctMediaTypesModel : IDistinctMediaTypesModel
	{
        [Key]
        [Column("media_type")]
        public string? mediaType { get; set; }
    }
}
