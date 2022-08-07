using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;

namespace TicketManagement.PurchaseApi.Proxys
{
    public interface ITicketService
    {
        public Task BuyTicketAsync(int userId, IEnumerable<EventSeat> seatsToBuy);

        public Task<List<Ticket>> GetUserTicektsAsync(int userId);
    }
}
