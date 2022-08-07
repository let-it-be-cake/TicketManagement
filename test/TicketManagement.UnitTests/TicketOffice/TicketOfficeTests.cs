using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;
using TicketManagement.PurchaseApi.Proxys;
using TicketManagement.VenueApi.Proxys;

namespace TicketManagement.UnitTests.TicketOfficeTesting
{
    public class TicketOfficeTests
    {
        private Mock<IRepository<Event>> _eventRepository;
        private Mock<IRepository<EventArea>> _eventAreaRepository;
        private Mock<IRepository<EventSeat>> _eventSeatRepository;
        private Mock<IRepository<Ticket>> _ticketRepository;
        private Mock<IRepository<User>> _userRepository;

        private Mock<IQuerableHelper> _toList;

        [SetUp]
        public void SetUp()
        {
            _eventRepository = new Mock<IRepository<Event>>();
            _eventRepository.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Events.AsQueryable());

            _eventAreaRepository = new Mock<IRepository<EventArea>>();
            _eventAreaRepository.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventAreas.AsQueryable());

            _eventSeatRepository = new Mock<IRepository<EventSeat>>();
            _eventSeatRepository.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventSeats.AsQueryable());

            _ticketRepository = new Mock<IRepository<Ticket>>();
            _ticketRepository.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Tickets.AsQueryable());

            _userRepository = new Mock<IRepository<User>>();

            _toList = new Mock<IQuerableHelper>();
            _toList.Setup(o => o.ToListAsync(It.IsAny<IQueryable<Ticket>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<Ticket> item, CancellationToken token) => Task.FromResult(item.ToList()));
        }

        [Test]
        public void BuyTicket_WhenAllSeatsEmpty_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository.Object,
                                         _eventAreaRepository.Object,
                                         _eventSeatRepository.Object,
                                         _ticketRepository.Object,
                                         _userRepository.Object,
                                         _toList.Object);
            var user = new User
            {
                Email = "user@mail.com",
                Money = 0,
            };

            var eventSeats = new List<EventSeat>();

            // Act
            Func<Task> result = async () => await proxy.BuyTicketAsync(user.Id, eventSeats);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'user')");
        }

        [Test]
        public void BuyTicket_WhenSeatsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository.Object,
                                         _eventAreaRepository.Object,
                                         _eventSeatRepository.Object,
                                         _ticketRepository.Object,
                                         _userRepository.Object,
                                         _toList.Object);
            var user = new User
            {
                Email = "user@mail.com",
            };

            // Act
            Func<Task> result = async () => await proxy.BuyTicketAsync(user.Id, null);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'user')");
        }

        [Test]
        public void BuyTicket_WhenUserIdIsZero_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository.Object,
                                         _eventAreaRepository.Object,
                                         _eventSeatRepository.Object,
                                         _ticketRepository.Object,
                                         _userRepository.Object,
                                         _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.BuyTicketAsync(0, DataBaseTableRecords.EventSeats);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'user')");
        }

        [Test]
        public async Task GetUserTicektsAsync_WhenUserIdCorrect_ShouldReturnUserTickets()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository.Object,
                                         _eventAreaRepository.Object,
                                         _eventSeatRepository.Object,
                                         _ticketRepository.Object,
                                         _userRepository.Object,
                                         _toList.Object);

            var expected = new List<Ticket>
            {
                new Ticket
                {
                    Id = 3,
                    Name = "Birthday",
                    Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                    StartEventDate = new DateTime(2021, 04, 19, 17, 10, 00),
                    EndEventDate = new DateTime(2021, 04, 19, 19, 10, 00),
                    Price = 0,
                    UserId = 3,
                },
                new Ticket
                {
                    Id = 100,
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    StartEventDate = new DateTime(2021, 04, 19, 20, 30, 00),
                    EndEventDate = new DateTime(2021, 04, 19, 22, 30, 00),
                    Price = 0,
                    UserId = 3,
                },
            };

            // Act
            List<Ticket> result = await proxy.GetUserTicektsAsync(3);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetUserTicektsAsync_WhenUserIdInCorrect_ShouldReturnEmptyCollection()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository.Object,
                                         _eventAreaRepository.Object,
                                         _eventSeatRepository.Object,
                                         _ticketRepository.Object,
                                         _userRepository.Object,
                                         _toList.Object);

            // Act
            List<Ticket> result = await proxy.GetUserTicektsAsync(0);

            // Assert
            result.Should()
                .BeEmpty();
        }
    }
}
