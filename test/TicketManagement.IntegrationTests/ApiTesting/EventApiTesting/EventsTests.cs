using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
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
    public class EventsTests : ControllerTestsSetUp, IDisposable
    {
        private HttpClient _httpClient;

        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<Event> _eventRepository;
        private IRepository<Area> _areaRepository;
        private IRepository<Seat> _seatRepository;
        private IRepository<EventArea> _eventAreaRepository;
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

            _eventRepository = new EFEventRepository(_context);
            _areaRepository = new EFRepository<Area>(_context);
            _seatRepository = new EFRepository<Seat>(_context);
            _eventAreaRepository = new EFRepository<EventArea>(_context);
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
        public async Task GetEventAsync_WhenIdCorrect_ShouldReturnCorrectEvent()
        {
            // Arrange
            var expected = new Event
            {
                Id = 100,
                Name = "SetUp Test Name",
                Description = "SetUp Test Description",
                LayoutId = 1,
                DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 00),
                DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 00),
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            HttpResponseMessage response =
                await _httpClient.GetAsync("/event-getter/100");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Event>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetEventAsync_WhenIdInCorrect_ShouldReturn404NotFound()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync("/event-getter/0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetRegisterEventsAsync_WhenFromAndHowManyCorrect_ShouldReturnCorrectCollection()
        {
            // Arrange
            var expected = new List<Event>
            {
                new Event
                {
                    Id = 6,
                    Name = "Birthday",
                    Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                    DateTimeStart = new DateTime(2023, 04, 19, 17, 10, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 19, 10, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
                new Event
                {
                    Id = 10,
                    Name = "Send-off to jail",
                    Description = "Send-off to jail for bad apologies",
                    DateTimeStart = new DateTime(2022, 05, 18, 15, 0, 0),
                    DateTimeEnd = new DateTime(2022, 05, 18, 19, 0, 0),
                    LayoutId = 5,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
                new Event
                {
                    Id = 11,
                    Name = "Release from prison",
                    Description = "Release from prison after bad apology",
                    DateTimeStart = new DateTime(2022, 09, 09, 10, 0, 0),
                    DateTimeEnd = new DateTime(2022, 09, 09, 20, 0, 0),
                    LayoutId = 6,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
                new Event
                {
                    Id = 100,
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
            };

            // Act
            HttpResponseMessage response =
                await _httpClient.GetAsync("/event-getter/register/0&10");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<Event>>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetRegisterEventsAsync_WhenFromAndHowManyInCorrect_ShouldReturn400BadRequest()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync("/event-getter/register/0&0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task GetUnregisterEventsAsync_WhenFromAndHowManyCorrect_ShouldReturnCorrectCollection()
        {
            // Arrange
            var expected = new List<Event>
            {
                new Event
                {
                    Id = 101,
                    Name = "SetUp Test Name 2",
                    Description = "SetUp Test Description 2",
                    DateTimeStart = new DateTime(2023, 04, 19, 10, 30, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 12, 30, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
            };

            // Act
            HttpResponseMessage response =
                await _httpClient.GetAsync("/event-getter/unregister/0&10");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<Event>>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetUnregisterEventsAsync_WhenFromAndHowManyInCorrect_ShouldReturn400BadRequest()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync("/event-getter/unregister/0&0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status400BadRequest);
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