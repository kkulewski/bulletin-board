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
using BulletinBoard.Infrastructure.AutoMapper;

namespace BulletinBoard.Tests.Controllers
{
    public class JobCategoryControllerTests
    {
        private readonly IMapper _mapper;

        public JobCategoryControllerTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DefaultProfile>();
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
            var item1 = new JobCategory { Name = "Item1" };
            var item2 = new JobCategory { Name = "Item2" };
            var item3 = new JobCategory { Name = "Item3" };

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetCategoryById("1")).ReturnsAsync(item1);
            serviceMock.Setup(x => x.GetCategoryById("2")).ReturnsAsync(item2);
            serviceMock.Setup(x => x.GetCategoryById("3")).ReturnsAsync(item3);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Details("9");

            // Assert
            var viewResult = (ViewResult) result;

            Assert.Equal("NotFound", viewResult.ViewName);
        }

        [Fact]
        public async Task Details_Given_ExistingItemId_Should_ReturnCorrectViewModel()
        {
            // Arrange
            var item1 = new JobCategory { Name = "Item1" };
            var item2 = new JobCategory { Name = "Item2" };
            var item3 = new JobCategory { Name = "Item3" };
            var expectedName = item2.Name;

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.GetCategoryById("1")).ReturnsAsync(item1);
            serviceMock.Setup(x => x.GetCategoryById("2")).ReturnsAsync(item2);
            serviceMock.Setup(x => x.GetCategoryById("3")).ReturnsAsync(item3);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Details("2");

            // Assert
            var viewResult = (ViewResult) result;
            var model = (DetailsJobCategoryViewModel) viewResult.Model;

            Assert.Equal(expectedName, model.Name);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_Given_NoModel_Should_ReturnViewResultType()
        {
            // Arrange
            var serviceMock = new Mock<IJobCategoryService>();
            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Given_ModelWithInvalidState_Should_ReturnSameModel()
        {
            // Arrange
            const string expectedName = "ExampleName";
            var vm = new CreateJobCategoryViewModel {Name = expectedName };

            var serviceMock = new Mock<IJobCategoryService>();
            var controller = new JobCategoryController(serviceMock.Object, _mapper);
            controller.ModelState.AddModelError("Key", "Message");

            // Act
            var result = await controller.Create(vm);

            // Assert
            var viewResult = (ViewResult) result;
            var model = (CreateJobCategoryViewModel) viewResult.Model;

            Assert.Equal(expectedName, model.Name);
        }

        [Fact]
        public async Task Create_Given_ServiceAddFailure_Should_ReturnNotFoundView()
        {
            // Arrange
            var vm = new CreateJobCategoryViewModel();

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.Add(It.IsAny<JobCategory>())).ReturnsAsync(false);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Create(vm);

            // Assert
            var viewResult = (ViewResult) result;

            Assert.Equal("NotFound", viewResult.ViewName);
        }

        [Fact]
        public async Task Create_Given_ServiceAddSuccess_Should_ReturnRedirectToActionResult()
        {
            // Arrange
            var vm = new CreateJobCategoryViewModel();

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.Add(It.IsAny<JobCategory>())).ReturnsAsync(true);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Create(vm);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Create_Given_ServiceAddSuccess_Should_RedirectToIndex()
        {
            // Arrange
            var vm = new CreateJobCategoryViewModel();

            var serviceMock = new Mock<IJobCategoryService>();
            serviceMock.Setup(x => x.Add(It.IsAny<JobCategory>())).ReturnsAsync(true);

            var controller = new JobCategoryController(serviceMock.Object, _mapper);

            // Act
            var result = await controller.Create(vm);
            var redirectResult = (RedirectToActionResult) result;

            // Assert
            Assert.Equal("Index", redirectResult.ActionName);
        }

        #endregion
    }
}
