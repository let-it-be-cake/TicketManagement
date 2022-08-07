using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels.Enums
{
    public enum SeatStateViewModel
    {
        [Display(Description = "Indefinite")]
        Indefinite,
        [Display(Description = "NotSold")]
        NotSold,
        [Display(Description = "Sold")]
        Sold,
    }
}