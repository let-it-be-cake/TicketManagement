using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Exceptions;

namespace TicketManagement.VenueApi.Proxys
{
    public class SeatProxy : IProxyService<Seat>
    {
        private readonly string _recordAlreadyContainsMessage = "The Seat record with this AreaId, Row, Number fields already exists in the database.";
        private readonly string _wrongNumberParameter = "The seat number cannot be less than zero";
        private readonly string _wrongRowParameter = "The seat row cannot be less than zero";

        private readonly IRepository<Seat> _seatRepository;

        private readonly IQuerableHelper _toList;

        public SeatProxy(IRepository<Seat> seatRepository, IQuerableHelper toList)
        {
            _seatRepository = seatRepository;
            _toList = toList;
        }

        /// <inheritdoc cref="IProxyService{Seat}"/>
        public async Task AddAsync(Seat item)
        {
            if (IsValid(item))
            {
                await _seatRepository.CreateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Seat}"/>
        public async Task ChangeAsync(Seat item)
        {
            if (IsValid(item))
            {
                await _seatRepository.UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Seat}"/>
        public async Task DeleteAsync(int id)
        {
            await _seatRepository.DeleteAsync(id);
        }

        /// <inheritdoc cref="IProxyService{Seat}"/>
        public Task<Seat> ReadAsync(int id)
        {
            Task<Seat> item = _seatRepository.GetAsync(id);
            return item;
        }

        /// <inheritdoc cref="IProxyService{Seat}"/>
        public Task<List<Seat>> ReadAllAsync()
        {
            Task<List<Seat>> listTask = _toList.ToListAsync(_seatRepository.GetAll());
            return listTask;
        }

        public bool IsValid(Seat item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (item.Number < 0)
            {
                throw new ArgumentOutOfRangeException(_wrongNumberParameter);
            }

            if (item.Row < 0)
            {
                throw new ArgumentOutOfRangeException(_wrongRowParameter);
            }

            if (_seatRepository.GetAll().Any(o =>
                o.AreaId == item.AreaId &&
                o.Number == item.Number &&
                o.Row == item.Row))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
