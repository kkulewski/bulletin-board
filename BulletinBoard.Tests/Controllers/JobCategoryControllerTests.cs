using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;

        public JobCategoryControllerTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }).CreateMapper();
        }

        [Fact]
        public async Task Index_Given_ThreeResultsFromService_Should_ReturnCorrectNumberOfViewModels()
        {
            // Arrange
            var items = new List<JobCategory>
            {
                new JobCategory(),
                new JobCategory(),
                new JobCategory()
            };

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(items);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = (ViewResult) result;
            var model = (IList<JobCategoryViewModel>) viewResult.Model;

            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Index_Given_NoResultsFromService_Should_ReturnEmptyModel()
        {
            // Arrange
            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(new List<JobCategory>());

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = (ViewResult) result;
            var model = (IList<JobCategoryViewModel>) viewResult.Model;

            Assert.Empty(model);
        }

        [Fact]
        public async Task Index_Given_ResultFromService_Should_ReturnViewModelWithExpectedName()
        {
            // Arrange
            const string categoryName = "Example";

            var items = new List<JobCategory>
            {
                new JobCategory
                {
                    Name = categoryName
                }
            };

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(items);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = (ViewResult) result;
            var model = (IList<JobCategoryViewModel>) viewResult.Model;

            Assert.Equal(categoryName, model.First().Name);
        }
    }
}
