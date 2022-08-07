using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    public interface IRepository<T>
        where T : class, new()
    {
        /// <summary>
        /// Get all objects from the DataBase.
        /// </summary>
        /// <returns>The objects located by the DataBase.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get an object by id from the DataBase.
        /// </summary>
        /// <param name="id">Id of the object to get.</param>
        /// <returns>The object located by this id in the DataBase.</returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Create a new row in the DataBase table.
        /// </summary>
        /// <param name="item">Object to create.</param>
        Task CreateAsync(T item);

        /// <summary>
        /// Updates a record in the DataBase.
        /// </summary>
        /// <param name="item">Object to update in the DataBase.</param>
        Task UpdateAsync(T item);

        /// <summary>
        /// Delete a row in the DataBase table.
        /// </summary>
        /// <param name="id">Id of the object to delete.</param>
        Task DeleteAsync(int id);
    }
}