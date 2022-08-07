using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.Entities.Tables.Enum;
using TicketManagement.EventApi.Exceptions;

namespace TicketManagement.EventApi.Proxys
{
    internal class EventProxy : IEventProxyService, IGetEvents
    {
        private readonly string _recordAlreadyContainsMessage = "The Event record with this LayoutId, Name, Description fields already exists in the database.";
        private readonly string _eventSeatsGreaterThanZero = "The number of seats for the event must be greater than zero";
        private readonly string _eventSeateSold = "Some seats were sold.";

        private readonly IRepository<Event> _event;
        private readonly IRepository<Area> _area;
        private readonly IRepository<Seat> _seat;
        private readonly IRepository<EventArea> _eventArea;
        private readonly IRepository<EventSeat> _eventSeat;

        private readonly IQuerableHelper _toList;

        public EventProxy(
            IRepository<Event> @event,
            IRepository<Area> area,
            IRepository<Seat> seat,
            IRepository<EventArea> eventArea,
            IRepository<EventSeat> eventSeat,
            IQuerableHelper toList)
        {
            _event = @event;
            _area = area;
            _seat = seat;
            _eventArea = eventArea;
            _eventSeat = eventSeat;
            _toList = toList;
        }

        /// <inheritdoc cref="IEventProxyService"/>
        public async Task AddAsync(Event item)
        {
            CreateValidate(item);

            await _event.CreateAsync(item);
            await CreateAsync(item);
        }

        /// <inheritdoc cref="IEventProxyService"/>
        public async Task ChangeAsync(Event item)
        {
            CreateValidate(item);
            AnySeatNotSould(item);

            if (_event.GetAll().Any(o => o.Id == item.Id))
            {
                await _event.UpdateAsync(item);
                await UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IEventProxyService"/>
        public async Task DeleteAsync(int id)
        {
            AnySeatNotSould(await ReadAsync(id));
            await _event.DeleteAsync(id);
        }

        /// <inheritdoc cref="IEventProxyService"/>
        public Task<Event> ReadAsync(int id)
        {
            Task<Event> @event = _event.GetAsync(id);
            return @event;
        }

        /// <inheritdoc cref="IEventProxyService"/>
        public Task<List<Event>> ReadAllAsync()
        {
            Task<List<Event>> collection = _toList.ToListAsync(_event.GetAll());
            return collection;
        }

        public Task<Event> GetEventAsync(int eventId)
        {
            Task<Event> @event = _event.GetAsync(eventId);
            return @event;
        }

        public async Task<List<Event>> GetRegisterEventsAsync(int from, int howMany)
        {
            FromGreaterOrEqualZero(from);
            HowManyGreaterThanZero(howMany);
            IQueryable<Event> eventsQuery = from area in _eventArea.GetAll()
                                            join @event in _event.GetAll()
                                                     on area.EventId equals @event.Id
                                            where area.Price.HasValue
                                            select @event;

            List<Event> events = await _toList.ToListAsync(eventsQuery
                .Skip(from)
                .Take(howMany)
                .Distinct());

            return events;
        }

        public async Task<List<Event>> GetUnregisterEventsAsync(int from, int howMany)
        {
            FromGreaterOrEqualZero(from);
            HowManyGreaterThanZero(howMany);

            IQueryable<Event> eventsQuery = from @event in _event.GetAll()
                                            join area in _eventArea.GetAll()
                                            on @event.Id equals area.EventId
                                            where !area.Price.HasValue
                                            select @event;

            List<Event> events = await _toList.ToListAsync(eventsQuery
                .Skip(from)
                .Take(howMany)
                .Distinct());

            return events;
        }

        private async Task CreateAsync(Event item)
        {
            int layoutId = item.LayoutId;

            var areasToAdd = from area in _area.GetAll()
                             where area.LayoutId == layoutId
                             select area;

            foreach (var areaToAdd in await _toList.ToListAsync(areasToAdd))
            {
                // Mapping event area
                var eventArea = new EventArea
                {
                    Description = areaToAdd.Description,
                    CoordX = areaToAdd.CoordX,
                    CoordY = areaToAdd.CoordY,
                    EventId = item.Id,
                };

                // Get event area id
                await _eventArea.CreateAsync(eventArea);

                // Get all seats to area, and map to event seat
                var eventSeats = from seat in _seat.GetAll()
                                 where seat.AreaId == areaToAdd.Id
                                 select new EventSeat
                                 {
                                     EventAreaId = eventArea.Id,
                                     Number = seat.Number,
                                     Row = seat.Row,
                                     State = SeatState.NotSold,
                                 };

                // Create all even seats
                foreach (var eventSeat in await _toList.ToListAsync(eventSeats))
                {
                    await _eventSeat.CreateAsync(eventSeat);
                }
            }
        }

        private async Task UpdateAsync(Event item)
        {
            var eventAreasToDelete = _eventArea.GetAll().Where(o => o.EventId == item.Id);

            foreach (var eventArea in await _toList.ToListAsync(eventAreasToDelete))
            {
                await _eventArea.DeleteAsync(eventArea.Id);
            }

            await CreateAsync(item);
        }

        private void CreateValidate(Event item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot be null");
            }

            if (item.DateTimeStart > item.DateTimeEnd)
            {
                throw new ArgumentException("The event ended before it started.");
            }

            if (item.DateTimeEnd < DateTime.UtcNow)
            {
                throw new ArgumentException("The event has already ended.");
            }

            var areaIds = from area in _area.GetAll()
                          where area.LayoutId == item.LayoutId
                          select area.Id;

            bool haventAnySeats = !_seat.GetAll()
                .Any(o => areaIds.Contains(o.AreaId));

            if (haventAnySeats)
            {
                throw new ArgumentException(_eventSeatsGreaterThanZero);
            }

            bool databaseConatinThisRecord = _event.GetAll()
                .Any(o =>
                    o.LayoutId == item.LayoutId &&
                    o.Name == item.Name &&
                    o.Description == item.Description);

            if (databaseConatinThisRecord)
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }
        }

        private void AnySeatNotSould(Event item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot be null");
            }

            var areaIds = from eventArea in _eventArea.GetAll()
                          where eventArea.EventId == item.Id
                          select eventArea.Id;

            bool isSolded = !_eventSeat.GetAll()
                .Where(o => areaIds.Contains(o.EventAreaId))
                .All(o =>
                    o.State == SeatState.Indefinite ||
                    o.State == SeatState.NotSold);

            if (isSolded)
            {
                throw new ArgumentException(_eventSeateSold);
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
    }
}