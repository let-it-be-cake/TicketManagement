using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories.EntityFramework
{
    internal class EFQuerableToListAsync : IQuerableHelper
    {
        public Task<List<T>> ToListAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task<List<T>> taskCollection = source.ToListAsync(cancellationToken);
            return taskCollection;
        }
    }
}
