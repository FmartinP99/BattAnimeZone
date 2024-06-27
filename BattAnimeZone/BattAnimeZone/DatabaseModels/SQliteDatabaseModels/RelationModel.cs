using BattAnimeZone.DatabaseModels._IDataBaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("Relation")]
    public class RelationModel : IRelationModel
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("parent_id")]
        public int ParentId { get; set; }

        [Required]
        [Column("child_id")]
        public int ChildId { get; set; }

        [Required]
        [Column("relationType")]
        public string RelationType { get; set; } = "";

        [ForeignKey("ParentId")]
        public AnimeModel ParentAnime { get; set; }
        [ForeignKey("ChildId")]
        public AnimeModel ChildAnime { get; set; }
    }
}
