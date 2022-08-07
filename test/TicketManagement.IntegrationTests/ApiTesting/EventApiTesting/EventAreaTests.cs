using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
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
using TicketManagement.EventApi.Models;
using TicketManagement.EventApi.Proxys;
using TicketManagement.UserInterface.Clients.EventApi;

namespace TicketManagement.IntegrationTests.ApiTesting.EventApiTesting
{
    public class EventAreaTests : ControllerTestsSetUp, IDisposable
    {
        private HttpClient _httpClient;

        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<EventArea> _eventAreaRepository;
        private IRepository<Event> _eventRepository;

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

            _eventAreaRepository = new EFRepository<EventArea>(_context);
            _eventRepository = new EFEventRepository(_context);

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
        public async Task GetEventAreaAsync_WhenEventExist_ShouldReturnCorrectEventArea()
        {
            // Arrange
            var expected = new EventArea
            {
                Id = 100,
                EventId = 100,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
                Price = 200,
            };

            // Act
            HttpResponseMessage response =
                await _httpClient.GetAsync("/area/get/100");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<EventArea>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetEventAreaAsync_WhenEventNotExist_ShouldReturn404NotFound()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync("/area/get/0");

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetAllFromEventAsync_WhenEventIdCorrect_ShouldReturnEventAreaCollection()
        {
            // Arrange
            List<EventArea> expected = _eventAreaRepository.GetAll()
                .Where(o => o.EventId == 6)
                .ToList();

            // Act
            HttpResponseMessage response =
                await _httpClient.GetAsync("/area/get-event-areas/6");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<EventArea>>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllFromEventAsync_WhenEventIdNotCorrect_ShouldReturn404NotFound()
        {
            // Arrange & Act
            HttpResponseMessage result =
                await _httpClient.GetAsync("/area/get-event-areas/0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetUnregisterModelAsync_WhenEventFromAndHowManyCorrect_ShouldReturnUnregisteredEventAreaModel()
        {
            // Arrange
            List<EventAreaModel> expected = new List<EventAreaModel>
            {
                new EventAreaModel
                {
                    Id = 6,
                    CoordX = 1,
                    CoordY = 1,
                    DateTimeStart = new DateTime(2023, 04, 19, 17, 10, 00),
                    DateTimeEnd = new DateTime(2023, 04, 19, 19, 10, 00),
                    Description = "In honor of such a person, you need to arrange a big holiday",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "Birthday",
                    Price = 10000,
                },
                new EventAreaModel
                {
                    Id = 7,
                    CoordX = 3,
                    CoordY = 12,
                    DateTimeStart = new DateTime(2022, 05, 18, 15, 00, 00),
                    DateTimeEnd = new DateTime(2022, 05, 18, 19, 00, 00),
                    Description = "Apparently the holiday was not big enough (or too expensive)",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "Send-off to jail",
                    Price = 0M,
                },
                new EventAreaModel
                {
                    Id = 8,
                    CoordX = 1,
                    CoordY = 1,
                    DateTimeStart = new DateTime(2022, 09, 09, 10, 00, 00),
                    DateTimeEnd = new DateTime(2022, 09, 09, 20, 00, 00),
                    Description = "Our boss is out of prison, and we need to have a big celebration in honor of this",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "Release from prison",
                    Price = 10000M,
                },
                new EventAreaModel
                {
                    Id = 100,
                    CoordX = 1,
                    CoordY = 1,
                    DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 00),
                    DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 00),
                    Description = "First area of first layout",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "SetUp Test Name",
                    Price = 200.00M,
                },
                new EventAreaModel
                {
                    Id = 102,
                    CoordX = 2,
                    CoordY = 2,
                    DateTimeStart = new DateTime(2023, 04, 19, 10, 30, 00),
                    DateTimeEnd = new DateTime(2023, 04, 19, 12, 30, 00),
                    Description = "Alternate First area of first layout",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "SetUp Test Name 2",
                },
                new EventAreaModel
                {
                    Id = 101,
                    CoordX = 2,
                    CoordY = 2,
                    DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 00),
                    DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 00),
                    Description = "Second area of first layout",
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                    Name = "SetUp Test Name",
                    Price = 400.00M,
                },
            };

            // Act
            HttpRequestHeaders header = _httpClient.DefaultRequestHeaders;
            header.Add("Authorization", AdminToken);

            HttpResponseMessage response =
                await _httpClient.GetAsync("/area/get-unregister/0&10");
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<List<EventAreaModel>>(await response.Content.ReadAsStringAsync());

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetUnregisterModelAsync_WhenEventFromAndHowManyInCorrect_ShouldReturn404BadRequest()
        {
            // Act
            HttpRequestHeaders header = _httpClient.DefaultRequestHeaders;
            header.Add("Authorization", AdminToken);

            // Arrange
            HttpResponseMessage result =
                await _httpClient.GetAsync("/area/get-unregister/0&0");

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task SetPriceAsync_WhenEventAreaPriceModelCollectionCorrect_ShouldChangeEventAreaPricesAndReturn200OK()
        {
            // Arrange
            var eventAreaPriceModels = new List<EventAreaPriceModel>
            {
                new EventAreaPriceModel
                {
                    EventAreaId = 100,
                    Price = 99,
                },
            };

            HttpRequestHeaders header = _httpClient.DefaultRequestHeaders;
            header.Add("Authorization", AdminToken);

            // Act
            HttpResponseMessage result = await _httpClient.PostAsync("/area/set-price",
                new StringContent(JsonConvert.SerializeObject(eventAreaPriceModels), Encoding.UTF8, "application/json"));

            // Assert
            result.StatusCode.Should()
                .Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task SetPriceAsync_WhenEventAreaPriceModelCollectionNull_ShouldReturn400BadRequest()
        {
            // Arrange
            HttpRequestHeaders header = _httpClient.DefaultRequestHeaders;
            header.Add("Authorization", AdminToken);

            // Act
            HttpResponseMessage result = await _httpClient.PostAsync("/area/set-price",
                new StringContent("", Encoding.UTF8, "application/json"));

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