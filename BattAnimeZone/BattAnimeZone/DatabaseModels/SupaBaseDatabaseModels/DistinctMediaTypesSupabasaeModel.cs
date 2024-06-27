using BattAnimeZone.DatabaseModels._IDataBaseModels;
using Microsoft.EntityFrameworkCore;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattAnimeZone.DatabaseModels.SuapaBaseDatabaseModels
{
    [Table("distinctmediatypes")]
    public class DistinctMediaTypesSupabasaeModel : BaseModel, IDistinctMediaTypesModel
	{
        [Key]
        [Column("media_type")]
        public string? mediaType { get; set; }
    }
}
