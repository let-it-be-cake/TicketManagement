using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Interfaces;

namespace TicketManagement.BusinessLogic.Proxys
{
    internal class Proxy<T> : IProxyService<T>
        where T : class, IHasId, new()
    {
        private readonly IRepository<T> _repository;
        private readonly IValidate<T> _validate;

        private readonly IQuerableHelper _toList;

        public Proxy(IRepository<T> repository, IValidate<T> validate, IQuerableHelper toList)
        {
            _repository = repository;
            _validate = validate;
            _toList = toList;
        }

        /// <inheritdoc cref="IProxyService{T}"/>
        public async Task AddAsync(T item)
        {
            if (_validate.IsValid(item))
            {
                await _repository.CreateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{T}"/>
        public async Task ChangeAsync(T item)
        {
            if (_validate.IsValid(item))
            {
                await _repository.UpdateAsync(item);
            }
        }

        /// <inheritdoc cref="IProxyService{T}"/>
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <inheritdoc cref="IProxyService{T}"/>
        public Task<T> ReadAsync(int id)
        {
            Task<T> item = _repository.GetAsync(id);
            return item;
        }

        /// <inheritdoc cref="IProxyService{T}"/>
        public Task<List<T>> ReadAllAsync()
        {
            Task<List<T>> listTask = _toList.ToListAsync(_repository.GetAll());
            return listTask;
        }
    }
}
