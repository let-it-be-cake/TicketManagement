using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class EventAreaViewModel
    {
        [Editable(allowEdit: false)]
        public int Id { get; set; }

        [Display(Name = "Description")]
        [Editable(allowEdit: false)]
        public string Description { get; set; }

        [Display(Name = "CoordX")]
        [Editable(allowEdit: false)]
        public int CoordX { get; set; }

        [Display(Name = "CoordY")]
        [Editable(allowEdit: false)]
        public int CoordY { get; set; }

        [Display(Name = "Price")]
        [Editable(allowEdit: true)]
        public decimal? Price { get; set; }

        [Display(Name = "Name")]
        [Editable(allowEdit: false)]
        public string Name { get; set; }

        [Display(Name = "Start event")]
        [Editable(allowEdit: false)]
        public DateTime DateTimeStart { get; set; }

        [Display(Name = "End event")]
        [Editable(allowEdit: false)]
        public DateTime DateTimeEnd { get; set; }

        [Editable(allowEdit: false)]
        public string ImageUrl { get; set; }
    }
}
