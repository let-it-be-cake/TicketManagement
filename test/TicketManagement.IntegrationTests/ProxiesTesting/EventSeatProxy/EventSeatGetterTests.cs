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
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.IntegrationTests.ProxiesTesting.EventSeatGetterTests
{
    public class EventSeatGetterTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<EventSeat> _eventSeatRepository;

        private IQuerableHelper _toListAsync;

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

            _eventSeatRepository = new EFRepository<EventSeat>(_context);

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
        public async Task GetAsync_WhenIdIsCorrect_ShouldReturnCorrectEventSeat()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            var expected = new EventSeat
            {
                Id = 100,
                EventAreaId = 100,
                Number = 1,
                TicketId = 100,
                Row = 1,
            };

            // Act
            EventSeat result = await proxy.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_WhenIdNotCorrect_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            // Act
            EventSeat result = await proxy.GetAsync(0);

            // Assert
            result.Should()
                .BeNull();
        }

        [Test]
        public async Task GetFromEventAreaAsync_WhenIdIsCorrect_ShouldReturnEventSeatCollection()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            var expected = new List<EventSeat>
            {
                new EventSeat
                {
                    Id = 100,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 1,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 101,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 2,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 102,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 3,
                    State = 0,
                },
            };

            // Act
            List<EventSeat> result = await proxy.GetFromEventAreaAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetFromEventAreaAsync_WhenIdNotCorrect_ShouldReturnEmptyCollection()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            // Act
            List<EventSeat> result = await proxy.GetFromEventAreaAsync(0);

            // Assert
            result.Should()
                .BeEmpty();
        }

        [Test]
        public async Task GetEventSeatsFromTicketAsync_WhenIdNotCorrect_ShouldReturnEmptyCollection()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            // Act
            List<EventSeat> result = await proxy.GetEventSeatsFromTicketAsync(0);

            // Assert
            result.Should()
                .BeEmpty();
        }

        [Test]
        public async Task GetEventSeatsFromTicketAsync_WhenIdCorrect_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new EventSeatGetter(_eventSeatRepository,
                                           _toListAsync);

            var expected = new List<EventSeat>
            {
                new EventSeat
                {
                    Id = 100,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 1,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 101,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 2,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 102,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 3,
                    State = 0,
                },
            };

            // Act
            List<EventSeat> result = await proxy.GetEventSeatsFromTicketAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
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
