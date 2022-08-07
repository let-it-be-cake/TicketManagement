using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class AdminEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Money")]
        [Range(0, int.MaxValue)]
        public decimal Money { get; set; }
    }
}
