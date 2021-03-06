using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace _30ViewModel
{
    public class LengthOnOtherPropertyValueAttribute : ValidationAttribute
    {
        private readonly string propertyNameToCheck;
        private readonly string propertyValueToCheck;
        private readonly int maxLengthOnMatch;
        private readonly int maxLength;

        public LengthOnOtherPropertyValueAttribute(string propertyNameToCheck, string propertyValueToCheck, int maxLengthOnMatch, int maxLength)
        {
            this.propertyValueToCheck = propertyValueToCheck;
            this.maxLengthOnMatch = maxLengthOnMatch;
            this.maxLength = maxLength;
            this.propertyNameToCheck = propertyNameToCheck;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyName = validationContext.ObjectType.GetProperty(propertyNameToCheck);
            if (propertyName == null)
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Unknown property {0}", new[] { propertyNameToCheck }));

            var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null) as string;

            if (propertyValueToCheck.Equals(propertyValue, StringComparison.InvariantCultureIgnoreCase) && value != null && ((string)value).Length > maxLengthOnMatch)
                return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, maxLengthOnMatch));

            if (value != null && ((string)value).Length > maxLength)
                return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, maxLength));

            return ValidationResult.Success;
        }
    }
}
