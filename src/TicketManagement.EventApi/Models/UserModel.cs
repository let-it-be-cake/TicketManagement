namespace TicketManagement.EventApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public decimal? Money { get; set; }

        public bool IsBlocked { get; set; }
    }
}
