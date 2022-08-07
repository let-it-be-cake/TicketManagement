using Microsoft.Extensions.DependencyInjection;
using TicketManagement.PurchaseApi.Proxys;

namespace TicketManagement.PurchaseApi
{
    public static class PurchaseDependency
    {
        public static void AddPurchaseDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITicketService, TicketService>();
        }
    }
}
