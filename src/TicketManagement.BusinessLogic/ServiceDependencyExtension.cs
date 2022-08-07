using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.Interfaces.TicketOffice;
using TicketManagement.BusinessLogic.Proxys;
using TicketManagement.BusinessLogic.TicketManager;
using TicketManagement.DataAccess;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic
{
    public static class ServiceDependencyExtension
    {
        public static void BLLDependency(this IServiceCollection services, string connectionString)
        {
            services.AddDALDependencies(connectionString);

            services.AddScoped<IValidate<Area>, AreaValidator>();
            services.AddScoped<IValidate<Layout>, LayoutValidator>();
            services.AddScoped<IValidate<Seat>, SeatValidator>();
            services.AddScoped<IValidate<Venue>, VenueValidator>();

            services.AddScoped<IProxyService<Area>, Proxy<Area>>();
            services.AddScoped<IProxyService<Layout>, Proxy<Layout>>();
            services.AddScoped<IProxyService<Seat>, Proxy<Seat>>();
            services.AddScoped<IProxyService<Venue>, Proxy<Venue>>();

            services.AddScoped<ITicketService, TicketOffice>();
        }
    }
}
