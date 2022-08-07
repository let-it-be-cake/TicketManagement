using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.VenueApi
{
    [Header("Content-Type", "application/json")]
    public interface IAreaClient
    {
        [Post("area")]
        public Task AddAsync([Body] Event eventModel,
                             [Header("Authorization")] string token,
                             CancellationToken cancellationToken = default);

        [Put("area")]
        public Task UpdateAsync([Body] Event eventModel,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Delete("area/{id}")]
        public Task DeleteAsync([Path] int id,
                                [Header("Authorization")] string token,
                                CancellationToken cancellationToken = default);

        [Get("area/{id}")]
        public Task<Area> GetAsync([Path] int id,
                                    CancellationToken cancellationToken = default);

        [Get("area")]
        public Task<List<Area>> GetAllAsync([Header("Authorization")] string token,
                                        CancellationToken cancellationToken = default);
    }
}
