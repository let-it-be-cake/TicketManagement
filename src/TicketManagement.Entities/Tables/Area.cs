using TicketManagement.Entities.Interfaces;

namespace TicketManagement.Entities.Tables
{
    public class Area : IHasId
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
