using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserApi.Database.Identity
{
    public class User : IdentityUser<int>
    {
        public string TimeZoneId { get; set; }

        [MaxLength(5)]
        public string Language { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public decimal Money { get; set; }

        public bool IsBlocked { get; set; }
    }
}
