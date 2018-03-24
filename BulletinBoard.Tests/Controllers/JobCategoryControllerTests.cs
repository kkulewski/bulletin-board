using System.Collections.Generic;
using System.Threading.Tasks;
using BulletinBoard.Controllers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobCategoryViewModels;
using BulletinBoard.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoMapper;
using BulletinBoard.Infrastructure;

namespace BulletinBoard.Tests.Controllers
{
    public class JobCategoryControllerTests
    {
        [Fact]
        public async Task Index_Given_MockedService_Should_ReturnCorrectNumberOfViewModels()
        {
            // Arrange
            var items = new List<JobCategory>
            {
                new JobCategory {Name = "Category1"},
                new JobCategory {Name = "Category2"},
                new JobCategory {Name = "Category3"},
            };

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(items);

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }).CreateMapper();

            var controller = new JobCategoryController(serviceMock.Object, mapper);

            // Act
            var result = (ViewResult) await controller.Index();
            var model = (IList<JobCategoryViewModel>) result.Model;

            // Assert
            Assert.Equal(3, model.Count);
        }
    }
}
