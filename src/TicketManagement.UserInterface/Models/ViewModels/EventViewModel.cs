using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "StartEvent")]
        public DateTime StartEvent { get; set; }

        [Display(Name = "EndEvent")]
        public DateTime EndEvent { get; set; }

        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Площадки")]
        public IEnumerable<LayoutViewModel> Layouts { get; set; }

        public int LayoutId { get; set; }
    }
}
