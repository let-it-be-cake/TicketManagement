namespace TicketManagement.UserInterface.Models
{
    public class PasswordModel
    {
        public int UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
