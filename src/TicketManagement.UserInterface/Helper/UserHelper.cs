using System.Security.Claims;
using TicketManagement.Entities.Identity;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Helper
{
    public static class UserExtention
    {
        public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimName)
        {
            string claimValue = null;
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                claimValue = identity.FindFirst(claimName)?.Value;
            }

            return claimValue;
        }

        public static UserModel GetUser(this ClaimsPrincipal claimsPrincipal)
        {
            var user = new UserModel
            {
                Id = int.Parse(GetClaim(claimsPrincipal, nameof(User.Id))),
                Email = GetClaim(claimsPrincipal, nameof(User.Email)),
                FirstName = GetClaim(claimsPrincipal, nameof(User.FirstName)),
                Surname = GetClaim(claimsPrincipal, nameof(User.Surname)),
                Language = GetClaim(claimsPrincipal, nameof(User.Language)),
                Money = decimal.Parse(GetClaim(claimsPrincipal, nameof(User.Money))),
                TimeZoneId = GetClaim(claimsPrincipal, nameof(User.TimeZoneId)),
            };

            return user;
        }
    }
}
