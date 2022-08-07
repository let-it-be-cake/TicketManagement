using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.VenueApi
{
    [Header("Content-Type", "application/json")]
    public interface ILayoutClient
    {
        [Post("layout")]
        public Task AddAsync([Body] Event eventModel,
                             [Header("Authorization")] string token,
                             CancellationToken cancellationToken = default);

        [Put("layout")]
        public Task UpdateAsync([Body] Event eventModel,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Delete("layout/{id}")]
        public Task DeleteAsync([Path] int id,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Get("layout/{id}")]
        public Task<Layout> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("layout")]
        public Task<List<Layout>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
