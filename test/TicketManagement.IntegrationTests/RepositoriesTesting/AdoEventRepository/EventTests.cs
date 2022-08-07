using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories.Ado;
using TicketManagement.Entities.Tables;

namespace TicketManagement.IntegrationTests.RepositoriesTesting.AdoEventRepositoryTests
{
    public class EventTests
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
        public async Task Create_WhenEventCorrect_ShouldReturnCreatedEvent()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var createdEvent = new Event
            {
                Name = "Created Test Name",
                Description = "Created Test Description",
                DateTimeStart = new DateTime(2021, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2021, 04, 19, 22, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            await repository.CreateAsync(createdEvent);

            Event result = repository.GetAll().Last();

            // Assert
            result.Should()
                .BeEquivalentTo(createdEvent, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenEventExist_ShouldReturnCorrectEvent()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var expected = new Event
            {
                Name = "SetUp Test Name",
                Description = "SetUp Test Description",
                DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            Event result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Get_WhenEventNotExist_ShouldReturnNull()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);
            Event expected = null;

            // Act
            Event result = await repository.GetAsync(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var addedEvents = new List<Event>
            {
                new Event
                {
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
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

            List<Event> expected = DataBaseTableRecords.Events.Concat(addedEvents).ToList();

            // Act
            List<Event> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenEventExist_ShouldReturnUpdatedEvent()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var expected = new Event
            {
                Id = 100,
                Name = "Name is Valid",
                Description = "Description is Valid",
                DateTimeStart = new DateTime(2021, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2021, 04, 19, 22, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            await repository.UpdateAsync(expected);

            Event result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Update_WhenEventDoesntHaveId_ShouldReturnOldEvent()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var update = new Event
            {
                Name = "Name is Valid",
                Description = "Description is Valid",
                DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            var expected = new Event
            {
                Id = 100,
                Name = "SetUp Test Name",
                Description = "SetUp Test Description",
                DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            await repository.UpdateAsync(update);

            Event result = await repository.GetAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedEvent()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            List<Event> expected = DataBaseTableRecords.Events;
            expected.Add(
                new Event
                {
                    Id = 101,
                    Name = "SetUp Test Name 2",
                    Description = "SetUp Test Description 2",
                    DateTimeStart = new DateTime(2023, 04, 19, 10, 30, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 12, 30, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                });

            // Act
            await repository.DeleteAsync(100);

            List<Event> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            var repository = new AdoEventRepository(_connectionString);

            var addedEvents = new List<Event>
            {
                new Event
                {
                    Name = "SetUp Test Name",
                    Description = "SetUp Test Description",
                    DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                    DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                    LayoutId = 1,
                    ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
                },
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

            List<Event> expected = DataBaseTableRecords.Events.Concat(addedEvents).ToList();

            // Act
            await repository.DeleteAsync(0);

            List<Event> result = repository.GetAll().ToList();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }
    }
}