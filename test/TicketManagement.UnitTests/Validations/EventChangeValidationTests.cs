using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.Entities.Tables.Enum;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.UnitTests.ValidationsTests
{
    public class EventChangeValidationTests
    {
        private Mock<IRepository<Event>> _eventRepositoryMock;
        private Mock<IRepository<Area>> _areaRepositoryMock;
        private Mock<IRepository<Seat>> _seatRepositoryMock;
        private Mock<IRepository<EventArea>> _eventAreaRepositoryMock;
        private Mock<IRepository<EventSeat>> _eventSeatRepositoryMock;

        private Mock<IQuerableHelper> _toListMock;

        private Event _changedEvent;

        [SetUp]
        public void SetUp()
        {
            _changedEvent = new Event
            {
                Id = 1,
                Name = "Event To Update",
                Description = "Event To Update",
                LayoutId = 1,
            };

            _eventRepositoryMock = new Mock<IRepository<Event>>();
            _eventRepositoryMock.Setup(o => o.CreateAsync(It.IsAny<Event>()));
            _eventRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<Event>()))
                .Callback((Event item) => _changedEvent = item);
            _eventRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Events.AsQueryable());

            _eventAreaRepositoryMock = new Mock<IRepository<EventArea>>();
            _eventAreaRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventAreas.AsQueryable());

            _eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            _eventSeatRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventSeats.AsQueryable());

            _areaRepositoryMock = new Mock<IRepository<Area>>();
            _areaRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Areas.AsQueryable());

            _seatRepositoryMock = new Mock<IRepository<Seat>>();
            _seatRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Seats.AsQueryable());

            _toListMock = new Mock<IQuerableHelper>();
            _toListMock.Setup(o => o.ToListAsync(It.IsAny<IQueryable<Area>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<Area> item, CancellationToken token) => Task.FromResult(item.ToList()));
            _toListMock.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventSeat>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventSeat> item, CancellationToken token) => Task.FromResult(item.ToList()));
            _toListMock.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventArea>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventArea> item, CancellationToken token) => Task.FromResult(item.ToList()));
        }

        [Test]
        public async Task ChangeValidation_WhenCorrectEventAndSeatsNotSould_ShouldReturnTrue()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepositoryMock.Object,
                _areaRepositoryMock.Object,
                _seatRepositoryMock.Object,
                _eventAreaRepositoryMock.Object,
                _eventSeatRepositoryMock.Object,
                _toListMock.Object);

            var eventToChange = new Event
            {
                Id = 6,
                Name = "My birthday",
                Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                LayoutId = 1,
                DateTimeStart = new DateTime(2030, 01, 01),
                DateTimeEnd = new DateTime(2030, 02, 02),
            };

            // Act
            await proxy.ChangeAsync(eventToChange);

            // Assert
            _changedEvent.Should()
                .BeEquivalentTo(eventToChange);
        }

        [Test]
        public void ChangeValidation_WhenCorrectEventAlreadyEnded_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepositoryMock.Object,
                _areaRepositoryMock.Object,
                _seatRepositoryMock.Object,
                _eventAreaRepositoryMock.Object,
                _eventSeatRepositoryMock.Object,
                _toListMock.Object);

            var eventToChange = new Event
            {
                Id = 6,
                Name = "My birthday",
                Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                LayoutId = 1,
                DateTimeStart = new DateTime(2000, 01, 01),
                DateTimeEnd = new DateTime(2000, 02, 02),
            };

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(eventToChange);

            // Assert
            testAction.Should().Throw<ArgumentException>()
                .WithMessage("The event has already ended.");
        }

        [Test]
        public void ChangeValidation_WhenCorrectEventAndSeatsSould_ShouldReturnArgumentException()
        {
            // Arrange
            var seats = DataBaseTableRecords.EventSeats;
            seats.Add(
                new EventSeat
                {
                    Id = 17,
                    EventAreaId = 6,
                    Number = 19,
                    Row = 15,
                    State = SeatState.Sold,
                });

            _eventSeatRepositoryMock = new Mock<IRepository<EventSeat>>();
            _eventSeatRepositoryMock.Setup(o => o.GetAll())
                .Returns(() => seats.AsQueryable());

            var proxy = new EventProxy(
                _eventRepositoryMock.Object,
                _areaRepositoryMock.Object,
                _seatRepositoryMock.Object,
                _eventAreaRepositoryMock.Object,
                _eventSeatRepositoryMock.Object,
                _toListMock.Object);

            var eventToChange = new Event
            {
                Id = 6,
                Name = "My birthday",
                Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                LayoutId = 1,
                DateTimeStart = new DateTime(2030, 01, 01),
                DateTimeEnd = new DateTime(2030, 02, 02),
            };

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(eventToChange);

            // Assert
            testAction.Should().Throw<ArgumentException>()
                .WithMessage("Some seats were sold.");
        }

        [Test]
        public void ChangeValidation_WhenIncorrectEvent_ShouldReturnNullExcption()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepositoryMock.Object,
                _areaRepositoryMock.Object,
                _seatRepositoryMock.Object,
                _eventAreaRepositoryMock.Object,
                _eventSeatRepositoryMock.Object,
                _toListMock.Object);

            // Act
            Func<Task> testAction = async () => await proxy.ChangeAsync(null);

            // Assert
            testAction.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}