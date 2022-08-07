using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;

namespace TicketManagement.EventApi.Proxys
{
    public interface IGetEvents
    {
        public Task<Event> GetEventAsync(int eventId);

        public Task<List<Event>> GetRegisterEventsAsync(int from, int howMany);

        public Task<List<Event>> GetUnregisterEventsAsync(int from, int howMany);
    }
}
