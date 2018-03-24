using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BulletinBoard.Models.JobCategoryViewModels;
using Xunit;

namespace BulletinBoard.Tests.ViewModels.JobCategoryViewModels
{
    public class CreateJobCategoryViewModelTests
    {
        [Fact]
        public void Given_ValidModel_Should_Validate()
        {
            // Arrange
            var model = new CreateJobCategoryViewModel
            {
                Name = "Example"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void Given_ModelWithNonCapitalizedName_Should_NotValidate()
        {
            // Arrange
            var model = new CreateJobCategoryViewModel
            {
                Name = "example"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            Assert.False(valid);
        }

        [Fact]
        public void Given_ModelWithNonCapitalizedName_Should_ContainCorrectError()
        {
            // Arrange
            var model = new CreateJobCategoryViewModel
            {
                Name = "example"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, result, true);

            // Assert
            var errorMessage = string.Format("{0} must start with capital letter.", nameof(model.Name));
            Assert.Contains(result, x => x.ErrorMessage == errorMessage);
        }
    }
}
