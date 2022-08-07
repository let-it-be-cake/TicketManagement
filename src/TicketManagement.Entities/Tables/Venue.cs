using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Venue : IHasId
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
