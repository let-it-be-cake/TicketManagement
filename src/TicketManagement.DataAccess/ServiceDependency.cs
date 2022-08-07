using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.EntityFramework;
using TicketManagement.Entities.Identity;
using TicketManagement.Entities.Tables;

namespace TicketManagement.DataAccess
{
    public static class ServiceDependencyExtension
    {
        public static void AddDALDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TicketManagementContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IRepository<Area>, EFRepository<Area>>();
            services.AddScoped<IRepository<EventArea>, EFRepository<EventArea>>();
            services.AddScoped<IRepository<EventSeat>, EFRepository<EventSeat>>();
            services.AddScoped<IRepository<Layout>, EFRepository<Layout>>();
            services.AddScoped<IRepository<Seat>, EFRepository<Seat>>();
            services.AddScoped<IRepository<Ticket>, EFRepository<Ticket>>();
            services.AddScoped<IRepository<Venue>, EFRepository<Venue>>();
            services.AddScoped<IRepository<User>, EFRepository<User>>();

            services.AddScoped<IRepository<Event>, EFEventRepository>();

            services.AddScoped<IQuerableHelper, EFQuerableToListAsync>();
        }
    }
}