using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Layout : IHasId
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public string Description { get; set; }
    }
}
