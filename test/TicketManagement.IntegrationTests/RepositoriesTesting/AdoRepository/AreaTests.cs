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
    public class AreaTests
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
        public async Task Create_WhenAreaCorrect_ShouldReturnCreatedArea()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

            var createdArea = new Area
            {
                LayoutId = 1,
                Description = "Created Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            await repository.CreateAsync(createdArea);

            Area result = repository.GetAll().Last();

            // Assert
            result.Should()
                .BeEquivalentTo(createdArea, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenAreaExist_ShouldReturnCorrectArea()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

            var expected = new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            Area result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenAreaNotExist_ShouldReturnNull()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);
            Area expected = null;

            // Act
            Area result = await repository.GetAsync(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);
            List<Area> expected = DataBaseTableRecords.Areas;
            expected.Add(new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            });

            // Act
            List<Area> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenAreaExist_ShouldReturnUpdatedArea()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

            var expected = new Area
            {
                Id = 100,
                LayoutId = 1,
                Description = "Test is valid",
                CoordX = 1,
                CoordY = 1,
            };

            // Act
            await repository.UpdateAsync(expected);

            Area result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenAreaDoesntHaveId_ShouldReturnOldArea()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

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
            await repository.UpdateAsync(update);

            Area result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedArea()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

            List<Area> expected = DataBaseTableRecords.Areas;

            // Act
            await repository.DeleteAsync(100);

            List<Area> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var repository = new AdoRepository<Area>(_connectionString);

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
            await repository.DeleteAsync(0);

            List<Area> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }
    }
}