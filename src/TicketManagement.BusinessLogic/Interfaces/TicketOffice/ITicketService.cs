using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic.Interfaces.TicketOffice
{
    public interface ITicketService
    {
        public Task BuyTicketAsync(int userId, IEnumerable<EventSeat> seatsToBuy);

        public Task<List<Ticket>> GetUserTicektsAsync(int userId);
    }
}
