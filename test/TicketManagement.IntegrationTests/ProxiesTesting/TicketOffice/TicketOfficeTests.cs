using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;
using TicketManagement.Entities.Tables.Enum;
using TicketManagement.PurchaseApi.Exceptions;
using TicketManagement.PurchaseApi.Proxys;

namespace TicketManagement.IntegrationTests.ProxiesTesting.TicketOfficeTests
{
    public class TicketOfficeTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<Event> _eventRepository;
        private IRepository<EventArea> _eventAreaRepository;
        private IRepository<EventSeat> _eventSeatRepository;
        private IRepository<Ticket> _ticketRepository;
        private IRepository<User> _userRepository;

        private IQuerableHelper _toListAsync;

        public object EvenSeatState { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configs = new ConfigurationBuilder()
                .AddXmlFile("App.config")
                .Build();
            _connectionString = configs["connectionStrings:add:SqlDataBaseConnectionString:connectionString"].ToString();
        }

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketManagementContext>()
                   .UseSqlServer(_connectionString)
                   .EnableSensitiveDataLogging()
                   .Options;
            _context = new TicketManagementContext(optionsBuilder);

            _eventRepository = new EFEventRepository(_context);
            _eventAreaRepository = new EFRepository<EventArea>(_context);
            _eventSeatRepository = new EFRepository<EventSeat>(_context);
            _ticketRepository = new EFRepository<Ticket>(_context);
            _userRepository = new EFRepository<User>(_context);

            _toListAsync = new EFQuerableToListAsync();

            using var sqlCommand = new SqlCommand
            {
                CommandText = @"EXEC [dbo].sp_AddTestingData",
            };

            using var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();
        }

        [TearDown]
        public void TearDown()
        {
            using var sqlCommand = new SqlCommand
            {
                CommandText = @"EXEC [dbo].[sp_DeleteTestingData]",
            };

            using var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();
        }

        [Test]
        public async Task BuyTicket_WhenAllCorrect_ShouldBuyTicketToUser()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            var eventSeats = new List<EventSeat>();

            eventSeats.Add(await _eventSeatRepository.GetAsync(103));
            eventSeats.Add(await _eventSeatRepository.GetAsync(104));

            User user = await _userRepository.GetAsync(3);
            user.Money = 10000;

            // Act
            await _userRepository.UpdateAsync(user);
            await proxy.BuyTicketAsync(user.Id, eventSeats);

            var resultEventSeats = new List<EventSeat>();

            resultEventSeats.Add(await _eventSeatRepository.GetAsync(103));
            resultEventSeats.Add(await _eventSeatRepository.GetAsync(104));

            int ticketId = resultEventSeats[0].TicketId.Value;

            Ticket resultTicket = await _ticketRepository.GetAsync(ticketId);

            // Assert
            resultTicket.Should()
                .BeEquivalentTo(
                new Ticket
                {
                    Id = 101,
                    UserId = 3,
                    Price = 800,
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    StartEventDate = new DateTime(2023, 04, 19, 20, 30, 00),
                    EndEventDate = new DateTime(2023, 04, 19, 22, 30, 00),
                });

            resultEventSeats.Should()
                .BeEquivalentTo(
                new List<EventSeat>
                {
                    new EventSeat
                    {
                        Id = 103,
                        EventAreaId = 101,
                        Number = 2,
                        Row = 2,
                        State = SeatState.Sold,
                        TicketId = 101,
                    },
                    new EventSeat
                    {
                        Id = 104,
                        EventAreaId = 101,
                        Number = 1,
                        Row = 1,
                        State = SeatState.Sold,
                        TicketId = 101,
                    },
                });
        }

        [Test]
        public async Task BuyTicket_WhenAllUserHaventEnoughtMoney_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            var eventSeats = new List<EventSeat>();

            eventSeats.Add(await _eventSeatRepository.GetAsync(103));
            eventSeats.Add(await _eventSeatRepository.GetAsync(104));

            User user = await _userRepository.GetAsync(3);
            user.Money = 0;

            // Act
            await _userRepository.UpdateAsync(user);
            Func<Task> result = async () => await proxy.BuyTicketAsync(user.Id, eventSeats);

            // Assert
            result.Should()
                .Throw<NotEnoughMoney>()
                .WithMessage("The user10@gmail.com doesn't have enough money.");
        }

        [Test]
        public async Task BuyTicket_WhenAllSeatsEmpty_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            var eventSeats = new List<EventSeat>();

            User user = await _userRepository.GetAsync(3);
            user.Money = 0;

            // Act
            await _userRepository.UpdateAsync(user);
            Func<Task> result = async () => await proxy.BuyTicketAsync(user.Id, eventSeats);

            // Assert
            result.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Sequence contains no elements");
        }

        [Test]
        public async Task BuyTicket_WhenSeatsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            User user = await _userRepository.GetAsync(3);
            user.Money = 0;

            // Act
            await _userRepository.UpdateAsync(user);
            Func<Task> result = async () => await proxy.BuyTicketAsync(user.Id, null);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'eventSeats')");
        }

        [Test]
        public async Task BuyTicket_WhenUserIdIsZero_ShouldReturnException()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            var eventSeats = new List<EventSeat>();

            eventSeats.Add(await _eventSeatRepository.GetAsync(103));
            eventSeats.Add(await _eventSeatRepository.GetAsync(104));

            // Act
            Func<Task> result = async () => await proxy.BuyTicketAsync(0, eventSeats);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'user')");
        }

        [Test]
        public async Task GetUserTicektsAsync_WhenUserIdCorrect_ShouldReturnUserTickets()
        {
            // Arrange
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            var expected = new List<Ticket>
            {
                new Ticket
                {
                    Id = 3,
                    Name = "Birthday",
                    Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                    StartEventDate = new DateTime(2023, 04, 19, 17, 10, 00),
                    EndEventDate = new DateTime(2023, 04, 19, 19, 10, 00),
                    Price = 0,
                    UserId = 3,
                },
                new Ticket
                {
                    Id = 100,
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    StartEventDate = new DateTime(2023, 04, 19, 20, 30, 00),
                    EndEventDate = new DateTime(2023, 04, 19, 22, 30, 00),
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
            var proxy = new TicketService(_eventRepository,
                                         _eventAreaRepository,
                                         _eventSeatRepository,
                                         _ticketRepository,
                                         _userRepository,
                                         _toListAsync);

            // Act
            List<Ticket> result = await proxy.GetUserTicektsAsync(0);

            // Assert
            result.Should()
                .BeEmpty();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }
    }
}
