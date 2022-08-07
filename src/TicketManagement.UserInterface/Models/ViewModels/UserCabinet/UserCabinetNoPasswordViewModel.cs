using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels.UserCabinet
{
    public class UserCabinetNoPasswordViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Time zone")]
        public string TimeOffsetId { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string Language { get; set; }
    }
}
