using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattAnimeZone.Shared.Models.AnimeDTOs;

namespace BattAnimeZone.Shared.Models.User
{
    public class ProfilePageDTO
    {
        public string Name {  get; set; }
        public string RegisteredAt { get; set; }
        public List<AnimeProfilePageDTO> Animes { get; set; }
    }
}
