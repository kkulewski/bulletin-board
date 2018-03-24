using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BulletinBoard.Models.AccountViewModels;
using Xunit;

namespace BulletinBoard.Tests.ViewModels.AccountViewModels
{
    public class LoginViewModelTests
    {
        [Fact]
        public void Given_ValidModel_Should_Validate()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "valid@email.com",
                Password = "validPassword1"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void Given_ValidModel_Should_ContainNoErrors()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "valid@email.com",
                Password = "validPassword1"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Given_ModelWithInvalidEmail_Should_NotValidate()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "wrong.email.com",
                Password = "validPassword1"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.False(valid);
        }

        [Fact]
        public void Given_ModelWithInvalidEmail_Should_ContainCorrectErrorMessage()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "wrong.email.com",
                Password = "validPassword1"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            var errorMessage = string.Format("The {0} field is not a valid e-mail address.", nameof(model.Email));
            Assert.Contains(result, x => x.ErrorMessage == errorMessage);
        }

        [Fact]
        public void Given_ModelWithEmptyPassword_Should_NotValidate()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "valid@email.com",
                Password = string.Empty
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.False(valid);
        }

        [Fact]
        public void Given_ModelWithEmptyPassword_Should_ContainCorrectErrorMessage()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "valid@email.com",
                Password = string.Empty
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            var errorMessage = string.Format("The {0} field is required.", nameof(model.Password));
            Assert.Contains(result, x => x.ErrorMessage == errorMessage);
        }

        [Fact]
        public void Given_ModelWithSingleError_Should_ContainSingleValidationResult()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "valid@email.com",
                Password = string.Empty
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void Given_ModelWithTwoErrors_Should_ContainTwoValidationResults()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "wrong.email.com",
                Password = string.Empty
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
