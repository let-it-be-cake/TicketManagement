using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.DataAccess.Repositories.EntityFramework
{
    internal class EFEventRepository : IRepository<Event>
    {
        private readonly TicketManagementContext _context;

        public EFEventRepository(TicketManagementContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Event item)
        {
            var query = @"EXEC [dbo].[sp_CreateEvent]
                @name = @NameValue, 
                @description = @DescriptionValue,
                @layoutId = @LayoutIdValue,
                @dateTimeStart = @StartValue,
                @dateTimeEnd = @EndValue,
                @imageUrl = @ImageUrlValue,
                @eventId = @EventIdValue OUTPUT";

            var name = new SqlParameter("NameValue", item.Name);
            var descr = new SqlParameter("DescriptionValue", item.Description);
            var layout = new SqlParameter("LayoutIdValue", item.LayoutId);
            var startDateTime = new SqlParameter("StartValue", item.DateTimeStart);
            var endDateTime = new SqlParameter("EndValue", item.DateTimeEnd);
            var imageUrl = new SqlParameter("ImageUrlValue", item.ImageUrl);
            var output = new SqlParameter
            {
                ParameterName = "EventIdValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            await _context.Database.ExecuteSqlRawAsync(query, name, descr, layout, startDateTime, endDateTime, imageUrl, output);
            item.Id = (int)output.Value;
        }

        public async Task DeleteAsync(int id)
        {
            var idParam = new SqlParameter("IdValue", id);
            await _context.Database.ExecuteSqlRawAsync(@"EXEC [dbo].[sp_DeleteEvent] @id = @IdValue;", idParam);
        }

        public IQueryable<Event> GetAll()
        {
            IQueryable<Event> querable = _context.Event.AsNoTracking();
            return querable;
        }

        public async Task<Event> GetAsync(int id)
        {
            var idParam = new SqlParameter("IdValue", id);
            var list = await _context.Event.FromSqlRaw(@"EXEC [dbo].sp_GetEvent @id = @IdValue;", idParam).ToListAsync();
            Event @event = list.FirstOrDefault();
            return @event;
        }

        public async Task UpdateAsync(Event item)
        {
            var query = @"EXEC [dbo].sp_UpdateEvent
                @id = @IdValue,
                @name = @NameValue, 
                @description = @DescriptionValue,
                @layoutId = @LayoutIdValue,
                @dateTimeStart = @StartValue,
                @dateTimeEnd = @EndValue,
                @imageUrl = @ImageUrlValue;";

            var id = new SqlParameter("@IdValue", item.Id);
            var name = new SqlParameter("NameValue", item.Name);
            var descr = new SqlParameter("DescriptionValue", item.Description);
            var layout = new SqlParameter("LayoutIdValue", item.LayoutId);
            var startDateTime = new SqlParameter("StartValue", item.DateTimeStart);
            var endDateTime = new SqlParameter("EndValue", item.DateTimeEnd);
            var imageUrl = new SqlParameter("ImageUrlValue", item.ImageUrl);

            await _context.Database.ExecuteSqlRawAsync(query, id, name, descr, layout, startDateTime, endDateTime, imageUrl);
        }
    }
}
