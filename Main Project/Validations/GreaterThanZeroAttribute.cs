using System.ComponentModel.DataAnnotations;

namespace Main_Project.Validations
{
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        public GreaterThanZeroAttribute() : base("The amount must be greater than zero.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is double doubleValue)
            {
                if (doubleValue > 0)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage);
            }
            // Fallback error message if the value is not a double
            return new ValidationResult("Invalid input.");
        }
    }
}