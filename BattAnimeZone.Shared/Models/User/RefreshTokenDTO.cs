using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public DateTime TokenExpiryTimeStamp { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTimeStamp { get; set; }
    }
}
