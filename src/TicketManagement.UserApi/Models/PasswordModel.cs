using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserApi.Models
{
    public class PasswordModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
