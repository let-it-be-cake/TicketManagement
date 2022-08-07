using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.VenueApi
{
    [Header("Content-Type", "application/json")]
    public interface IVenueClient
    {
        [Post("venue")]
        public Task AddAsync([Body] Event eventModel,
                             [Header("Authorization")] string token,
                             CancellationToken cancellationToken = default);

        [Put("venue")]
        public Task UpdateAsync([Body] Event eventModel,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Delete("venue/{id}")]
        public Task DeleteAsync([Path] int id,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Get("venue/{id}")]
        public Task<Venue> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("venue")]
        public Task<List<Venue>> GetAllAsync([Header("Authorization")] string token,
                                        CancellationToken cancellationToken = default);
    }
}
