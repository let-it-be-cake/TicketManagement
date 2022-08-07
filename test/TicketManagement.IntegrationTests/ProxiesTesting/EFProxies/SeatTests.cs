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

namespace TicketManagement.IntegrationTests.ProxiesTesting.EFProxies
{
    public class SeatTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<Seat> _seatRepository;

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

            _seatRepository = new EFRepository<Seat>(_context);

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
        public async Task Add_WhenSeatCorrect_ShouldReturnCreatedSeat()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            var createdSeat = new Seat
            {
                AreaId = 6,
                Number = 13,
                Row = 13,
            };

            // Act
            await proxy.AddAsync(createdSeat);

            Seat result = await proxy.ReadAsync(createdSeat.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdSeat);
        }

        [Test]
        public void Add_WhenSeatIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            Seat createdSeat = null;

            // Act
            Func<Task> result = async () => await proxy.AddAsync(createdSeat);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Read_WhenSeatExist_ShouldReturnCorrectSeat()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            var expected = new Seat
            {
                Id = 100,
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            // Act
            Seat result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Read_WhenSeatNotExist_ShouldReturnNull()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            // Act
            Seat result = await proxy.ReadAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ReadAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);
            List<Seat> expected = DataBaseTableRecords.Seats;
            expected.Add(new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            });

            // Act
            List<Seat> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Change_WhenSeatExist_ShouldReturnUpdatedSeat()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            var expected = new Seat
            {
                Id = 100,
                AreaId = 6,
                Number = 13,
                Row = 13,
            };

            // Act
            await proxy.ChangeAsync(expected);

            Seat result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Change_WhenSeatIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            Seat expected = null;

            // Act
            Func<Task> result = async () => await proxy.ChangeAsync(expected);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Change_WhenSeatDoesntHaveId_ShouldReturnOldSeat()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            var update = new Seat
            {
                AreaId = 5,
                Number = 13,
                Row = 13,
            };

            var expected = new Seat
            {
                Id = 100,
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            // Act
            await proxy.ChangeAsync(update);

            Seat result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedSeat()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            List<Seat> expected = DataBaseTableRecords.Seats;

            // Act
            await proxy.DeleteAsync(100);

            List<Seat> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var proxy = new SeatProxy(_seatRepository, _toListAsync);

            var addedSeat = new Seat
            {
                AreaId = 6,
                Row = 15,
                Number = 20,
            };

            List<Seat> expected = DataBaseTableRecords.Seats;
            expected.Add(addedSeat);

            // Act
            await proxy.DeleteAsync(0);

            List<Seat> result = await proxy.ReadAllAsync();

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
