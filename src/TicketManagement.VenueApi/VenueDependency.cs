using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Entities.Tables;
using TicketManagement.VenueApi.Proxys;

namespace TicketManagement.VenueApi
{
    public static class VenueDependency
    {
        public static void AddVenueDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProxyService<Area>, AreaProxy>();
            services.AddScoped<IProxyService<Layout>, LayoutProxy>();
            services.AddScoped<IProxyService<Seat>, SeatProxy>();
            services.AddScoped<IProxyService<Venue>, VenueProxy>();
        }
    }
}
