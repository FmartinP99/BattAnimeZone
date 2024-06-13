using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattAnimeZone.DatabaseModels
{
    [Table("DistinctMediaTypes")]
    public class DistinctMediaTypesModel
    {
        [Key]
        [Column("media_type")]
        public string? mediaType { get; set; }
    }
}
