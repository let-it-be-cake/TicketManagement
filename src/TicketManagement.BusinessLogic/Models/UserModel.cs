using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.BusinessLogic.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string TimeZoneId { get; set; }

        public string Language { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public decimal? Money { get; set; }
    }
}
