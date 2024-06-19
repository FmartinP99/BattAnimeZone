using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.DatabaseModels
{
    [Table("UserAccount")]
    public class UserAccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        public string UserName { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }

        [Column("token")]
        public string? Token { get; set; } = null;

        [Column("refreshToken")]
        public string? RefreshToken { get; set; } = null;

        [Column("refreshTokenExpiryTime")]
        public string? RefreshTokenExpiryTime { get; set; } = null;

    }
}
