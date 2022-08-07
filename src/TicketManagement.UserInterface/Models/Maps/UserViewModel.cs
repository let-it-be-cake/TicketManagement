using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.Maps
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string TimeZoneId { get; set; }

        [MaxLength(5)]
        public string Language { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public decimal? Money { get; set; }

        public bool IsBlocked { get; set; }
    }
}
