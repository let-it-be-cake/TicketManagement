using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Event name")]
        [Editable(allowEdit: false)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Editable(allowEdit: false)]
        public string Description { get; set; }

        [Editable(allowEdit: false)]
        public string ImageUrl { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Start")]
        public DateTime StartEventDate { get; set; }

        [Display(Name = "End")]
        public DateTime EndEventDate { get; set; }

        [Display(Name = "Row")]
        public int Row { get; set; }

        [Display(Name = "Number")]
        public int Number { get; set; }
    }
}
