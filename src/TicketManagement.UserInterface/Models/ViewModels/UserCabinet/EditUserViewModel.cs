namespace TicketManagement.UserInterface.Models.ViewModels.UserCabinet
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        public UserCabinetNoPasswordViewModel NoPassword { get; set; }

        public UserCabinetWithPasswordViewModel WithPassword { get; set; }
    }
}
