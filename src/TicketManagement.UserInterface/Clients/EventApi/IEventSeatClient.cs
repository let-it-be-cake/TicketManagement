using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.EventApi
{
    [Header("Content-Type", "application/json")]
    public interface IEventSeatClient
    {
        [Get("seat/{id}")]
        public Task<EventSeat> GetAsync([Path] int id,
                                        CancellationToken cancellationToken = default);

        [Get("seat/get-area-seats/{eventAreaId}")]
        public Task<List<EventSeat>> GetFromEventAreaAsync([Path] int eventAreaId,
                                        CancellationToken cancellationToken = default);

        [Get("seat/get-ticket-seats/{ticketId}")]
        public Task<List<EventSeat>> GetEventSeatsFromTicketAsync([Path] int ticketId,
                                        CancellationToken cancellationToken = default);
    }
}
