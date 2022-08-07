using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories.Ado;
using TicketManagement.Entities.Tables;

namespace TicketManagement.IntegrationTests.RepositoriesTesting.AdoRepositoryTests
{
    public class LayoutTests
    {
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
        public async Task Create_WhenLayoutCorrect_ShouldReturnCreatedLayout()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            var createdLayout = new Layout
            {
                Description = "Created Test Description",
                VenueId = 6,
            };

            // Act
            await repository.CreateAsync(createdLayout);

            Layout result = repository.GetAll().Last();

            // Assert
            result.Should()
                .BeEquivalentTo(createdLayout, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenLayoutExist_ShouldReturnCorrectLayout()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            var expected = new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            // Act
            Layout result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenLayoutNotExist_ShouldReturnNull()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);
            Layout expected = null;

            // Act
            Layout result = await repository.GetAsync(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);
            List<Layout> expected = DataBaseTableRecords.Layouts;
            expected.Add(new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            });

            // Act
            List<Layout> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenLayoutExist_ShouldReturnUpdatedLayout()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            var expected = new Layout
            {
                Id = 100,
                VenueId = 1,
                Description = "Test is Valid",
            };

            // Act
            await repository.UpdateAsync(expected);

            Layout result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenLayoutDoesntHaveId_ShouldReturnOldLayout()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            var update = new Layout
            {
                VenueId = 2,
                Description = "Test is valid",
            };

            var expected = new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            // Act
            await repository.UpdateAsync(update);

            Layout result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedLayout()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            List<Layout> expected = DataBaseTableRecords.Layouts;

            // Act
            await repository.DeleteAsync(100);

            List<Layout> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var repository = new AdoRepository<Layout>(_connectionString);

            var addedLayout = new Layout
            {
                VenueId = 5,
                Description = "SetUp Test Description",
            };

            List<Layout> expected = DataBaseTableRecords.Layouts;
            expected.Add(addedLayout);

            // Act
            await repository.DeleteAsync(0);

            List<Layout> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }
    }
}