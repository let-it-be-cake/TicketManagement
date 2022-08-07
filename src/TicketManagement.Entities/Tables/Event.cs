using System;
using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Event : IHasId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LayoutId { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public string ImageUrl { get; set; }
    }
}
