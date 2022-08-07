using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Exceptions;

namespace TicketManagement.VenueApi.Proxys
{
    internal class AreaProxy : IProxyService<Area>
    {
        private readonly string _recordAlreadyContainsMessage = "The Area record with this LayoutId, CoordX, CoordY, Description fields already exists in the database.";
        private readonly IRepository<Area> _areaRepository;

        private readonly IQuerableHelper _toList;

        public AreaProxy(IRepository<Area> repository, IQuerableHelper toList)
        {
            _areaRepository = repository;
            _toList = toList;
        }

        /// <inheritdoc cref="IProxyService{Area}"/>
        public async Task AddAsync(Area item)
        {
            if (IsValid(item))
            {
                await _areaRepository.CreateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Area}"/>
        public async Task ChangeAsync(Area item)
        {
            if (IsValid(item))
            {
                await _areaRepository.UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{Area}"/>
        public async Task DeleteAsync(int id)
        {
            await _areaRepository.DeleteAsync(id);
        }

        /// <inheritdoc cref="IProxyService{Area}"/>
        public Task<Area> ReadAsync(int id)
        {
            Task<Area> item = _areaRepository.GetAsync(id);
            return item;
        }

        /// <inheritdoc cref="IProxyService{Area}"/>
        public Task<List<Area>> ReadAllAsync()
        {
            Task<List<Area>> listTask = _toList.ToListAsync(_areaRepository.GetAll());
            return listTask;
        }

        private bool IsValid(Area item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_areaRepository.GetAll().Any(o =>
                o.LayoutId == item.LayoutId &&
                o.CoordX == item.CoordX &&
                o.CoordY == item.CoordY &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
