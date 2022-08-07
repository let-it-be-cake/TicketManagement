using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Exceptions;
using TicketManagement.VenueApi.Proxys;

namespace TicketManagement.UnitTests.ValidationsTests
{
    public class AreaValidationTests
    {
        private Mock<IRepository<Area>> _repositoryMock;
        private Area _addedArea;
        private Area _changedArea;

        private Mock<IQuerableHelper> _toListMock;

        [SetUp]
        public void SetUp()
        {
            _addedArea = null;
            _changedArea = new Area
            {
                Id = 1,
                LayoutId = 2,
                Description = "Area To Update",
                CoordX = 1,
                CoordY = 1,
            };

            _repositoryMock = new Mock<IRepository<Area>>();
            _repositoryMock.Setup(o => o.GetAll()).
                Returns(() => DataBaseTableRecords.Areas.AsQueryable());
            _repositoryMock.Setup(o => o.CreateAsync(It.IsAny<Area>()))
                .Callback((Area item) => _addedArea = item);
            _repositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Area>()))
                .Callback((Area item) => _changedArea = item);

            _toListMock = new Mock<IQuerableHelper>();
        }

        [Test]
        public async Task AddValidation_WhenCorrectArea_ShouldReturnTrue()
        {
            // Arrange
            var areaToAdd = new Area
            {
                Id = 1,
                LayoutId = 2,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
            };

            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.AddAsync(areaToAdd);

            // Assert
            _addedArea.Should()
                .BeEquivalentTo(areaToAdd);
        }

        [Test]
        public void AddValidation_WhenIncorrectArea_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var areaToAdd = new Area
            {
                Id = 1,
                LayoutId = 1,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
            };

            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(areaToAdd);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Area record with this LayoutId, CoordX, CoordY, Description fields already exists in the database.");
        }

        [Test]
        public void AddValidation_WhenIncorrectArea_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task ChangeValidation_WhenCorrectArea_ShouldReturnTrue()
        {
            // Arrange
            var areaToChange = new Area
            {
                Id = 1,
                LayoutId = 2,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
            };

            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.ChangeAsync(areaToChange);

            // Assert
            _changedArea.Should()
                .BeEquivalentTo(areaToChange);
        }

        [Test]
        public void ChangeValidation_WhenIncorrectArea_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var areaToChange = new Area
            {
                Id = 1,
                LayoutId = 1,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
            };

            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(areaToChange);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Area record with this LayoutId, CoordX, CoordY, Description fields already exists in the database.");
        }

        [Test]
        public void ChangeValidation_WhenIncorrectArea_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new AreaProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}