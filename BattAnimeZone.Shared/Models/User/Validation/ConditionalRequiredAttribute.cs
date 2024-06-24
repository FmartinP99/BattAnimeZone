using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattAnimeZone.Shared.Models.User.Validation
{
    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string _conditionPropertyName;

        public ConditionalRequiredAttribute(string conditionPropertyName)
        {
            _conditionPropertyName = conditionPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conditionProperty = validationContext.ObjectType.GetProperty(_conditionPropertyName);
            if (conditionProperty == null)
                return new ValidationResult($"Unknown property: {_conditionPropertyName}");

            var conditionValue = (bool)conditionProperty.GetValue(validationContext.ObjectInstance);
            if (conditionValue && value == null)
                return new ValidationResult($"{validationContext.DisplayName} is required.");

            return ValidationResult.Success;
        }
    }
}
