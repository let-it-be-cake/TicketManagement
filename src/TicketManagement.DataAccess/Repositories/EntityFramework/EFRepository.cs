using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.EntityFramework
{
    internal class EFRepository<T> : IRepository<T>
        where T : class, new()
    {
        private readonly TicketManagementContext _context;

        public EFRepository(TicketManagementContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetAsync(id);

            if (item == null)
            {
                return;
            }

            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> collection = _context.Set<T>().AsNoTracking();
            return collection;
        }

        public async Task<T> GetAsync(int id)
        {
            T item = await _context.FindAsync<T>(id);
            return item;
        }

        public async Task UpdateAsync(T item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}