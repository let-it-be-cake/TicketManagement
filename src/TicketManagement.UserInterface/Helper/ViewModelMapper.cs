using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Entities.Tables;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Clients.VenueApi;
using TicketManagement.UserInterface.Models;
using TicketManagement.UserInterface.Models.ViewModels;
using TicketManagement.UserInterface.Models.ViewModels.Enums;
using TicketManagement.UserInterface.Models.ViewModels.UserCabinet;

namespace TicketManagement.UserInterface.Helper
{
    internal class ViewModelMapper : IMapToViewModel
    {
        private readonly IVenueClient _venueClient;
        private readonly IEventSeatClient _getEventSeats;

        public ViewModelMapper(IVenueClient venueClient, IEventSeatClient getEventSeats)
        {
            _venueClient = venueClient;
            _getEventSeats = getEventSeats;
        }

        public EditUserViewModel UserEditViewModel(UserModel user)
        {
            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                NoPassword = new UserCabinetNoPasswordViewModel
                {
                    Name = user.FirstName,
                    Surname = user.Surname,
                    Language = user.Language,
                    TimeOffsetId = user.TimeZoneId,
                },

                WithPassword = new UserCabinetWithPasswordViewModel
                {
                    Email = user.Email,
                    Money = user.Money.HasValue ? user.Money.Value : 0,
                },
            };

            return viewModel;
        }

        public List<EventViewModel> EventToViewModel(TimeSpan offset, IEnumerable<Event> events)
        {
            List<EventViewModel> eventViews = new List<EventViewModel>();

            foreach (var @event in events)
            {
                eventViews.Add(new EventViewModel
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Description = @event.Description,
                    StartEvent = @event.DateTimeStart + offset,
                    EndEvent = @event.DateTimeEnd + offset,
                    ImageUrl = @event.ImageUrl,
                });
            }

            return eventViews;
        }

        public List<EventAreaViewModel> EventAreaModelToViewModel(TimeSpan offset, IEnumerable<EventAreaModel> eventAreas)
        {
            List<EventAreaViewModel> eventAreaViews = new List<EventAreaViewModel>();

            foreach (var eventArea in eventAreas)
            {
                eventAreaViews.Add(new EventAreaViewModel
                {
                    Id = eventArea.Id,
                    Description = eventArea.Description,
                    CoordX = eventArea.CoordX,
                    CoordY = eventArea.CoordY,
                    Price = eventArea.Price,
                    Name = eventArea.Name,
                    DateTimeStart = eventArea.DateTimeStart + offset,
                    DateTimeEnd = eventArea.DateTimeEnd + offset,
                    ImageUrl = eventArea.ImageUrl,
                });
            }

            return eventAreaViews;
        }

        public List<EventAreaViewModel> EventAreaToViewModel(IEnumerable<EventArea> eventAreas)
        {
            var eventAreaViews = new List<EventAreaViewModel>();

            foreach (var eventArea in eventAreas)
            {
                eventAreaViews.Add(new EventAreaViewModel
                {
                    Id = eventArea.Id,
                    Description = eventArea.Description,
                    Price = eventArea.Price,
                    CoordX = eventArea.CoordX,
                    CoordY = eventArea.CoordY,
                });
            }

            return eventAreaViews;
        }

        public List<EventSeatsViewModel> EventSeatToViewModel(IEnumerable<EventSeat> eventSeats)
        {
            var eventSeatsViews = new List<EventSeatsViewModel>();

            foreach (var eventSeat in eventSeats)
            {
                eventSeatsViews.Add(new EventSeatsViewModel
                {
                    Id = eventSeat.Id,
                    EventAreaId = eventSeat.EventAreaId,
                    Number = eventSeat.Number,
                    Row = eventSeat.Row,
                    State = (SeatStateViewModel)eventSeat.State,
                });
            }

            return eventSeatsViews;
        }

        public async Task<List<LayoutViewModel>> LayoutToViewModel(List<Layout> layouts)
        {
            var layoutsViewModel = new List<LayoutViewModel>();

            foreach (var layout in layouts)
            {
                var layoutVenue = await _venueClient.GetAsync(layout.VenueId);

                layoutsViewModel.Add(
                    new LayoutViewModel
                    {
                        Id = layout.Id,
                        Description = layout.Description,
                        Address = layoutVenue.Address,
                        Phone = layoutVenue.Phone,
                    });
            }

            return layoutsViewModel;
        }

        public async Task<List<TicketViewModel>> TicketToViewModelaAsync(TimeSpan offset, IEnumerable<Ticket> tickets)
        {
            var ticketViewModels = new List<TicketViewModel>();

            foreach (var ticket in tickets)
            {
                ticketViewModels.Add(new TicketViewModel
                {
                    Id = ticket.Id,
                    Description = ticket.Description,
                    StartEventDate = ticket.StartEventDate + offset,
                    EndEventDate = ticket.EndEventDate + offset,
                    Name = ticket.Name,
                    Price = ticket.Price,
                    EventSeats = EventSeatToViewModel(await _getEventSeats.GetEventSeatsFromTicketAsync(ticket.Id)),
                });
            }

            return ticketViewModels;
        }

        public List<ThirdPartyEventViewModel> ThirdPartyEventsToViewModel(IEnumerable<ThirdPartyEvent> events)
        {
            var thirdPartyEventsViewModel = new List<ThirdPartyEventViewModel>();

            foreach (var @event in events)
            {
                thirdPartyEventsViewModel.Add(new ThirdPartyEventViewModel
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Description = @event.Description,
                    StartDate = @event.StartDate,
                    EndDate = @event.EndDate,
                    PosterImage = @event.PosterImage,
                });
            }

            return thirdPartyEventsViewModel;
        }
    }
}
