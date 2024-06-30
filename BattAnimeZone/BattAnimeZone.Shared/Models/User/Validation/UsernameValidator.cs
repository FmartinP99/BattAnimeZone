using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class UsernameValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string username)
            {
                if (username.Length < 6 || username.Length > 30)
                {
                    return new ValidationResult("Username must be between 6 and 30 characters.");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid username format.");
        }
    }
}
