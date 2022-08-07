using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Models;

namespace TicketManagement.EventApi.Proxys
{
    public interface IEventAreaProxy
    {
        public Task<EventArea> GetEventAreaAsync(int id);

        public Task<List<EventArea>> GetAllFromEventAsync(int eventId);

        public Task<List<EventAreaModel>> GetUnregisterModelAsync(int from, int howMany);

        public Task SetPriceAsync(IEnumerable<EventAreaPriceModel> eventAreaPrices);
    }
}
