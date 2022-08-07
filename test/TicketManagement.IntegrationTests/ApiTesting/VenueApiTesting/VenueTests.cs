using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.Ado;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Proxys;

namespace TicketManagement.IntegrationTests.VenueApiTesting
{
    public class VenueTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<Venue> _venueRepository;

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
                   .Options;

            _context = new TicketManagementContext(optionsBuilder);

            _venueRepository = new AdoRepository<Venue>(_connectionString);

            _toListAsync = new AdoQuerableToListAsync();

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
        public async Task Add_WhenVenueCorrect_ShouldReturnCreatedVenue()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            var createdVenue = new Venue
            {
                Address = "Created Tes Address",
                Description = "Created Test Description",
                Phone = "567 89 012 34 56",
            };

            // Act
            await proxy.AddAsync(createdVenue);

            Venue result = await proxy.ReadAsync(createdVenue.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdVenue);
        }

        [Test]
        public void Add_WhenVenueIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            Venue createdVenue = null;

            // Act
            Func<Task> result = async () => await proxy.AddAsync(createdVenue);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Read_WhenVenueExist_ShouldReturnCorrectVenue()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            var expected = new Venue
            {
                Id = 100,
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            // Act
            Venue result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Read_WhenVenueNotExist_ShouldReturnNull()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            // Act
            Venue result = await proxy.ReadAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ReadAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);
            List<Venue> expected = DataBaseTableRecords.Venues;
            expected.Add(new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            });

            // Act
            List<Venue> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Change_WhenVenueExist_ShouldReturnUpdatedVenue()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            var expected = new Venue
            {
                Id = 100,
                Address = "Test Address is valid",
                Description = "Test Description is valid",
                Phone = "456 78 901 23 45",
            };

            // Act
            await proxy.ChangeAsync(expected);

            Venue result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Change_WhenVenueIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            Venue expected = null;

            // Act
            Func<Task> result = async () => await proxy.ChangeAsync(expected);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Change_WhenVenueDoesntHaveId_ShouldReturnOldVenue()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            var update = new Venue
            {
                Address = "Test Address is valid",
                Description = "Test Description is valid",
                Phone = "456 78 901 23 45",
            };

            var expected = new Venue
            {
                Id = 100,
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            // Act
            await proxy.ChangeAsync(update);

            Venue result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedVenue()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            List<Venue> expected = DataBaseTableRecords.Venues;

            // Act
            await proxy.DeleteAsync(100);

            List<Venue> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var proxy = new VenueProxy(_venueRepository, _toListAsync);

            var addedVenue = new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            List<Venue> expected = DataBaseTableRecords.Venues;
            expected.Add(addedVenue);

            // Act
            await proxy.DeleteAsync(0);

            List<Venue> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
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
