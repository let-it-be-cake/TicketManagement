using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.VenueApi
{
    [Header("Content-Type", "application/json")]
    public interface ISeatClient
    {
        [Post("seat")]
        public Task AddAsync([Body] Event eventModel,
                             [Header("Authorization")] string token,
                             CancellationToken cancellationToken = default);

        [Put("seat")]
        public Task UpdateAsync([Body] Event eventModel,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Delete("seat/{id}")]
        public Task DeleteAsync([Path] int id,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Get("seat/{id}")]
        public Task<Seat> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("seat")]
        public Task<List<Seat>> GetAllAsync([Header("Authorization")] string token,
                                        CancellationToken cancellationToken = default);
    }
}
