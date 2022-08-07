using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.UserApi.Database.Identity;

namespace TicketManagement.UserApi.Database
{
    public class UserApiDbContext : IdentityDbContext<User, Role, int>
    {
        public UserApiDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
