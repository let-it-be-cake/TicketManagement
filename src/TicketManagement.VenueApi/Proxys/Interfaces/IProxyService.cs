using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.VenueApi.Proxys
{
    public interface IProxyService<T>
        where T : class, new()
    {
        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>The collection of objects.</returns>
        Task<List<T>> ReadAllAsync();

        /// <summary>
        /// Get an object by id.
        /// </summary>
        /// <param name="id">Id of the object to get.</param>
        /// <returns>The object located by this id.</returns>
        Task<T> ReadAsync(int id);

        /// <summary>
        /// Add a new object.
        /// </summary>
        /// <param name="item">Object to add.</param>
        Task AddAsync(T item);

        /// <summary>
        /// Change a record.
        /// </summary>
        /// <param name="item">A new instance of the object.</param>
        Task ChangeAsync(T item);

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="id">Id of the object to delete.</param>
        Task DeleteAsync(int id);
    }
}
