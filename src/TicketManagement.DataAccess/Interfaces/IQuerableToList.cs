using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    public interface IQuerableHelper
    {
        public Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));
    }
}
