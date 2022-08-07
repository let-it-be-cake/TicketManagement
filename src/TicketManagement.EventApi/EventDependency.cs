using Microsoft.Extensions.DependencyInjection;
using TicketManagement.EventApi.Proxys;

namespace TicketManagement.EventApi
{
    public static class EventDependency
    {
        public static void AddEventDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEventProxyService, EventProxy>();
            services.AddScoped<IEventAreaProxy, EventAreaProxy>();
            services.AddScoped<IGetEvents, EventProxy>();
            services.AddScoped<IGetEventSeat, EventSeatGetter>();
        }
    }
}
