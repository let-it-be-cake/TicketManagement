using System;
using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Ticket : IHasId
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartEventDate { get; set; }

        public DateTime EndEventDate { get; set; }
    }
}
