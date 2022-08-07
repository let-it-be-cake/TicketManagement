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
    public class LayoutValidationTests
    {
        private Mock<IRepository<Layout>> _repositoryMock;
        private Layout _addedLayout;
        private Layout _changedLayout;

        private Mock<IQuerableHelper> _toListMock;

        [SetUp]
        public void SetUp()
        {
            _addedLayout = null;
            _changedLayout = new Layout
            {
                Id = 1,
                VenueId = 1,
                Description = "Layout To Update",
            };

            _repositoryMock = new Mock<IRepository<Layout>>();
            _repositoryMock.Setup(o => o.GetAll()).
                Returns(() => DataBaseTableRecords.Layouts.AsQueryable());
            _repositoryMock.Setup(o => o.CreateAsync(It.IsAny<Layout>()))
                .Callback((Layout item) => _addedLayout = item);
            _repositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Layout>()))
                .Callback((Layout item) => _changedLayout = item);

            _toListMock = new Mock<IQuerableHelper>();
        }

        [Test]
        public async Task AddValidation_WhenCorrectLayout_ShouldReturnTrue()
        {
            // Arrange
            var layoutToAdd = new Layout
            {
                Id = 1,
                VenueId = 1,
                Description = "Not first layout",
            };

            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.AddAsync(layoutToAdd);

            // Assert
            _addedLayout.Should()
                .BeEquivalentTo(layoutToAdd);
        }

        [Test]
        public void AddValidation_WhenIncorrectLayout_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var layoutToAdd = new Layout
            {
                Id = 1,
                VenueId = 1,
                Description = "First layout",
            };

            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = () => proxy.AddAsync(layoutToAdd);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Layout record with this VenueId, Description fields already exists in the database.");
        }

        [Test]
        public void AddValidation_WhenIncorrectLayout_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task ChangeValidation_WhenCorrectLayout_ShouldReturnTrue()
        {
            // Arrange
            var layoutToChange = new Layout
            {
                Id = 1,
                VenueId = 1,
                Description = "Not first layout",
            };

            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.ChangeAsync(layoutToChange);

            // Assert
            _changedLayout.Should()
                .BeEquivalentTo(layoutToChange);
        }

        [Test]
        public void ChangeValidation_WhenIncorrectLayout_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var layoutToChange = new Layout
            {
                Id = 1,
                VenueId = 1,
                Description = "First layout",
            };

            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(layoutToChange);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Layout record with this VenueId, Description fields already exists in the database.");
        }

        [Test]
        public void ChangeValidation_WhenIncorrectLayout_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new LayoutProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}