using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class LayoutViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        public override string ToString()
            => Id + ") " + Description + " " + Address + " " + Phone;
    }
}
