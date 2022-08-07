using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.EventApi
{
    [Header("Content-Type", "application/json")]
    public interface IEventClient
    {
        [Post("events")]
        public Task AddAsync([Body] Event eventModel,
                             [Header("Authorization")] string token,
                             CancellationToken cancellationToken = default);

        [Put("events")]
        public Task UpdateAsync([Body] Event eventModel,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Delete("events/{id}")]
        public Task DeleteAsync([Path] int id,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Get("events/{id}")]
        public Task<Event> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("events")]
        public Task<List<Event>> GetAllAsync([Header("Authorization")] string token,
                                        CancellationToken cancellationToken = default);
    }
}
