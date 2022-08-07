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
    public class AreaTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;
        private IRepository<Area> _areaRepository;

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

            _areaRepository = new EFRepository<Area>(_context);

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
        public async Task Add_WhenAreaCorrect_ShouldReturnCreatedArea()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            var createdArea = new Area
            {
                LayoutId = 1,
                Description = "Created Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            await proxy.AddAsync(createdArea);

            Area result = await proxy.ReadAsync(createdArea.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdArea);
        }

        [Test]
        public void Add_WhenAreaIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            Area createdArea = null;

            // Act
            Func<Task> result = async () => await proxy.AddAsync(createdArea);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Read_WhenAreaExist_ShouldReturnCorrectArea()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            var expected = new Area
            {
                Id = 100,
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            Area result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Read_WhenAreaNotExist_ShouldReturnNull()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            // Act
            Area result = await proxy.ReadAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ReadAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);
            List<Area> expected = DataBaseTableRecords.Areas;
            expected.Add(new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            });

            // Act
            List<Area> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Change_WhenAreaExist_ShouldReturnUpdatedArea()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            var expected = new Area
            {
                Id = 100,
                LayoutId = 1,
                Description = "Test is valid",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            await proxy.ChangeAsync(expected);

            Area result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Change_WhenAreaIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            Area expected = null;

            // Act
            Func<Task> result = async () => await proxy.ChangeAsync(expected);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Change_WhenAreaDoesntHaveId_ShouldReturnOldArea()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            var update = new Area
            {
                LayoutId = 1,
                Description = "Test is valid",
                CoordX = 1,
                CoordY = 1,
            };

            var expected = new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            await proxy.ChangeAsync(update);

            Area result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedArea()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            List<Area> expected = DataBaseTableRecords.Areas;

            // Act
            await proxy.DeleteAsync(100);

            List<Area> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var proxy = new AreaProxy(_areaRepository, _toListAsync);

            var addedArea = new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            List<Area> expected = DataBaseTableRecords.Areas;
            expected.Add(addedArea);

            // Act
            await proxy.DeleteAsync(0);

            List<Area> result = await proxy.ReadAllAsync();

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
