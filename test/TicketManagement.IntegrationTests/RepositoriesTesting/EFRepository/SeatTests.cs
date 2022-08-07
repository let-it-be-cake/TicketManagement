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
    public class SeatTests : IDisposable
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
        public async Task Create_WhenSeatCorrect_ShouldReturnCreatedSeat()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            var createdSeat = new Seat
            {
                AreaId = 5,
                Number = 13,
                Row = 13,
            };

            // Act
            await repository.CreateAsync(createdSeat);

            Seat result = await repository.GetAsync(createdSeat.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdSeat, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenSeatExist_ShouldReturnCorrectSeat()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            var expected = new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            // Act
            Seat result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenSeatNotExist_ShouldReturnNull()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);
            Seat expected = null;

            // Act
            Seat result = await repository.GetAsync(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);
            List<Seat> expected = DataBaseTableRecords.Seats;
            expected.Add(new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            });

            // Act
            List<Seat> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenSeatExist_ShouldReturnUpdatedSeat()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            var expected = new Seat
            {
                Id = 100,
                AreaId = 5,
                Number = 13,
                Row = 13,
            };

            // Act
            await repository.UpdateAsync(expected);

            Seat result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenSeatDoesntHaveId_ShouldReturnOldSeat()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            var update = new Seat
            {
                AreaId = 5,
                Number = 13,
                Row = 13,
            };

            var expected = new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            // Act
            await repository.UpdateAsync(update);

            Seat result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedSeat()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            List<Seat> expected = DataBaseTableRecords.Seats;

            // Act
            await repository.DeleteAsync(100);

            List<Seat> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var repository = new EFRepository<Seat>(_context);

            var addedSeat = new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            List<Seat> expected = DataBaseTableRecords.Seats;
            expected.Add(addedSeat);

            // Act
            await repository.DeleteAsync(0);

            List<Seat> result = repository.GetAll().ToList();

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
