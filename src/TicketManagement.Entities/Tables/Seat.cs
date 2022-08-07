using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Seat : IHasId
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }
    }
}
