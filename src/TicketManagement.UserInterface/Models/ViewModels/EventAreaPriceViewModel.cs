using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class EventAreaPriceViewModel
    {
        [Display(Name = "Event")]
        public int EventAreaId { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }
    }
}
