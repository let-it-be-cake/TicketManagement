using System.Threading.Tasks;

namespace TicketManagement.BusinessLogic.Interfaces
{
    public interface IManager<T>
    {
        public Task CreateAsync(T item);

        public Task UpdateAsync(T item);
    }
}
