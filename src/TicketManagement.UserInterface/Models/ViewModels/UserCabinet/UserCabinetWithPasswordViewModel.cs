using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels.UserCabinet
{
    public class UserCabinetWithPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Money")]
        [Range(0, int.MaxValue)]
        public decimal Money { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(4)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [MinLength(4)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
    }
}
