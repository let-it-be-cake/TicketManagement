using System.Collections.Generic;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Helper
{
    public interface IThirdPartyEventHelper
    {
        public List<ThirdPartyEvent> GetEvents();

        public void SaveEvents(IEnumerable<ThirdPartyEvent> events);
    }
}