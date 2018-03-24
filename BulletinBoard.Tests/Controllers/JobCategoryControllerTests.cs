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

        #region Index

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
            var viewResult = (ViewResult)result;
            var model = (IList<JobCategoryViewModel>)viewResult.Model;

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
            var viewResult = (ViewResult)result;
            var model = (IList<JobCategoryViewModel>)viewResult.Model;

            Assert.Empty(model);
        }

        [Fact]
        public async Task Index_Given_ResultFromService_Should_ReturnViewModelWithExpectedName()
        {
            // Arrange
            const string expectedName = "Example";

            var items = new List<JobCategory>
            {
                new JobCategory
                {
                    Name = expectedName
                }
            };

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetAllCategories()).ReturnsAsync(items);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            var model = (IList<JobCategoryViewModel>)viewResult.Model;
            var actualName = model.First().Name;

            Assert.Equal(expectedName, actualName);
        }

        #endregion

        #region Details

        [Fact]
        public async Task Details_Given_NullId_Should_ReturnNotFoundView()
        {
            // Arrange
            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetCategoryById(It.IsAny<string>())).ReturnsAsync((JobCategory) null);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Details(null);

            // Assert
            var viewResult = (ViewResult) result;

            Assert.Equal("NotFound", viewResult.ViewName);
        }

        [Fact]
        public async Task Details_Given_IdThatDoesNotExist_Should_ReturnNotFoundView()
        {
            // Arrange
            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetCategoryById(It.IsAny<string>())).ReturnsAsync((JobCategory)null);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Details("1");

            // Assert
            var viewResult = (ViewResult)result;

            Assert.Equal("NotFound", viewResult.ViewName);
        }

        #endregion
    }
}
