using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BulletinBoard.Helpers.CustomValidators
{
    /// <summary>
    /// Specifies that the value is a valid postal code.
    /// </summary>
    public class PostalCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(validationContext.DisplayName + " is required.");
            }

            var postalCode = value.ToString();
            if (Regex.IsMatch(postalCode, "^[0-9]{2}[-][0-9]{3}$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid post code (use format: xx-xxx).");
        }
    }
}
