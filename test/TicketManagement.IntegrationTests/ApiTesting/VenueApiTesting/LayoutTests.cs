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
    public class LayoutTests : IDisposable
    {
        private bool _disposed;
        private TicketManagementContext _context;
        private string _connectionString;

        private IRepository<Layout> _layoutRepository;

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

            _layoutRepository = new EFRepository<Layout>(_context);

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
        public async Task Add_WhenLayoutCorrect_ShouldReturnCreatedLayout()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            var createdLayout = new Layout
            {
                Description = "Created Test Description",
                VenueId = 5,
            };

            // Act
            await proxy.AddAsync(createdLayout);

            Layout result = await proxy.ReadAsync(createdLayout.Id);

            // Assert
            result.Should()
                .BeEquivalentTo(createdLayout);
        }

        [Test]
        public void Add_WhenLayoutIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            Layout createdLayout = null;

            // Act
            Func<Task> result = async () => await proxy.AddAsync(createdLayout);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Read_WhenLayoutExist_ShouldReturnCorrectLayout()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            var expected = new Layout
            {
                Id = 100,
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            // Act
            Layout result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Read_WhenLayoutNotExist_ShouldReturnNull()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            // Act
            Layout result = await proxy.ReadAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ReadAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);
            List<Layout> expected = DataBaseTableRecords.Layouts;
            expected.Add(new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            });

            // Act
            List<Layout> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Change_WhenLayoutExist_ShouldReturnUpdatedLayout()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            var expected = new Layout
            {
                Id = 100,
                VenueId = 1,
                Description = "Test is Valid",
            };

            // Act
            await proxy.ChangeAsync(expected);

            Layout result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Change_WhenLayoutIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            Layout expected = null;

            // Act
            Func<Task> result = async () => await proxy.ChangeAsync(expected);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Change_WhenLayoutDoesntHaveId_ShouldReturnOldLayout()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            var update = new Layout
            {
                VenueId = 1,
                Description = "Test is valid",
            };

            var expected = new Layout
            {
                Id = 100,
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            // Act
            await proxy.ChangeAsync(update);

            Layout result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedLayout()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            List<Layout> expected = DataBaseTableRecords.Layouts;

            // Act
            await proxy.DeleteAsync(100);

            List<Layout> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var proxy = new LayoutProxy(_layoutRepository, _toListAsync);

            var addedLayout = new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            List<Layout> expected = DataBaseTableRecords.Layouts;
            expected.Add(addedLayout);

            // Act
            await proxy.DeleteAsync(0);

            List<Layout> result = await proxy.ReadAllAsync();

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
