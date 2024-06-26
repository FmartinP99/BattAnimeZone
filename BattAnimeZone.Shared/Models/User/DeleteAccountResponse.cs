using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User
{
    public class DeleteAccountResponse
    {
        public bool result { get; set; } = false;
        public string Message { get; set; } = "";
    }
}
