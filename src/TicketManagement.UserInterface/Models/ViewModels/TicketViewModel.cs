using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "StartEvent")]
        public DateTime StartEventDate { get; set; }

        [Display(Name = "EndEvent")]
        public DateTime EndEventDate { get; set; }

        [Display(Name = "Seats")]
        public ICollection<EventSeatsViewModel> EventSeats { get; set; }
    }
}
