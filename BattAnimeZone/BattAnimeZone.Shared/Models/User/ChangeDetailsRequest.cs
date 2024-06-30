using BattAnimeZone.Shared.Models.User.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User
{
    public class ChangeDetailsRequest
    {

        //this is filled automatically before sending the request
        public string UserName { get; set; }

        [Required]
        [PasswordValidator(ErrorMessage = "Password must be between 12 and 30 characters, and contain at least one number, one lowercase letter, and one uppercase letter.")]
        public string Password { get; set; }


        [ConditionalRequired(nameof(ChangePassword))]
        [ConditionalPasswordValidator(nameof(ChangePassword), "Password must be between 12 and 30 characters, and contain at least one number, one lowercase letter, and one uppercase letter.")]
        public string? NewPassword { get; set; } = null;


        [ConditionalRequired(nameof(ChangeUserName))]
        [ConditionalUsernameValidator(nameof(ChangeUserName), ErrorMessage = "Password must be between 12 and 30 characters, and contain at least one number, one lowercase letter, and one uppercase letter.")]
        public string? NewUsername { get; set; } = null;

        [ConditionalRequired(nameof(ChangeEmail))]
        [ConditionalEmailValidator(nameof(ChangeEmail))]
        public string? NewEmail { get; set; } = null;

        public bool ChangePassword { get; set; } = false;

        public bool ChangeEmail { get; set; } = false;

        public bool ChangeUserName { get; set; } = false;

    }
}
