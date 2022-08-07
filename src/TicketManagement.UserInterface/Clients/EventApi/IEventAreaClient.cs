using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Clients.EventApi
{
    [Header("Content-Type", "application/json")]
    public interface IEventAreaClient
    {
        [Get("area/get/{id}")]
        public Task<EventArea> GetAsync([Path] int id,
                                        [Header("Authorization")] string token,
                                        CancellationToken cancellationToken = default);

        [Get("area/get-event-areas/{eventId}")]
        public Task<List<EventArea>> GetEventAreasAsync([Path] int eventId,
                                                       CancellationToken cancellationToken = default);

        [Get("area/get-unregister/{from}&{howMany}")]
        public Task<List<EventAreaModel>> GetUnregisterAsync([Path] int from,
                                                        [Path] int howMany,
                                                        [Header("Authorization")] string token,
                                                        CancellationToken cancellationToken = default);

        [Post("area/set-price")]
        public Task SetPriceAsync([Body] IEnumerable<EventAreaPriceModel> areaPrices,
                                  [Header("Authorization")] string token,
                                  CancellationToken cancellationToken = default);
    }
}
