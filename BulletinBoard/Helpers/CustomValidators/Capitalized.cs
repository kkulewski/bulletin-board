using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BulletinBoard.Helpers.CustomValidators
{
    /// <summary>
    /// Specifies that a first letter of the value is uppercase.
    /// </summary>
    public class Capitalized : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(validationContext.DisplayName + " is required.");
            }

            return char.IsUpper(value.ToString().First()) ? 
                ValidationResult.Success 
                : new ValidationResult(validationContext.DisplayName + " must start with capital letter.");
        }
    }
}
