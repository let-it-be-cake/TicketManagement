using System;

namespace TicketManagement.BusinessLogic.Models
{
    public class TicketModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartEventDate { get; set; }

        public DateTime EndEventDate { get; set; }

        public string ImageUrl { get; set; }
    }
}
