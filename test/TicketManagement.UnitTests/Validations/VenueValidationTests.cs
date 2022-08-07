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
    public class VenueValidationTests
    {
        private Mock<IRepository<Venue>> _repositoryMock;
        private Venue _addedVenue;
        private Venue _changedVenue;

        private Mock<IQuerableHelper> _toListMock;

        [SetUp]
        public void SetUp()
        {
            _addedVenue = null;
            _changedVenue = new Venue
            {
                Id = 1,
                Address = "Venue To Update",
                Description = "Venue To Update",
                Phone = "823 45 678 90 12",
            };

            _repositoryMock = new Mock<IRepository<Venue>>();
            _repositoryMock.Setup(o => o.GetAll()).
                Returns(() => DataBaseTableRecords.Venues.AsQueryable());
            _repositoryMock.Setup(o => o.CreateAsync(It.IsAny<Venue>()))
                .Callback((Venue item) => _addedVenue = item);
            _repositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Venue>()))
                .Callback((Venue item) => _changedVenue = item);

            _toListMock = new Mock<IQuerableHelper>();
        }

        [Test]
        public async Task AddValidation_WhenCorrectVenue_ShouldReturnTrue()
        {
            // Arrange
            var venueToAdd = new Venue
            {
                Id = 1,
                Address = "First venue",
                Description = "First venue address",
                Phone = "823 45 678 90 12",
            };

            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.AddAsync(venueToAdd);

            // Assert
            _addedVenue.Should()
                .BeEquivalentTo(venueToAdd);
        }

        [Test]
        public void AddValidation_WhenIncorrectVenue_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var venueToAdd = new Venue
            {
                Id = 1,
                Description = "First venue",
                Address = "First venue Address",
                Phone = "123 45 678 90 12",
            };

            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(venueToAdd);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Venue record with this Address, Phone, Description fields already exists in the database.");
        }

        [Test]
        public void AddValidation_WhenIncorrectVenue_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task ChangeValidation_WhenCorrectVenue_ShouldReturnTrue()
        {
            // Arrange
            var venueToChange = new Venue
            {
                Id = 1,
                Address = "First venue",
                Description = "First venue address",
                Phone = "823 45 678 90 12",
            };

            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.ChangeAsync(venueToChange);

            // Assert
            _changedVenue.Should()
                .BeEquivalentTo(venueToChange);
        }

        [Test]
        public void ChangeValidation_WhenIncorrectVenue_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var venueToChange = new Venue
            {
                Id = 1,
                Description = "First venue",
                Address = "First venue Address",
                Phone = "123 45 678 90 12",
            };

            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(venueToChange);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Venue record with this Address, Phone, Description fields already exists in the database.");
        }

        [Test]
        public void ChangeValidation_WhenIncorrectVenue_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new VenueProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}