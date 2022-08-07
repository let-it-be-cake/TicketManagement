using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.VenueApi.Clients
{
    public interface ITokenClient
    {
        [Get("users/validate")]
        public Task ValidateTokenAsync([Query] string token, CancellationToken cancellationToken = default);
    }
}
