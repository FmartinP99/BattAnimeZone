using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.DatabaseModels.Relation
{
	[Table("Relation")]
	public class RelationModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public int Id { get; set; }
		[Column("parent_id")]
		public int ParentId { get; set; }
		[Column("child_id")]
		public int ChildId { get; set; }
		[Column("relationType")]
		public string RelationType { get; set; } = "";
	}
}
