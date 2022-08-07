using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserApi.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string TimeZoneId { get; set; }

        [Required]
        [MaxLength(5)]
        public string Language { get; set; }
    }
}
