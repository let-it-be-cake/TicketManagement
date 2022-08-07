using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ThirdPartyEventEditor.Models
{
    public class ThirdPartyEventViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Event name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Event description")]
        public string Description { get; set; }

        [Display(Name = "Event start date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Event end date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Poster")]
        public string PosterImage { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}