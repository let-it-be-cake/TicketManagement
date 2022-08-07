namespace TicketManagement.BusinessLogic.Interfaces
{
    public interface IValidate<T>
        where T : class, new()
    {
        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <param name="item">Object to validate.</param>
        /// <returns>The correctness of the object.</returns>
        bool IsValid(T item);
    }
}
