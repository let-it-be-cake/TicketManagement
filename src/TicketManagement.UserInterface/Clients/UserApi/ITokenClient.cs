using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Clients.UserApi
{
    [Header("Content-Type", "application/json")]
    public interface ITokenClient
    {
        [Post("users/register")]
        public Task<string> RegisterAsync([Body] RegisterModel registerModel,
                                          CancellationToken cancellationToken = default);

        [Post("users/login")]
        public Task<string> LoginAsync([Body] LoginModel loginModel,
                                       CancellationToken cancellationToken = default);

        [Get("users/validate")]
        public Task ValidateTokenAsync([Query] string token,
                                       CancellationToken cancellationToken = default);
    }
}
