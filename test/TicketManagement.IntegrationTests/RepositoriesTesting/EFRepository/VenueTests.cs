using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.Entities.Tables;

namespace TicketManagement.IntegrationTests.RepositoriesTesting.EFRepository
{
    public class VenueTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

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
        public async Task Create_WhenVenueCorrect_ShouldReturnCreatedVenue()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            var createdVenue = new Venue
            {
                Address = "Created Tes Address",
                Description = "Created Test Description",
                Phone = "567 89 012 34 56",
            };

            // Act
            await repository.CreateAsync(createdVenue);

            Venue result = await repository.GetAsync(createdVenue.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdVenue, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenVenueExist_ShouldReturnCorrectVenue()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            var expected = new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            // Act
            Venue result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenVenueNotExist_ShouldReturnNull()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);
            Venue expected = null;

            // Act
            Venue result = await repository.GetAsync(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);
            List<Venue> expected = DataBaseTableRecords.Venues;
            expected.Add(new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            });

            // Act
            List<Venue> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenVenueExist_ShouldReturnUpdatedVenue()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            var expected = new Venue
            {
                Id = 100,
                Address = "Test Address is valid",
                Description = "Test Description is valid",
                Phone = "456 78 901 23 45",
            };

            // Act
            await repository.UpdateAsync(expected);

            Venue result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenVenueDoesntHaveId_ShouldReturnOldVenue()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            var update = new Venue
            {
                Address = "Test Address is valid",
                Description = "Test Description is valid",
                Phone = "456 78 901 23 45",
            };

            var expected = new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            // Act
            await repository.UpdateAsync(update);

            Venue result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedVenue()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            List<Venue> expected = DataBaseTableRecords.Venues;

            // Act
            await repository.DeleteAsync(100);

            List<Venue> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var repository = new EFRepository<Venue>(_context);

            var addedVenue = new Venue
            {
                Address = "SetUp Test Address",
                Description = "SetUp Test Description",
                Phone = "456 78 901 23 45",
            };

            List<Venue> expected = DataBaseTableRecords.Venues;
            expected.Add(addedVenue);

            // Act
            await repository.DeleteAsync(0);

            List<Venue> result = repository.GetAll().ToList();

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
