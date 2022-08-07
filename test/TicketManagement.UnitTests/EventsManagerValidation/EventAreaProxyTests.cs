using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Models;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.UnitTests.EventGetterValidation
{
    public class EventManagerTests
    {
        private Mock<IRepository<Seat>> _seatRepositoryMock;
        private Mock<IRepository<Area>> _areaRepositoryMock;

        private Mock<IRepository<EventSeat>> _eventSeatRepositoryMock;
        private Mock<IRepository<EventArea>> _eventAreaRepositoryMock;
        private Mock<IRepository<Event>> _eventRepositoryMock;

        private Mock<IQuerableHelper> _toList;

        [SetUp]
        public void SetUp()
        {
            _seatRepositoryMock = new Mock<IRepository<Seat>>();
            _seatRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Seats.AsQueryable());
            _seatRepositoryMock.Setup(o => o.GetAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataBaseTableRecords.Seats.FirstOrDefault(o => o.Id == id)));

            _areaRepositoryMock = new Mock<IRepository<Area>>();
            _areaRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Areas.AsQueryable());
            _areaRepositoryMock.Setup(o => o.GetAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataBaseTableRecords.Areas.FirstOrDefault(o => o.Id == id)));

            _eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            _eventSeatRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventSeats.AsQueryable());
            _eventSeatRepositoryMock.Setup(o => o.GetAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataBaseTableRecords.EventSeats.FirstOrDefault(o => o.Id == id)));

            _eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            _eventAreaRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventAreas.AsQueryable());
            _eventAreaRepositoryMock.Setup(o => o.GetAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataBaseTableRecords.EventAreas.FirstOrDefault(o => o.Id == id)));

            _eventRepositoryMock = new Mock<IRepository<Event>>();
            _eventRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Events.AsQueryable());

            _toList = new Mock<IQuerableHelper>();
            _toList.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventAreaModel>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventAreaModel> item, CancellationToken token) => Task.FromResult(item.ToList()));
            _toList.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventArea>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventArea> item, CancellationToken token) => Task.FromResult(item.ToList()));
        }

        [Test]
        public async Task GetUnregisterEventsAsync_WhenFromGreater0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            var result = await proxy.GetUnregisterEventsAsync(1, 10);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetUnregisterEventsAsync_WhenFromEqual0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            List<Event> result = await proxy.GetUnregisterEventsAsync(0, 10);

            // Assert
            result.Should()
                .BeNull();
        }

        [Test]
        public void GetUnregisterEventsAsync_WhenFromLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterEventsAsync(-1, 10);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'from')");
        }

        [Test]
        public async Task GetUnregisterEventsAsync_WhenHowManyGreater0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            var result = await proxy.GetUnregisterEventsAsync(0, 10);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetUnregisterEventsAsync_WhenHowManyLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterEventsAsync(0, -1);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }

        [Test]
        public void GetUnregisterEventsAsync_WhenHowManyEqual0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterEventsAsync(0, 0);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }

        [Test]
        public async Task GetRegisterEventsAsync_WhenFromGreater0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            var result = await proxy.GetRegisterEventsAsync(1, 10);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetRegisterEventsAsync_WhenFromEqual0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            List<Event> result = await proxy.GetRegisterEventsAsync(0, 10);

            // Assert
            result.Should()
                .BeNull();
        }

        [Test]
        public void GetRegisterEventsAsync_WhenFromLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetRegisterEventsAsync(-1, 10);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'from')");
        }

        [Test]
        public async Task GetRegisterEventsAsync_WhenHowManyGreater0_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            var result = await proxy.GetRegisterEventsAsync(0, 10);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetRegisterEventsAsync_WhenHowManyLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetRegisterEventsAsync(0, -1);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }

        [Test]
        public void GetRegisterEventsAsync_WhenHowManyEqual0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(_eventRepositoryMock.Object,
                                       _areaRepositoryMock.Object,
                                       _seatRepositoryMock.Object,
                                       _eventAreaRepositoryMock.Object,
                                       _eventSeatRepositoryMock.Object,
                                       _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetRegisterEventsAsync(0, 0);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }
    }
}