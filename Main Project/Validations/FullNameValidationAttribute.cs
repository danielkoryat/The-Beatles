using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Main_Project.Validations
{
    public class FullNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fullUsername = (string)value;
            if (string.IsNullOrEmpty(fullUsername))
                return new ValidationResult("Username cannot be empty");

            var pattern = @"^\w{2,}\s\w{2,}$";
            if (Regex.IsMatch(fullUsername, pattern))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Full name must be two words, each with at least two letters, separated by a space.");
            }
        }
    }
}