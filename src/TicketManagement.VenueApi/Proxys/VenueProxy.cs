using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Exceptions;

namespace TicketManagement.VenueApi.Proxys
{
    public class VenueProxy : IProxyService<Venue>
    {
        private readonly string _recordAlreadyContainsMessage = "The Venue record with this Address, Phone, Description fields already exists in the database.";

        private readonly IRepository<Venue> _venueRepository;

        private readonly IQuerableHelper _toList;

        public VenueProxy(IRepository<Venue> venueRepository, IQuerableHelper toList)
        {
            _venueRepository = venueRepository;
            _toList = toList;
        }

        /// <inheritdoc cref="IProxyService{Venue}"/>
        public async Task AddAsync(Venue item)
        {
            if (IsValid(item))
            {
                await _venueRepository.CreateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Venue}"/>
        public async Task ChangeAsync(Venue item)
        {
            if (IsValid(item))
            {
                await _venueRepository.UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Venue}"/>
        public async Task DeleteAsync(int id)
        {
            await _venueRepository.DeleteAsync(id);
        }

        /// <inheritdoc cref="IProxyService{Venue}"/>
        public Task<Venue> ReadAsync(int id)
        {
            Task<Venue> item = _venueRepository.GetAsync(id);
            return item;
        }

        /// <inheritdoc cref="IProxyService{Venue}"/>
        public Task<List<Venue>> ReadAllAsync()
        {
            Task<List<Venue>> listTask = _toList.ToListAsync(_venueRepository.GetAll());
            return listTask;
        }

        public bool IsValid(Venue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_venueRepository.GetAll().Any(o =>
                o.Address == item.Address &&
                o.Phone == item.Phone &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
