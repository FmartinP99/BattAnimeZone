using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BattAnimeZone.DatabaseModels._IDataBaseModels
{
    public interface IExternalModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Url { get; set; } 
        public int AnimeId { get; set; }

    }
}
