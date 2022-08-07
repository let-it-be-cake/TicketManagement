using System.ComponentModel.DataAnnotations;
using TicketManagement.UserInterface.Models.ViewModels.Enums;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class EventSeatsViewModel
    {
        [Editable(allowEdit: false)]
        public int Id { get; set; }

        public int EventAreaId { get; set; }

        [Display(Name = "Row")]
        public int Row { get; set; }

        [Display(Name = "Number")]
        public int Number { get; set; }

        [Display(Name = "State")]
        public SeatStateViewModel State { get; set; }
    }
}
