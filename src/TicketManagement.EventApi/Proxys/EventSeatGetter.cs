using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.EventApi.Proxys
{
    internal class EventSeatGetter : IGetEventSeat
    {
        private readonly IRepository<EventSeat> _eventSeat;
        private readonly IQuerableHelper _toList;

        public EventSeatGetter(IRepository<EventSeat> eventSeat, IQuerableHelper toList)
        {
            _eventSeat = eventSeat;
            _toList = toList;
        }

        public Task<EventSeat> GetAsync(int id)
        {
            Task<EventSeat> eventSeat = _eventSeat.GetAsync(id);
            return eventSeat;
        }

        public Task<List<EventSeat>> GetFromEventAreaAsync(int eventAreaId)
        {
            var eventSeats = from eventSeat in _eventSeat.GetAll()
                             where eventSeat.EventAreaId == eventAreaId
                             select eventSeat;

            Task<List<EventSeat>> eventSeatsCollection = _toList.ToListAsync(eventSeats);
            return eventSeatsCollection;
        }

        public Task<List<EventSeat>> GetEventSeatsFromTicketAsync(int ticketId)
        {
            var eventSeats = from eventSeat in _eventSeat.GetAll()
                             where eventSeat.TicketId == ticketId
                             select eventSeat;

            Task<List<EventSeat>> eventSeatsList = _toList.ToListAsync(eventSeats);
            return eventSeatsList;
        }
    }
}
