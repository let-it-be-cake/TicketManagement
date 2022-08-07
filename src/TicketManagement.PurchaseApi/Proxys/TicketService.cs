using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;
using TicketManagement.PurchaseApi.Exceptions;

namespace TicketManagement.PurchaseApi.Proxys
{
    internal class TicketService : ITicketService
    {
        private readonly IRepository<Event> _event;
        private readonly IRepository<EventArea> _eventArea;
        private readonly IRepository<EventSeat> _eventSeat;
        private readonly IRepository<Ticket> _ticket;
        private readonly IRepository<User> _userRepository;

        private readonly IQuerableHelper _toList;

        public TicketService(
            IRepository<Event> eventRepository,
            IRepository<EventArea> eventAreaRepository,
            IRepository<EventSeat> eventSeatRepository,
            IRepository<Ticket> ticketRepository,
            IRepository<User> userRepository,
            IQuerableHelper toList)
        {
            _event = eventRepository;
            _eventArea = eventAreaRepository;
            _eventSeat = eventSeatRepository;
            _ticket = ticketRepository;
            _userRepository = userRepository;
            _toList = toList;
        }

        public async Task BuyTicketAsync(int userId, IEnumerable<EventSeat> seatsToBuy)
        {
            User user = await _userRepository.GetAsync(userId);

            UserNotNull(user);
            EventSeatsNotNull(seatsToBuy);
            AllSeatsFromOneArea(seatsToBuy);

            var mainSeat = seatsToBuy.First();

            var eventArea = await _eventArea.GetAll().FirstAsync(eventArea => eventArea.Id == mainSeat.EventAreaId);

            UserHaveEnoughMoney(user, seatsToBuy, eventArea);

            var @event = await _event.GetAll().FirstAsync(@event => @event.Id == eventArea.EventId);

            var ticket = new Ticket
            {
                UserId = user.Id,
                Description = @event.Description,
                Name = @event.Name,
                StartEventDate = @event.DateTimeStart,
                EndEventDate = @event.DateTimeEnd,
                Price = (eventArea.Price.HasValue ? eventArea.Price.Value : 1) * seatsToBuy.Count(),
            };

            await _ticket.CreateAsync(ticket);
            foreach (var seat in seatsToBuy)
            {
                seat.State = Entities.Tables.Enum.SeatState.Sold;
                seat.TicketId = ticket.Id;

                await _eventSeat.UpdateAsync(seat);
            }

            user.Money -= ticket.Price;

            await _userRepository.UpdateAsync(user);
        }

        public Task<List<Ticket>> GetUserTicektsAsync(int userId)
        {
            var ticekts = from ticket in _ticket.GetAll()
                          where ticket.UserId == userId
                          select ticket;

            var ticketsToReturn = _toList.ToListAsync(ticekts);
            return ticketsToReturn;
        }

        private void AllSeatsFromOneArea(IEnumerable<EventSeat> seats)
        {
            var mainSeat = seats.First();
            if (!seats.All(seat => seat.EventAreaId == mainSeat.EventAreaId))
            {
                throw new ArgumentException("Seats from different zones.");
            }
        }

        private void UserHaveEnoughMoney(User user, IEnumerable<EventSeat> seats, EventArea area)
        {
            decimal totalPrice = area.Price.Value * seats.Count();
            if (totalPrice > user.Money)
            {
                throw new NotEnoughMoney($"The {user.UserName} doesn't have enough money.");
            }
        }

        private void UserNotNull(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }

        private void EventSeatsNotNull(IEnumerable<EventSeat> eventSeats)
        {
            if (eventSeats == null)
            {
                throw new ArgumentNullException(nameof(eventSeats));
            }
        }
    }
}
