using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.AnimeDTOs
{
    public class LiGenreAnimeDTOContainer
    {
        public string GenreName { get; set; }
        public List<LiGenreAnimeDTO> Animes { get; set; }
    }
}
