using System;
using System.Collections.Generic;
using TicketManagement.Entities.Tables;
using TicketManagement.Entities.Tables.Enum;

namespace TicketManagement.UnitTests
{
    internal static class DataBaseTableRecords
    {
        private static List<Area> _areas = new List<Area>
        {
            new Area
            {
                Id = 5,
                LayoutId = 1,
                Description = "First area of first layout",
                CoordX = 1,
                CoordY = 1,
            },
            new Area
            {
                Id = 6,
                LayoutId = 1,
                Description = "Second area of first layout",
                CoordX = 2,
                CoordY = 2,
            },
            new Area
            {
                Id = 7,
                LayoutId = 5,
                Description = "First area of second layout",
                CoordX = 3,
                CoordY = 3,
            },
        };

        private static List<Layout> _layouts = new List<Layout>
        {
                new Layout
                {
                    Id = 1,
                    VenueId = 1,
                    Description = "First layout",
                },
                new Layout
                {
                    Id = 5,
                    VenueId = 1,
                    Description = "Second layout",
                },
                new Layout
                {
                    Id = 6,
                    VenueId = 5,
                    Description = "First layout to second venue",
                },
                new Layout
                {
                    Id = 9,
                    VenueId = 5,
                    Description = "Second layout to second venue",
                },
        };

        private static List<Seat> _seats = new List<Seat>
        {
                new Seat
                {
                    Id = 5,
                    AreaId = 5,
                    Row = 1,
                    Number = 1,
                },
                new Seat
                {
                    Id = 6,
                    AreaId = 5,
                    Row = 1,
                    Number = 2,
                },
                new Seat
                {
                    Id = 7,
                    AreaId = 5,
                    Row = 1,
                    Number = 3,
                },
                new Seat
                {
                    Id = 8,
                    AreaId = 6,
                    Row = 2,
                    Number = 2,
                },
                new Seat
                {
                    Id = 9,
                    AreaId = 6,
                    Row = 1,
                    Number = 1,
                },
                new Seat
                {
                    Id = 10,
                    AreaId = 6,
                    Row = 2,
                    Number = 1,
                },
                new Seat
                {
                    Id = 11,
                    AreaId = 7,
                    Row = 1,
                    Number = 1,
                },
        };

        private static List<Venue> _venues = new List<Venue>
        {
                new Venue
                {
                    Id = 1,
                    Description = "First venue",
                    Address = "First venue Address",
                    Phone = "123 45 678 90 12",
                },
                new Venue
                {
                    Id = 5,
                    Description = "Second venue",
                    Address = "Second venue Address",
                    Phone = "234 56 789 01 23",
                },
                new Venue
                {
                    Id = 6,
                    Description = "Third venue",
                    Address = "Third venue Address",
                    Phone = "345 67 890 12 34",
                },
        };

        private static List<Event> _events = new List<Event>
        {
            new Event
            {
                Id = 6,
                Name = "Birthday",
                Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                DateTimeStart = new DateTime(2021, 04, 19, 17, 10, 0),
                DateTimeEnd = new DateTime(2021, 04, 19, 19, 10, 0),
                LayoutId = 1,
                ImageUrl = "",
            },
            new Event
            {
                Id = 10,
                Name = "Send-off to jail",
                Description = "Send-off to jail for bad apologies",
                DateTimeStart = new DateTime(2020, 05, 18, 15, 0, 0),
                DateTimeEnd = new DateTime(2020, 05, 18, 19, 0, 0),
                LayoutId = 5,
                ImageUrl = "",
            },
            new Event
            {
                Id = 11,
                Name = "Release from prison",
                Description = "Release from prison after bad apology",
                DateTimeStart = new DateTime(2020, 09, 09, 10, 0, 0),
                DateTimeEnd = new DateTime(2020, 09, 09, 20, 0, 0),
                LayoutId = 6,
                ImageUrl = "",
            },
            new Event
            {
                Id = 14,
                Name = "Name",
                Description = "Description",
                DateTimeStart = new DateTime(2020, 08, 07, 19, 0, 0),
                DateTimeEnd = new DateTime(2020, 08, 08, 5, 0, 0),
                LayoutId = 9,
                ImageUrl = "",
            },
        };

        private static List<EventArea> _eventAreas = new List<EventArea>
        {
            new EventArea
            {
                Id = 6,
                EventId = 6,
                Description = "In honor of such a person, you need to arrange a big holiday",
                CoordX = 1,
                CoordY = 1,
                Price = 10000,
            },
            new EventArea
            {
                Id = 7,
                EventId = 10,
                Description = "Apparently the holiday was not big enough (or too expensive)",
                CoordX = 3,
                CoordY = 12,
                Price = 0,
            },
            new EventArea
            {
                Id = 8,
                EventId = 11,
                Description = "Our boss is out of prison, and we need to have a big celebration in honor of this",
                CoordX = 1,
                CoordY = 1,
                Price = 10000,
            },
        };

        private static List<EventSeat> _eventSeats = new List<EventSeat>
        {
            new EventSeat
            {
                Id = 8,
                EventAreaId = 6,
                Row = 1,
                Number = 1,
                State = SeatState.NotSold,
            },
            new EventSeat
            {
                Id = 9,
                EventAreaId = 7,
                Row = 1,
                Number = 2,
                State = SeatState.Sold,
            },
            new EventSeat
            {
                Id = 10,
                EventAreaId = 8,
                Row = 1,
                Number = 3,
                State = SeatState.NotSold,
            },
        };

        private static List<Ticket> _tickets = new List<Ticket>
        {
            new Ticket
            {
                Id = 3,
                Name = "Birthday",
                Description = "Birthday of Ramz Ahm (I apologize in advance...)",
                StartEventDate = new DateTime(2021, 04, 19, 17, 10, 00),
                EndEventDate = new DateTime(2021, 04, 19, 19, 10, 00),
                Price = 0,
                UserId = 3,
            },
            new Ticket
            {
                Id = 100,
                Name = "SetUp Test Name",
                Description = "SetUp Test Description",
                StartEventDate = new DateTime(2021, 04, 19, 20, 30, 00),
                EndEventDate = new DateTime(2021, 04, 19, 22, 30, 00),
                Price = 0,
                UserId = 3,
            },
        };

        public static List<Area> Areas { get => new List<Area>(_areas); }

        public static List<Layout> Layouts { get => new List<Layout>(_layouts); }

        public static List<Seat> Seats { get => new List<Seat>(_seats); }

        public static List<Venue> Venues { get => new List<Venue>(_venues); }

        public static List<Event> Events { get => new List<Event>(_events); }

        public static List<EventArea> EventAreas { get => new List<EventArea>(_eventAreas); }

        public static List<EventSeat> EventSeats { get => new List<EventSeat>(_eventSeats); }

        public static List<Ticket> Tickets { get => new List<Ticket>(_tickets); }
    }
}