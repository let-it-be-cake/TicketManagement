using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using ThirdPartyEventEditor.Services;
using ThirdPartyEventEditor.Services.Interfaces;
using ThirdPartyEventEditor.ServiceTables;

namespace ThirdPartyEventEditor.IntegrationTests
{
    public class EventTests
    {
        private IProxyService<Event> _eventService;
        private string _connectionString;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configs = new ConfigurationBuilder()
                .AddXmlFile("App.config")
                .Build();

            _connectionString = configs["appSettings:add:value"].ToString();
        }

        [SetUp]
        public void SetUp()
        {
            var events = new List<string>();
            events.Add(JsonConvert.SerializeObject(Data.Events));
            File.WriteAllLines(_connectionString, events);

            _eventService = new EventService(_connectionString);
        }

        [Test]
        public void Create_WhenEventCorrect_ShouldReturnCreatedEvent()
        {
            // Arrange
            var createdEvent = new Event
            {
                Id = 4,
                Name = "Event 4",
                Description = "Description 4",
                StartDate = new DateTime(2025, 04, 04, 01, 01, 01),
                EndDate = new DateTime(2025, 04, 04, 11, 01, 01),
                PosterImage = "",
            };

            // Act
            _eventService.Add(createdEvent);

            Event result = _eventService.ReadAll().Last();

            // Assert
            result.Should()
                .BeEquivalentTo(createdEvent, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Get_WhenEventExist_ShouldReturnCorrectEvent()
        {
            // Arrange
            var expected = new Event
            {
                Id = 3,
                Name = "Event 3",
                Description = "Description 3",
                StartDate = new DateTime(2025, 03, 03, 01, 01, 01),
                EndDate = new DateTime(2025, 03, 03, 11, 01, 01),
                PosterImage = "",
            };

            int idLastAddedElement = 3;

            // Act
            Event result = _eventService.Read(idLastAddedElement);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Get_WhenEventNotExist_ShouldReturnNull()
        {
            // Arrange
            Event expected = null;

            // Act
            Event result = _eventService.Read(0);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void GetAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            List<Event> expected = Data.Events;

            // Act
            List<Event> result = _eventService.ReadAll();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Update_WhenEventExist_ShouldReturnUpdatedEvent()
        {
            // Arrange
            var expected = new Event
            {
                Id = 3,
                Name = "Event 4",
                Description = "Description 4",
                StartDate = new DateTime(2025, 04, 04, 01, 01, 01),
                EndDate = new DateTime(2025, 04, 04, 11, 01, 01),
                PosterImage = "",
            };

            int idLastAddedElement = 3;

            // Act
            _eventService.Change(expected);

            Event result = _eventService.Read(idLastAddedElement);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Update_WhenEventDoesntHaveId_ShouldReturnOldEvent()
        {
            // Arrange
            var update = new Event
            {
                Name = "Event 4",
                Description = "Description 4",
                StartDate = new DateTime(2025, 04, 04, 01, 01, 01),
                EndDate = new DateTime(2025, 04, 04, 11, 01, 01),
                PosterImage = "",
            };

            var expected = new Event
            {
                Id = 3,
                Name = "Event 3",
                Description = "Description 3",
                StartDate = new DateTime(2025, 03, 03, 01, 01, 01),
                EndDate = new DateTime(2025, 03, 03, 11, 01, 01),
                PosterImage = "",
            };

            int idLastAddedElement = 3;

            // Act
            _eventService.Change(update);

            Event result = _eventService.Read(idLastAddedElement);

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedEvent()
        {
            // Arrange
            List<Event> expected = Data.Events.Where(o => o.Id != 3).ToList();

            int idLastAddedElement = 3;

            // Act
            _eventService.Delete(idLastAddedElement);

            List<Event> result = _eventService.ReadAll();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public void Delete_WhenIdNotExist_ShouldReturnOldCollection()
        {
            // Arrange
            List<Event> expected = Data.Events;

            // Act
            _eventService.Delete(0);

            List<Event> result = _eventService.ReadAll();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }
    }
}