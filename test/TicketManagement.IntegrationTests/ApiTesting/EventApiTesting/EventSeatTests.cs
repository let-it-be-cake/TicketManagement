using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.IntegrationTests.ApiTesting.EventApiTesting
{
    public class EventSeatTests : ControllerTestsSetUp, IDisposable
    {
        private HttpClient _httpClient;

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
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(EventApiAddress);

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
            var expected = new EventSeat
            {
                Id = 100,
                EventAreaId = 100,
                Number = 1,
                TicketId = 100,
                Row = 1,
            };

            // Act
            HttpResponseMessage response = await _httpClient.GetAsync("/seat/100");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<EventSeat>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_WhenIdNotCorrect_ShouldReturn404NotFound()
        {
            // Arrange & Act
            HttpResponseMessage result = await _httpClient.GetAsync("/seat/0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetFromEventAreaAsync_WhenIdIsCorrect_ShouldReturnEventSeatCollection()
        {
            // Arrange
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
            HttpResponseMessage response =
                await _httpClient.GetAsync(EventApiAddress + "/seat/get-area-seats/100");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<EventSeat>>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetFromEventAreaAsync_WhenIdNotCorrect_ShouldReturnEmptyCollection()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync(EventApiAddress + "/seat/get-area-seats/0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetEventSeatsFromTicketAsync_WhenIdNotCorrect_ShouldReturn404NotFound()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync(EventApiAddress + "/seat/get-ticket-seats/0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetEventSeatsFromTicketAsync_WhenIdCorrect_ShouldReturnCorrectCollection()
        {
            // Arrange
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
            HttpResponseMessage response =
                await _httpClient.GetAsync(EventApiAddress + "/seat/get-ticket-seats/100");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<EventSeat>>(await response.Content.ReadAsStringAsync());

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
                _httpClient?.Dispose();
            }

            _disposed = true;
        }
    }
}
