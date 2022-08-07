using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserApi.Database.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
            : base()
        {
        }

        public Role(string role)
            : base(role)
        {
        }
    }
}
