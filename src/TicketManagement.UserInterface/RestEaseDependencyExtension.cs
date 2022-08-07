using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;
using TicketManagement.UserInterface.Clients.EventApi;
using TicketManagement.UserInterface.Clients.PurchaseApi;
using TicketManagement.UserInterface.Clients.UserApi;
using TicketManagement.UserInterface.Clients.VenueApi;

namespace TicketManagement.UserInterface
{
    public static class RestEaseDependencyExtension
    {
        public static void RestEaseEventApiConfig(this IServiceCollection services, string connectionString)
        {
            services.AddRestEaseClient<IEventClient>(connectionString);
            services.AddRestEaseClient<IEventAreaClient>(connectionString);
            services.AddRestEaseClient<IEventSeatClient>(connectionString);
            services.AddRestEaseClient<IEventGetterClient>(connectionString);
        }

        public static void RestEaseUserApiConfig(this IServiceCollection services, string connectionString)
        {
            services.AddRestEaseClient<ITokenClient>(connectionString);
            services.AddRestEaseClient<IUserClient>(connectionString);
        }

        public static void RestEasePurchaseApiConfig(this IServiceCollection services, string connectionString)
        {
            services.AddRestEaseClient<IPurchaseClient>(connectionString);
        }

        public static void RestEaseVenueApiConfig(this IServiceCollection services, string connectionString)
        {
            services.AddRestEaseClient<IAreaClient>(connectionString);
            services.AddRestEaseClient<ILayoutClient>(connectionString);
            services.AddRestEaseClient<ISeatClient>(connectionString);
            services.AddRestEaseClient<IVenueClient>(connectionString);
        }
    }
}
