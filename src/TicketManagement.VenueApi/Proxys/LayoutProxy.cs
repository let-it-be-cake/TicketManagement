using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Exceptions;

namespace TicketManagement.VenueApi.Proxys
{
    public class LayoutProxy : IProxyService<Layout>
    {
        private readonly string _recordAlreadyContainsMessage = "The Layout record with this VenueId, Description fields already exists in the database.";

        private readonly IRepository<Layout> _layoutRepository;

        private readonly IQuerableHelper _toList;

        public LayoutProxy(IRepository<Layout> repository, IQuerableHelper toList)
        {
            _layoutRepository = repository;
            _toList = toList;
        }

        /// <inheritdoc cref="IProxyService{Layout}"/>
        public async Task AddAsync(Layout item)
        {
            if (IsValid(item))
            {
                await _layoutRepository.CreateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Layout}"/>
        public async Task ChangeAsync(Layout item)
        {
            if (IsValid(item))
            {
                await _layoutRepository.UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Layout}"/>
        public async Task DeleteAsync(int id)
        {
            await _layoutRepository.DeleteAsync(id);
        }

        /// <inheritdoc cref="IProxyService{Layout}"/>
        public Task<Layout> ReadAsync(int id)
        {
            Task<Layout> item = _layoutRepository.GetAsync(id);
            return item;
        }

        /// <inheritdoc cref="IProxyService{Layout}"/>
        public Task<List<Layout>> ReadAllAsync()
        {
            Task<List<Layout>> listTask = _toList.ToListAsync(_layoutRepository.GetAll());
            return listTask;
        }

        public bool IsValid(Layout item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_layoutRepository.GetAll().Any(o =>
                o.VenueId == item.VenueId &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
