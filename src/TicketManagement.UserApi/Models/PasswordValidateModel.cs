using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.UserApi.Models
{
    public class PasswordValidateModel
    {
        public int UserId { get; set; }

        public string Password { get; set; }
    }
}
