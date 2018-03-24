using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BulletinBoard.Models.AccountViewModels;
using Xunit;

namespace BulletinBoard.Tests.ViewModels.AccountViewModels
{
    public class RegisterViewModelTests
    {
        [Fact]
        public void Given_ValidModel_Should_Validate()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "valid@email.com",
                Password = "validPassword1",
                ConfirmPassword = "validPassword1"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void Given_ModelWithInvalidConfirmPassword_Should_NotValidate()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "valid@email.com",
                Password = "validPassword1",
                ConfirmPassword = "otherPassword2"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.False(valid);
        }

        [Fact]
        public void Given_ModelWithInvalidConfirmPassword_Should_ContainCorrectErrorMessage()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "valid@email.com",
                Password = "validPassword1",
                ConfirmPassword = "otherPassword2"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);
            
            // Assert
            var errorMessage = "The password and confirmation password do not match.";
            Assert.Contains(result, x => x.ErrorMessage == errorMessage);
        }
    }
}
