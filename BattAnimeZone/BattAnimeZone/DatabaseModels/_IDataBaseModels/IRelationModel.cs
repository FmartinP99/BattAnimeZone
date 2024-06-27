using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IRelationModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string RelationType { get; set; } 

    }
}
