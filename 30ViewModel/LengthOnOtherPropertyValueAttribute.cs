using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace _30ViewModel
{
    public class LengthOnOtherPropertyValueAttribute : ValidationAttribute
    {
        private readonly string propertyNameToCheck; //имя свойства
        private readonly string propertyValueToCheck; //значение валидируемого свойства
        private readonly int lengthOnMatch; //ИП
        private readonly int length; //Иначе

        public LengthOnOtherPropertyValueAttribute(string propertyNameToCheck, string propertyValueToCheck, int lengthOnMatch, int length)
        {
            this.propertyValueToCheck = propertyValueToCheck;
            this.lengthOnMatch = lengthOnMatch;
            this.length = length;
            this.propertyNameToCheck = propertyNameToCheck;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(propertyNameToCheck); //получение значения свойства
            if (propertyInfo == null)
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Unknown property {0}", 
                    new[] { propertyNameToCheck }), new[] { validationContext.MemberName });

            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance) as string;

            if (propertyValueToCheck.Equals(propertyValue, StringComparison.InvariantCultureIgnoreCase))
            {
                if (value == null || ((string)value).Length != lengthOnMatch)
                {
                    return new ValidationResult(
                        string.Format(ErrorMessageString, validationContext.DisplayName, lengthOnMatch),
            new[] { validationContext.MemberName });
                }
            }
            else
            {
                if (value == null || ((string)value).Length != length)
                {
                    return new ValidationResult(
                        string.Format(ErrorMessageString, validationContext.DisplayName, length),
                        new[] { validationContext.MemberName }
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}
