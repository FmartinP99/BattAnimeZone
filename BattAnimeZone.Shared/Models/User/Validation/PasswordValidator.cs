using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class PasswordValidator : ValidationAttribute
    {
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string password)
            {
                string errorMessage = "Password ";
                if (password.Length < 10 || password.Length > 30)
                {
                    errorMessage += "must be between 10 and 30 characters, ";
                }
                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    errorMessage += "must contain at least one number, ";
                }
                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    errorMessage += "must contain at least one lowercase letter, ";
                }
                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    errorMessage += "must contain at least one uppercase letter, ";
                }
                if (errorMessage != "Password ")
                {
                    if (errorMessage.EndsWith(", "))
                    {
                        errorMessage = errorMessage.Substring(0, errorMessage.Length - 2) + ".";
                    }
                    int lastCommaIndex = errorMessage.LastIndexOf(",");
                    if (lastCommaIndex != -1)
                    {
                        errorMessage = errorMessage.Remove(lastCommaIndex, 1).Insert(lastCommaIndex, " and");
                    }
                    return new ValidationResult(errorMessage);
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid password format.");
        }

    }
}
