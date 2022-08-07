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
    public class SeatValidationTests
    {
        private Mock<IRepository<Seat>> _repositoryMock;
        private Seat _addedSeat;
        private Seat _changedSeat;

        private Mock<IQuerableHelper> _toListMock;

        [SetUp]
        public void SetUp()
        {
            _addedSeat = null;
            _changedSeat = new Seat
            {
                AreaId = 1,
                Row = 1,
                Number = 1,
            };

            _repositoryMock = new Mock<IRepository<Seat>>();
            _repositoryMock.Setup(o => o.GetAll()).
                Returns(() => DataBaseTableRecords.Seats.AsQueryable());
            _repositoryMock.Setup(o => o.CreateAsync(It.IsAny<Seat>()))
                .Callback((Seat item) => _addedSeat = item);
            _repositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Seat>()))
                .Callback((Seat item) => _changedSeat = item);

            _toListMock = new Mock<IQuerableHelper>();
        }

        [Test]
        public async Task AddValidation_WhenCorrectSeat_ShouldReturnTrue()
        {
            // Arrange
            var seatToAdd = new Seat
            {
                AreaId = 1,
                Row = 3,
                Number = 1,
            };

            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.AddAsync(seatToAdd);

            // Assert
            _addedSeat.Should()
                .BeEquivalentTo(seatToAdd);
        }

        [Test]
        public void AddValidation_WhenIncorrectSeat_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var seatToAdd = new Seat
            {
                AreaId = 6,
                Row = 2,
                Number = 1,
            };

            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(seatToAdd);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Seat record with this AreaId, Row, Number fields already exists in the database.");
        }

        [Test]
        public void AddValidation_WhenIncorrectSeat_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.AddAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task ChangeValidation_WhenCorrectSeat_ShouldReturnTrue()
        {
            // Arrange
            var seatToChange = new Seat
            {
                AreaId = 1,
                Row = 3,
                Number = 1,
            };

            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            await proxy.ChangeAsync(seatToChange);

            // Assert
            _changedSeat.Should()
                .BeEquivalentTo(seatToChange);
        }

        [Test]
        public void ChangeValidation_WhenIncorrectSeat_ShouldReturnItemAlreadyContainsException()
        {
            // Arrange
            var seatToChange = new Seat
            {
                AreaId = 6,
                Row = 2,
                Number = 1,
            };

            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(seatToChange);

            // Assert
            testAction.Should().Throw<RecordAlreadyContainsException>()
                .WithMessage("The Seat record with this AreaId, Row, Number fields already exists in the database.");
        }

        [Test]
        public void ChangeValidation_WhenIncorrectSeat_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new SeatProxy(_repositoryMock.Object, _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}