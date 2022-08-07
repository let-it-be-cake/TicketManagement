using System;

namespace TicketManagement.UserInterface.Models
{
    public class EventAreaModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }

        public decimal? Price { get; set; }

        public string Name { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public string ImageUrl { get; set; }
    }
}
