using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.BrowserStorageModels
{
    public class AnimeActionTransfer
    {
        public string UserName { get; set; }
        public int AnimeId { get; set; }
        public int Rating { get; set; }
        public string? Status { get; set; }
        public bool Favorite { get; set; } = false;
    }
}
