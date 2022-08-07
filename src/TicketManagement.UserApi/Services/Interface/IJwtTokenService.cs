using System.Collections.Generic;
using TicketManagement.UserApi.Database.Identity;

namespace TicketManagement.UserApi.Services
{
    public interface IJwtTokenService
    {
        public string GenerateToken(User user, IList<string> roles);

        public bool ValidateToken(string token);
    }
}
