using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class ConditionalEmailValidatorAttribute : ValidationAttribute
    {
        private readonly string _conditionPropertyName;

        public ConditionalEmailValidatorAttribute(string conditionPropertyName)
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
                    return new ValidationResult($"{validationContext.DisplayName} is required.");
                }

                string email = value as string;

             
              
                if ((!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$")))
                {
                    return new ValidationResult("Invalid email format.");
                }

                return ValidationResult.Success;
            }

           
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

           
            return new ValidationResult("Email change not allowed.");
        }
    }
}
