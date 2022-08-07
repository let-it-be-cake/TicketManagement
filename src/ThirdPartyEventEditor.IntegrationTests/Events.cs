using System;
using System.Collections.Generic;
using ThirdPartyEventEditor.ServiceTables;

namespace ThirdPartyEventEditor.IntegrationTests
{
    public static class Data
    {
        private static readonly List<Event> _events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Name = "Event 1",
                Description = "Description 1",
                StartDate = new DateTime(2025, 01, 01, 01, 01, 01),
                EndDate = new DateTime(2025, 01, 01, 11, 01, 01),
                PosterImage = "",
            },
            new Event
            {
                Id = 2,
                Name = "Event 2",
                Description = "Description 2",
                StartDate = new DateTime(2025, 02, 02, 01, 01, 01),
                EndDate = new DateTime(2025, 02, 02, 11, 01, 01),
                PosterImage = "",
            },
            new Event
            {
                Id = 3,
                Name = "Event 3",
                Description = "Description 3",
                StartDate = new DateTime(2025, 03, 03, 01, 01, 01),
                EndDate = new DateTime(2025, 03, 03, 11, 01, 01),
                PosterImage = "",
            },
        };

        public static List<Event> Events => new List<Event>(_events);
    }
}
