using System.ComponentModel.DataAnnotations;

namespace WebAppEmployeeApi.CustomValidations
{
    public class ValidatePasswordAttribute : ValidationAttribute
    {
        private readonly string _passwordPropertyName;

        public ValidatePasswordAttribute(string passwordPropertyName)
        {
            _passwordPropertyName = passwordPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var passwordProperty = validationContext.ObjectType.GetProperty(_passwordPropertyName);

            if (passwordProperty == null)
            {
                return new ValidationResult($"Unknown property:{_passwordPropertyName}");
            }

            var passwordValue = passwordProperty.GetValue(validationContext.ObjectInstance, null).ToString();
            var confirmPasswordValue = value?.ToString();

            if (passwordValue != confirmPasswordValue)
            {
                return new ValidationResult("Password and Confirm Password do not match");
            }

            return ValidationResult.Success;
        }
    }
}
