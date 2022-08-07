using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserInterface.Models.ViewModels
{
    public class ThirdPartyEventViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Layouts")]
        public IEnumerable<LayoutViewModel> Layouts { get; set; }

        [Display(Name = "Selected_layout")]
        public int? LayoutId { get; set; }

        public bool Saved { get; set; }

        public string ErrorMessage { get; set; }

        [Display(Name = "Image")]
        public string PosterImage { get; set; }
    }
}
