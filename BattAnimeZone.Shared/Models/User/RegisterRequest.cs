using System.ComponentModel.DataAnnotations;
using BattAnimeZone.Shared.Models.User.Validation;

namespace BattAnimeZone.Shared.Models.User
{
    public class RegisterRequest
    {
        [Required]
        [UsernameValidator(ErrorMessage = "Username must be between 6 and 30 characters.")]
        public string UserName { get; set; }

        [Required]
        [PasswordValidator(ErrorMessage = "Password must be between 12 and 30 characters, and contain at least one number, one lowercase letter, and one uppercase letter.")]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
