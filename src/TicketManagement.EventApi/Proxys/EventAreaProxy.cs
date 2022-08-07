using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.EventApi.Models;

namespace TicketManagement.EventApi.Proxys
{
    internal class EventAreaProxy : IEventAreaProxy
    {
        private readonly IRepository<EventArea> _eventArea;
        private readonly IRepository<Event> _event;

        private readonly IQuerableHelper _toList;

        public EventAreaProxy(IRepository<EventArea> eventArea, IRepository<Event> @event, IQuerableHelper toList)
        {
            _eventArea = eventArea;
            _event = @event;
            _toList = toList;
        }

        public Task<List<EventArea>> GetAllFromEventAsync(int eventId)
        {
            var eventAreas = from eventArea in _eventArea.GetAll()
                             where eventArea.EventId == eventId
                             select eventArea;
            Task<List<EventArea>> eventAreaList = _toList.ToListAsync(eventAreas);
            return eventAreaList;
        }

        public Task<EventArea> GetEventAreaAsync(int id)
        {
            Task<EventArea> eventArea = _eventArea.GetAsync(id);
            return eventArea;
        }

        public async Task<List<EventAreaModel>> GetUnregisterModelAsync(int from, int howMany)
        {
            FromGreaterOrEqualZero(from);
            HowManyGreaterThanZero(howMany);

            var unregisterEventsArea = from eventArea in _eventArea.GetAll()
                                       join @event in _event.GetAll()
                                       on eventArea.EventId equals @event.Id
                                       select new EventAreaModel
                                       {
                                           Id = eventArea.Id,
                                           Description = eventArea.Description,
                                           CoordX = eventArea.CoordX,
                                           CoordY = eventArea.CoordY,
                                           Price = eventArea.Price,
                                           Name = @event.Name,
                                           DateTimeStart = @event.DateTimeStart,
                                           DateTimeEnd = @event.DateTimeEnd,
                                           ImageUrl = @event.ImageUrl,
                                       };
            List<EventAreaModel> unregEventsAreaList = await _toList.ToListAsync(unregisterEventsArea
                .Skip(from)
                .Take(howMany));
            return unregEventsAreaList;
        }

        public async Task SetPriceAsync(IEnumerable<EventAreaPriceModel> eventAreaPrices)
        {
            EventAreaPricesIsValid(eventAreaPrices);

            foreach (var areaPrice in eventAreaPrices)
            {
                var eventArea = await _eventArea.GetAsync(areaPrice.EventAreaId);
                eventArea.Price = areaPrice.Price;

                await _eventArea.UpdateAsync(eventArea);
            }
        }

        private void HowManyGreaterThanZero(int howMany)
        {
            if (howMany <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(howMany), "Must be greater than 0.");
            }
        }

        private void FromGreaterOrEqualZero(int from)
        {
            if (from < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(from), "Must be greater than 0.");
            }
        }

        private void EventAreaPricesIsValid(IEnumerable<EventAreaPriceModel> eventAreaPrices)
        {
            if (eventAreaPrices == null)
            {
                throw new ArgumentNullException(nameof(eventAreaPrices));
            }
        }
    }
}
