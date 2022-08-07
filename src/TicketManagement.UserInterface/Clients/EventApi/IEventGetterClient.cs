using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.EventApi
{
    [Header("Content-Type", "application/json")]
    public interface IEventGetterClient
    {
        [Get("event-getter/{id}")]
        public Task<Event> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("event-getter/register/{from}&{howMany}")]
        public Task<List<Event>> GetRegisterAsync([Path] int from,
                                                  [Path] int howMany,
                                                  CancellationToken cancellationToken = default);

        [Get("event-getter/unregister/{from}&{howMany}")]
        public Task<List<Event>> GetUnregisterAsync([Path] int from,
                                                    [Path] int howMany,
                                                    [Header("Authorization")] string token,
                                                    CancellationToken cancellationToken = default);
    }
}
