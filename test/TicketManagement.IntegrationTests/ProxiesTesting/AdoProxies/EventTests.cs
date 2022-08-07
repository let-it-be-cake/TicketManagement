using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.Ado;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.IntegrationTests.ProxiesTesting.AdoProxies
{
    public class EventTests
    {
        private string _connectionString;
        private IRepository<Event> _eventRepository;
        private IRepository<Seat> _seatRepository;
        private IRepository<Area> _areaRepository;
        private IRepository<EventSeat> _eventSeatRepository;
        private IRepository<EventArea> _eventAreaRepository;

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
            _eventRepository = new AdoEventRepository(_connectionString);
            _areaRepository = new AdoRepository<Area>(_connectionString);
            _seatRepository = new AdoRepository<Seat>(_connectionString);

            _eventSeatRepository = new AdoRepository<EventSeat>(_connectionString);
            _eventAreaRepository = new AdoRepository<EventArea>(_connectionString);

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
        public async Task Add_WhenEventCorrect_ShouldReturnCreatedEvent()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            var createdEvent = new Event
            {
                Name = "Created Test Name",
                Description = "Created Test Description",
                DateTimeStart = new DateTime(2030, 1, 1, 10, 0, 0),
                DateTimeEnd = new DateTime(2030, 2, 2, 10, 0, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            // Act
            await proxy.AddAsync(createdEvent);

            List<Area> areas = DataBaseTableRecords.Areas.Where(o => o.LayoutId == createdEvent.LayoutId).ToList();
            areas.Add(new Area
            {
                LayoutId = 1,
                Description = "SetUp Test Description",
                CoordX = 1,
                CoordY = 1,
            });

            List<int> areasId = areas.Select(o => o.Id).ToList();
            List<Seat> seats = DataBaseTableRecords.Seats.Where(o => areasId.Contains(o.AreaId)).ToList();
            seats.Add(new Seat
            {
                Id = 112,
                AreaId = 103,
                Row = 15,
                Number = 20,
            });

            Event eventResult = await proxy.ReadAsync(createdEvent.Id);
            List<EventArea> eventAreaResult = _eventAreaRepository.GetAll()
                .Where(o => o.EventId == createdEvent.Id)
                .ToList();
            List<int> eventAreasId = eventAreaResult.Select(o => o.Id).ToList();
            List<EventSeat> eventSeatResult = _eventSeatRepository.GetAll()
                .Where(o => eventAreasId.Contains(o.EventAreaId))
                .ToList();

            // Assert
            eventResult.Should()
                .BeEquivalentTo(createdEvent, option => option.Excluding(o => o.Id));

            eventSeatResult.Should()
                .BeEquivalentTo(seats, option => option
                    .Including(o => o.Number)
                    .Including(o => o.Row));

            eventAreaResult.Should()
                .BeEquivalentTo(areas, option => option
                    .Including(o => o.Description)
                    .Including(o => o.CoordX)
                    .Including(o => o.CoordY));
        }

        [Test]
        public void Add_WhenEventIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            Event createdEvent = null;

            // Act
            Func<Task> result = async () => await proxy.AddAsync(createdEvent);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Read_WhenEventExist_ShouldReturnCorrectEvent()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

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
            Event result = await proxy.ReadAsync(100);

            // Assert
            result.Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public async Task Read_WhenEventNotExist_ShouldReturnNull()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            // Act
            Event result = await proxy.ReadAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ReadAll_ShouldReturnCorrectCollection()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

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
            List<Event> result = await proxy.ReadAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(expected, option => option.Excluding(o => o.Id));
        }

        [Test]
        public async Task Change_WhenEventExist_ShouldReturnUpdatedEvent()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            var changedEvent = new Event
            {
                Id = 100,
                Name = "Changed Test Name",
                Description = "Changed Test Description",
                DateTimeStart = new DateTime(2023, 04, 19, 20, 30, 0),
                DateTimeEnd = new DateTime(2023, 04, 19, 22, 30, 0),
                LayoutId = 5,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            var addedEvent = new Event
            {
                Id = 101,
                Name = "SetUp Test Name 2",
                Description = "SetUp Test Description 2",
                DateTimeStart = new DateTime(2023, 04, 19, 10, 30, 0),
                DateTimeEnd = new DateTime(2023, 04, 19, 12, 30, 0),
                LayoutId = 1,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            var addedEventAreas = new List<EventArea>
            {
                new EventArea
                {
                    Description = "First area of second layout",
                    CoordX = 3,
                    CoordY = 3,
                },
                new EventArea
                {
                    Id = 102,
                    EventId = 101,
                    Description = "Alternate First area of first layout",
                    CoordX = 2,
                    CoordY = 2,
                },
            };

            var addedEventSeats = new List<EventSeat>
            {
                new EventSeat
                {
                    Row = 1,
                    Number = 1,
                },
            };

            List<Event> expectedEvents = DataBaseTableRecords.Events;
            expectedEvents.Add(changedEvent);
            expectedEvents.Add(addedEvent);

            List<EventArea> expectedEventAreas = DataBaseTableRecords.EventAreas.Concat(addedEventAreas).ToList();

            List<EventSeat> expectedEventSeats = DataBaseTableRecords.EventSeats.Concat(addedEventSeats).ToList();

            // Act
            await proxy.ChangeAsync(changedEvent);

            List<Event> resultEvents = await proxy.ReadAllAsync();
            List<EventArea> resultEventAreas = _eventAreaRepository.GetAll().ToList();
            List<EventSeat> resultEventSeats = _eventSeatRepository.GetAll().ToList();

            // Assert
            resultEvents.Should()
                .BeEquivalentTo(expectedEvents, option => option.Excluding(o => o.Id));

            resultEventAreas.Should()
                .BeEquivalentTo(expectedEventAreas, option => option
                    .Including(o => o.Description)
                    .Including(o => o.CoordX)
                    .Including(o => o.CoordY));

            resultEventSeats.Should()
                .BeEquivalentTo(expectedEventSeats, option => option
                    .Including(o => o.Row)
                    .Including(o => o.Number));
        }

        [Test]
        public void Change_WhenEventIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            Event expected = null;

            // Act
            Func<Task> result = async () => await proxy.ChangeAsync(expected);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }

        [Test]
        public async Task Change_WhenEventDoesntHaveId_ShouldReturnOldEvent()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            var update = new Event
            {
                Name = "Test Name is Valid",
                Description = "Test Description is Valid",
                DateTimeStart = new DateTime(2022, 10, 10, 10, 0, 0),
                DateTimeEnd = new DateTime(2022, 10, 10, 11, 0, 0),
                LayoutId = 5,
                ImageUrl = "1494294b-2274-44ad-8536-268324b799a2_12 (1).jpg",
            };

            var addedEvents = new List<Event>
            {
                new Event
                {
                    Id = 100,
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
            var addedEventAreas = new List<EventArea>
            {
                new EventArea
                {
                    Id = 100,
                    EventId = 100,
                    Description = "First area of first layout",
                    CoordX = 1,
                    CoordY = 1,
                    Price = 200,
                },
                new EventArea
                {
                    Id = 101,
                    EventId = 100,
                    Description = "Second area of first layout",
                    CoordX = 2,
                    CoordY = 2,
                    Price = 400,
                },
                new EventArea
                {
                    Id = 102,
                    EventId = 101,
                    Description = "Alternate First area of first layout",
                    CoordX = 2,
                    CoordY = 2,
                },
            };
            var addedEventSeats = new List<EventSeat>
            {
                new EventSeat
                {
                    Id = 100,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 1,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 101,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 2,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 102,
                    EventAreaId = 100,
                    TicketId = 100,
                    Row = 1,
                    Number = 3,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 103,
                    EventAreaId = 101,
                    Row = 2,
                    Number = 2,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 104,
                    EventAreaId = 101,
                    Row = 1,
                    Number = 1,
                    State = 0,
                },
                new EventSeat
                {
                    Id = 105,
                    EventAreaId = 101,
                    Row = 2,
                    Number = 1,
                    State = 0,
                },
            };

            List<Event> expectedEvents = DataBaseTableRecords.Events.Concat(addedEvents).ToList();
            List<EventArea> expectedEventAreas = DataBaseTableRecords.EventAreas.Concat(addedEventAreas).ToList();
            List<EventSeat> expectedEventSeats = DataBaseTableRecords.EventSeats.Concat(addedEventSeats).ToList();

            // Act
            await proxy.ChangeAsync(update);

            List<Event> resultEvents = await proxy.ReadAllAsync();
            List<EventArea> resultEventAreas = _eventAreaRepository.GetAll().ToList();
            List<EventSeat> resultEventSeats = _eventSeatRepository.GetAll().ToList();

            // Assert
            resultEvents.Should()
                .BeEquivalentTo(expectedEvents);

            resultEventAreas.Should()
                .BeEquivalentTo(expectedEventAreas);

            resultEventSeats.Should()
                .BeEquivalentTo(expectedEventSeats);
        }

        [Test]
        public async Task Delete_WhenIdExist_ShouldReturnCollectionWithoutDeletedEvent()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            List<Event> expectedEvents = DataBaseTableRecords.Events;
            expectedEvents.Add(
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

            List<EventArea> expectedEventAreas = DataBaseTableRecords.EventAreas;
            expectedEventAreas.Add(
                new EventArea
                {
                    Id = 102,
                    EventId = 101,
                    Description = "Alternate First area of first layout",
                    CoordX = 2,
                    CoordY = 2,
                });

            List<EventSeat> expectedEventSeats = DataBaseTableRecords.EventSeats;

            // Act
            await proxy.DeleteAsync(100);

            List<Event> resultEvent = await proxy.ReadAllAsync();
            List<EventArea> resultEventArea = _eventAreaRepository.GetAll().ToList();
            List<EventSeat> resultEventSeat = _eventSeatRepository.GetAll().ToList();

            // Assert
            resultEvent.Should()
                .BeEquivalentTo(expectedEvents, option => option.Excluding(o => o.Id));

            resultEventArea.Should()
                .BeEquivalentTo(expectedEventAreas, option => option
                    .Including(o => o.Description)
                    .Including(o => o.CoordX)
                    .Including(o => o.CoordY));

            resultEventSeat.Should()
                .BeEquivalentTo(expectedEventSeats, option => option
                    .Including(o => o.Number)
                    .Including(o => o.Row));
        }

        [Test]
        public void Delete_WhenIdNotExist_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventProxy(
                _eventRepository,
                _areaRepository,
                _seatRepository,
                _eventAreaRepository,
                _eventSeatRepository,
                _toListAsync);

            // Act
            Func<Task> result = async () => await proxy.DeleteAsync(0);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Cannot be null (Parameter 'item')");
        }
    }
}