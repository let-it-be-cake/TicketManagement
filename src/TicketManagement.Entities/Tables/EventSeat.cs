using TicketManagement.Entities.Interfaces;
using TicketManagement.Entities.Tables.Enum;

namespace TicketManagement.Entities.Tables
{
    public class EventSeat : IHasId
    {
        public int Id { get; set; }

        public int? TicketId { get; set; }

        public int EventAreaId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public SeatState State { get; set; }
    }
}
