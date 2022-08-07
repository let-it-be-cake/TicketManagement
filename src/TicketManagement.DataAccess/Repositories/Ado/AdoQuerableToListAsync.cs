using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.Ado
{
    internal class AdoQuerableToListAsync : IQuerableHelper
    {
        public Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task<List<T>> taskCollection = Task.FromResult(source.ToList());
            return taskCollection;
        }
    }
}
