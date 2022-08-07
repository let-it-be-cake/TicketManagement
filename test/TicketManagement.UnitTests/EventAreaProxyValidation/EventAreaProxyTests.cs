using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Models;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.UnitTests.EventAreaProxyValidation
{
    public class EventAreaProxyTests
    {
        private Mock<IRepository<EventArea>> _eventArea;
        private Mock<IRepository<Event>> _event;

        private Mock<IQuerableHelper> _toList;

        [SetUp]
        public void SetUp()
        {
            _eventArea = new Mock<IRepository<EventArea>>();
            _eventArea.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.EventAreas.AsQueryable());
            _eventArea.Setup(o => o.GetAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataBaseTableRecords.EventAreas.FirstOrDefault(o => o.Id == id)));

            _event = new Mock<IRepository<Event>>();
            _event.Setup(o => o.GetAll())
                .Returns(() => DataBaseTableRecords.Events.AsQueryable());

            _toList = new Mock<IQuerableHelper>();
            _toList.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventAreaModel>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventAreaModel> item, CancellationToken token) => Task.FromResult(item.ToList()));
            _toList.Setup(o => o.ToListAsync(It.IsAny<IQueryable<EventArea>>(), It.IsAny<CancellationToken>()))
                .Returns((IQueryable<EventArea> item, CancellationToken token) => Task.FromResult(item.ToList()));
        }

        [Test]
        public async Task GetUnregisterModelAsync_WhenFromGreater0_ShouldReturnCollection()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            var result = await proxy.GetUnregisterModelAsync(1, 10);

            // Assert
            result.Should()
                .BeEquivalentTo(new List<EventAreaModel>
                {
                    new EventAreaModel
                    {
                        CoordX = 3,
                        CoordY = 12,
                        DateTimeEnd = new DateTime(2020, 05, 18, 19, 00, 00),
                        DateTimeStart = new DateTime(2020, 05, 18, 15, 00, 00),
                        Description = "Apparently the holiday was not big enough (or too expensive)",
                        Id = 7,
                        ImageUrl = "",
                        Name = "Send-off to jail",
                        Price = 0,
                    },
                    new EventAreaModel
                    {
                        CoordX = 1,
                        CoordY = 1,
                        DateTimeEnd = new DateTime(2020, 09, 09, 20, 00, 00),
                        DateTimeStart = new DateTime(2020, 09, 09, 10, 00, 00),
                        Description = "Our boss is out of prison, and we need to have a big celebration in honor of this",
                        Id = 8,
                        ImageUrl = "",
                        Name = "Release from prison",
                        Price = 10000,
                    },
                });
        }

        [Test]
        public async Task GetUnregisterModelAsync_WhenFromEqual0_ShouldReturnCollection()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            List<EventAreaModel> result = await proxy.GetUnregisterModelAsync(0, 10);

            // Assert
            result.Should()
                .BeEquivalentTo(new List<EventAreaModel>
                {
                    new EventAreaModel
                    {
                        CoordX = 1,
                        CoordY = 1,
                        DateTimeEnd = new DateTime(2021, 04, 19, 19, 10, 00),
                        DateTimeStart = new DateTime(2021, 04, 19, 17, 10, 00),
                        Description = "In honor of such a person, you need to arrange a big holiday",
                        Id = 6,
                        ImageUrl = "",
                        Name = "Birthday",
                        Price = 10000,
                    },
                    new EventAreaModel
                    {
                        CoordX = 3,
                        CoordY = 12,
                        DateTimeEnd = new DateTime(2020, 05, 18, 19, 00, 00),
                        DateTimeStart = new DateTime(2020, 05, 18, 15, 00, 00),
                        Description = "Apparently the holiday was not big enough (or too expensive)",
                        Id = 7,
                        ImageUrl = "",
                        Name = "Send-off to jail",
                        Price = 0,
                    },
                    new EventAreaModel
                    {
                        CoordX = 1,
                        CoordY = 1,
                        DateTimeEnd = new DateTime(2020, 09, 09, 20, 00, 00),
                        DateTimeStart = new DateTime(2020, 09, 09, 10, 00, 00),
                        Description = "Our boss is out of prison, and we need to have a big celebration in honor of this",
                        Id = 8,
                        ImageUrl = "",
                        Name = "Release from prison",
                        Price = 10000,
                    },
                });
        }

        [Test]
        public void GetUnregisterModelAsync_WhenFromLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterModelAsync(-1, 10);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'from')");
        }

        [Test]
        public async Task GetUnregisterModelAsync_WhenHowManyGreater0_ShouldReturnCollection()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            var result = await proxy.GetUnregisterModelAsync(0, 10);

            // Assert
            result.Should()
                .BeEquivalentTo(new List<EventAreaModel>
                {
                    new EventAreaModel
                    {
                        CoordX = 1,
                        CoordY = 1,
                        DateTimeEnd = new DateTime(2021, 04, 19, 19, 10, 00),
                        DateTimeStart = new DateTime(2021, 04, 19, 17, 10, 00),
                        Description = "In honor of such a person, you need to arrange a big holiday",
                        Id = 6,
                        ImageUrl = "",
                        Name = "Birthday",
                        Price = 10000,
                    },
                    new EventAreaModel
                    {
                        CoordX = 3,
                        CoordY = 12,
                        DateTimeEnd = new DateTime(2020, 05, 18, 19, 00, 00),
                        DateTimeStart = new DateTime(2020, 05, 18, 15, 00, 00),
                        Description = "Apparently the holiday was not big enough (or too expensive)",
                        Id = 7,
                        ImageUrl = "",
                        Name = "Send-off to jail",
                        Price = 0,
                    },
                    new EventAreaModel
                    {
                        CoordX = 1,
                        CoordY = 1,
                        DateTimeEnd = new DateTime(2020, 09, 09, 20, 00, 00),
                        DateTimeStart = new DateTime(2020, 09, 09, 10, 00, 00),
                        Description = "Our boss is out of prison, and we need to have a big celebration in honor of this",
                        Id = 8,
                        ImageUrl = "",
                        Name = "Release from prison",
                        Price = 10000,
                    },
                });
        }

        [Test]
        public void GetUnregisterModelAsync_WhenHowManyLessThen0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterModelAsync(0, -1);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }

        [Test]
        public void GetUnregisterModelAsync_WhenHowManyEqual0_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.GetUnregisterModelAsync(0, 0);

            // Assert
            result.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("Must be greater than 0. (Parameter 'howMany')");
        }

        [Test]
        public void SetPriceAsync_WhenEventAreaPriceModelIsNull_ShouldReturnException()
        {
            // Arrange
            var proxy = new EventAreaProxy(_eventArea.Object, _event.Object, _toList.Object);

            // Act
            Func<Task> result = async () => await proxy.SetPriceAsync(null);

            // Assert
            result.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'eventAreaPrices')");
        }
    }
}