using System.Collections.Generic;

namespace ThirdPartyEventEditor.Services.Interfaces
{
    public interface IProxyService<T>
        where T : class
    {
        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>The collection of objects.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Get an object by id.
        /// </summary>
        /// <param name="id">Id of the object to get.</param>
        /// <returns>The object located by this id.</returns>
        T Read(int id);

        /// <summary>
        /// Add a new object.
        /// </summary>
        /// <param name="item">Object to add.</param>
        void Add(T item);

        /// <summary>
        /// Change a record.
        /// </summary>
        /// <param name="item">A new instance of the object.</param>
        void Change(T item);

        /// <summary>
        /// Delete an object.
        /// </summary>
        /// <param name="id">Id of the object to delete.</param>
        void Delete(int id);
    }
}
