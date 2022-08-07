using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Entities.Tables;

namespace TicketManagement.UserInterface.Clients.PurchaseApi
{
    [Header("Content-Type", "application/json")]
    public interface IPurchaseClient
    {
        [Post("ticket/buy/{userId}")]
        public Task BuyTickets([Path] int userId,
                               [Body] IEnumerable<int> eventSeatsId,
                               [Header("Authorization")] string token,
                               CancellationToken cancellationToken = default);

        [Get("ticket/get-user-tickets/{userId}")]
        public Task<List<Ticket>> GetUserTickets([Path] int userId,
                                                 [Header("Authorization")] string token,
                                                 CancellationToken cancellationToken = default);
    }
}
