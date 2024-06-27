using BattAnimeZone.DatabaseModels._IDataBaseModels;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace BattAnimeZone.DatabaseModels.SQliteDatabaseModels
{
    [Table("relation")]
    public class RelationSupabaseModel : BaseModel, IRelationModel
	{
        [PrimaryKey("id",false)]
        public int Id { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [Column("child_id")]
        public int ChildId { get; set; }

        [Column("relationtype")]
        public string RelationType { get; set; } = "";

    }
}
