using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class ConditionalUsernameValidatorAttribute : ValidationAttribute
    {
        private readonly string _conditionPropertyName;

        public ConditionalUsernameValidatorAttribute(string conditionPropertyName)
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

         
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

          
            return new ValidationResult("Username change not allowed.");
        }
    }
}
