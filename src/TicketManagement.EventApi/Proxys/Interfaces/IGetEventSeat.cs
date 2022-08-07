using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;

namespace TicketManagement.EventApi.Proxys
{
    public interface IGetEventSeat
    {
        public Task<EventSeat> GetAsync(int id);

        public Task<List<EventSeat>> GetFromEventAreaAsync(int eventAreaId);

        public Task<List<EventSeat>> GetEventSeatsFromTicketAsync(int ticketId);
    }
}
