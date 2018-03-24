using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using BulletinBoard.Services;
using Moq;
using Xunit;

namespace BulletinBoard.Tests.Services
{
    public class JobCategoryServiceTests
    {
        #region GetAllCategories

        [Fact]
        public async Task GetAllCategories_Given_EmptyRepository_Should_ReturnEmptyResult()
        {
            // Arrange
            var repoMock = new Mock<IJobCategoryRepository>();
            repoMock.Setup(x => x.GetAll()).ReturnsAsync(new List<JobCategory>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var service = new JobCategoryService(repoMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await service.GetAllCategories();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCategories_Given_RepositoryWithItems_Should_ReturnCorrectNumberOfItems()
        {
            // Arrange
            var items = new List<JobCategory>
            {
                new JobCategory(),
                new JobCategory(),
                new JobCategory()
            };

            var repoMock = new Mock<IJobCategoryRepository>();
            repoMock.Setup(x => x.GetAll()).ReturnsAsync(items);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var service = new JobCategoryService(repoMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await service.GetAllCategories();

            // Assert
            var list = result.ToList();
            Assert.Equal(3, list.Count);
        }

        [Fact]
        public async Task GetAllCategories_Given_RepositoryWithItem_Should_ReturnResultThatContainsIt()
        {
            // Arrange
            const string expectedName = "Category2";
            var items = new List<JobCategory>
            {
                new JobCategory {Name = "Category1"},
                new JobCategory {Name = expectedName},
                new JobCategory {Name = "Category3"}
            };

            var repoMock = new Mock<IJobCategoryRepository>();
            repoMock.Setup(x => x.GetAll()).ReturnsAsync(items);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var service = new JobCategoryService(repoMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await service.GetAllCategories();

            // Assert
            Assert.Contains(result, x => x.Name == expectedName);
        }

        #endregion
    }
}
