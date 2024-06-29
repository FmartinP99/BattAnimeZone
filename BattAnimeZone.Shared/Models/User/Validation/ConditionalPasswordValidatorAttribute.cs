using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class ConditionalPasswordValidatorAttribute : ValidationAttribute
    {
        private readonly string _conditionPropertyName;

        public ConditionalPasswordValidatorAttribute(string conditionPropertyName, string errorMessage) : base(errorMessage)
        {
            _conditionPropertyName = conditionPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conditionProperty = validationContext.ObjectType.GetProperty(_conditionPropertyName);
            if (conditionProperty == null)
                return new ValidationResult($"Unknown property: {_conditionPropertyName}");

            var conditionValue = (bool)conditionProperty.GetValue(validationContext.ObjectInstance);

            if (conditionValue)
            {
                if (value == null)
                {
                    return new ValidationResult("Password is required.");
                }

                string password = value as string;

           
                if (password.Length < 10 || password.Length > 30)
                {
                    return new ValidationResult("Password must be between 10 and 30 characters.");
                }

                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    return new ValidationResult("Password must contain at least one number.");
                }

                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    return new ValidationResult("Password must contain at least one lowercase letter.");
                }

                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    return new ValidationResult("Password must contain at least one uppercase letter.");
                }
                return ValidationResult.Success;

            }
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }


            return new ValidationResult("Password change not allowed.");
        }
    }
}
