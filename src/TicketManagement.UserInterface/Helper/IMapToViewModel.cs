using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Models.ViewModels.UserCabinet;

namespace TicketManagement.UserInterface.Helper
{
    public interface IMapToViewModel
    {
        public EditUserViewModel UserEditViewModel(UserModel user);

        public List<EventViewModel> EventToViewModel(TimeSpan offset, IEnumerable<Event> events);

        public List<EventAreaViewModel> EventAreaModelToViewModel(TimeSpan offset, IEnumerable<EventAreaModel> eventAreas);

        public List<EventAreaViewModel> EventAreaToViewModel(IEnumerable<EventArea> eventAreas);

        public List<EventSeatsViewModel> EventSeatToViewModel(IEnumerable<EventSeat> eventSeats);

        public Task<List<LayoutViewModel>> LayoutToViewModel(List<Layout> layouts);

        public Task<List<TicketViewModel>> TicketToViewModelaAsync(TimeSpan offset, IEnumerable<Ticket> tickets);

        public List<ThirdPartyEventViewModel> ThirdPartyEventsToViewModel(IEnumerable<ThirdPartyEvent> events);
    }
}
